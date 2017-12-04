using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TidyDesktopMonster.Scheduling;
using TidyDesktopMonster.WinApi;

namespace TidyDesktopMonster
{
    static class Program
    {
        static Assembly _appAssembly = typeof(Program).Assembly;

        static string AppPath { get; } = _appAssembly.Location;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var directoryToMonitor = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var retryLogic = new ExponentialBackoffLogic(min: TimeSpan.FromMilliseconds(10), max: TimeSpan.FromHours(1));

            using (var scheduler = new WorkScheduler(retryLogic.CalculateRetryAfter))
            using (var subject = new FilesInDirectorySubject(directoryToMonitor, "*.lnk"))
            {
                var service = new PerformActionOnUpdatingSubject<string>(subject, action: File.Delete, scheduler: scheduler);
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
