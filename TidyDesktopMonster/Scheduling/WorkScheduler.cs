using System;
using System.Threading;
using System.Threading.Tasks;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.Scheduling
{
    internal sealed class WorkScheduler : IDisposable
    {
        // Cases:
        // 1. WaitForWork -> RunNow -> Work starts + completes -> WaitForWork
        // 2. RunNow -> WaitForWork -> Work starts + completes -> WaitForWork
        // 3. WaitForWork -> RunNow -> Work starts -> RunNow -> Work completes -> WaitForWork

        bool _hasTaskBeenPickedUp = false;
        readonly object _lock = new object();
        int _numAttempts = 0;
        readonly CalculateRetryAfter _retryAfter;
        CancellationTokenSource _scheduledTaskCanceler = new CancellationTokenSource();
        TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();

        public WorkScheduler(CalculateRetryAfter retryAfter)
        {
            _retryAfter = retryAfter;

            _scheduledTaskCanceler.Cancel();
        }

        public void RunNow()
        {
            Run(resetNumAttempts: true);
        }

        void Run(bool resetNumAttempts = false)
        {
            TaskCompletionSource<object> originalTcs;
            lock (_lock)
            {
                if (resetNumAttempts)
                    _numAttempts = 0;

                _scheduledTaskCanceler.Cancel();
                originalTcs = _tcs;
                if (_hasTaskBeenPickedUp)
                {
                    _tcs = new TaskCompletionSource<object>();
                    _hasTaskBeenPickedUp = false;
                }
            }

            originalTcs.TrySetResult(null);
        }

        public void RunAfterBackoff()
        {
            lock (_lock)
            {
                if (_scheduledTaskCanceler.IsCancellationRequested)
                {
                    _numAttempts++;

                    _scheduledTaskCanceler.Dispose();
                    _scheduledTaskCanceler = new CancellationTokenSource();

                    Task.Delay(_retryAfter(_numAttempts), _scheduledTaskCanceler.Token)
                        .ContinueWith(t => Run(), TaskContinuationOptions.OnlyOnRanToCompletion);
                }
            }
        }

        public Task WaitForWork()
        {
            lock (_lock)
            {
                var result = _tcs.Task;
                _hasTaskBeenPickedUp = true;

                if (_tcs.Task.Status == TaskStatus.RanToCompletion)
                {
                    _tcs = new TaskCompletionSource<object>();
                    _hasTaskBeenPickedUp = false;
                }

                return result;
            }
        }

        public void Dispose()
        {
            _scheduledTaskCanceler.Dispose();
        }
    }
}
