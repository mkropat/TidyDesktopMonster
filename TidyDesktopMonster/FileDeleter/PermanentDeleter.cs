using TidyDesktopMonster.Interface;
using TidyDesktopMonster.WinApi.Shell32;

namespace TidyDesktopMonster.FileDeleter
{
    internal class PermanentDeleter : IFileDeleter
    {
        public void DeleteFile(string path)
        {
            Shell32Delete.DeleteFile(path, DeleteBehavior.PermanentlyDelete);
        }
    }
}
