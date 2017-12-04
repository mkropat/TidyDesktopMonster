using System;
using System.Threading;
using System.Threading.Tasks;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Scheduling;

namespace TidyDesktopMonster
{
    internal class PerformActionOnUpdatingSubject<T>
    {
        readonly Action<T> _action;
        readonly WorkScheduler _scheduler;
        readonly IUpdatingSubject<T> _subject;

        public PerformActionOnUpdatingSubject(IUpdatingSubject<T> subject, Action<T> action, WorkScheduler scheduler)
        {
            _action = action;
            _scheduler = scheduler;
            _subject = subject;
        }

        public async Task Run(CancellationToken cancelToken)
        {
            _subject.SubjectChanged += (obj, evt) => _scheduler.RunNow();
            _subject.StartWatching();

            var cancelTask = TaskFromCancellationToken(cancelToken);

            while (!cancelTask.IsCanceled)
            {
                foreach (var x in _subject.GetSubjects())
                {
                    try
                    {
                        _action(x);
                    }
                    catch
                    {
                        _scheduler.RunAfterBackoff();
                    }
                }

                await Task.WhenAny(_scheduler.WaitForWork(), cancelTask)
                    .ConfigureAwait(false);
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
