using System;
using System.Runtime.InteropServices;

namespace TidyDesktopMonster.WinApi.Shell32
{
    // https://msdn.microsoft.com/en-us/library/windows/desktop/bb759795.aspx
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 2)]
    internal struct ShellFileOptions
    {
        public IntPtr handle;
        public FileOperation func;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string from;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string to;

        public FileOperationFlags flags;

        [MarshalAs(UnmanagedType.Bool)]
        public bool anyOperationsAborted;

        public IntPtr nameMappings;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string progressTitle;
    }
}
