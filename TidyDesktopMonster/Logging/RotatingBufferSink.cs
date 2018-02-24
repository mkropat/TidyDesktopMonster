using System;
using System.Collections.Generic;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.Logging
{
    internal class RotatingBufferSink : ILogSink, IUpdatingSubject<LogEntry>
    {
        readonly LogEntry[] _entries;
        int _start = 0;
        int _end = 0;

        public event EventHandler SubjectChanged;

        public RotatingBufferSink(int maxEntries=100)
        {
            _entries = new LogEntry[maxEntries];
        }

        public void Write(LogEntry entry)
        {
            _entries[_end] = entry;
            _end = _end + 1 % _entries.Length;
            if (_end == _start)
                _start = _start + 1 % _entries.Length;

            SubjectChanged(this, new EventArgs());
        }

        public IEnumerable<LogEntry> GetSubjects()
        {
            for (var i = _start; i < _end && i < _entries.Length; i++)
                yield return _entries[i];

            if (_end < _start)
                for (var i = 0; i < _end; i++)
                    yield return _entries[i];
        }

        public void StartWatching()
        {
        }

        public void Dispose()
        {
        }
    }
}
