using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TidyDesktopMonster.AppHelper;
using TidyDesktopMonster.Scheduling;
using TidyDesktopMonster.WinApi;
using TidyDesktopMonster.WinApi.Shell32;

namespace TidyDesktopMonster
{
    static class Program
    {
        static Assembly _appAssembly = typeof(Program).Assembly;

        static string AppPath { get; } = _appAssembly.Location;

        static string ProgramId { get; } = _appAssembly
            .GetCustomAttribute<GuidAttribute>()
            .Value;

        [STAThread]
        static void Main()
        {
            using (var guard = new SingleInstanceGuard(ProgramId, SingleInstanceGuard.Scope.CurrentUser))
            {
                if (guard.IsPrimaryInstance)
                    RunApp();
            }
        }

        static void RunApp()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var directoryToMonitor = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var retryLogic = new ExponentialBackoffLogic(min: TimeSpan.FromMilliseconds(10), max: TimeSpan.FromHours(1));

            using (var scheduler = new WorkScheduler(retryLogic.CalculateRetryAfter))
            using (var subject = new FilesInDirectorySubject(directoryToMonitor, "*.lnk"))
            {
                var service = new PerformActionOnUpdatingSubject<string>(subject, action: Shell32Delete.DeleteFile, scheduler: scheduler);
                RunForm(new MainForm(
                    AppPath,
                    startService: service.Run));
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
