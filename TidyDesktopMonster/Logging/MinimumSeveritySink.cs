namespace TidyDesktopMonster.Logging
{
    internal class MinimumSeveritySink : ILogSink
    {
        readonly LogLevel _minimumSeverity;
        readonly ILogSink _sink;

        public MinimumSeveritySink(ILogSink sink, LogLevel minimumSeverity)
        {
            _minimumSeverity = minimumSeverity;
            _sink = sink;
        }

        public void Write(LogEntry entry)
        {
            if (entry.Level >= _minimumSeverity)
                _sink.Write(entry);
        }
    }
}
