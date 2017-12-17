namespace TidyDesktopMonster.Interface
{
    internal delegate void CreateShortcut(string lnkPath, ShortcutOptions options);

    internal delegate ShortcutOptions ReadShortcut(string lnkPath);

    internal class ShortcutOptions
    {
        public string Arguments { get; set; }
        public string Description { get; set; }
        public string Hotkey { get; set; }
        public string Target { get; set; }
        public string WorkingDirectory { get; set; }
    }
}
