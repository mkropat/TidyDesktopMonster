using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace TidyDesktopMonster.WinApi
{
    internal class SystemMenu
    {
        readonly IntPtr _handle;
        readonly bool _addedSeparator = false;

        public SystemMenu(IntPtr formHandle)
        {
            _handle = formHandle;
        }

        public void PrependItem(uint itemId, string text)
        {
            var sysMenu = NativeMethods.GetSystemMenu(_handle, shouldRevert: false);
            if (sysMenu == IntPtr.Zero)
                throw new Win32Exception();

            if (!_addedSeparator)
                if (!NativeMethods.InsertMenu(sysMenu, 0, MenuItemFlag.MF_BYPOSITION | MenuItemFlag.MF_SEPARATOR, itemId: new UIntPtr(0), newItem: string.Empty))
                    throw new Win32Exception();

            if (!NativeMethods.InsertMenu(sysMenu, 0, MenuItemFlag.MF_BYPOSITION | MenuItemFlag.MF_STRING, new UIntPtr(itemId), text))
                throw new Win32Exception();
        }

        class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr GetSystemMenu(IntPtr handle, bool shouldRevert);

            [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern bool InsertMenu(IntPtr handle, int position, MenuItemFlag flags, UIntPtr itemId, string newItem);
        }

        enum MenuItemFlag : uint
        {
            MF_INSERT           = 0x00000000,
            MF_CHANGE           = 0x00000080,
            MF_APPEND           = 0x00000100,
            MF_DELETE           = 0x00000200,
            MF_REMOVE           = 0x00001000,

            MF_BYCOMMAND        = 0x00000000,
            MF_BYPOSITION       = 0x00000400,

            MF_SEPARATOR        = 0x00000800,

            MF_ENABLED          = 0x00000000,
            MF_GRAYED           = 0x00000001,
            MF_DISABLED         = 0x00000002,

            MF_UNCHECKED        = 0x00000000,
            MF_CHECKED          = 0x00000008,
            MF_USECHECKBITMAPS  = 0x00000200,

            MF_STRING           = 0x00000000,
            MF_BITMAP           = 0x00000004,
            MF_OWNERDRAW        = 0x00000100,

            MF_POPUP            = 0x00000010,
            MF_MENUBARBREAK     = 0x00000020,
            MF_MENUBREAK        = 0x00000040,

            MF_UNHILITE         = 0x00000000,
            MF_HILITE           = 0x00000080,
        }
    }
}
