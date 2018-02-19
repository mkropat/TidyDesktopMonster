using Shellify;
using Shellify.IO;
using System.IO;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.WinApi
{
    internal static class ShellifyWrapper
    {
        public static ShortcutOptions ReadShortcut(string lnkPath)
        {
            var link = ReadShellLinkFile(lnkPath);
            return new ShortcutOptions
            {
                Arguments = link.Arguments,
                Description = link.Name,
                Target = Path.GetFullPath(link.RelativePath),
                WorkingDirectory = link.WorkingDirectory,
            };
        }

        static ShellLinkFile ReadShellLinkFile(string path)
        {
            var result = new ShellLinkFile();

            var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            using (var binaryReader = new BinaryReader(stream))
            {
                var reader = new ShellLinkFileHandler(result);
                reader.ReadFrom(binaryReader);
            }

            return result;
        }
    }
}
