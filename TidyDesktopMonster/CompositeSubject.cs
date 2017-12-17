using System;
using System.Collections.Generic;
using System.Linq;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster
{
    internal sealed class CompositeSubject<T> : IUpdatingSubject<T>
    {
        readonly IEnumerable<IUpdatingSubject<T>> _subjects;

        public event EventHandler SubjectChanged = delegate { };

        public CompositeSubject(IEnumerable<IUpdatingSubject<T>> subjects)
        {
            _subjects = subjects;

            foreach (var s in _subjects)
                s.SubjectChanged += (subject, evt) => SubjectChanged(subject, evt);
        }

        public IEnumerable<T> GetSubjects()
        {
            return _subjects
                .SelectMany(x => x.GetSubjects())
                .ToArray();
        }

        public void StartWatching()
        {
            foreach (var s in _subjects)
                s.StartWatching();
        }

        public void Dispose()
        {
            foreach (var s in _subjects)
                s.Dispose();
        }
    }
}
