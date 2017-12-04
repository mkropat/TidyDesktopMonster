using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TidyDesktopMonster
{
    public partial class MainForm : Form
    {
        readonly string _appPath;
        readonly CancellationTokenSource _serviceCts = new CancellationTokenSource();
        readonly Func<CancellationToken, Task> _startService;
        readonly Container _trayContainer = new Container();

        public MainForm(string appPath, Func<CancellationToken, Task> startService)
        {
            InitializeComponent();

            _appPath = appPath;
            _startService = startService;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            CreateTrayIcon();
            CloseWindow();

            _startService(_serviceCts.Token);
        }

        void CreateTrayIcon()
        {
            var contextMenu = new ContextMenuStrip();
            var trayIcon = new NotifyIcon(_trayContainer)
            {
                ContextMenuStrip = contextMenu,
                Icon = Icon.ExtractAssociatedIcon(_appPath),
                Text = "Tidy Desktop Monster",
                Visible = true,
            };

            contextMenu.Items.Add(new ToolStripLabel("Desktop monitoring is active.")
            {
                ForeColor = Color.DarkGray,
            });
            contextMenu.Items.Add("Exit", null, (sender, evt) => Close());
        }

        void CloseWindow()
        {
            WindowState = FormWindowState.Minimized;
            Visible = false;
            ShowInTaskbar = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _serviceCts.Cancel();
                _serviceCts.Dispose();
                _trayContainer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
