using System;
using System.Reflection;
using System.Windows.Forms;

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

            RunForm(new MainForm(AppPath));
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
