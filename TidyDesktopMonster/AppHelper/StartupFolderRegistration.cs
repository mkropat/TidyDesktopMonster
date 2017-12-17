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

        public StartupFolderRegistration(string appName, ShortcutOptions options, CreateShortcut createShortcut)
        {
            _appName = appName;
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
                    _createShortcut(LinkPath, _options);
                else
                    File.Delete(LinkPath);
            }
        }
    }
}
