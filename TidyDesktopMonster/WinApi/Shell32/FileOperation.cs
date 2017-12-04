namespace TidyDesktopMonster.WinApi.Shell32
{
    // http://pinvoke.net/default.aspx/Enums/FileFuncFlags.html
    internal enum FileOperation : uint
    {
        FO_MOVE     = 0x0001,
        FO_COPY     = 0x0002,
        FO_DELETE   = 0x0003,
        FO_RENAME   = 0x0004,
    }
}
