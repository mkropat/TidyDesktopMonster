using System;
using System.Collections.Generic;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;

namespace TidyDesktopMonster.Subject
{
    internal sealed class FilteringSubject<T> : IUpdatingSubject<T>
    {
        readonly Func<T, bool> _predicate;
        readonly IUpdatingSubject<T> _subject;

        public event EventHandler SubjectChanged = delegate { };

        public FilteringSubject(IUpdatingSubject<T> subject, Func<T, bool> predicate)
        {
            _predicate = predicate;
            _subject = subject;

            _subject.SubjectChanged += (sender, evt) => SubjectChanged(sender, evt);
        }

        public IEnumerable<T> GetSubjects()
        {
            var result = new List<T>();
            foreach (var x in _subject.GetSubjects())
            {
                if (_predicate(x))
                    result.Add(x);
                else
                    Log.Debug($"Filtering out '{x}'");
            }
            return result;
        }

        public void StartWatching()
        {
            _subject.StartWatching();
        }

        public void Dispose()
        {
            _subject.Dispose();
        }
    }
}
