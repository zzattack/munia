using MUNIA.Util;

namespace MUNIA.Forms {
	partial class SkinRemapperForm {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkinRemapperForm));
			this.lbGroups = new System.Windows.Forms.ListBox();
			this.ckbBase = new System.Windows.Forms.CheckBox();
			this.rbHighlights = new System.Windows.Forms.RadioButton();
			this.rbNonHighlights = new System.Windows.Forms.RadioButton();
			this.gbFillAndStroke = new System.Windows.Forms.GroupBox();
			this.btnRevert = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.lblColorPickInstruction = new System.Windows.Forms.Label();
			this.colorPicker = new MUNIA.Util.ColorPickerControl();
			this.pnlStroke = new System.Windows.Forms.Panel();
			this.pnlFill = new System.Windows.Forms.Panel();
			this.pbSvg = new System.Windows.Forms.PictureBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.tableLeft = new System.Windows.Forms.TableLayoutPanel();
			this.pnlTopLeft = new System.Windows.Forms.Panel();
			this.btnSaveRemap = new System.Windows.Forms.Button();
			this.tbSkinName = new System.Windows.Forms.TextBox();
			this.lblSkinName = new System.Windows.Forms.Label();
			this.gbSvgGroups = new System.Windows.Forms.GroupBox();
			this.lblItemsToShow = new System.Windows.Forms.Label();
			this.pnlBottomLeft = new System.Windows.Forms.Panel();
			this.split = new System.Windows.Forms.SplitContainer();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.gbFillAndStroke.SuspendLayout();
			this.pnlStroke.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSvg)).BeginInit();
			this.tableLeft.SuspendLayout();
			this.pnlTopLeft.SuspendLayout();
			this.gbSvgGroups.SuspendLayout();
			this.pnlBottomLeft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
			this.split.Panel1.SuspendLayout();
			this.split.Panel2.SuspendLayout();
			this.split.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbGroups
			// 
			this.lbGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbGroups.FormattingEnabled = true;
			this.lbGroups.Location = new System.Drawing.Point(6, 15);
			this.lbGroups.Name = "lbGroups";
			this.lbGroups.Size = new System.Drawing.Size(385, 95);
			this.lbGroups.TabIndex = 0;
			this.lbGroups.SelectedIndexChanged += new System.EventHandler(this.lbGroups_SelectedIndexChanged);
			// 
			// ckbBase
			// 
			this.ckbBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ckbBase.AutoSize = true;
			this.ckbBase.Checked = true;
			this.ckbBase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckbBase.Location = new System.Drawing.Point(97, 119);
			this.ckbBase.Name = "ckbBase";
			this.ckbBase.Size = new System.Drawing.Size(95, 17);
			this.ckbBase.TabIndex = 2;
			this.ckbBase.Text = "Base elements";
			this.ckbBase.UseVisualStyleBackColor = true;
			this.ckbBase.CheckedChanged += new System.EventHandler(this.ckbBase_CheckedChanged);
			// 
			// rbHighlights
			// 
			this.rbHighlights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbHighlights.AutoSize = true;
			this.rbHighlights.Location = new System.Drawing.Point(289, 118);
			this.rbHighlights.Name = "rbHighlights";
			this.rbHighlights.Size = new System.Drawing.Size(71, 17);
			this.rbHighlights.TabIndex = 4;
			this.rbHighlights.Text = "Highlights";
			this.rbHighlights.UseVisualStyleBackColor = true;
			this.rbHighlights.CheckedChanged += new System.EventHandler(this.rbHighlights_CheckedChanged);
			// 
			// rbNonHighlights
			// 
			this.rbNonHighlights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbNonHighlights.AutoSize = true;
			this.rbNonHighlights.Checked = true;
			this.rbNonHighlights.Location = new System.Drawing.Point(198, 118);
			this.rbNonHighlights.Name = "rbNonHighlights";
			this.rbNonHighlights.Size = new System.Drawing.Size(85, 17);
			this.rbNonHighlights.TabIndex = 3;
			this.rbNonHighlights.TabStop = true;
			this.rbNonHighlights.Text = "Non-pressed";
			this.rbNonHighlights.UseVisualStyleBackColor = true;
			this.rbNonHighlights.CheckedChanged += new System.EventHandler(this.rbBaseItems_CheckedChanged);
			// 
			// gbFillAndStroke
			// 
			this.gbFillAndStroke.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbFillAndStroke.Controls.Add(this.btnRevert);
			this.gbFillAndStroke.Controls.Add(this.btnSave);
			this.gbFillAndStroke.Controls.Add(this.lblColorPickInstruction);
			this.gbFillAndStroke.Controls.Add(this.colorPicker);
			this.gbFillAndStroke.Controls.Add(this.pnlStroke);
			this.gbFillAndStroke.Location = new System.Drawing.Point(11, 3);
			this.gbFillAndStroke.Name = "gbFillAndStroke";
			this.gbFillAndStroke.Size = new System.Drawing.Size(397, 340);
			this.gbFillAndStroke.TabIndex = 0;
			this.gbFillAndStroke.TabStop = false;
			this.gbFillAndStroke.Text = "Fill and stroke";
			// 
			// btnRevert
			// 
			this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRevert.Location = new System.Drawing.Point(289, 46);
			this.btnRevert.Name = "btnRevert";
			this.btnRevert.Size = new System.Drawing.Size(75, 23);
			this.btnRevert.TabIndex = 2;
			this.btnRevert.Text = "Revert";
			this.btnRevert.UseVisualStyleBackColor = true;
			this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(289, 19);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// lblColorPickInstruction
			// 
			this.lblColorPickInstruction.AutoSize = true;
			this.lblColorPickInstruction.Location = new System.Drawing.Point(21, 59);
			this.lblColorPickInstruction.Name = "lblColorPickInstruction";
			this.lblColorPickInstruction.Size = new System.Drawing.Size(247, 13);
			this.lblColorPickInstruction.TabIndex = 3;
			this.lblColorPickInstruction.Text = "Left click to change fill, right click to change stroke";
			// 
			// colorPicker
			// 
			this.colorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.colorPicker.Cursor = System.Windows.Forms.Cursors.Hand;
			this.colorPicker.Location = new System.Drawing.Point(6, 75);
			this.colorPicker.MinimumSize = new System.Drawing.Size(393, 144);
			this.colorPicker.Name = "colorPicker";
			this.colorPicker.PrimaryColor = System.Drawing.Color.Empty;
			this.colorPicker.PrimaryEnabled = false;
			this.colorPicker.SecondaryColor = System.Drawing.Color.Empty;
			this.colorPicker.SecondaryEnabled = false;
			this.colorPicker.Size = new System.Drawing.Size(393, 259);
			this.colorPicker.TabIndex = 4;
			// 
			// pnlStroke
			// 
			this.pnlStroke.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlStroke.Controls.Add(this.pnlFill);
			this.pnlStroke.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.pnlStroke.Location = new System.Drawing.Point(17, 21);
			this.pnlStroke.Name = "pnlStroke";
			this.pnlStroke.Padding = new System.Windows.Forms.Padding(5);
			this.pnlStroke.Size = new System.Drawing.Size(262, 30);
			this.pnlStroke.TabIndex = 0;
			// 
			// pnlFill
			// 
			this.pnlFill.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlFill.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.pnlFill.Location = new System.Drawing.Point(5, 5);
			this.pnlFill.Name = "pnlFill";
			this.pnlFill.Size = new System.Drawing.Size(252, 20);
			this.pnlFill.TabIndex = 0;
			// 
			// pbSvg
			// 
			this.pbSvg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbSvg.Location = new System.Drawing.Point(0, 0);
			this.pbSvg.Name = "pbSvg";
			this.pbSvg.Size = new System.Drawing.Size(442, 532);
			this.pbSvg.TabIndex = 2;
			this.pbSvg.TabStop = false;
			this.pbSvg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbSvg_MouseDown);
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 500;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// tableLeft
			// 
			this.tableLeft.ColumnCount = 1;
			this.tableLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLeft.Controls.Add(this.pnlTopLeft, 0, 0);
			this.tableLeft.Controls.Add(this.pnlBottomLeft, 0, 1);
			this.tableLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLeft.Location = new System.Drawing.Point(0, 0);
			this.tableLeft.Name = "tableLeft";
			this.tableLeft.RowCount = 2;
			this.tableLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36F));
			this.tableLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64F));
			this.tableLeft.Size = new System.Drawing.Size(424, 556);
			this.tableLeft.TabIndex = 5;
			// 
			// pnlTopLeft
			// 
			this.pnlTopLeft.Controls.Add(this.btnSaveRemap);
			this.pnlTopLeft.Controls.Add(this.tbSkinName);
			this.pnlTopLeft.Controls.Add(this.lblSkinName);
			this.pnlTopLeft.Controls.Add(this.gbSvgGroups);
			this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTopLeft.Location = new System.Drawing.Point(3, 3);
			this.pnlTopLeft.Name = "pnlTopLeft";
			this.pnlTopLeft.Size = new System.Drawing.Size(418, 194);
			this.pnlTopLeft.TabIndex = 0;
			// 
			// btnSaveRemap
			// 
			this.btnSaveRemap.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSaveRemap.Location = new System.Drawing.Point(306, 17);
			this.btnSaveRemap.Name = "btnSaveRemap";
			this.btnSaveRemap.Size = new System.Drawing.Size(102, 23);
			this.btnSaveRemap.TabIndex = 10;
			this.btnSaveRemap.Text = "Save && close";
			this.btnSaveRemap.UseVisualStyleBackColor = true;
			this.btnSaveRemap.Click += new System.EventHandler(this.btnSaveRemap_Click);
			// 
			// tbSkinName
			// 
			this.tbSkinName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbSkinName.Location = new System.Drawing.Point(82, 19);
			this.tbSkinName.Name = "tbSkinName";
			this.tbSkinName.Size = new System.Drawing.Size(212, 20);
			this.tbSkinName.TabIndex = 0;
			// 
			// lblSkinName
			// 
			this.lblSkinName.AutoSize = true;
			this.lblSkinName.Location = new System.Drawing.Point(19, 22);
			this.lblSkinName.Name = "lblSkinName";
			this.lblSkinName.Size = new System.Drawing.Size(57, 13);
			this.lblSkinName.TabIndex = 7;
			this.lblSkinName.Text = "Skin name";
			// 
			// gbSvgGroups
			// 
			this.gbSvgGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbSvgGroups.Controls.Add(this.lblItemsToShow);
			this.gbSvgGroups.Controls.Add(this.rbNonHighlights);
			this.gbSvgGroups.Controls.Add(this.rbHighlights);
			this.gbSvgGroups.Controls.Add(this.ckbBase);
			this.gbSvgGroups.Controls.Add(this.lbGroups);
			this.gbSvgGroups.Location = new System.Drawing.Point(11, 50);
			this.gbSvgGroups.Name = "gbSvgGroups";
			this.gbSvgGroups.Size = new System.Drawing.Size(397, 141);
			this.gbSvgGroups.TabIndex = 9;
			this.gbSvgGroups.TabStop = false;
			this.gbSvgGroups.Text = "SVG elements, grouped by matching fill and stroke";
			// 
			// lblItemsToShow
			// 
			this.lblItemsToShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblItemsToShow.AutoSize = true;
			this.lblItemsToShow.Location = new System.Drawing.Point(19, 120);
			this.lblItemsToShow.Name = "lblItemsToShow";
			this.lblItemsToShow.Size = new System.Drawing.Size(72, 13);
			this.lblItemsToShow.TabIndex = 1;
			this.lblItemsToShow.Text = "Items to show";
			// 
			// pnlBottomLeft
			// 
			this.pnlBottomLeft.Controls.Add(this.gbFillAndStroke);
			this.pnlBottomLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlBottomLeft.Location = new System.Drawing.Point(3, 203);
			this.pnlBottomLeft.Name = "pnlBottomLeft";
			this.pnlBottomLeft.Size = new System.Drawing.Size(418, 350);
			this.pnlBottomLeft.TabIndex = 7;
			// 
			// split
			// 
			this.split.Dock = System.Windows.Forms.DockStyle.Fill;
			this.split.Location = new System.Drawing.Point(0, 0);
			this.split.Name = "split";
			// 
			// split.Panel1
			// 
			this.split.Panel1.Controls.Add(this.tableLeft);
			this.split.Panel1MinSize = 424;
			// 
			// split.Panel2
			// 
			this.split.Panel2.Controls.Add(this.toolStrip1);
			this.split.Panel2.Controls.Add(this.pbSvg);
			this.split.Size = new System.Drawing.Size(870, 556);
			this.split.SplitterDistance = 424;
			this.split.TabIndex = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 531);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(442, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(257, 22);
			this.toolStripLabel1.Text = "Click in the skin to select corresponding groups";
			// 
			// SkinRemapperForm
			// 
			this.AcceptButton = this.btnSaveRemap;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(870, 556);
			this.Controls.Add(this.split);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(780, 580);
			this.Name = "SkinRemapperForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Skin color remapper";
			this.Load += new System.EventHandler(this.SkinRemapperForm_Load);
			this.ResizeEnd += new System.EventHandler(this.SkinRemapperForm_ResizeEnd);
			this.gbFillAndStroke.ResumeLayout(false);
			this.gbFillAndStroke.PerformLayout();
			this.pnlStroke.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbSvg)).EndInit();
			this.tableLeft.ResumeLayout(false);
			this.pnlTopLeft.ResumeLayout(false);
			this.pnlTopLeft.PerformLayout();
			this.gbSvgGroups.ResumeLayout(false);
			this.gbSvgGroups.PerformLayout();
			this.pnlBottomLeft.ResumeLayout(false);
			this.split.Panel1.ResumeLayout(false);
			this.split.Panel2.ResumeLayout(false);
			this.split.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
			this.split.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbGroups;
		private System.Windows.Forms.GroupBox gbFillAndStroke;
		private System.Windows.Forms.Panel pnlStroke;
		private System.Windows.Forms.PictureBox pbSvg;
		private System.Windows.Forms.RadioButton rbHighlights;
		private System.Windows.Forms.RadioButton rbNonHighlights;
		private System.Windows.Forms.CheckBox ckbBase;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Panel pnlFill;
		private ColorPickerControl colorPicker;
		private System.Windows.Forms.TableLayoutPanel tableLeft;
		private System.Windows.Forms.Panel pnlTopLeft;
		private System.Windows.Forms.Panel pnlBottomLeft;
		private System.Windows.Forms.SplitContainer split;
		private System.Windows.Forms.Label lblColorPickInstruction;
		private System.Windows.Forms.Button btnRevert;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox tbSkinName;
		private System.Windows.Forms.Label lblSkinName;
		private System.Windows.Forms.GroupBox gbSvgGroups;
		private System.Windows.Forms.Label lblItemsToShow;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.Button btnSaveRemap;
	}
}