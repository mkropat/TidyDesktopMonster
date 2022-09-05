using System;

namespace TidyDesktopMonster.Logging
{
    internal static class Log
    {
        public static ILogSink Sink { get; set; } = NullLog.Instance;

        static Exception _lastException = null;
        public static Exception LastException => _lastException;

        static readonly object _lock = new object();

        public static void Debug(params object[] messages)
        {
            Write(LogLevel.Debug, messages);
        }

        public static void Info(params object[] messages)
        {
            Write(LogLevel.Info, messages);
        }

        public static void Warn(params object[] messages)
        {
            Write(LogLevel.Warn, messages);
        }

        public static void Error(params object[] messages)
        {
            Write(LogLevel.Error, messages);
        }

        static void Write(LogLevel level, object[] messages)
        {
            lock (_lock)
            {
                try
                {
                    var asStrings = Array.ConvertAll(messages, ObjectFormatter.Format);
                    var entry = new LogEntry(DateTime.UtcNow, level, string.Join(" - ", asStrings));
                    Sink.Write(entry);
                }
                catch (Exception ex)
                {
                    _lastException = ex;
                }
            }
        }
    }
}
