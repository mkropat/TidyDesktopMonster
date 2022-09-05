using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster.WinApi.Shell32
{
    internal static class Shell32Delete
    {
        const int DE_ACCESSDENIEDSRC = 0x78;

        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb762164.aspx
        public static void DeleteFile(string path, DeleteBehavior deleteBehavior)
        {
            var fileOp = default(ShellFileOptions);
            fileOp.func = FileOperation.FO_DELETE;
            fileOp.from = Path.GetFullPath(path) + '\0';

            var noUiFlags = FileOperationFlags.FOF_SILENT | FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_NOCONFIRMMKDIR;
            fileOp.flags = noUiFlags;
            if (deleteBehavior == DeleteBehavior.DeleteToRecycleBin)
                fileOp.flags |= FileOperationFlags.FOF_ALLOWUNDO;

            var result = NativeMethods.SHFileOperation(ref fileOp);
            if (result == DE_ACCESSDENIEDSRC)
                throw new AccessDeniedException();
            if (result != 0)
                throw new Exception($"SHFileOperation returned 0x{result:X}: {GetErrorMessage(result)}");

            if (fileOp.anyOperationsAborted)
                throw new Exception("SHFileOperation was aborted");
        }

        static string GetErrorMessage(int errorCode)
        {
            var builtinException = new Win32Exception(errorCode);
            return builtinException.Message;
        }

        static class NativeMethods
        {
            [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
            public static extern int SHFileOperation(ref ShellFileOptions fileOp);
        }
    }
}
