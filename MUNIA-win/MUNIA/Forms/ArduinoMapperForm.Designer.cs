namespace MUNIA.Forms {
	partial class ArduinoMapperForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArduinoMapperForm));
			this.lbSerialPorts = new System.Windows.Forms.ListBox();
			this.cbDeviceType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnTest = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.BtnAccept = new System.Windows.Forms.Button();
			this.gb = new System.Windows.Forms.GroupBox();
			this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			this.gb.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbSerialPorts
			// 
			this.lbSerialPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbSerialPorts.FormattingEnabled = true;
			this.lbSerialPorts.Location = new System.Drawing.Point(12, 25);
			this.lbSerialPorts.Name = "lbSerialPorts";
			this.lbSerialPorts.Size = new System.Drawing.Size(157, 134);
			this.lbSerialPorts.TabIndex = 0;
			// 
			// cbDeviceType
			// 
			this.cbDeviceType.FormattingEnabled = true;
			this.cbDeviceType.Location = new System.Drawing.Point(13, 35);
			this.cbDeviceType.Name = "cbDeviceType";
			this.cbDeviceType.Size = new System.Drawing.Size(121, 21);
			this.cbDeviceType.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Select COM port";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Device type";
			// 
			// btnTest
			// 
			this.btnTest.Location = new System.Drawing.Point(17, 62);
			this.btnTest.Name = "btnTest";
			this.btnTest.Size = new System.Drawing.Size(75, 23);
			this.btnTest.TabIndex = 4;
			this.btnTest.Text = "Test";
			this.btnTest.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(191, 135);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnAccept
			// 
			this.BtnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnAccept.Location = new System.Drawing.Point(272, 135);
			this.BtnAccept.Name = "BtnAccept";
			this.BtnAccept.Size = new System.Drawing.Size(75, 23);
			this.BtnAccept.TabIndex = 6;
			this.BtnAccept.Text = "Accept";
			this.BtnAccept.UseVisualStyleBackColor = true;
			// 
			// gb
			// 
			this.gb.Controls.Add(this.label2);
			this.gb.Controls.Add(this.cbDeviceType);
			this.gb.Controls.Add(this.btnTest);
			this.gb.Enabled = false;
			this.gb.Location = new System.Drawing.Point(175, 25);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(172, 100);
			this.gb.TabIndex = 7;
			this.gb.TabStop = false;
			this.gb.Text = "Mapping";
			// 
			// ArduinoMapperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(359, 170);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.BtnAccept);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbSerialPorts);
			this.Controls.Add(this.gb);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ArduinoMapperForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Arduino mapper";
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbSerialPorts;
		private System.Windows.Forms.ComboBox cbDeviceType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button BtnAccept;
		private System.Windows.Forms.GroupBox gb;
		private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}