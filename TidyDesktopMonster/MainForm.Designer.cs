﻿namespace TidyDesktopMonster
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.ToggleService = new System.Windows.Forms.Button();
			this.ServiceStatusText = new System.Windows.Forms.Label();
			this.RunOnStartup = new System.Windows.Forms.CheckBox();
			this.TidyAllUsers = new System.Windows.Forms.CheckBox();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.ShortcutFilter = new System.Windows.Forms.ComboBox();
			this.SkipRecycleBin = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.ToggleService, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.ServiceStatusText, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.RunOnStartup, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.TidyAllUsers, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.SkipRecycleBin, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 8;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(731, 483);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
			this.label1.Location = new System.Drawing.Point(61, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Padding = new System.Windows.Forms.Padding(0, 19, 0, 19);
			this.label1.Size = new System.Drawing.Size(609, 100);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tidy Desktop Monster watches your desktop and auto-deletes any shortcuts that get" +
    " created.";
			// 
			// ToggleService
			// 
			this.ToggleService.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.ToggleService.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ToggleService.Location = new System.Drawing.Point(168, 330);
			this.ToggleService.Margin = new System.Windows.Forms.Padding(4);
			this.ToggleService.Name = "ToggleService";
			this.ToggleService.Size = new System.Drawing.Size(395, 75);
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
			this.ServiceStatusText.Location = new System.Drawing.Point(266, 409);
			this.ServiceStatusText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ServiceStatusText.Name = "ServiceStatusText";
			this.ServiceStatusText.Size = new System.Drawing.Size(198, 25);
			this.ServiceStatusText.TabIndex = 2;
			this.ServiceStatusText.Text = "[service status text]";
			// 
			// RunOnStartup
			// 
			this.RunOnStartup.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.RunOnStartup.AutoSize = true;
			this.RunOnStartup.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RunOnStartup.Location = new System.Drawing.Point(246, 104);
			this.RunOnStartup.Margin = new System.Windows.Forms.Padding(4);
			this.RunOnStartup.Name = "RunOnStartup";
			this.RunOnStartup.Padding = new System.Windows.Forms.Padding(0, 11, 0, 0);
			this.RunOnStartup.Size = new System.Drawing.Size(239, 46);
			this.RunOnStartup.TabIndex = 3;
			this.RunOnStartup.Text = "Run on startup?";
			this.RunOnStartup.UseVisualStyleBackColor = true;
			// 
			// TidyAllUsers
			// 
			this.TidyAllUsers.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.TidyAllUsers.AutoSize = true;
			this.TidyAllUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TidyAllUsers.Location = new System.Drawing.Point(178, 160);
			this.TidyAllUsers.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
			this.TidyAllUsers.Name = "TidyAllUsers";
			this.TidyAllUsers.Size = new System.Drawing.Size(375, 34);
			this.TidyAllUsers.TabIndex = 4;
			this.TidyAllUsers.Text = "Tidy \"All Users\" desktop too?";
			this.TidyAllUsers.UseVisualStyleBackColor = true;
			this.TidyAllUsers.CheckedChanged += new System.EventHandler(this.TidyAllUsers_CheckedChanged);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.label2);
			this.flowLayoutPanel1.Controls.Add(this.ShortcutFilter);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(162, 250);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 25);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(406, 72);
			this.flowLayoutPanel1.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(4, 8);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(101, 31);
			this.label2.TabIndex = 0;
			this.label2.Text = "Delete:";
			// 
			// ShortcutFilter
			// 
			this.ShortcutFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ShortcutFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ShortcutFilter.FormattingEnabled = true;
			this.ShortcutFilter.Location = new System.Drawing.Point(113, 4);
			this.ShortcutFilter.Margin = new System.Windows.Forms.Padding(4);
			this.ShortcutFilter.Name = "ShortcutFilter";
			this.ShortcutFilter.Size = new System.Drawing.Size(289, 39);
			this.ShortcutFilter.TabIndex = 1;
			// 
			// SkipRecycleBin
			// 
			this.SkipRecycleBin.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.SkipRecycleBin.AutoSize = true;
			this.SkipRecycleBin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SkipRecycleBin.Location = new System.Drawing.Point(239, 206);
			this.SkipRecycleBin.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
			this.SkipRecycleBin.Name = "SkipRecycleBin";
			this.SkipRecycleBin.Size = new System.Drawing.Size(252, 34);
			this.SkipRecycleBin.TabIndex = 6;
			this.SkipRecycleBin.Text = "Skip Recycle Bin?";
			this.SkipRecycleBin.UseVisualStyleBackColor = true;
			this.SkipRecycleBin.CheckedChanged += new System.EventHandler(this.SkipRecycleBin_CheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(731, 483);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Tidy Desktop Monster";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ToggleService;
        private System.Windows.Forms.Label ServiceStatusText;
        private System.Windows.Forms.CheckBox RunOnStartup;
        private System.Windows.Forms.CheckBox TidyAllUsers;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ShortcutFilter;
		private System.Windows.Forms.CheckBox SkipRecycleBin;
	}
}

