using System;
using System.Collections.Generic;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.Subject
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
            var result = new List<T>();
            foreach (var x in _subjects)
                result.AddRange(x.GetSubjects());
            return result;
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
