using System;

namespace TidyDesktopMonster.Logging
{
    public class LogEntry
    {
        public LogLevel Level { get; }
        public string Message { get; }
        public DateTime Timestamp { get; }

        public LogEntry(DateTime timestamp, LogLevel level, string message)
        {
            Level = level;
            Message = message;
            Timestamp = timestamp;
        }
    }

    public enum LogLevel
    {
        All = 0,
        Debug,
        Info,
        Warn,
        Error,
    }
}
