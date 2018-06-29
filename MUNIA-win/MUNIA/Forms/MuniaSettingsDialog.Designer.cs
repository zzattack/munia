	namespace MUNIA.Forms {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MuniaSettingsDialog));
			this.gbMunia = new System.Windows.Forms.GroupBox();
			this.lblDeviceType = new System.Windows.Forms.Label();
			this.tbMCURevision = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbMCUId = new System.Windows.Forms.TextBox();
			this.lblMicrocontroller = new System.Windows.Forms.Label();
			this.tbHardware = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbFirmware = new System.Windows.Forms.TextBox();
			this.lblFirmwareVersion = new System.Windows.Forms.Label();
			this.gbSettings = new System.Windows.Forms.GroupBox();
			this.pnlOutput = new System.Windows.Forms.Panel();
			this.rbOutputN64 = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.rbOutputPC = new System.Windows.Forms.RadioButton();
			this.rbOutputNGC = new System.Windows.Forms.RadioButton();
			this.rbOutputSNES = new System.Windows.Forms.RadioButton();
			this.pnlInputs = new System.Windows.Forms.Panel();
			this.rbInputN64 = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.rbInputNGC = new System.Windows.Forms.RadioButton();
			this.rbInputSNES = new System.Windows.Forms.RadioButton();
			this.pnlInputsPC = new System.Windows.Forms.Panel();
			this.ckbSNES = new System.Windows.Forms.CheckBox();
			this.ckbN64 = new System.Windows.Forms.CheckBox();
			this.ckbNGC = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.gbSettingsLegacy = new System.Windows.Forms.GroupBox();
			this.pnlSNES = new System.Windows.Forms.Panel();
			this.lblSNESMode = new System.Windows.Forms.Label();
			this.rbSnesPC = new System.Windows.Forms.RadioButton();
			this.rbSnesConsole = new System.Windows.Forms.RadioButton();
			this.rbSnesNgc = new System.Windows.Forms.RadioButton();
			this.pnlN64 = new System.Windows.Forms.Panel();
			this.rbN64PC = new System.Windows.Forms.RadioButton();
			this.lblN64Mode = new System.Windows.Forms.Label();
			this.rbN64Console = new System.Windows.Forms.RadioButton();
			this.pnlNGC = new System.Windows.Forms.Panel();
			this.rbNgcPC = new System.Windows.Forms.RadioButton();
			this.lblNGCMode = new System.Windows.Forms.Label();
			this.rbNgcConsole = new System.Windows.Forms.RadioButton();
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbMunia.SuspendLayout();
			this.gbSettings.SuspendLayout();
			this.pnlOutput.SuspendLayout();
			this.pnlInputs.SuspendLayout();
			this.pnlInputsPC.SuspendLayout();
			this.gbSettingsLegacy.SuspendLayout();
			this.pnlSNES.SuspendLayout();
			this.pnlN64.SuspendLayout();
			this.pnlNGC.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbMunia
			// 
			this.gbMunia.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbMunia.Controls.Add(this.lblDeviceType);
			this.gbMunia.Controls.Add(this.tbMCURevision);
			this.gbMunia.Controls.Add(this.label1);
			this.gbMunia.Controls.Add(this.tbMCUId);
			this.gbMunia.Controls.Add(this.lblMicrocontroller);
			this.gbMunia.Controls.Add(this.tbHardware);
			this.gbMunia.Controls.Add(this.label2);
			this.gbMunia.Controls.Add(this.tbFirmware);
			this.gbMunia.Controls.Add(this.lblFirmwareVersion);
			this.gbMunia.Controls.Add(this.gbSettings);
			this.gbMunia.Controls.Add(this.gbSettingsLegacy);
			this.gbMunia.Location = new System.Drawing.Point(12, 12);
			this.gbMunia.Name = "gbMunia";
			this.gbMunia.Size = new System.Drawing.Size(311, 220);
			this.gbMunia.TabIndex = 0;
			this.gbMunia.TabStop = false;
			this.gbMunia.Text = "MUNIA Info";
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
			// tbMCURevision
			// 
			this.tbMCURevision.Enabled = false;
			this.tbMCURevision.Location = new System.Drawing.Point(242, 62);
			this.tbMCURevision.Name = "tbMCURevision";
			this.tbMCURevision.ReadOnly = true;
			this.tbMCURevision.Size = new System.Drawing.Size(50, 20);
			this.tbMCURevision.TabIndex = 10;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(171, 65);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 9;
			this.label1.Text = "µC revision";
			// 
			// tbMCUId
			// 
			this.tbMCUId.Enabled = false;
			this.tbMCUId.Location = new System.Drawing.Point(113, 62);
			this.tbMCUId.Name = "tbMCUId";
			this.tbMCUId.ReadOnly = true;
			this.tbMCUId.Size = new System.Drawing.Size(49, 20);
			this.tbMCUId.TabIndex = 8;
			// 
			// lblMicrocontroller
			// 
			this.lblMicrocontroller.AutoSize = true;
			this.lblMicrocontroller.Location = new System.Drawing.Point(21, 65);
			this.lblMicrocontroller.Name = "lblMicrocontroller";
			this.lblMicrocontroller.Size = new System.Drawing.Size(76, 13);
			this.lblMicrocontroller.TabIndex = 7;
			this.lblMicrocontroller.Text = "Microcontroller";
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
			this.gbSettings.Controls.Add(this.pnlOutput);
			this.gbSettings.Controls.Add(this.pnlInputs);
			this.gbSettings.Controls.Add(this.pnlInputsPC);
			this.gbSettings.Location = new System.Drawing.Point(9, 83);
			this.gbSettings.Name = "gbSettings";
			this.gbSettings.Size = new System.Drawing.Size(296, 85);
			this.gbSettings.TabIndex = 15;
			this.gbSettings.TabStop = false;
			// 
			// pnlOutput
			// 
			this.pnlOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlOutput.Controls.Add(this.rbOutputN64);
			this.pnlOutput.Controls.Add(this.label3);
			this.pnlOutput.Controls.Add(this.rbOutputPC);
			this.pnlOutput.Controls.Add(this.rbOutputNGC);
			this.pnlOutput.Controls.Add(this.rbOutputSNES);
			this.pnlOutput.Location = new System.Drawing.Point(7, 11);
			this.pnlOutput.Name = "pnlOutput";
			this.pnlOutput.Size = new System.Drawing.Size(281, 31);
			this.pnlOutput.TabIndex = 11;
			// 
			// rbOutputN64
			// 
			this.rbOutputN64.AutoSize = true;
			this.rbOutputN64.Location = new System.Drawing.Point(123, 7);
			this.rbOutputN64.Name = "rbOutputN64";
			this.rbOutputN64.Size = new System.Drawing.Size(45, 17);
			this.rbOutputN64.TabIndex = 4;
			this.rbOutputN64.Text = "N64";
			this.rbOutputN64.UseVisualStyleBackColor = true;
			this.rbOutputN64.CheckedChanged += new System.EventHandler(this.rbOutput_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Output to";
			// 
			// rbOutputPC
			// 
			this.rbOutputPC.AutoSize = true;
			this.rbOutputPC.Location = new System.Drawing.Point(232, 7);
			this.rbOutputPC.Name = "rbOutputPC";
			this.rbOutputPC.Size = new System.Drawing.Size(39, 17);
			this.rbOutputPC.TabIndex = 2;
			this.rbOutputPC.Text = "PC";
			this.rbOutputPC.UseVisualStyleBackColor = true;
			this.rbOutputPC.CheckedChanged += new System.EventHandler(this.rbOutput_CheckedChanged);
			// 
			// rbOutputNGC
			// 
			this.rbOutputNGC.AutoSize = true;
			this.rbOutputNGC.Location = new System.Drawing.Point(70, 7);
			this.rbOutputNGC.Name = "rbOutputNGC";
			this.rbOutputNGC.Size = new System.Drawing.Size(48, 17);
			this.rbOutputNGC.TabIndex = 1;
			this.rbOutputNGC.Text = "NGC";
			this.rbOutputNGC.UseVisualStyleBackColor = true;
			this.rbOutputNGC.CheckedChanged += new System.EventHandler(this.rbOutput_CheckedChanged);
			// 
			// rbOutputSNES
			// 
			this.rbOutputSNES.AutoSize = true;
			this.rbOutputSNES.Location = new System.Drawing.Point(173, 7);
			this.rbOutputSNES.Name = "rbOutputSNES";
			this.rbOutputSNES.Size = new System.Drawing.Size(54, 17);
			this.rbOutputSNES.TabIndex = 3;
			this.rbOutputSNES.Text = "SNES";
			this.rbOutputSNES.UseVisualStyleBackColor = true;
			this.rbOutputSNES.CheckedChanged += new System.EventHandler(this.rbOutput_CheckedChanged);
			// 
			// pnlInputs
			// 
			this.pnlInputs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlInputs.Controls.Add(this.rbInputN64);
			this.pnlInputs.Controls.Add(this.label4);
			this.pnlInputs.Controls.Add(this.rbInputNGC);
			this.pnlInputs.Controls.Add(this.rbInputSNES);
			this.pnlInputs.Location = new System.Drawing.Point(7, 48);
			this.pnlInputs.Name = "pnlInputs";
			this.pnlInputs.Size = new System.Drawing.Size(281, 31);
			this.pnlInputs.TabIndex = 12;
			// 
			// rbInputN64
			// 
			this.rbInputN64.AutoSize = true;
			this.rbInputN64.Location = new System.Drawing.Point(123, 7);
			this.rbInputN64.Name = "rbInputN64";
			this.rbInputN64.Size = new System.Drawing.Size(45, 17);
			this.rbInputN64.TabIndex = 4;
			this.rbInputN64.Text = "N64";
			this.rbInputN64.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Input from";
			// 
			// rbInputNGC
			// 
			this.rbInputNGC.AutoSize = true;
			this.rbInputNGC.Location = new System.Drawing.Point(70, 7);
			this.rbInputNGC.Name = "rbInputNGC";
			this.rbInputNGC.Size = new System.Drawing.Size(48, 17);
			this.rbInputNGC.TabIndex = 1;
			this.rbInputNGC.Text = "NGC";
			this.rbInputNGC.UseVisualStyleBackColor = true;
			// 
			// rbInputSNES
			// 
			this.rbInputSNES.AutoSize = true;
			this.rbInputSNES.Location = new System.Drawing.Point(173, 7);
			this.rbInputSNES.Name = "rbInputSNES";
			this.rbInputSNES.Size = new System.Drawing.Size(54, 17);
			this.rbInputSNES.TabIndex = 3;
			this.rbInputSNES.Text = "SNES";
			this.rbInputSNES.UseVisualStyleBackColor = true;
			// 
			// pnlInputsPC
			// 
			this.pnlInputsPC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlInputsPC.Controls.Add(this.ckbSNES);
			this.pnlInputsPC.Controls.Add(this.ckbN64);
			this.pnlInputsPC.Controls.Add(this.ckbNGC);
			this.pnlInputsPC.Controls.Add(this.label5);
			this.pnlInputsPC.Location = new System.Drawing.Point(7, 48);
			this.pnlInputsPC.Name = "pnlInputsPC";
			this.pnlInputsPC.Size = new System.Drawing.Size(281, 31);
			this.pnlInputsPC.TabIndex = 13;
			// 
			// ckbSNES
			// 
			this.ckbSNES.AutoSize = true;
			this.ckbSNES.Location = new System.Drawing.Point(173, 8);
			this.ckbSNES.Name = "ckbSNES";
			this.ckbSNES.Size = new System.Drawing.Size(55, 17);
			this.ckbSNES.TabIndex = 3;
			this.ckbSNES.Text = "SNES";
			this.ckbSNES.UseVisualStyleBackColor = true;
			// 
			// ckbN64
			// 
			this.ckbN64.AutoSize = true;
			this.ckbN64.Location = new System.Drawing.Point(123, 8);
			this.ckbN64.Name = "ckbN64";
			this.ckbN64.Size = new System.Drawing.Size(46, 17);
			this.ckbN64.TabIndex = 2;
			this.ckbN64.Text = "N64";
			this.ckbN64.UseVisualStyleBackColor = true;
			// 
			// ckbNGC
			// 
			this.ckbNGC.AutoSize = true;
			this.ckbNGC.Location = new System.Drawing.Point(70, 8);
			this.ckbNGC.Name = "ckbNGC";
			this.ckbNGC.Size = new System.Drawing.Size(49, 17);
			this.ckbNGC.TabIndex = 1;
			this.ckbNGC.Text = "NGC";
			this.ckbNGC.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(59, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Inputs from";
			// 
			// gbSettingsLegacy
			// 
			this.gbSettingsLegacy.Controls.Add(this.pnlSNES);
			this.gbSettingsLegacy.Controls.Add(this.pnlN64);
			this.gbSettingsLegacy.Controls.Add(this.pnlNGC);
			this.gbSettingsLegacy.Location = new System.Drawing.Point(9, 83);
			this.gbSettingsLegacy.Name = "gbSettingsLegacy";
			this.gbSettingsLegacy.Size = new System.Drawing.Size(296, 123);
			this.gbSettingsLegacy.TabIndex = 14;
			this.gbSettingsLegacy.TabStop = false;
			// 
			// pnlSNES
			// 
			this.pnlSNES.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlSNES.Controls.Add(this.lblSNESMode);
			this.pnlSNES.Controls.Add(this.rbSnesPC);
			this.pnlSNES.Controls.Add(this.rbSnesConsole);
			this.pnlSNES.Controls.Add(this.rbSnesNgc);
			this.pnlSNES.Location = new System.Drawing.Point(7, 11);
			this.pnlSNES.Name = "pnlSNES";
			this.pnlSNES.Size = new System.Drawing.Size(270, 31);
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
			// pnlN64
			// 
			this.pnlN64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlN64.Controls.Add(this.rbN64PC);
			this.pnlN64.Controls.Add(this.lblN64Mode);
			this.pnlN64.Controls.Add(this.rbN64Console);
			this.pnlN64.Location = new System.Drawing.Point(7, 48);
			this.pnlN64.Name = "pnlN64";
			this.pnlN64.Size = new System.Drawing.Size(270, 31);
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
			// pnlNGC
			// 
			this.pnlNGC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlNGC.Controls.Add(this.rbNgcPC);
			this.pnlNGC.Controls.Add(this.lblNGCMode);
			this.pnlNGC.Controls.Add(this.rbNgcConsole);
			this.pnlNGC.Location = new System.Drawing.Point(7, 85);
			this.pnlNGC.Name = "pnlNGC";
			this.pnlNGC.Size = new System.Drawing.Size(270, 31);
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
			// btnAccept
			// 
			this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Location = new System.Drawing.Point(248, 245);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 2;
			this.btnAccept.Text = "&Apply";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(167, 245);
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
			this.ClientSize = new System.Drawing.Size(335, 280);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Controls.Add(this.gbMunia);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(337, 251);
			this.Name = "MuniaSettingsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MUNIA Device Configuration";
			this.gbMunia.ResumeLayout(false);
			this.gbMunia.PerformLayout();
			this.gbSettings.ResumeLayout(false);
			this.pnlOutput.ResumeLayout(false);
			this.pnlOutput.PerformLayout();
			this.pnlInputs.ResumeLayout(false);
			this.pnlInputs.PerformLayout();
			this.pnlInputsPC.ResumeLayout(false);
			this.pnlInputsPC.PerformLayout();
			this.gbSettingsLegacy.ResumeLayout(false);
			this.pnlSNES.ResumeLayout(false);
			this.pnlSNES.PerformLayout();
			this.pnlN64.ResumeLayout(false);
			this.pnlN64.PerformLayout();
			this.pnlNGC.ResumeLayout(false);
			this.pnlNGC.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox gbMunia;
		private System.Windows.Forms.TextBox tbHardware;
		private System.Windows.Forms.TextBox tbFirmware;
		private System.Windows.Forms.Panel pnlN64;
		private System.Windows.Forms.Panel pnlSNES;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblFirmwareVersion;
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
		private System.Windows.Forms.Panel pnlOutput;
		private System.Windows.Forms.RadioButton rbOutputN64;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RadioButton rbOutputPC;
		private System.Windows.Forms.RadioButton rbOutputNGC;
		private System.Windows.Forms.RadioButton rbOutputSNES;
		private System.Windows.Forms.Panel pnlInputsPC;
		private System.Windows.Forms.CheckBox ckbSNES;
		private System.Windows.Forms.CheckBox ckbN64;
		private System.Windows.Forms.CheckBox ckbNGC;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Panel pnlInputs;
		private System.Windows.Forms.RadioButton rbInputN64;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RadioButton rbInputNGC;
		private System.Windows.Forms.RadioButton rbInputSNES;
		private System.Windows.Forms.GroupBox gbSettingsLegacy;
		private System.Windows.Forms.GroupBox gbSettings;
		private System.Windows.Forms.Panel pnlNGC;
		private System.Windows.Forms.RadioButton rbNgcPC;
		private System.Windows.Forms.Label lblNGCMode;
		private System.Windows.Forms.RadioButton rbNgcConsole;
		private System.Windows.Forms.Label lblDeviceType;
	}
}