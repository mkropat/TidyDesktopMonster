using System;

namespace TidyDesktopMonster.Scheduling
{
    internal class ExponentialBackoffLogic
    {
        TimeSpan _min;
        TimeSpan _max;

        public ExponentialBackoffLogic(TimeSpan min, TimeSpan max)
        {
            _min = min;
            _max = max;
        }

        public TimeSpan CalculateRetryAfter(int numAttempts)
        {
            var backoffByAttempts = _min.TotalMilliseconds * (Math.Pow(2, numAttempts) - 1);
            return TimeSpan.FromMilliseconds(Math.Min(backoffByAttempts, _max.TotalMilliseconds));
        }
    }
}
