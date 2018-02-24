using System;
using System.Linq;
using System.Windows.Forms;
using TidyDesktopMonster.Interface;
using TidyDesktopMonster.Logging;

namespace TidyDesktopMonster
{
    internal partial class LogViewer : Form
    {
        readonly IUpdatingSubject<LogEntry> _logEntries;

        public LogViewer(IUpdatingSubject<LogEntry> logEntries)
        {
            InitializeComponent();

            _logEntries = logEntries;
        }

        void LogViewer_Load(object sender, EventArgs e)
        {
            UpdateLog();
            _logEntries.SubjectChanged += (o, e2) => UpdateLog();
        }

        void UpdateLog()
        {
            var lines = _logEntries.GetSubjects()
                .Select(x => $"{x.Timestamp.ToLocalTime():HH:mm:ss.fff}: {x.Message}");
            logTextBox.Text = string.Join(Environment.NewLine, lines);

            logTextBox.Select(logTextBox.Text.Length, 0);
            logTextBox.ScrollToCaret();
        }
    }
}
