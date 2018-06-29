namespace MUNIA.Forms {
	partial class DevicePicker {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevicePicker));
			this.btnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lbDevices = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnAccept
			// 
			this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAccept.Enabled = false;
			this.btnAccept.Location = new System.Drawing.Point(223, 132);
			this.btnAccept.Name = "btnAccept";
			this.btnAccept.Size = new System.Drawing.Size(75, 23);
			this.btnAccept.TabIndex = 2;
			this.btnAccept.Text = "Accept";
			this.btnAccept.UseVisualStyleBackColor = true;
			this.btnAccept.Click += new System.EventHandler(this.BtnAccept_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(142, 132);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Multiple devices found, select one";
			// 
			// lbDevices
			// 
			this.lbDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbDevices.FormattingEnabled = true;
			this.lbDevices.Location = new System.Drawing.Point(15, 34);
			this.lbDevices.Name = "lbDevices";
			this.lbDevices.Size = new System.Drawing.Size(283, 82);
			this.lbDevices.TabIndex = 4;
			this.lbDevices.SelectedIndexChanged += new System.EventHandler(this.lbDevices_SelectedIndexChanged);
			this.lbDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbDevices_MouseDoubleClick);
			// 
			// DevicePicker
			// 
			this.AcceptButton = this.btnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(310, 167);
			this.Controls.Add(this.lbDevices);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAccept);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DevicePicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a device";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnAccept;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lbDevices;
	}
}