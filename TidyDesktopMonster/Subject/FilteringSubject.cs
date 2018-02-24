using System;
using System.Collections.Generic;
using System.Linq;
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
            return _subject.GetSubjects()
                .Where(x =>
                {
                    var result = _predicate(x);
                    if (!result)
                        Log.Debug($"Filtering out '{x}'");
                    return result;
                })
                .ToArray();
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
