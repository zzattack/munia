namespace MUNIA.Forms {
	partial class GamepadTesterForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GamepadTesterForm));
			this.lbMuniaDevices = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblNSpyDevices = new System.Windows.Forms.Label();
			this.lbNintendoSpyDevices = new System.Windows.Forms.ListBox();
			this.lblGenericGamepads = new System.Windows.Forms.Label();
			this.lbGenericDevices = new System.Windows.Forms.ListBox();
			this.btnConfigure = new System.Windows.Forms.Button();
			this.btnTestMUNIA = new System.Windows.Forms.Button();
			this.btnTestNSpy = new System.Windows.Forms.Button();
			this.btnTestGeneric = new System.Windows.Forms.Button();
			this.gbDeviceSelection = new System.Windows.Forms.GroupBox();
			this.pnlTest = new System.Windows.Forms.Panel();
			this.gamepadViewer = new MUNIA.Forms.GamepadViewerControl();
			this.rtb = new MUNIA.Util.RichTextBoxEx();
			this.gbDeviceSelection.SuspendLayout();
			this.pnlTest.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbMuniaDevices
			// 
			this.lbMuniaDevices.FormattingEnabled = true;
			this.lbMuniaDevices.Location = new System.Drawing.Point(19, 45);
			this.lbMuniaDevices.Name = "lbMuniaDevices";
			this.lbMuniaDevices.Size = new System.Drawing.Size(188, 95);
			this.lbMuniaDevices.TabIndex = 0;
			this.lbMuniaDevices.SelectedValueChanged += new System.EventHandler(this.UpdateUI);
			this.lbMuniaDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbMuniaDevices_MouseDoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "MUNIA devices";
			// 
			// lblNSpyDevices
			// 
			this.lblNSpyDevices.AutoSize = true;
			this.lblNSpyDevices.Location = new System.Drawing.Point(222, 29);
			this.lblNSpyDevices.Name = "lblNSpyDevices";
			this.lblNSpyDevices.Size = new System.Drawing.Size(108, 13);
			this.lblNSpyDevices.TabIndex = 3;
			this.lblNSpyDevices.Text = "NintendoSpy devices";
			// 
			// lbNintendoSpyDevices
			// 
			this.lbNintendoSpyDevices.FormattingEnabled = true;
			this.lbNintendoSpyDevices.Location = new System.Drawing.Point(225, 45);
			this.lbNintendoSpyDevices.Name = "lbNintendoSpyDevices";
			this.lbNintendoSpyDevices.Size = new System.Drawing.Size(188, 95);
			this.lbNintendoSpyDevices.TabIndex = 2;
			this.lbNintendoSpyDevices.SelectedValueChanged += new System.EventHandler(this.UpdateUI);
			this.lbNintendoSpyDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbNintendoSpyDevices_MouseDoubleClick);
			// 
			// lblGenericGamepads
			// 
			this.lblGenericGamepads.AutoSize = true;
			this.lblGenericGamepads.Location = new System.Drawing.Point(427, 29);
			this.lblGenericGamepads.Name = "lblGenericGamepads";
			this.lblGenericGamepads.Size = new System.Drawing.Size(96, 13);
			this.lblGenericGamepads.TabIndex = 5;
			this.lblGenericGamepads.Text = "Generic gamepads";
			// 
			// lbGenericDevices
			// 
			this.lbGenericDevices.FormattingEnabled = true;
			this.lbGenericDevices.Location = new System.Drawing.Point(430, 45);
			this.lbGenericDevices.Name = "lbGenericDevices";
			this.lbGenericDevices.Size = new System.Drawing.Size(188, 95);
			this.lbGenericDevices.TabIndex = 4;
			this.lbGenericDevices.SelectedValueChanged += new System.EventHandler(this.UpdateUI);
			this.lbGenericDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbGenericDevices_MouseDoubleClick);
			// 
			// btnConfigure
			// 
			this.btnConfigure.Location = new System.Drawing.Point(234, 146);
			this.btnConfigure.Name = "btnConfigure";
			this.btnConfigure.Size = new System.Drawing.Size(70, 24);
			this.btnConfigure.TabIndex = 6;
			this.btnConfigure.Text = "Configure";
			this.btnConfigure.UseVisualStyleBackColor = true;
			// 
			// btnTestMUNIA
			// 
			this.btnTestMUNIA.Location = new System.Drawing.Point(137, 146);
			this.btnTestMUNIA.Name = "btnTestMUNIA";
			this.btnTestMUNIA.Size = new System.Drawing.Size(70, 24);
			this.btnTestMUNIA.TabIndex = 7;
			this.btnTestMUNIA.Text = "Test";
			this.btnTestMUNIA.UseVisualStyleBackColor = true;
			this.btnTestMUNIA.Click += new System.EventHandler(this.btnTestMUNIA_Click);
			// 
			// btnTestNSpy
			// 
			this.btnTestNSpy.Location = new System.Drawing.Point(343, 146);
			this.btnTestNSpy.Name = "btnTestNSpy";
			this.btnTestNSpy.Size = new System.Drawing.Size(70, 24);
			this.btnTestNSpy.TabIndex = 8;
			this.btnTestNSpy.Text = "Test";
			this.btnTestNSpy.UseVisualStyleBackColor = true;
			this.btnTestNSpy.Click += new System.EventHandler(this.btnTestNSpy_Click);
			// 
			// btnTestGeneric
			// 
			this.btnTestGeneric.Location = new System.Drawing.Point(548, 146);
			this.btnTestGeneric.Name = "btnTestGeneric";
			this.btnTestGeneric.Size = new System.Drawing.Size(70, 24);
			this.btnTestGeneric.TabIndex = 9;
			this.btnTestGeneric.Text = "Test";
			this.btnTestGeneric.UseVisualStyleBackColor = true;
			this.btnTestGeneric.Click += new System.EventHandler(this.btnTestGeneric_Click);
			// 
			// gbDeviceSelection
			// 
			this.gbDeviceSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbDeviceSelection.Controls.Add(this.btnTestGeneric);
			this.gbDeviceSelection.Controls.Add(this.lbMuniaDevices);
			this.gbDeviceSelection.Controls.Add(this.btnTestNSpy);
			this.gbDeviceSelection.Controls.Add(this.label1);
			this.gbDeviceSelection.Controls.Add(this.btnTestMUNIA);
			this.gbDeviceSelection.Controls.Add(this.lbNintendoSpyDevices);
			this.gbDeviceSelection.Controls.Add(this.btnConfigure);
			this.gbDeviceSelection.Controls.Add(this.lblNSpyDevices);
			this.gbDeviceSelection.Controls.Add(this.lblGenericGamepads);
			this.gbDeviceSelection.Controls.Add(this.lbGenericDevices);
			this.gbDeviceSelection.Location = new System.Drawing.Point(12, 12);
			this.gbDeviceSelection.Name = "gbDeviceSelection";
			this.gbDeviceSelection.Size = new System.Drawing.Size(644, 189);
			this.gbDeviceSelection.TabIndex = 10;
			this.gbDeviceSelection.TabStop = false;
			this.gbDeviceSelection.Text = "Device selection";
			// 
			// pnlTest
			// 
			this.pnlTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlTest.Controls.Add(this.gamepadViewer);
			this.pnlTest.Controls.Add(this.rtb);
			this.pnlTest.Location = new System.Drawing.Point(12, 207);
			this.pnlTest.Name = "pnlTest";
			this.pnlTest.Size = new System.Drawing.Size(644, 231);
			this.pnlTest.TabIndex = 11;
			// 
			// statePainter
			// 
			this.gamepadViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gamepadViewer.Location = new System.Drawing.Point(0, 76);
			this.gamepadViewer.Name = "gamepadViewer";
			this.gamepadViewer.Padding = new System.Windows.Forms.Padding(5);
			this.gamepadViewer.Size = new System.Drawing.Size(644, 155);
			this.gamepadViewer.TabIndex = 0;
			this.gamepadViewer.Text = "controllerStatePainter1";
			// 
			// rtb
			// 
			this.rtb.Dock = System.Windows.Forms.DockStyle.Top;
			this.rtb.Location = new System.Drawing.Point(0, 0);
			this.rtb.Name = "rtb";
			this.rtb.Size = new System.Drawing.Size(644, 70);
			this.rtb.TabIndex = 1;
			this.rtb.Text = "";
			// 
			// GamepadTesterForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(668, 450);
			this.Controls.Add(this.pnlTest);
			this.Controls.Add(this.gbDeviceSelection);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GamepadTesterForm";
			this.Text = "Gamepad tester";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GamepadTesterForm_FormClosing);
			this.gbDeviceSelection.ResumeLayout(false);
			this.gbDeviceSelection.PerformLayout();
			this.pnlTest.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbMuniaDevices;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblNSpyDevices;
		private System.Windows.Forms.ListBox lbNintendoSpyDevices;
		private System.Windows.Forms.Label lblGenericGamepads;
		private System.Windows.Forms.ListBox lbGenericDevices;
		private System.Windows.Forms.Button btnConfigure;
		private System.Windows.Forms.Button btnTestMUNIA;
		private System.Windows.Forms.Button btnTestNSpy;
		private System.Windows.Forms.Button btnTestGeneric;
		private System.Windows.Forms.GroupBox gbDeviceSelection;
		private System.Windows.Forms.Panel pnlTest;
		private GamepadViewerControl gamepadViewer;
		private Util.RichTextBoxEx rtb;
	}
}