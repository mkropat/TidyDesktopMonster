using System;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.AppHelper
{
    internal class StartupFolderRegistration : IStartupRegistration
    {
        readonly string _appName;
        readonly string _appPath;
        readonly CreateShortcut _createShortcut;
        readonly ShortcutOptions _options;

        public StartupFolderRegistration(string appName, string appPath, ShortcutOptions options, CreateShortcut createShortcut)
        {
            _appName = appName;
            _appPath = Path.GetFullPath(appPath);
            _createShortcut = createShortcut;
            _options = options;
        }

        string LinkPath => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Startup),
            _appName + ".lnk");

        public bool RunOnStartup
        {
            get => File.Exists(LinkPath);
            set
            {
                if (value)
                    _createShortcut(LinkPath, _appPath, _options);
                else
                    File.Delete(LinkPath);
            }
        }
    }
}
