namespace TidyDesktopMonster
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ToggleService = new System.Windows.Forms.Button();
            this.ServiceStatusText = new System.Windows.Forms.Label();
            this.RunOnStartup = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ToggleService, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ServiceStatusText, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.RunOnStartup, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(548, 248);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label1.Location = new System.Drawing.Point(54, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 15, 0, 15);
            this.label1.Size = new System.Drawing.Size(440, 80);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tidy Desktop Monster watches your desktop and auto-deletes any shortcuts that get" +
    " created.";
            // 
            // ToggleService
            // 
            this.ToggleService.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ToggleService.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToggleService.Location = new System.Drawing.Point(126, 143);
            this.ToggleService.Name = "ToggleService";
            this.ToggleService.Size = new System.Drawing.Size(295, 60);
            this.ToggleService.TabIndex = 1;
            this.ToggleService.Text = "[toggle service text]";
            this.ToggleService.UseVisualStyleBackColor = true;
            this.ToggleService.Click += new System.EventHandler(this.ToggleService_Click);
            // 
            // ServiceStatusText
            // 
            this.ServiceStatusText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ServiceStatusText.AutoSize = true;
            this.ServiceStatusText.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ServiceStatusText.Location = new System.Drawing.Point(202, 206);
            this.ServiceStatusText.Name = "ServiceStatusText";
            this.ServiceStatusText.Size = new System.Drawing.Size(144, 20);
            this.ServiceStatusText.TabIndex = 2;
            this.ServiceStatusText.Text = "[service status text]";
            // 
            // RunOnStartup
            // 
            this.RunOnStartup.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RunOnStartup.AutoSize = true;
            this.RunOnStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunOnStartup.Location = new System.Drawing.Point(186, 83);
            this.RunOnStartup.Name = "RunOnStartup";
            this.RunOnStartup.Padding = new System.Windows.Forms.Padding(0, 10, 0, 15);
            this.RunOnStartup.Size = new System.Drawing.Size(175, 54);
            this.RunOnStartup.TabIndex = 3;
            this.RunOnStartup.Text = "Run on startup?";
            this.RunOnStartup.UseVisualStyleBackColor = true;
            this.RunOnStartup.CheckedChanged += new System.EventHandler(this.RunOnStartup_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 248);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Tidy Desktop Monster";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ToggleService;
        private System.Windows.Forms.Label ServiceStatusText;
        private System.Windows.Forms.CheckBox RunOnStartup;
    }
}

