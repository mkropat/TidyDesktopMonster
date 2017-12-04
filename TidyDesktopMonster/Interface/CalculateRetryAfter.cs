using System;

namespace TidyDesktopMonster.Interface
{
    internal delegate TimeSpan CalculateRetryAfter(int numAttempts);
}
