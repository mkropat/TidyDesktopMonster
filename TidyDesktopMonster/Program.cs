using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TidyDesktopMonster.AppHelper;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Scheduling;
using TidyDesktopMonster.WinApi;
using TidyDesktopMonster.WinApi.Shell32;

namespace TidyDesktopMonster
{
    static class Program
    {
        static Assembly _appAssembly = typeof(Program).Assembly;

        static string AppName { get; } = _appAssembly
            .GetCustomAttribute<AssemblyTitleAttribute>()
            .Title
            .ToLowerInvariant();

        static string AppPath { get; } = _appAssembly.Location;

        static string ProgramId { get; } = _appAssembly
            .GetCustomAttribute<GuidAttribute>()
            .Value;

        const string openWindowMessage = "TD_OPENWINDOW";

        [STAThread]
        static void Main(string[] args)
        {
            using (var guard = new SingleInstanceGuard(ProgramId, SingleInstanceGuard.Scope.CurrentUser))
            {
                if (guard.IsPrimaryInstance)
                    RunApp(args);
                else
                    User32Messages.BroadcastMessage(openWindowMessage);
            }
        }

        static void RunApp(string[] args)
        {
            var shouldStartService = args.Any(x => "-StartService".Equals(x, StringComparison.InvariantCultureIgnoreCase));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var directoryToMonitor = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var retryLogic = new ExponentialBackoffLogic(min: TimeSpan.FromMilliseconds(10), max: TimeSpan.FromHours(1));
            var settingsStore = new RegistryKeyValueStore("TidyDesktopMonster");
            var startupRegistration = new StartupFolderRegistration(AppName, AppPath, new ShortcutOptions { Arguments = "-StartService" }, WindowsScriptHostWrapper.CreateShortcut);

            using (var scheduler = new WorkScheduler(retryLogic.CalculateRetryAfter))
            using (var subject = new FilesInDirectorySubject(directoryToMonitor, "*.lnk"))
            {
                var service = new PerformActionOnUpdatingSubject<string>(subject, action: Shell32Delete.DeleteFile, scheduler: scheduler);
                RunForm(new MainForm(
                    showSettingsForm: !shouldStartService,
                    appPath: AppPath,
                    openWindowMessage: (int)User32Messages.GetMessage(openWindowMessage),
                    settingsStore: settingsStore,
                    startService: service.Run,
                    startupRegistration: startupRegistration));
            }
        }

        static void RunForm(Form form)
        {
            using (var ctx = new ApplicationContext(form))
            {
                Application.Run(ctx);
            }
        }
    }
}
