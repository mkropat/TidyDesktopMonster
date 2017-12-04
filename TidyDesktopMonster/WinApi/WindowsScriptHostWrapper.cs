using IWshRuntimeLibrary;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.WinApi
{
    internal static class WindowsScriptHostWrapper
    {
        public static void CreateShortcut(string lnkPath, string targetPath, ShortcutOptions options = null)
        {
            options = options ?? new ShortcutOptions();

            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(Path.GetFullPath(lnkPath));
            shortcut.Arguments = options.Arguments;
            shortcut.Description = options.Description;
            shortcut.Hotkey = options.Hotkey ?? string.Empty;
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = options.WorkingDirectory;

            shortcut.Save();
        }
    }
}
