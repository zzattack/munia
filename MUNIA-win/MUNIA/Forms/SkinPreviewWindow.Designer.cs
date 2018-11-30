namespace MUNIA.Forms {
	partial class SkinPreviewWindow {
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.pbPreview = new System.Windows.Forms.PictureBox();
			this.lblSkinPath = new System.Windows.Forms.Label();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.lblClickedItems = new System.Windows.Forms.Label();
			this.tmrHideLabel = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
			this.pnlTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// pbPreview
			// 
			this.pbPreview.BackColor = System.Drawing.Color.DimGray;
			this.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbPreview.Location = new System.Drawing.Point(0, 41);
			this.pbPreview.Name = "pbPreview";
			this.pbPreview.Size = new System.Drawing.Size(455, 290);
			this.pbPreview.TabIndex = 5;
			this.pbPreview.TabStop = false;
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
			// pnlTop
			// 
			this.pnlTop.Controls.Add(this.lblSkinPath);
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 0);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(455, 41);
			this.pnlTop.TabIndex = 4;
			// 
			// lblClickedItems
			// 
			this.lblClickedItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblClickedItems.AutoSize = true;
			this.lblClickedItems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblClickedItems.Location = new System.Drawing.Point(6, 304);
			this.lblClickedItems.Name = "lblClickedItems";
			this.lblClickedItems.Size = new System.Drawing.Size(371, 20);
			this.lblClickedItems.TabIndex = 6;
			this.lblClickedItems.Text = "Click skin elements to reveal their numbering!";
			this.lblClickedItems.Visible = false;
			// 
			// tmrHideLabel
			// 
			this.tmrHideLabel.Interval = 1500;
			this.tmrHideLabel.Tick += new System.EventHandler(this.tmrHideLabel_Tick);
			// 
			// SkinPreviewWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblClickedItems);
			this.Controls.Add(this.pbPreview);
			this.Controls.Add(this.pnlTop);
			this.Name = "SkinPreviewWindow";
			this.Size = new System.Drawing.Size(455, 331);
			((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
			this.pnlTop.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbPreview;
		private System.Windows.Forms.Label lblSkinPath;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Label lblClickedItems;
		private System.Windows.Forms.Timer tmrHideLabel;
	}
}
