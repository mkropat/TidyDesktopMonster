using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TidyDesktopMonster.AppHelper;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;
using TidyDesktopMonster.Scheduling;
using TidyDesktopMonster.Subject;
using TidyDesktopMonster.WinApi;
using TidyDesktopMonster.WinApi.Shell32;

namespace TidyDesktopMonster
{
    static class Program
    {
        static readonly Assembly _appAssembly = typeof(Program).Assembly;

        static string AppName { get; } = _appAssembly
            .GetCustomAttribute<AssemblyTitleAttribute>()
            .Title;

        static string AppPath { get; } = _appAssembly.Location;

        static string AppVersion { get; } = _appAssembly.GetName().Version.ToString();

        static string ProgramId { get; } = _appAssembly
            .GetCustomAttribute<GuidAttribute>()
            .Value;

        static string[] ApplicationExtensions { get; } = Environment.GetEnvironmentVariable("PATHEXT")
            ?.Split(';')
            ?? new string[0];

        [STAThread]
        static void Main(string[] args)
        {
            using (var guard = new SingleInstanceGuard(ProgramId, SingleInstanceGuard.Scope.CurrentUser))
            {
                if (guard.IsPrimaryInstance)
                    RunApp(args);
                else
                    User32Messages.BroadcastMessage(Constants.OpenWindowMessage);
            }
        }

        static void RunApp(string[] args)
        {
            var shouldStartService = args.Any(x => "-StartService".Equals(x, StringComparison.InvariantCultureIgnoreCase));

            var settingsStore = new InMemoryKeyValueCache(new RegistryKeyValueStore(AppName));

            var logBuffer = new RotatingBufferSink();
            InitializeLogging(logBuffer, settingsStore, defaultLogLevel: LogLevel.Info);
            Log.Info($"Running version: {AppVersion}");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var retryLogic = new ExponentialBackoffLogic(min: TimeSpan.FromMilliseconds(10), max: TimeSpan.FromHours(1));
            var startupRegistration = new StartupFolderRegistration(
                AppName.ToLowerInvariant(),
                new ShortcutOptions { Arguments = "-StartService", Target = AppPath },
                WindowsScriptHostWrapper.CreateShortcut,
                ShellifyWrapper.ReadShortcut);

            using (var scheduler = new WorkScheduler(retryLogic.CalculateRetryAfter))
            {
                var service = new WatchForFilesToDelete<string>(
                    subjectFactory: () => CreateSubject(settingsStore),
                    delete: Shell32Delete.DeleteFile,
                    scheduler: scheduler,
                    settingsStore: settingsStore);

                RunForm(new MainForm(
                    showSettingsForm: !shouldStartService,
                    appPath: AppPath,
                    logEntries: logBuffer,
                    openWindowMessage: (int)User32Messages.GetMessage(Constants.OpenWindowMessage),
                    settingsStore: settingsStore,
                    startService: service.Run,
                    startupRegistration: startupRegistration));
            }
        }

        static void InitializeLogging(ILogSink sink, IKeyValueStore settingsStore, LogLevel defaultLogLevel)
        {
            Log.Sink = sink;
            Log.Info("Logging initialized");

            var minimumSeverity = settingsStore.Read<LogLevel?>("MinimumSeverity") ?? defaultLogLevel;
            Log.Info($"Setting minimum severity to {minimumSeverity}");

            Log.Sink = new MinimumSeveritySink(sink, minimumSeverity);
        }

        static IUpdatingSubject<string> CreateSubject(IKeyValueStore settingsStore)
        {
            var filter = settingsStore.Read<ShortcutFilterType?>(Constants.ShortcutFilterSetting);
            switch (filter)
            {
                case ShortcutFilterType.All:
                    return CreateDirectoryWatcher(settingsStore);
                case ShortcutFilterType.Apps:
                default:
                    return new FilteringSubject<string>(
                        CreateDirectoryWatcher(settingsStore),
                        path => PathHasExtension(ShellifyWrapper.ReadShortcut(path).Target, ApplicationExtensions));
            }
        }

        static IUpdatingSubject<string> CreateDirectoryWatcher(IKeyValueStore settingsStore)
        {
            var allUsersDesktop = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
            var currentUserDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            return settingsStore.Read<bool?>(Constants.TidyAllUsersSetting) == true
                ? new CompositeSubject<string>(new[]
                {
                    new FilesInDirectorySubject(allUsersDesktop, "*.lnk"),
                    new FilesInDirectorySubject(allUsersDesktop, "*.url"),
                    new FilesInDirectorySubject(currentUserDesktop, "*.lnk"),
                    new FilesInDirectorySubject(currentUserDesktop, "*.url"),
                })
                : new CompositeSubject<string>(new[]
                {
                    new FilesInDirectorySubject(currentUserDesktop, "*.lnk"),
                    new FilesInDirectorySubject(currentUserDesktop, "*.url"),
                });
        }

        static bool PathHasExtension(string path, string[] extensions)
        {
            var pathExtension = Path.GetExtension(path);
            return extensions.Contains(pathExtension, StringComparer.InvariantCultureIgnoreCase);
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
