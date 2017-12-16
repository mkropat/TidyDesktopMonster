using System;
using System.Collections.Generic;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.WinApi
{
    internal sealed class FilesInDirectorySubject : IUpdatingSubject<string>
    {
        readonly string _directory;
        readonly string _pattern;
        readonly FileSystemWatcher _watcher;

        public FilesInDirectorySubject(string directoryPath, string searchPattern = "*.*")
        {
            _directory = directoryPath;
            _pattern = searchPattern;

            _watcher = new FileSystemWatcher
            {
                Path = _directory,
                Filter = _pattern,
                NotifyFilter = NotifyFilters.FileName,
            };
            _watcher.Created += (obj, evt) =>
            {
                SubjectChanged(this, new EventArgs());
            };
            _watcher.Renamed += (obj, evt) =>
            {
                // Ideally this would only fire when evt.Name matches the searchPattern.
                // (Currently it will fire even if only evt.OldName matches.)
                SubjectChanged(this, new EventArgs());
            };
        }

        public event EventHandler SubjectChanged;

        public IEnumerable<string> GetSubjects()
        {
            return Directory.EnumerateFiles(_directory, _pattern);
        }

        public void StartWatching()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}
