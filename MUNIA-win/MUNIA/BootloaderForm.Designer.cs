namespace MUNIA {
	partial class BootloaderForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BootloaderForm));
			this.rtbHelp = new System.Windows.Forms.RichTextBox();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.tsbEnterBootloader = new System.Windows.Forms.ToolStripButton();
			this.tsbLoadHex = new System.Windows.Forms.ToolStripButton();
			this.tsbProgram = new System.Windows.Forms.ToolStripButton();
			this.tsbReset = new System.Windows.Forms.ToolStripButton();
			this.ofd = new System.Windows.Forms.OpenFileDialog();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.imgBlStatus = new System.Windows.Forms.ToolStripSplitButton();
			this.lblHEX = new System.Windows.Forms.ToolStripStatusLabel();
			this.imgHexStatus = new System.Windows.Forms.ToolStripSplitButton();
			this.lblFill = new System.Windows.Forms.ToolStripStatusLabel();
			this.lblProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.pbFlash = new System.Windows.Forms.ToolStripProgressBar();
			this.toolStrip.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtbHelp
			// 
			this.rtbHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbHelp.Location = new System.Drawing.Point(12, 28);
			this.rtbHelp.Name = "rtbHelp";
			this.rtbHelp.Size = new System.Drawing.Size(433, 208);
			this.rtbHelp.TabIndex = 0;
			this.rtbHelp.Text = resources.GetString("rtbHelp.Text");
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEnterBootloader,
            this.tsbLoadHex,
            this.tsbProgram,
            this.tsbReset});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(457, 25);
			this.toolStrip.TabIndex = 1;
			this.toolStrip.Text = "toolStrip1";
			// 
			// tsbEnterBootloader
			// 
			this.tsbEnterBootloader.Image = ((System.Drawing.Image)(resources.GetObject("tsbEnterBootloader.Image")));
			this.tsbEnterBootloader.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbEnterBootloader.Name = "tsbEnterBootloader";
			this.tsbEnterBootloader.Size = new System.Drawing.Size(115, 22);
			this.tsbEnterBootloader.Text = "Enter bootloader";
			this.tsbEnterBootloader.Click += new System.EventHandler(this.tsbEnterBootloader_Click);
			// 
			// tsbLoadHex
			// 
			this.tsbLoadHex.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadHex.Image")));
			this.tsbLoadHex.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbLoadHex.Name = "tsbLoadHex";
			this.tsbLoadHex.Size = new System.Drawing.Size(103, 22);
			this.tsbLoadHex.Text = "Load firmware";
			this.tsbLoadHex.Click += new System.EventHandler(this.tsbLoadHex_Click);
			// 
			// tsbProgram
			// 
			this.tsbProgram.Image = ((System.Drawing.Image)(resources.GetObject("tsbProgram.Image")));
			this.tsbProgram.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbProgram.Name = "tsbProgram";
			this.tsbProgram.Size = new System.Drawing.Size(73, 22);
			this.tsbProgram.Text = "Program";
			this.tsbProgram.Click += new System.EventHandler(this.tsbProgram_Click);
			// 
			// tsbReset
			// 
			this.tsbReset.Image = ((System.Drawing.Image)(resources.GetObject("tsbReset.Image")));
			this.tsbReset.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbReset.Name = "tsbReset";
			this.tsbReset.Size = new System.Drawing.Size(92, 22);
			this.tsbReset.Text = "Reset device";
			this.tsbReset.Click += new System.EventHandler(this.tsbReset_Click);
			// 
			// ofd
			// 
			this.ofd.DefaultExt = "*.hex";
			this.ofd.FileName = "ofd";
			this.ofd.Filter = "Hex files (*.hex)|*.hex|All files (*.*)|*";
			this.ofd.RestoreDirectory = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.imgBlStatus,
            this.lblHEX,
            this.imgHexStatus,
            this.lblFill,
            this.lblProgress,
            this.pbFlash});
			this.statusStrip1.Location = new System.Drawing.Point(0, 239);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.ShowItemToolTips = true;
			this.statusStrip1.Size = new System.Drawing.Size(457, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip";
			// 
			// lblStatus
			// 
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(95, 17);
			this.lblStatus.Text = "Status: unknown";
			// 
			// imgBlStatus
			// 
			this.imgBlStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.imgBlStatus.DropDownButtonWidth = 0;
			this.imgBlStatus.Image = ((System.Drawing.Image)(resources.GetObject("imgBlStatus.Image")));
			this.imgBlStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.imgBlStatus.Name = "imgBlStatus";
			this.imgBlStatus.Size = new System.Drawing.Size(21, 20);
			this.imgBlStatus.Text = "imgBlStatus";
			// 
			// lblHEX
			// 
			this.lblHEX.Name = "lblHEX";
			this.lblHEX.Size = new System.Drawing.Size(126, 17);
			this.lblHEX.Text = "Firmware .hex selected";
			// 
			// imgHexStatus
			// 
			this.imgHexStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.imgHexStatus.DropDownButtonWidth = 0;
			this.imgHexStatus.Image = ((System.Drawing.Image)(resources.GetObject("imgHexStatus.Image")));
			this.imgHexStatus.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.imgHexStatus.Name = "imgHexStatus";
			this.imgHexStatus.Size = new System.Drawing.Size(21, 20);
			this.imgHexStatus.Text = "imgFwStatus";
			// 
			// lblFill
			// 
			this.lblFill.Name = "lblFill";
			this.lblFill.Size = new System.Drawing.Size(22, 17);
			this.lblFill.Spring = true;
			// 
			// lblProgress
			// 
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(55, 17);
			this.lblProgress.Text = "Progress:";
			// 
			// pbFlash
			// 
			this.pbFlash.Name = "pbFlash";
			this.pbFlash.Size = new System.Drawing.Size(100, 16);
			this.pbFlash.Value = 1;
			// 
			// BootloaderForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(457, 261);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.rtbHelp);
			this.Name = "BootloaderForm";
			this.Text = "MUNIA Bootloader";
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbHelp;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton tsbEnterBootloader;
		private System.Windows.Forms.ToolStripButton tsbLoadHex;
		private System.Windows.Forms.ToolStripButton tsbProgram;
		private System.Windows.Forms.ToolStripButton tsbReset;
		private System.Windows.Forms.OpenFileDialog ofd;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private System.Windows.Forms.ToolStripSplitButton imgBlStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblHEX;
		private System.Windows.Forms.ToolStripSplitButton imgHexStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblProgress;
		private System.Windows.Forms.ToolStripProgressBar pbFlash;
		private System.Windows.Forms.ToolStripStatusLabel lblFill;
	}
}