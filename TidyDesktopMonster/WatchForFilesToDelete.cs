using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;
using TidyDesktopMonster.Scheduling;
using TidyDesktopMonster.WinApi.Shell32;

namespace TidyDesktopMonster
{
    internal class WatchForFilesToDelete
    {
        readonly Func<IFileDeleter> _deleterFactory;
        readonly WorkScheduler _scheduler;
        readonly Func<IUpdatingSubject<string>> _subjectFactory;
        readonly IKeyValueStore _settingsStore;

        public WatchForFilesToDelete(Func<IUpdatingSubject<string>> subjectFactory, Func<IFileDeleter> deleterFactory, WorkScheduler scheduler)
        {
            _deleterFactory = deleterFactory;
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

        async Task Run(CancellationToken cancelToken, IUpdatingSubject<string> subject)
        {
            var deleter = _deleterFactory();

            subject.SubjectChanged += (obj, evt) => _scheduler.RunNow();
            subject.StartWatching();

            var cancelTask = TaskFromCancellationToken(cancelToken);

            while (!cancelTask.IsCanceled)
            {
                var subjects = Enumerable.Empty<string>();
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
                        deleter.DeleteFile(x);
                        Log.Info($"Deleted the file '{x}'");
                    }
                    catch (AccessDeniedException)
                    {
                        Log.Warn($"Access denied when deleting the file '{x}'");
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
