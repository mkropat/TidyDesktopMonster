using System;
using System.Collections.Generic;

namespace TidyDesktopMonster.Interface
{
    public interface IUpdatingSubject<T> : IDisposable
    {
        event EventHandler SubjectChanged;
        void StartWatching();
        IEnumerable<T> GetSubjects();
    }
}
