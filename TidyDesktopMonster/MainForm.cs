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
        readonly IKeyValueStore _settingsStore;
        readonly bool _showSettingsForm;
        readonly Func<CancellationToken, Task> _startService;
        readonly IStartupRegistration _startupRegistration;
        Container _trayContainer = new Container();

        bool ExistsTrayIcon => _trayContainer.Components.Count > 0;

        public MainForm(bool showSettingsForm, string appPath, int openWindowMessage, IKeyValueStore settingsStore, Func<CancellationToken, Task> startService, IStartupRegistration startupRegistration)
        {
            InitializeComponent();

            _appPath = appPath;
            _openWindowMessage = openWindowMessage;
            _settingsStore = settingsStore;
            _showSettingsForm = showSettingsForm;
            _startService = startService;
            _startupRegistration = startupRegistration;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            TidyAllUsers.Checked = _settingsStore.Read<bool?>(Constants.TidyAllUsersSetting) ?? true;
            RunOnStartup.Checked = _startupRegistration.RunOnStartup;

            ShortcutFilter.ValueMember = "Item1";
            ShortcutFilter.DisplayMember = "Item2";
            ShortcutFilter.DataSource = new[]
            {
                Tuple.Create(ShortcutFilterType.All, "All Shortcuts"),
                Tuple.Create(ShortcutFilterType.Apps, "App Shortcuts"),
            };

            ShortcutFilter.SelectedValue = _settingsStore.Read<ShortcutFilterType?>(Constants.ShortcutFilterSetting) ?? ShortcutFilterType.Apps;
            ShortcutFilter.SelectedIndexChanged += ShortcutFilter_SelectedIndexChanged;

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
                    ShortcutFilter.Enabled = false;
                    TidyAllUsers.Enabled = false;
                    ToggleService.Enabled = true;
                    ToggleService.Text = "Stop Tidying Desktop";
                    ServiceStatusText.Text = "Service is running.";
                    break;

                case ServiceState.Stopping:
                    ShortcutFilter.Enabled = false;
                    TidyAllUsers.Enabled = false;
                    ToggleService.Enabled = false;
                    ToggleService.Text = "Stop Tidying Desktop";
                    ServiceStatusText.Text = "Stopping the service.";
                    break;

                case ServiceState.Stopped:
                    ShortcutFilter.Enabled = true;
                    TidyAllUsers.Enabled = true;
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

        void TidyAllUsers_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            _settingsStore.Write(Constants.TidyAllUsersSetting, checkbox.Checked);
        }

        void RunOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            _startupRegistration.RunOnStartup = checkbox.Checked;
        }

        void ShortcutFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var type = (ShortcutFilterType)comboBox.SelectedValue;
            _settingsStore.Write(Constants.ShortcutFilterSetting, type);
        }

        void ToggleService_Click(object sender, EventArgs e)
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
