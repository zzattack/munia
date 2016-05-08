namespace MUNIA {
	partial class MuniaSettingsDialog {
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
			this.gbMunia = new System.Windows.Forms.GroupBox();
			this.tbMCURevision = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbMCUId = new System.Windows.Forms.TextBox();
			this.lblMicrocontroller = new System.Windows.Forms.Label();
			this.tbHardware = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbFirmware = new System.Windows.Forms.TextBox();
			this.pnlNGC = new System.Windows.Forms.Panel();
			this.rbNgcPC = new System.Windows.Forms.RadioButton();
			this.lblNGCMode = new System.Windows.Forms.Label();
			this.rbNgcConsole = new System.Windows.Forms.RadioButton();
			this.pnlN64 = new System.Windows.Forms.Panel();
			this.rbN64PC = new System.Windows.Forms.RadioButton();
			this.lblN64Mode = new System.Windows.Forms.Label();
			this.rbN64Console = new System.Windows.Forms.RadioButton();
			this.pnlSNES = new System.Windows.Forms.Panel();
			this.lblSNESMode = new System.Windows.Forms.Label();
			this.rbSnesPC = new System.Windows.Forms.RadioButton();
			this.rbSnesConsole = new System.Windows.Forms.RadioButton();
			this.rbSnesNgc = new System.Windows.Forms.RadioButton();
			this.lblFirmwareVersion = new System.Windows.Forms.Label();
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbMunia.SuspendLayout();
			this.pnlNGC.SuspendLayout();
			this.pnlN64.SuspendLayout();
			this.pnlSNES.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbMunia
			// 
			this.gbMunia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbMunia.Controls.Add(this.tbMCURevision);
			this.gbMunia.Controls.Add(this.label1);
			this.gbMunia.Controls.Add(this.tbMCUId);
			this.gbMunia.Controls.Add(this.lblMicrocontroller);
			this.gbMunia.Controls.Add(this.tbHardware);
			this.gbMunia.Controls.Add(this.label2);
			this.gbMunia.Controls.Add(this.tbFirmware);
			this.gbMunia.Controls.Add(this.pnlNGC);
			this.gbMunia.Controls.Add(this.pnlN64);
			this.gbMunia.Controls.Add(this.pnlSNES);
			this.gbMunia.Controls.Add(this.lblFirmwareVersion);
			this.gbMunia.Location = new System.Drawing.Point(12, 12);
			this.gbMunia.Name = "gbMunia";
			this.gbMunia.Size = new System.Drawing.Size(311, 185);
			this.gbMunia.TabIndex = 0;
			this.gbMunia.TabStop = false;
			this.gbMunia.Text = "MUNIA Info";
			// 
			// tbMCURevision
			// 
			this.tbMCURevision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMCURevision.Enabled = false;
			this.tbMCURevision.Location = new System.Drawing.Point(242, 45);
			this.tbMCURevision.Name = "tbMCURevision";
			this.tbMCURevision.ReadOnly = true;
			this.tbMCURevision.Size = new System.Drawing.Size(50, 20);
			this.tbMCURevision.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(171, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "µC revision";
			// 
			// tbMCUId
			// 
			this.tbMCUId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMCUId.Enabled = false;
			this.tbMCUId.Location = new System.Drawing.Point(113, 45);
			this.tbMCUId.Name = "tbMCUId";
			this.tbMCUId.ReadOnly = true;
			this.tbMCUId.Size = new System.Drawing.Size(49, 20);
			this.tbMCUId.TabIndex = 8;
			// 
			// lblMicrocontroller
			// 
			this.lblMicrocontroller.AutoSize = true;
			this.lblMicrocontroller.Location = new System.Drawing.Point(21, 48);
			this.lblMicrocontroller.Name = "lblMicrocontroller";
			this.lblMicrocontroller.Size = new System.Drawing.Size(76, 13);
			this.lblMicrocontroller.TabIndex = 7;
			this.lblMicrocontroller.Text = "Microcontroller";
			// 
			// tbHardware
			// 
			this.tbHardware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHardware.Enabled = false;
			this.tbHardware.Location = new System.Drawing.Point(242, 23);
			this.tbHardware.Name = "tbHardware";
			this.tbHardware.ReadOnly = true;
			this.tbHardware.Size = new System.Drawing.Size(50, 20);
			this.tbHardware.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(171, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "HW revision";
			// 
			// tbFirmware
			// 
			this.tbFirmware.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbFirmware.Enabled = false;
			this.tbFirmware.Location = new System.Drawing.Point(113, 23);
			this.tbFirmware.Name = "tbFirmware";
			this.tbFirmware.ReadOnly = true;
			this.tbFirmware.Size = new System.Drawing.Size(49, 20);
			this.tbFirmware.TabIndex = 1;
			// 
			// pnlNGC
			// 
			this.pnlNGC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlNGC.Controls.Add(this.rbNgcPC);
			this.pnlNGC.Controls.Add(this.lblNGCMode);
			this.pnlNGC.Controls.Add(this.rbNgcConsole);
			this.pnlNGC.Location = new System.Drawing.Point(19, 144);
			this.pnlNGC.Name = "pnlNGC";
			this.pnlNGC.Size = new System.Drawing.Size(281, 31);
			this.pnlNGC.TabIndex = 6;
			// 
			// rbNgcPC
			// 
			this.rbNgcPC.AutoSize = true;
			this.rbNgcPC.Location = new System.Drawing.Point(161, 7);
			this.rbNgcPC.Name = "rbNgcPC";
			this.rbNgcPC.Size = new System.Drawing.Size(39, 17);
			this.rbNgcPC.TabIndex = 2;
			this.rbNgcPC.TabStop = true;
			this.rbNgcPC.Text = "PC";
			this.rbNgcPC.UseVisualStyleBackColor = true;
			// 
			// lblNGCMode
			// 
			this.lblNGCMode.AutoSize = true;
			this.lblNGCMode.Location = new System.Drawing.Point(8, 9);
			this.lblNGCMode.Name = "lblNGCMode";
			this.lblNGCMode.Size = new System.Drawing.Size(59, 13);
			this.lblNGCMode.TabIndex = 0;
			this.lblNGCMode.Text = "NGC mode";
			// 
			// rbNgcConsole
			// 
			this.rbNgcConsole.AutoSize = true;
			this.rbNgcConsole.Location = new System.Drawing.Point(93, 7);
			this.rbNgcConsole.Name = "rbNgcConsole";
			this.rbNgcConsole.Size = new System.Drawing.Size(63, 17);
			this.rbNgcConsole.TabIndex = 1;
			this.rbNgcConsole.TabStop = true;
			this.rbNgcConsole.Text = "Console";
			this.rbNgcConsole.UseVisualStyleBackColor = true;
			// 
			// pnlN64
			// 
			this.pnlN64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlN64.Controls.Add(this.rbN64PC);
			this.pnlN64.Controls.Add(this.lblN64Mode);
			this.pnlN64.Controls.Add(this.rbN64Console);
			this.pnlN64.Location = new System.Drawing.Point(19, 107);
			this.pnlN64.Name = "pnlN64";
			this.pnlN64.Size = new System.Drawing.Size(281, 31);
			this.pnlN64.TabIndex = 5;
			// 
			// rbN64PC
			// 
			this.rbN64PC.AutoSize = true;
			this.rbN64PC.Location = new System.Drawing.Point(161, 7);
			this.rbN64PC.Name = "rbN64PC";
			this.rbN64PC.Size = new System.Drawing.Size(39, 17);
			this.rbN64PC.TabIndex = 2;
			this.rbN64PC.TabStop = true;
			this.rbN64PC.Text = "PC";
			this.rbN64PC.UseVisualStyleBackColor = true;
			// 
			// lblN64Mode
			// 
			this.lblN64Mode.AutoSize = true;
			this.lblN64Mode.Location = new System.Drawing.Point(8, 9);
			this.lblN64Mode.Name = "lblN64Mode";
			this.lblN64Mode.Size = new System.Drawing.Size(56, 13);
			this.lblN64Mode.TabIndex = 0;
			this.lblN64Mode.Text = "N64 mode";
			// 
			// rbN64Console
			// 
			this.rbN64Console.AutoSize = true;
			this.rbN64Console.Location = new System.Drawing.Point(93, 7);
			this.rbN64Console.Name = "rbN64Console";
			this.rbN64Console.Size = new System.Drawing.Size(63, 17);
			this.rbN64Console.TabIndex = 1;
			this.rbN64Console.TabStop = true;
			this.rbN64Console.Text = "Console";
			this.rbN64Console.UseVisualStyleBackColor = true;
			// 
			// pnlSNES
			// 
			this.pnlSNES.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSNES.Controls.Add(this.lblSNESMode);
			this.pnlSNES.Controls.Add(this.rbSnesPC);
			this.pnlSNES.Controls.Add(this.rbSnesConsole);
			this.pnlSNES.Controls.Add(this.rbSnesNgc);
			this.pnlSNES.Location = new System.Drawing.Point(19, 70);
			this.pnlSNES.Name = "pnlSNES";
			this.pnlSNES.Size = new System.Drawing.Size(281, 31);
			this.pnlSNES.TabIndex = 4;
			// 
			// lblSNESMode
			// 
			this.lblSNESMode.AutoSize = true;
			this.lblSNESMode.Location = new System.Drawing.Point(8, 9);
			this.lblSNESMode.Name = "lblSNESMode";
			this.lblSNESMode.Size = new System.Drawing.Size(65, 13);
			this.lblSNESMode.TabIndex = 0;
			this.lblSNESMode.Text = "SNES mode";
			// 
			// rbSnesPC
			// 
			this.rbSnesPC.AutoSize = true;
			this.rbSnesPC.Location = new System.Drawing.Point(161, 7);
			this.rbSnesPC.Name = "rbSnesPC";
			this.rbSnesPC.Size = new System.Drawing.Size(39, 17);
			this.rbSnesPC.TabIndex = 2;
			this.rbSnesPC.TabStop = true;
			this.rbSnesPC.Text = "PC";
			this.rbSnesPC.UseVisualStyleBackColor = true;
			// 
			// rbSnesConsole
			// 
			this.rbSnesConsole.AutoSize = true;
			this.rbSnesConsole.Location = new System.Drawing.Point(93, 7);
			this.rbSnesConsole.Name = "rbSnesConsole";
			this.rbSnesConsole.Size = new System.Drawing.Size(63, 17);
			this.rbSnesConsole.TabIndex = 1;
			this.rbSnesConsole.TabStop = true;
			this.rbSnesConsole.Text = "Console";
			this.rbSnesConsole.UseVisualStyleBackColor = true;
			// 
			// rbSnesNgc
			// 
			this.rbSnesNgc.AutoSize = true;
			this.rbSnesNgc.Location = new System.Drawing.Point(206, 7);
			this.rbSnesNgc.Name = "rbSnesNgc";
			this.rbSnesNgc.Size = new System.Drawing.Size(48, 17);
			this.rbSnesNgc.TabIndex = 3;
			this.rbSnesNgc.TabStop = true;
			this.rbSnesNgc.Text = "NGC";
			this.rbSnesNgc.UseVisualStyleBackColor = true;
			// 
			// lblFirmwareVersion
			// 
			this.lblFirmwareVersion.AutoSize = true;
			this.lblFirmwareVersion.Location = new System.Drawing.Point(21, 26);
			this.lblFirmwareVersion.Name = "lblFirmwareVersion";
			this.lblFirmwareVersion.Size = new System.Drawing.Size(86, 13);
			this.lblFirmwareVersion.TabIndex = 0;
			this.lblFirmwareVersion.Text = "Firmware version";
			// 
			// btnAccept
			// 
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Location = new System.Drawing.Point(237, 210);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 2;
			this.btnAccept.Text = "&Apply";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(156, 210);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// MuniaSettingsDialog
			// 
			this.AcceptButton = this.btnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(335, 245);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.gbMunia);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(337, 251);
			this.Name = "MuniaSettingsDialog";
			this.Text = "MUNIA Device Configuration";
			this.gbMunia.ResumeLayout(false);
			this.gbMunia.PerformLayout();
			this.pnlNGC.ResumeLayout(false);
			this.pnlNGC.PerformLayout();
			this.pnlN64.ResumeLayout(false);
			this.pnlN64.PerformLayout();
			this.pnlSNES.ResumeLayout(false);
			this.pnlSNES.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbMunia;
		private System.Windows.Forms.TextBox tbHardware;
		private System.Windows.Forms.TextBox tbFirmware;
		private System.Windows.Forms.Panel pnlNGC;
		private System.Windows.Forms.Panel pnlN64;
		private System.Windows.Forms.Panel pnlSNES;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblFirmwareVersion;
		private System.Windows.Forms.RadioButton rbNgcPC;
		private System.Windows.Forms.Label lblNGCMode;
		private System.Windows.Forms.RadioButton rbNgcConsole;
		private System.Windows.Forms.RadioButton rbN64PC;
		private System.Windows.Forms.Label lblN64Mode;
		private System.Windows.Forms.RadioButton rbN64Console;
		private System.Windows.Forms.Label lblSNESMode;
		private System.Windows.Forms.RadioButton rbSnesPC;
		private System.Windows.Forms.RadioButton rbSnesConsole;
		private System.Windows.Forms.RadioButton rbSnesNgc;
		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox tbMCUId;
		private System.Windows.Forms.Label lblMicrocontroller;
		private System.Windows.Forms.TextBox tbMCURevision;
		private System.Windows.Forms.Label label1;
	}
}