namespace MUNIA.Forms {
	partial class DelayValuePicker {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DelayValuePicker));
			this.BtnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblWidth = new System.Windows.Forms.Label();
			this.gb = new System.Windows.Forms.GroupBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.lblMs = new System.Windows.Forms.Label();
			this.nudDelay = new System.Windows.Forms.NumericUpDown();
			this.gb.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDelay)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnAccept
			// 
			this.BtnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnAccept.Location = new System.Drawing.Point(152, 81);
			this.BtnAccept.Name = "BtnAccept";
			this.BtnAccept.Size = new System.Drawing.Size(75, 23);
			this.BtnAccept.TabIndex = 2;
			this.BtnAccept.Text = "Accept";
			this.BtnAccept.UseVisualStyleBackColor = true;
			this.BtnAccept.Click += new System.EventHandler(this.BtnAccept_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(71, 81);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblWidth
			// 
			this.lblWidth.AutoSize = true;
			this.lblWidth.Location = new System.Drawing.Point(12, 31);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(72, 13);
			this.lblWidth.TabIndex = 0;
			this.lblWidth.Text = "Delay amount";
			// 
			// gb
			// 
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.btnReset);
			this.gb.Controls.Add(this.lblMs);
			this.gb.Controls.Add(this.nudDelay);
			this.gb.Controls.Add(this.lblWidth);
			this.gb.Location = new System.Drawing.Point(12, 12);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(220, 60);
			this.gb.TabIndex = 0;
			this.gb.TabStop = false;
			this.gb.Text = "Delay shown inputs";
			// 
			// btnReset
			// 
			this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReset.Location = new System.Drawing.Point(183, 25);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(27, 24);
			this.btnReset.TabIndex = 4;
			this.btnReset.Text = "⟳";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// lblMs
			// 
			this.lblMs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMs.AutoSize = true;
			this.lblMs.Location = new System.Drawing.Point(157, 31);
			this.lblMs.Name = "lblMs";
			this.lblMs.Size = new System.Drawing.Size(20, 13);
			this.lblMs.TabIndex = 2;
			this.lblMs.Text = "ms";
			// 
			// nudDelay
			// 
			this.nudDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nudDelay.Location = new System.Drawing.Point(83, 29);
			this.nudDelay.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.nudDelay.Name = "nudDelay";
			this.nudDelay.Size = new System.Drawing.Size(68, 20);
			this.nudDelay.TabIndex = 1;
			this.nudDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudDelay.Enter += new System.EventHandler(this.nudDelay_Enter);
			// 
			// DelayValuePicker
			// 
			this.AcceptButton = this.BtnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(239, 116);
			this.Controls.Add(this.gb);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.BtnAccept);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DelayValuePicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Delay picker";
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudDelay)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnAccept;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.GroupBox gb;
		private System.Windows.Forms.NumericUpDown nudDelay;
		private System.Windows.Forms.Label lblMs;
		private System.Windows.Forms.Button btnReset;
	}
}