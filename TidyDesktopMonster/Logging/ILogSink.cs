using System;

namespace TidyDesktopMonster.Logging
{
    internal interface ILogSink
    {
        void Write(LogEntry entry);
    }
}
