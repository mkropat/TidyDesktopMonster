using System;
using System.IO;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;

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
                {
                    _createShortcut(LinkPath, _options);
                    Log.Info($"Created startup shortcut at '{LinkPath}'");
                }
                else
                {
                    File.Delete(LinkPath);
                    Log.Info($"Deleted startup shortcut at '{LinkPath}'");
                }
            }
        }

        bool LinkTargetExists(string linkPath)
        {
            try
            {
                if (!File.Exists(linkPath))
                {
                    Log.Debug($"Startup link does not exist at '{linkPath}'");
                    return false;
                }

                var link = _readShortcut(linkPath);
                var result = File.Exists(link.Target);
                if (!result)
                    Log.Debug($"Startup link target does not exist '{link.Target}'");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("Error when attempting to read startup link", ex);
                return false;
            }
        }
    }
}
