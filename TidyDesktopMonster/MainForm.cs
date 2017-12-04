using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TidyDesktopMonster.Interface;

namespace TidyDesktopMonster
{
    public partial class MainForm : Form
    {
        readonly string _appPath;
        readonly int _openWindowMessage;
        CancellationTokenSource _serviceCts = new CancellationTokenSource();
        Task _serviceTask = Task.FromResult<object>(null);
        readonly bool _showSettingsForm;
        readonly Func<CancellationToken, Task> _startService;
        readonly IStartupRegistration _startupRegistration;
        Container _trayContainer = new Container();

        bool ExistsTrayIcon => _trayContainer.Components.Count > 0;

        public MainForm(bool showSettingsForm, string appPath, int openWindowMessage, Func<CancellationToken, Task> startService, IStartupRegistration startupRegistration)
        {
            InitializeComponent();

            _appPath = appPath;
            _openWindowMessage = openWindowMessage;
            _showSettingsForm = showSettingsForm;
            _startService = startService;
            _startupRegistration = startupRegistration;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            RunOnStartup.Checked = _startupRegistration.RunOnStartup;

            SetServiceState(ServiceState.Stopped);

            if (!_showSettingsForm)
            {
                var fireAndForgetTask = RunStartService();
            }
        }

        void SetServiceState(ServiceState state)
        {
            switch (state)
            {
                case ServiceState.Started:
                    ToggleService.Enabled = true;
                    ToggleService.Text = "Stop Tidying Desktop";
                    ServiceStatusText.Text = "Service is running.";
                    break;

                case ServiceState.Stopping:
                    ToggleService.Enabled = false;
                    ToggleService.Text = "Stop Tidying Desktop";
                    ServiceStatusText.Text = "Stopping the service.";
                    break;

                case ServiceState.Stopped:
                    ToggleService.Enabled = true;
                    ToggleService.Text = "Start Tidying Desktop";
                    ServiceStatusText.Text = string.Empty;
                    break;

                default:
                    throw new ArgumentException("Unhandled case", "state");
            }
        }

        async Task RunStartService()
        {
            CreateTrayIcon();
            CloseWindow();
            SetServiceState(ServiceState.Started);

            await _startService(_serviceCts.Token);

            _trayContainer.Dispose();
            _trayContainer = new Container();

            SetServiceState(ServiceState.Stopped);
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
            trayIcon.DoubleClick += (sender, evt) => OpenWindow();

            contextMenu.Items.Add(new ToolStripLabel("Desktop monitoring is active.")
            {
                ForeColor = Color.DarkGray,
            });
            contextMenu.Items.Add("Open Settings", null, (sender, evt) => OpenWindow());
            contextMenu.Items.Add("Exit", null, (sender, evt) => Close());
        }

        void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && ExistsTrayIcon)
                CloseWindow();
        }

        void RunOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            _startupRegistration.RunOnStartup = checkbox.Checked;
        }

        private void ToggleService_Click(object sender, EventArgs e)
        {
            if (_serviceTask.IsCompleted)
            {
                _serviceCts.Dispose();
                _serviceCts = new CancellationTokenSource();
                _serviceTask = RunStartService();
            }
            else
            {
                SetServiceState(ServiceState.Stopping);
                _serviceCts.Cancel();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == _openWindowMessage)
                OpenWindow();

            base.WndProc(ref m);
        }

        void OpenWindow()
        {
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();
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

        enum ServiceState
        {
            Started,
            Stopping,
            Stopped,
        }
    }
}
