using System;

namespace TidyDesktopMonster.Logging
{
    internal class NullLog : ILogSink
    {
        public static NullLog Instance { get; } = new NullLog();

        private NullLog() { }

        public void Write(LogEntry entry)
        {
        }
    }
}
