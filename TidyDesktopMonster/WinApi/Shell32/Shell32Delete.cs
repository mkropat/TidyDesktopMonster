using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace TidyDesktopMonster.WinApi.Shell32
{
    internal static class Shell32Delete
    {
        // https://msdn.microsoft.com/en-us/library/windows/desktop/bb762164.aspx
        public static void DeleteFile(string path)
        {
            var fileOp = default(ShellFileOptions);
            fileOp.func = FileOperation.FO_DELETE;
            fileOp.from = Path.GetFullPath(path) + '\0';

            var noUiFlags = FileOperationFlags.FOF_SILENT | FileOperationFlags.FOF_NOCONFIRMATION | FileOperationFlags.FOF_NOERRORUI | FileOperationFlags.FOF_NOCONFIRMMKDIR;
            fileOp.flags = FileOperationFlags.FOF_ALLOWUNDO | noUiFlags;

            var result = NativeMethods.SHFileOperation(ref fileOp);
            if (result != 0)
                throw new Exception($"SHFileOperation returned {result}: {GetErrorMessage(result)}");

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
