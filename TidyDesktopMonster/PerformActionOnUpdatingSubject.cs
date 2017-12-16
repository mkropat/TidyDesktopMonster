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
        readonly Func<IUpdatingSubject<T>> _subjectFactory;

        public PerformActionOnUpdatingSubject(Func<IUpdatingSubject<T>> subjectFactory, Action<T> action, WorkScheduler scheduler)
        {
            _action = action;
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
                foreach (var x in subject.GetSubjects())
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
