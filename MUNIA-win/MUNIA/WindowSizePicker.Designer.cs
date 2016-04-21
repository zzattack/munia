namespace MUNIA {
	partial class WindowSizePicker {
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
			this.BtnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblWidth = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.gb = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblPx = new System.Windows.Forms.Label();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.gb.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnAccept
			// 
			this.BtnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnAccept.Location = new System.Drawing.Point(119, 110);
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
			this.btnCancel.Location = new System.Drawing.Point(38, 110);
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
			this.lblWidth.Size = new System.Drawing.Size(35, 13);
			this.lblWidth.TabIndex = 0;
			this.lblWidth.Text = "Width";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 54);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Height";
			// 
			// gb
			// 
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.label1);
			this.gb.Controls.Add(this.lblPx);
			this.gb.Controls.Add(this.nudHeight);
			this.gb.Controls.Add(this.nudWidth);
			this.gb.Controls.Add(this.label2);
			this.gb.Controls.Add(this.lblWidth);
			this.gb.Location = new System.Drawing.Point(12, 12);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(182, 92);
			this.gb.TabIndex = 0;
			this.gb.TabStop = false;
			this.gb.Text = "Inner window dimensions";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(152, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(18, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "px";
			// 
			// lblPx
			// 
			this.lblPx.AutoSize = true;
			this.lblPx.Location = new System.Drawing.Point(152, 31);
			this.lblPx.Name = "lblPx";
			this.lblPx.Size = new System.Drawing.Size(18, 13);
			this.lblPx.TabIndex = 2;
			this.lblPx.Text = "px";
			// 
			// nudHeight
			// 
			this.nudHeight.Location = new System.Drawing.Point(72, 52);
			this.nudHeight.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
			this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(75, 20);
			this.nudHeight.TabIndex = 4;
			this.nudHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudHeight.Enter += new System.EventHandler(this.nudHeight_Enter);
			// 
			// nudWidth
			// 
			this.nudWidth.Location = new System.Drawing.Point(72, 29);
			this.nudWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
			this.nudWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(75, 20);
			this.nudWidth.TabIndex = 1;
			this.nudWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudWidth.Enter += new System.EventHandler(this.nudWidth_Enter);
			// 
			// WindowSizePicker
			// 
			this.AcceptButton = this.BtnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(206, 145);
			this.Controls.Add(this.gb);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.BtnAccept);
			this.Name = "WindowSizePicker";
			this.Text = "Window size";
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnAccept;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox gb;
		private System.Windows.Forms.NumericUpDown nudWidth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblPx;
		private System.Windows.Forms.NumericUpDown nudHeight;
	}
}