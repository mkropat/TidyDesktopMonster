using System;
using System.Collections.Generic;

namespace TidyDesktopMonster.Interface
{
    internal interface IUpdatingSubject<T>
    {
        event EventHandler SubjectChanged;
        void StartWatching();
        IEnumerable<T> GetSubjects();
    }
}
