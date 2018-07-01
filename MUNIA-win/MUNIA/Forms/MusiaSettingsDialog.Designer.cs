namespace MUNIA.Forms {
	partial class MusiaSettingsDialog {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MusiaSettingsDialog));
			this.lblDeviceType = new System.Windows.Forms.Label();
			this.gbMunia = new System.Windows.Forms.GroupBox();
			this.tbMCUId = new System.Windows.Forms.TextBox();
			this.lblDeviceSerial = new System.Windows.Forms.Label();
			this.tbHardware = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbFirmware = new System.Windows.Forms.TextBox();
			this.lblFirmwareVersion = new System.Windows.Forms.Label();
			this.gbSettings = new System.Windows.Forms.GroupBox();
			this.lblPollingFrequency = new System.Windows.Forms.Label();
			this.ckbRumble = new System.Windows.Forms.CheckBox();
			this.cbPollingFrequency = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.rbOutputPC = new System.Windows.Forms.RadioButton();
			this.rbOutputPS2 = new System.Windows.Forms.RadioButton();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnAccept = new System.Windows.Forms.Button();
			this.tbMicroController = new System.Windows.Forms.TextBox();
			this.lblMicrocontroller = new System.Windows.Forms.Label();
			this.gbMunia.SuspendLayout();
			this.gbSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblDeviceType
			// 
			this.lblDeviceType.AutoSize = true;
			this.lblDeviceType.Location = new System.Drawing.Point(24, 20);
			this.lblDeviceType.Name = "lblDeviceType";
			this.lblDeviceType.Size = new System.Drawing.Size(70, 13);
			this.lblDeviceType.TabIndex = 16;
			this.lblDeviceType.Text = "Device type: ";
			// 
			// gbMunia
			// 
			this.gbMunia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbMunia.Controls.Add(this.tbMicroController);
			this.gbMunia.Controls.Add(this.lblMicrocontroller);
			this.gbMunia.Controls.Add(this.lblDeviceType);
			this.gbMunia.Controls.Add(this.tbMCUId);
			this.gbMunia.Controls.Add(this.lblDeviceSerial);
			this.gbMunia.Controls.Add(this.tbHardware);
			this.gbMunia.Controls.Add(this.label2);
			this.gbMunia.Controls.Add(this.tbFirmware);
			this.gbMunia.Controls.Add(this.lblFirmwareVersion);
			this.gbMunia.Controls.Add(this.gbSettings);
			this.gbMunia.Location = new System.Drawing.Point(12, 12);
			this.gbMunia.Name = "gbMunia";
			this.gbMunia.Size = new System.Drawing.Size(312, 225);
			this.gbMunia.TabIndex = 6;
			this.gbMunia.TabStop = false;
			this.gbMunia.Text = "MUNIA Info";
			// 
			// tbMCUId
			// 
			this.tbMCUId.Enabled = false;
			this.tbMCUId.Location = new System.Drawing.Point(113, 62);
			this.tbMCUId.Name = "tbMCUId";
			this.tbMCUId.ReadOnly = true;
			this.tbMCUId.Size = new System.Drawing.Size(179, 20);
			this.tbMCUId.TabIndex = 8;
			// 
			// lblDeviceSerial
			// 
			this.lblDeviceSerial.AutoSize = true;
			this.lblDeviceSerial.Location = new System.Drawing.Point(21, 65);
			this.lblDeviceSerial.Name = "lblDeviceSerial";
			this.lblDeviceSerial.Size = new System.Drawing.Size(68, 13);
			this.lblDeviceSerial.TabIndex = 7;
			this.lblDeviceSerial.Text = "Device serial";
			// 
			// tbHardware
			// 
			this.tbHardware.Enabled = false;
			this.tbHardware.Location = new System.Drawing.Point(242, 40);
			this.tbHardware.Name = "tbHardware";
			this.tbHardware.ReadOnly = true;
			this.tbHardware.Size = new System.Drawing.Size(50, 20);
			this.tbHardware.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(171, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "HW revision";
			// 
			// tbFirmware
			// 
			this.tbFirmware.Enabled = false;
			this.tbFirmware.Location = new System.Drawing.Point(113, 40);
			this.tbFirmware.Name = "tbFirmware";
			this.tbFirmware.ReadOnly = true;
			this.tbFirmware.Size = new System.Drawing.Size(49, 20);
			this.tbFirmware.TabIndex = 1;
			// 
			// lblFirmwareVersion
			// 
			this.lblFirmwareVersion.AutoSize = true;
			this.lblFirmwareVersion.Location = new System.Drawing.Point(21, 43);
			this.lblFirmwareVersion.Name = "lblFirmwareVersion";
			this.lblFirmwareVersion.Size = new System.Drawing.Size(86, 13);
			this.lblFirmwareVersion.TabIndex = 0;
			this.lblFirmwareVersion.Text = "Firmware version";
			// 
			// gbSettings
			// 
			this.gbSettings.Controls.Add(this.lblPollingFrequency);
			this.gbSettings.Controls.Add(this.ckbRumble);
			this.gbSettings.Controls.Add(this.cbPollingFrequency);
			this.gbSettings.Controls.Add(this.label3);
			this.gbSettings.Controls.Add(this.rbOutputPC);
			this.gbSettings.Controls.Add(this.rbOutputPS2);
			this.gbSettings.Location = new System.Drawing.Point(15, 118);
			this.gbSettings.Name = "gbSettings";
			this.gbSettings.Size = new System.Drawing.Size(291, 89);
			this.gbSettings.TabIndex = 15;
			this.gbSettings.TabStop = false;
			this.gbSettings.Text = "Settings";
			// 
			// lblPollingFrequency
			// 
			this.lblPollingFrequency.AutoSize = true;
			this.lblPollingFrequency.Location = new System.Drawing.Point(15, 40);
			this.lblPollingFrequency.Name = "lblPollingFrequency";
			this.lblPollingFrequency.Size = new System.Drawing.Size(88, 13);
			this.lblPollingFrequency.TabIndex = 8;
			this.lblPollingFrequency.Text = "Polling frequency";
			// 
			// ckbRumble
			// 
			this.ckbRumble.AutoSize = true;
			this.ckbRumble.Location = new System.Drawing.Point(18, 64);
			this.ckbRumble.Name = "ckbRumble";
			this.ckbRumble.Size = new System.Drawing.Size(164, 17);
			this.ckbRumble.TabIndex = 7;
			this.ckbRumble.Text = "Vibration feedback over USB";
			this.ckbRumble.UseVisualStyleBackColor = true;
			// 
			// cbPollingFrequency
			// 
			this.cbPollingFrequency.FormattingEnabled = true;
			this.cbPollingFrequency.Items.AddRange(new object[] {
            "120 Hz",
            "100 Hz",
            "60 Hz",
            "50 Hz",
            "30 Hz",
            "25 Hz"});
			this.cbPollingFrequency.Location = new System.Drawing.Point(122, 37);
			this.cbPollingFrequency.Name = "cbPollingFrequency";
			this.cbPollingFrequency.Size = new System.Drawing.Size(121, 21);
			this.cbPollingFrequency.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Output to";
			// 
			// rbOutputPC
			// 
			this.rbOutputPC.AutoSize = true;
			this.rbOutputPC.Location = new System.Drawing.Point(204, 16);
			this.rbOutputPC.Name = "rbOutputPC";
			this.rbOutputPC.Size = new System.Drawing.Size(39, 17);
			this.rbOutputPC.TabIndex = 5;
			this.rbOutputPC.Text = "PC";
			this.rbOutputPC.UseVisualStyleBackColor = true;
			// 
			// rbOutputPS2
			// 
			this.rbOutputPS2.AutoSize = true;
			this.rbOutputPS2.Location = new System.Drawing.Point(122, 16);
			this.rbOutputPS2.Name = "rbOutputPS2";
			this.rbOutputPS2.Size = new System.Drawing.Size(45, 17);
			this.rbOutputPS2.TabIndex = 4;
			this.rbOutputPS2.Text = "PS2";
			this.rbOutputPS2.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(168, 242);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnAccept
			// 
			this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Location = new System.Drawing.Point(249, 242);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 8;
			this.btnAccept.Text = "&Apply";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			// 
			// tbMicroController
			// 
			this.tbMicroController.Enabled = false;
			this.tbMicroController.Location = new System.Drawing.Point(113, 84);
			this.tbMicroController.Name = "tbMicroController";
			this.tbMicroController.ReadOnly = true;
			this.tbMicroController.Size = new System.Drawing.Size(179, 20);
			this.tbMicroController.TabIndex = 18;
			// 
			// lblMicrocontroller
			// 
			this.lblMicrocontroller.AutoSize = true;
			this.lblMicrocontroller.Location = new System.Drawing.Point(21, 87);
			this.lblMicrocontroller.Name = "lblMicrocontroller";
			this.lblMicrocontroller.Size = new System.Drawing.Size(76, 13);
			this.lblMicrocontroller.TabIndex = 17;
			this.lblMicrocontroller.Text = "Microcontroller";
			// 
			// MusiaSettingsDialog
			// 
			this.AcceptButton = this.btnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(336, 277);
			this.Controls.Add(this.gbMunia);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(352, 292);
			this.Name = "MusiaSettingsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MUSIA Device Configuration";
			this.gbMunia.ResumeLayout(false);
			this.gbMunia.PerformLayout();
			this.gbSettings.ResumeLayout(false);
			this.gbSettings.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label lblDeviceType;
		private System.Windows.Forms.GroupBox gbMunia;
		private System.Windows.Forms.TextBox tbMCUId;
		private System.Windows.Forms.Label lblDeviceSerial;
		private System.Windows.Forms.TextBox tbHardware;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbFirmware;
		private System.Windows.Forms.Label lblFirmwareVersion;
		private System.Windows.Forms.GroupBox gbSettings;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.CheckBox ckbRumble;
		private System.Windows.Forms.ComboBox cbPollingFrequency;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton rbOutputPC;
		private System.Windows.Forms.RadioButton rbOutputPS2;
		private System.Windows.Forms.Label lblPollingFrequency;
		private System.Windows.Forms.TextBox tbMicroController;
		private System.Windows.Forms.Label lblMicrocontroller;
	}
}