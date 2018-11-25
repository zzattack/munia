namespace MUNIA.Forms {
	partial class SkinPreviewWindow {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.pnlTop = new System.Windows.Forms.Panel();
			this.lblSkinPath = new System.Windows.Forms.Label();
			this.pbPreview = new System.Windows.Forms.PictureBox();
			this.pnlTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlTop
			// 
			this.pnlTop.Controls.Add(this.lblSkinPath);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(456, 41);
			this.pnlTop.TabIndex = 2;
			// 
			// lblSkinPath
			// 
			this.lblSkinPath.Location = new System.Drawing.Point(3, 0);
			this.lblSkinPath.Name = "lblSkinPath";
			this.lblSkinPath.Size = new System.Drawing.Size(422, 38);
			this.lblSkinPath.TabIndex = 0;
			this.lblSkinPath.Text = "skin path";
			this.lblSkinPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbPreview
			// 
			this.pbPreview.BackColor = System.Drawing.Color.DimGray;
			this.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbPreview.Location = new System.Drawing.Point(0, 41);
			this.pbPreview.Name = "pbPreview";
			this.pbPreview.Size = new System.Drawing.Size(456, 313);
			this.pbPreview.TabIndex = 3;
			this.pbPreview.TabStop = false;
			// 
			// SkinPreviewWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pbPreview);
			this.Controls.Add(this.pnlTop);
			this.Name = "SkinPreviewWindow";
			this.Size = new System.Drawing.Size(456, 354);
			this.pnlTop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Label lblSkinPath;
		private System.Windows.Forms.PictureBox pbPreview;
	}
}