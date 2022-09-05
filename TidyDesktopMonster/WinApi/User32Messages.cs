using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TidyDesktopMonster.WinApi
{
    internal static class User32Messages
    {
        static readonly ConcurrentDictionary<string, uint> _messages = new ConcurrentDictionary<string, uint>();

        public static void PostMessage(IntPtr handle, string message)
        {
            if (!NativeMethods.PostMessage(handle, GetMessage(message), IntPtr.Zero, IntPtr.Zero))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        public static void BroadcastMessage(string message)
        {
            var hwndBroadcast = (IntPtr)0xffff;
            PostMessage(hwndBroadcast, message);
        }

        public static uint GetMessage(string name)
        {
            return _messages.GetOrAdd(name, n =>
            {
                var result = NativeMethods.RegisterWindowMessage(n);
                if (result == 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                return result;
            });
        }

        static class NativeMethods
        {
            [return: MarshalAs(UnmanagedType.Bool)]
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern bool PostMessage(IntPtr handle, uint windowMessage, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern uint RegisterWindowMessage(string messageName);
        }
    }
}
