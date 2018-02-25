using Shellify;
using Shellify.IO;
using System.IO;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;

namespace TidyDesktopMonster.WinApi
{
    internal static class ShellifyWrapper
    {
        public static ShortcutOptions ReadShortcut(string lnkPath)
        {
            var link = ReadShellLinkFile(lnkPath);
            Log.Debug($"Read link", new
            {
                link.LinkInfo?.LocalBasePath,
                link.RelativePath,
            });
            return new ShortcutOptions
            {
                Arguments = link.Arguments,
                Description = link.Name,
                Target = GetTarget(link, lnkPath),
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

        static string GetTarget(ShellLinkFile link, string lnkPath)
        {
            if (link.LinkInfo?.LocalBasePath != null)
                return link.LinkInfo.LocalBasePath;

            if (link.RelativePath != null)
                return ResolveRelativePath(Path.GetDirectoryName(lnkPath), link.RelativePath);

            return null;
        }

        static string ResolveRelativePath(string baseDir, string relativePath)
        {
            return Path.GetFullPath(Path.Combine(baseDir, relativePath));
        }
    }
}
