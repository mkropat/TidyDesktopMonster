using Shell32;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.WinApi
{
    internal static class Shell32Wrapper
    {
        public static ShortcutOptions ReadShortcut(string lnkPath)
        {
            var shell = new Shell();
            var dir = shell.NameSpace(Path.GetFullPath(Path.GetDirectoryName(lnkPath)));
            var file = dir.Items().Item(Path.GetFileName(lnkPath));
            var link = (ShellLinkObject)file.GetLink;

            return new ShortcutOptions
            {
                Arguments = link.Arguments,
                Description = link.Description,
                Target = link.Target.Path,
                WorkingDirectory = link.WorkingDirectory,
            };
        }

        public static ShortcutOptions TryReadShortcut(string lnkPath)
        {
            try
            {
                return ReadShortcut(lnkPath);
            }
            catch
            {
                return new ShortcutOptions();
            }
        }
    }
}
