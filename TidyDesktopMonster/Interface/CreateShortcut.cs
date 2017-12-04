namespace TidyDesktopMonster.Interface
{
    internal delegate void CreateShortcut(string lnkPath, string targetPath, ShortcutOptions options);

    internal class ShortcutOptions
    {
        public string Arguments { get; set; }
        public string Description { get; set; }
        public string Hotkey { get; set; }
        public string WorkingDirectory { get; set; }
    }
}
