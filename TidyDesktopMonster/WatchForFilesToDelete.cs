using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;
using TidyDesktopMonster.Scheduling;

namespace TidyDesktopMonster
{
    internal class WatchForFilesToDelete<T>
    {
        readonly Action<T> _delete;
        readonly WorkScheduler _scheduler;
        readonly Func<IUpdatingSubject<T>> _subjectFactory;

        public WatchForFilesToDelete(Func<IUpdatingSubject<T>> subjectFactory, Action<T> delete, WorkScheduler scheduler)
        {
            _delete = delete;
            _scheduler = scheduler;
            _subjectFactory = subjectFactory;
        }

        public async Task Run(CancellationToken cancelToken)
        {
            using (var subject = _subjectFactory())
            {
                await Run(cancelToken, subject)
                    .ConfigureAwait(false);
            }
        }

        async Task Run(CancellationToken cancelToken, IUpdatingSubject<T> subject)
        {
            subject.SubjectChanged += (obj, evt) => _scheduler.RunNow();
            subject.StartWatching();

            var cancelTask = TaskFromCancellationToken(cancelToken);

            while (!cancelTask.IsCanceled)
            {
                var subjects = Enumerable.Empty<T>();
                try
                {
                    subjects = subject.GetSubjects();
                }
                catch (Exception ex)
                {
                    Log.Info("Error when looking for files to delete", ex);
                    _scheduler.RunAfterBackoff();
                }

                foreach (var x in subjects)
                {
                    try
                    {
                        _delete(x);
                        Log.Info($"Deleted the file '{x}'");
                    }
                    catch (Exception ex)
                    {
                        Log.Warn($"Error when deleting the file '{x}'", ex);
                        _scheduler.RunAfterBackoff();
                    }
                }

                await Task.WhenAny(_scheduler.WaitForWork(), cancelTask);
            }
        }

        Task TaskFromCancellationToken(CancellationToken cancelToken)
        {
            // https://stackoverflow.com/a/18672893/27581
            var tcs = new TaskCompletionSource<object>();
            cancelToken.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false);
            return tcs.Task;
        }
    }
}
