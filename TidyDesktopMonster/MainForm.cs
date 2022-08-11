using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;
using TidyDesktopMonster.WinApi;

namespace TidyDesktopMonster
{
    public partial class MainForm : Form
    {
        const uint viewLogCommandId = 0x0001;

        readonly string _appPath;
        readonly int _openWindowMessage;
        Form _logViewer = null;
        readonly IUpdatingSubject<LogEntry> _logEntries;
        CancellationTokenSource _serviceCts = new CancellationTokenSource();
        Task _serviceTask = Task.FromResult<object>(null);
        readonly IKeyValueStore _settingsStore;
        readonly bool _showSettingsForm;
        readonly Func<CancellationToken, Task> _startService;
        readonly IStartupRegistration _startupRegistration;
        readonly SystemMenuManager _systemMenu;
        Container _trayContainer = new Container();

        bool ExistsTrayIcon => _trayContainer.Components.Count > 0;

        public MainForm(bool showSettingsForm, string appPath, IUpdatingSubject<LogEntry> logEntries, int openWindowMessage, IKeyValueStore settingsStore, Func<CancellationToken, Task> startService, IStartupRegistration startupRegistration)
        {
            InitializeComponent();

            _appPath = appPath;
            _logEntries = logEntries;
            _openWindowMessage = openWindowMessage;
            _settingsStore = settingsStore;
            _showSettingsForm = showSettingsForm;
            _startService = startService;
            _startupRegistration = startupRegistration;
            _systemMenu = new SystemMenuManager(() => Handle);
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            _systemMenu.EnsureAdded();

            TidyAllUsers.Checked = _settingsStore.Read<bool?>(Constants.TidyAllUsersSetting) ?? true;

            RunOnStartup.Checked = _startupRegistration.RunOnStartup;
            RunOnStartup.CheckedChanged += RunOnStartup_CheckedChanged;

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
                _serviceTask = RunStartService();
            }
        }

        void SetServiceState(ServiceState state, string statusMessage="")
        {
            switch (state)
            {
                case ServiceState.Started:
                    ShortcutFilter.Enabled = false;
                    TidyAllUsers.Enabled = false;
                    SkipRecycleBin.Enabled = false;
                    ToggleService.Enabled = true;
                    ToggleService.Text = "Stop Tidying Desktop";
                    ServiceStatusText.ForeColor = SystemColors.WindowText;
                    ServiceStatusText.Text = "Service is running.";
                    break;

                case ServiceState.Stopping:
                    ShortcutFilter.Enabled = false;
                    TidyAllUsers.Enabled = false;
                    SkipRecycleBin.Enabled = false;
                    ToggleService.Enabled = false;
                    ToggleService.Text = "Stop Tidying Desktop";
                    ServiceStatusText.ForeColor = SystemColors.WindowText;
                    ServiceStatusText.Text = "Stopping the service.";
                    break;

                case ServiceState.Stopped:
                    ShortcutFilter.Enabled = true;
                    TidyAllUsers.Enabled = true;
                    SkipRecycleBin.Enabled = true;
                    ToggleService.Enabled = true;
                    ToggleService.Text = "Start Tidying Desktop";
                    ServiceStatusText.ForeColor = SystemColors.WindowText;
                    ServiceStatusText.Text = statusMessage;
                    break;

                case ServiceState.Errored:
                    ShortcutFilter.Enabled = true;
                    TidyAllUsers.Enabled = true;
                    ToggleService.Enabled = true;
                    ToggleService.Text = "Start Tidying Desktop";
                    ServiceStatusText.ForeColor = Color.Red;
                    ServiceStatusText.Text = statusMessage;
                    break;

                default:
                    throw new ArgumentException("Unhandled case", "state");
            }
        }

        async Task RunStartService()
        {
            Log.Info("Starting background service");

            CreateTrayIcon();
            CloseWindow();
            SetServiceState(ServiceState.Started);

            try
            {
                await _startService(_serviceCts.Token);
                Log.Info("Background service stopped");

                SetServiceState(ServiceState.Stopped);
            }
            catch (Exception ex)
            {
                Log.Error("Background service terminated with error", ex);
                SetServiceState(ServiceState.Errored, ex.Message);
            }

            if (IsInBackground)
                OpenWindow();

            _trayContainer.Dispose();
            _trayContainer = new Container();
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
                ForeColor = SystemColors.GrayText,
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

        private void SkipRecycleBin_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            _settingsStore.Write(Constants.SkipRecycleBinSetting, checkbox.Checked);
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
            const int WM_SYSCOMMAND = 0x0112;

            if (m.Msg == WM_SYSCOMMAND)
            {
                switch ((uint)m.WParam)
                {
                    case viewLogCommandId:
                        OpenLogViewer();
                        break;
                }
            }
            else if (m.Msg == _openWindowMessage)
                OpenWindow();

            base.WndProc(ref m);
        }

        void OpenLogViewer()
        {
            if (_logViewer != null)
                return;

            _logViewer = new LogViewer(_logEntries)
            {
                Owner = this,
            };
            _logViewer.Show();

            _logViewer.FormClosed += (o, e) => _logViewer = null;
        }

        bool IsInBackground => !TopLevel;

        void OpenWindow()
        {
            TopLevel = true;
            Visible = true;
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Activate();

            _systemMenu.EnsureAdded();
        }

        void CloseWindow()
        {
            WindowState = FormWindowState.Minimized;
            Visible = false;
            ShowInTaskbar = false;
            TopLevel = false;

            _systemMenu.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _logViewer?.Dispose();
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
            Errored,
        }

        class SystemMenuManager
        {
            bool _added;
            readonly Func<IntPtr> _getHandle;

            public SystemMenuManager(Func<IntPtr> getHandle)
            {
                _getHandle = getHandle;
                Reset();
            }

            public void EnsureAdded()
            {
                if (_added)
                    return;

                var sysMenu = new SystemMenu(_getHandle());
                sysMenu.PrependItem(viewLogCommandId, "&View Logs");
                _added = true;
            }

            public void Reset()
            {
                _added = false;
            }
        }
	}
}
