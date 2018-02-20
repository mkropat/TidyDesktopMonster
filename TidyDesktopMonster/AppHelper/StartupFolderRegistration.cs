using System;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.AppHelper
{
    internal class StartupFolderRegistration : IStartupRegistration
    {
        readonly string _appName;
        readonly CreateShortcut _createShortcut;
        readonly ShortcutOptions _options;
        readonly ReadShortcut _readShortcut;

        public StartupFolderRegistration(string appName, ShortcutOptions options, CreateShortcut createShortcut, ReadShortcut readShortcut)
        {
            _appName = appName;
            _createShortcut = createShortcut;
            _options = options;
            _readShortcut = readShortcut;
        }

        string LinkPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            _appName + ".lnk");

        public bool RunOnStartup
        {
            get => LinkTargetExists(LinkPath);
            set
            {
                if (value)
                    _createShortcut(LinkPath, _options);
                else
                    File.Delete(LinkPath);
            }
        }

        bool LinkTargetExists(string linkPath)
        {
            try
            {
                if (!File.Exists(linkPath))
                    return false;

                var link = _readShortcut(linkPath);
                return File.Exists(link.Target);
            }
            catch
            {
                return false;
            }
        }
    }
}
