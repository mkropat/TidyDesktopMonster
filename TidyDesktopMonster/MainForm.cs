using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TidyDesktopMonster
{
    public partial class MainForm : Form
    {
        readonly string _appPath;
        Container _trayContainer = new Container();

        public MainForm(string appPath)
        {
            InitializeComponent();

            _appPath = appPath;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            CreateTrayIcon();
            CloseWindow();
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
                _trayContainer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
