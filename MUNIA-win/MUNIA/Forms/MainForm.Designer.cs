using OpenTK;

namespace MUNIA.Forms {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menu = new System.Windows.Forms.MenuStrip();
			this.tsmiControllers = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSetWindowSize = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSetLagCompensation = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSkinFolders = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiDeviceConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiFirmwareUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiMapArduinoDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiManageSkinThemes = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiTestController = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCheckUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.glControl = new OpenTK.GLControl();
			this.status = new System.Windows.Forms.StatusStrip();
			this.lblSkins = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblFill = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.pbProgress = new System.Windows.Forms.ToolStripProgressBar();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.popup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiBackground = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiBackGroundColor = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiBackgroundTransparent = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiBackgroundNSpy = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSkinTheme = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiApplySkinTheme = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiApplyCustomTheme = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiManageThemes = new System.Windows.Forms.ToolStripMenuItem();
			this.menu.SuspendLayout();
			this.status.SuspendLayout();
			this.popup.SuspendLayout();
			this.SuspendLayout();
			// 
			// menu
			// 
			this.menu.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiControllers,
            this.tsmiOptions,
            this.tsmiHelp});
			this.menu.Location = new System.Drawing.Point(0, 0);
			this.menu.Name = "menu";
			this.menu.Size = new System.Drawing.Size(577, 24);
			this.menu.TabIndex = 1;
			this.menu.Text = "menuStrip1";
			// 
			// tsmiControllers
			// 
			this.tsmiControllers.Name = "tsmiControllers";
			this.tsmiControllers.Size = new System.Drawing.Size(104, 20);
			this.tsmiControllers.Text = "&Select controller";
			this.tsmiControllers.DropDownClosed += new System.EventHandler(this.tsmiControllers_DropDownClosed);
			// 
			// tsmiOptions
			// 
			this.tsmiOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetWindowSize,
            this.tsmiSetLagCompensation,
            this.tsmiSkinFolders,
            this.toolStripSeparator1,
            this.tsmiDeviceConfig,
            this.tsmiFirmwareUpdate,
            this.tsmiMapArduinoDevices,
            this.toolStripSeparator2,
            this.tsmiManageSkinThemes,
            this.tsmiTestController});
			this.tsmiOptions.Name = "tsmiOptions";
			this.tsmiOptions.Size = new System.Drawing.Size(61, 20);
			this.tsmiOptions.Text = "&Options";
			// 
			// tsmiSetWindowSize
			// 
			this.tsmiSetWindowSize.Name = "tsmiSetWindowSize";
			this.tsmiSetWindowSize.Size = new System.Drawing.Size(188, 22);
			this.tsmiSetWindowSize.Text = "Set &window size";
			this.tsmiSetWindowSize.Click += new System.EventHandler(this.tsmiSetWindowSize_Click);
			// 
			// tsmiSetLagCompensation
			// 
			this.tsmiSetLagCompensation.Name = "tsmiSetLagCompensation";
			this.tsmiSetLagCompensation.Size = new System.Drawing.Size(188, 22);
			this.tsmiSetLagCompensation.Text = "Set &lag compensation";
			this.tsmiSetLagCompensation.Click += new System.EventHandler(this.tsmiSetLagCompensation_Click);
			// 
			// tsmiSkinFolders
			// 
			this.tsmiSkinFolders.Name = "tsmiSkinFolders";
			this.tsmiSkinFolders.Size = new System.Drawing.Size(188, 22);
			this.tsmiSkinFolders.Text = "&Skin folders";
			this.tsmiSkinFolders.Click += new System.EventHandler(this.tsmiSkinFolders_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
			// 
			// tsmiDeviceConfig
			// 
			this.tsmiDeviceConfig.Name = "tsmiDeviceConfig";
			this.tsmiDeviceConfig.Size = new System.Drawing.Size(188, 22);
			this.tsmiDeviceConfig.Text = "Device &config";
			this.tsmiDeviceConfig.Click += new System.EventHandler(this.tsmiMuniaSettings_Click);
			// 
			// tsmiFirmwareUpdate
			// 
			this.tsmiFirmwareUpdate.Name = "tsmiFirmwareUpdate";
			this.tsmiFirmwareUpdate.Size = new System.Drawing.Size(188, 22);
			this.tsmiFirmwareUpdate.Text = "Firmware &update";
			this.tsmiFirmwareUpdate.Click += new System.EventHandler(this.tsmiFirmware_Click);
			// 
			// tsmiMapArduinoDevices
			// 
			this.tsmiMapArduinoDevices.Name = "tsmiMapArduinoDevices";
			this.tsmiMapArduinoDevices.Size = new System.Drawing.Size(188, 22);
			this.tsmiMapArduinoDevices.Text = "Map arduino devices";
			this.tsmiMapArduinoDevices.Click += new System.EventHandler(this.tsmiMapArduinoDevices_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
			// 
			// tsmiManageSkinThemes
			// 
			this.tsmiManageSkinThemes.Name = "tsmiManageSkinThemes";
			this.tsmiManageSkinThemes.Size = new System.Drawing.Size(188, 22);
			this.tsmiManageSkinThemes.Text = "Manage skin &themes";
			this.tsmiManageSkinThemes.Click += new System.EventHandler(this.tsmiManageThemes_Click);
			// 
			// tsmiTestController
			// 
			this.tsmiTestController.Name = "tsmiTestController";
			this.tsmiTestController.Size = new System.Drawing.Size(188, 22);
			this.tsmiTestController.Text = "&Test controllers";
			this.tsmiTestController.Click += new System.EventHandler(this.testControllerToolStripMenuItem_Click);
			// 
			// tsmiHelp
			// 
			this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCheckUpdates,
            this.tsmiAbout});
			this.tsmiHelp.Name = "tsmiHelp";
			this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
			this.tsmiHelp.Text = "&Help";
			// 
			// tsmiCheckUpdates
			// 
			this.tsmiCheckUpdates.Name = "tsmiCheckUpdates";
			this.tsmiCheckUpdates.Size = new System.Drawing.Size(170, 22);
			this.tsmiCheckUpdates.Text = "&Check for updates";
			this.tsmiCheckUpdates.Click += new System.EventHandler(this.tsmiCheckUpdates_Click);
			// 
			// tsmiAbout
			// 
			this.tsmiAbout.Name = "tsmiAbout";
			this.tsmiAbout.Size = new System.Drawing.Size(170, 22);
			this.tsmiAbout.Text = "&About";
			this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
			// 
			// glControl
			// 
			this.glControl.BackColor = System.Drawing.Color.Black;
			this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl.Location = new System.Drawing.Point(0, 24);
			this.glControl.Name = "glControl";
			this.glControl.Size = new System.Drawing.Size(577, 336);
			this.glControl.TabIndex = 0;
			this.tooltip.SetToolTip(this.glControl, "Right click for theming options.");
			this.glControl.VSync = false;
			this.glControl.Load += new System.EventHandler(this.glControl_Load);
			this.glControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseClick);
			// 
			// status
			// 
			this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSkins,
            this.lblFill,
            this.lblStatus,
            this.pbProgress});
			this.status.Location = new System.Drawing.Point(0, 360);
			this.status.Name = "status";
			this.status.Size = new System.Drawing.Size(577, 22);
			this.status.TabIndex = 2;
			this.status.Text = "statusStrip";
			// 
			// lblSkins
			// 
			this.lblSkins.Name = "lblSkins";
			this.lblSkins.Size = new System.Drawing.Size(0, 17);
			// 
			// lblFill
			// 
			this.lblFill.Name = "lblFill";
			this.lblFill.Size = new System.Drawing.Size(418, 17);
			this.lblFill.Spring = true;
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(42, 17);
			this.lblStatus.Text = "Status:";
			// 
			// pbProgress
			// 
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(100, 16);
			this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// tooltip
			// 
			this.tooltip.AutomaticDelay = 100;
			this.tooltip.AutoPopDelay = 15000;
			this.tooltip.InitialDelay = 100;
			this.tooltip.ReshowDelay = 20;
			// 
			// popup
			// 
			this.popup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBackground,
            this.tsmiBackgroundNSpy,
            this.tsmiSkinTheme});
			this.popup.Name = "popup";
			this.popup.Size = new System.Drawing.Size(181, 92);
			this.popup.Opening += new System.ComponentModel.CancelEventHandler(this.popup_Opening);
			// 
			// tsmiBackground
			// 
			this.tsmiBackground.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBackGroundColor,
            this.tsmiBackgroundTransparent});
			this.tsmiBackground.Name = "tsmiBackground";
			this.tsmiBackground.Size = new System.Drawing.Size(180, 22);
			this.tsmiBackground.Text = "&Background color";
			// 
			// tsmiBackGroundColor
			// 
			this.tsmiBackGroundColor.Name = "tsmiBackGroundColor";
			this.tsmiBackGroundColor.Size = new System.Drawing.Size(180, 22);
			this.tsmiBackGroundColor.Text = "&Change";
			this.tsmiBackGroundColor.Click += new System.EventHandler(this.tsmiBackgroundChange_Click);
			// 
			// tsmiBackgroundTransparent
			// 
			this.tsmiBackgroundTransparent.Checked = true;
			this.tsmiBackgroundTransparent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiBackgroundTransparent.Name = "tsmiBackgroundTransparent";
			this.tsmiBackgroundTransparent.Size = new System.Drawing.Size(180, 22);
			this.tsmiBackgroundTransparent.Text = "&Transparent";
			this.tsmiBackgroundTransparent.ToolTipText = "If checked, background will have an alpha channel of 0 allowing OBS to blend the " +
    "background with overlapping content.\r\nIn your OBS scene, use a game capture sour" +
    "ce with transparency enabled.";
			this.tsmiBackgroundTransparent.Click += new System.EventHandler(this.tsmiBackgroundTransparent_Click);
			// 
			// tsmiBackgroundNSpy
			// 
			this.tsmiBackgroundNSpy.Name = "tsmiBackgroundNSpy";
			this.tsmiBackgroundNSpy.Size = new System.Drawing.Size(180, 22);
			this.tsmiBackgroundNSpy.Text = "&Background";
			// 
			// tsmiSkinTheme
			// 
			this.tsmiSkinTheme.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiApplySkinTheme,
            this.tsmiApplyCustomTheme,
            this.tsmiManageThemes});
			this.tsmiSkinTheme.Name = "tsmiSkinTheme";
			this.tsmiSkinTheme.Size = new System.Drawing.Size(180, 22);
			this.tsmiSkinTheme.Text = "Skin &theme";
			// 
			// tsmiApplySkinTheme
			// 
			this.tsmiApplySkinTheme.Name = "tsmiApplySkinTheme";
			this.tsmiApplySkinTheme.Size = new System.Drawing.Size(185, 22);
			this.tsmiApplySkinTheme.Text = "&Apply skin theme";
			// 
			// tsmiApplyCustomTheme
			// 
			this.tsmiApplyCustomTheme.Name = "tsmiApplyCustomTheme";
			this.tsmiApplyCustomTheme.Size = new System.Drawing.Size(185, 22);
			this.tsmiApplyCustomTheme.Text = "Apply &custom theme";
			// 
			// tsmiManageThemes
			// 
			this.tsmiManageThemes.Name = "tsmiManageThemes";
			this.tsmiManageThemes.Size = new System.Drawing.Size(185, 22);
			this.tsmiManageThemes.Text = "&Manage themes";
			this.tsmiManageThemes.Click += new System.EventHandler(this.tsmiManageThemes_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(577, 382);
			this.Controls.Add(this.glControl);
			this.Controls.Add(this.status);
			this.Controls.Add(this.menu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menu;
			this.Name = "MainForm";
			this.Text = "MUNIA";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.menu.ResumeLayout(false);
			this.menu.PerformLayout();
			this.status.ResumeLayout(false);
			this.status.PerformLayout();
			this.popup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem tsmiControllers;
        private GLControl glControl;
		private System.Windows.Forms.ToolStripMenuItem tsmiOptions;
		private System.Windows.Forms.ToolStripMenuItem tsmiSetWindowSize;
		private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
		private System.Windows.Forms.ToolStripMenuItem tsmiCheckUpdates;
		private System.Windows.Forms.ToolStripMenuItem tsmiAbout;
		private System.Windows.Forms.StatusStrip status;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblFill;
		private System.Windows.Forms.ToolStripProgressBar pbProgress;
		private System.Windows.Forms.ToolStripStatusLabel lblSkins;
		private System.Windows.Forms.ToolStripMenuItem tsmiDeviceConfig;
		private System.Windows.Forms.ToolStripMenuItem tsmiFirmwareUpdate;
		private System.Windows.Forms.ToolStripMenuItem tsmiSetLagCompensation;
		private System.Windows.Forms.ToolStripMenuItem tsmiMapArduinoDevices;
		private System.Windows.Forms.ToolTip tooltip;
		private System.Windows.Forms.ContextMenuStrip popup;
		private System.Windows.Forms.ToolStripMenuItem tsmiBackgroundNSpy;
		private System.Windows.Forms.ToolStripMenuItem tsmiSkinTheme;
		private System.Windows.Forms.ToolStripMenuItem tsmiManageThemes;
		private System.Windows.Forms.ToolStripMenuItem tsmiApplyCustomTheme;
		private System.Windows.Forms.ToolStripMenuItem tsmiSkinFolders;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem tsmiManageSkinThemes;
		private System.Windows.Forms.ToolStripMenuItem tsmiBackground;
		private System.Windows.Forms.ToolStripMenuItem tsmiBackGroundColor;
		private System.Windows.Forms.ToolStripMenuItem tsmiBackgroundTransparent;
		private System.Windows.Forms.ToolStripMenuItem tsmiTestController;
		private System.Windows.Forms.ToolStripMenuItem tsmiApplySkinTheme;
	}
}

