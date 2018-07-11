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
			this.lbGroups = new System.Windows.Forms.ListBox();
			this.ckbBase = new System.Windows.Forms.CheckBox();
			this.rbHighlights = new System.Windows.Forms.RadioButton();
			this.rbNonHighlights = new System.Windows.Forms.RadioButton();
			this.gbFillAndStroke = new System.Windows.Forms.GroupBox();
			this.btnRevert = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.colorPicker = new MUNIA.Util.ColorPickerControl();
			this.pnlStroke = new System.Windows.Forms.Panel();
			this.pnlFill = new System.Windows.Forms.Panel();
			this.pbSvg = new System.Windows.Forms.PictureBox();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.tableLeft = new System.Windows.Forms.TableLayoutPanel();
			this.pnlTopLeft = new System.Windows.Forms.Panel();
			this.lblAboveBox = new System.Windows.Forms.Label();
			this.pnlBottomLeft = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gbFillAndStroke.SuspendLayout();
			this.pnlStroke.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbSvg)).BeginInit();
			this.tableLeft.SuspendLayout();
			this.pnlTopLeft.SuspendLayout();
			this.pnlBottomLeft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbGroups
			// 
			this.lbGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbGroups.FormattingEnabled = true;
			this.lbGroups.Location = new System.Drawing.Point(11, 36);
			this.lbGroups.Name = "lbGroups";
			this.lbGroups.Size = new System.Drawing.Size(397, 95);
			this.lbGroups.TabIndex = 0;
			this.lbGroups.SelectedIndexChanged += new System.EventHandler(this.lbGroups_SelectedIndexChanged);
			// 
			// ckbBase
			// 
			this.ckbBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ckbBase.AutoSize = true;
			this.ckbBase.Checked = true;
			this.ckbBase.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckbBase.Location = new System.Drawing.Point(18, 147);
			this.ckbBase.Name = "ckbBase";
			this.ckbBase.Size = new System.Drawing.Size(77, 17);
			this.ckbBase.TabIndex = 4;
			this.ckbBase.Text = "Base items";
			this.ckbBase.UseVisualStyleBackColor = true;
			this.ckbBase.CheckedChanged += new System.EventHandler(this.ckbBase_CheckedChanged);
			// 
			// rbHighlights
			// 
			this.rbHighlights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbHighlights.AutoSize = true;
			this.rbHighlights.Location = new System.Drawing.Point(107, 165);
			this.rbHighlights.Name = "rbHighlights";
			this.rbHighlights.Size = new System.Drawing.Size(71, 17);
			this.rbHighlights.TabIndex = 3;
			this.rbHighlights.Text = "Highlights";
			this.rbHighlights.UseVisualStyleBackColor = true;
			this.rbHighlights.CheckedChanged += new System.EventHandler(this.rbHighlights_CheckedChanged);
			// 
			// rbNonHighlights
			// 
			this.rbNonHighlights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.rbNonHighlights.AutoSize = true;
			this.rbNonHighlights.Checked = true;
			this.rbNonHighlights.Location = new System.Drawing.Point(16, 165);
			this.rbNonHighlights.Name = "rbNonHighlights";
			this.rbNonHighlights.Size = new System.Drawing.Size(85, 17);
			this.rbNonHighlights.TabIndex = 2;
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
			this.gbFillAndStroke.Controls.Add(this.label1);
			this.gbFillAndStroke.Controls.Add(this.colorPicker);
			this.gbFillAndStroke.Controls.Add(this.pnlStroke);
			this.gbFillAndStroke.Location = new System.Drawing.Point(11, 3);
			this.gbFillAndStroke.Name = "gbFillAndStroke";
			this.gbFillAndStroke.Size = new System.Drawing.Size(397, 340);
			this.gbFillAndStroke.TabIndex = 1;
			this.gbFillAndStroke.TabStop = false;
			this.gbFillAndStroke.Text = "Fill and stroke";
			// 
			// btnRevert
			// 
			this.btnRevert.Location = new System.Drawing.Point(289, 46);
			this.btnRevert.Name = "btnRevert";
			this.btnRevert.Size = new System.Drawing.Size(75, 23);
			this.btnRevert.TabIndex = 6;
			this.btnRevert.Text = "Revert";
			this.btnRevert.UseVisualStyleBackColor = true;
			this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(289, 19);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 5;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(21, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(247, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Left click to change fill, right click to change stroke";
			// 
			// colorPicker
			// 
			this.colorPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.colorPicker.Cursor = System.Windows.Forms.Cursors.Hand;
			this.colorPicker.Location = new System.Drawing.Point(6, 75);
			this.colorPicker.Name = "colorPicker";
			this.colorPicker.SelectedPrimaryColor = System.Drawing.SystemColors.Control;
			this.colorPicker.SelectedSecondaryColor = System.Drawing.SystemColors.Control;
			this.colorPicker.Size = new System.Drawing.Size(391, 259);
			this.colorPicker.TabIndex = 3;
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
			this.pnlStroke.TabIndex = 2;
			// 
			// pnlFill
			// 
			this.pnlFill.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlFill.ForeColor = System.Drawing.SystemColors.ButtonFace;
			this.pnlFill.Location = new System.Drawing.Point(5, 5);
			this.pnlFill.Name = "pnlFill";
			this.pnlFill.Size = new System.Drawing.Size(252, 20);
			this.pnlFill.TabIndex = 3;
			// 
			// pbSvg
			// 
			this.pbSvg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbSvg.Location = new System.Drawing.Point(0, 0);
			this.pbSvg.Name = "pbSvg";
			this.pbSvg.Size = new System.Drawing.Size(845, 556);
			this.pbSvg.TabIndex = 2;
			this.pbSvg.TabStop = false;
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
			this.pnlTopLeft.Controls.Add(this.lblAboveBox);
			this.pnlTopLeft.Controls.Add(this.lbGroups);
			this.pnlTopLeft.Controls.Add(this.ckbBase);
			this.pnlTopLeft.Controls.Add(this.rbHighlights);
			this.pnlTopLeft.Controls.Add(this.rbNonHighlights);
			this.pnlTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlTopLeft.Location = new System.Drawing.Point(3, 3);
			this.pnlTopLeft.Name = "pnlTopLeft";
			this.pnlTopLeft.Size = new System.Drawing.Size(418, 194);
			this.pnlTopLeft.TabIndex = 6;
			// 
			// lblAboveBox
			// 
			this.lblAboveBox.AutoSize = true;
			this.lblAboveBox.Location = new System.Drawing.Point(15, 15);
			this.lblAboveBox.Name = "lblAboveBox";
			this.lblAboveBox.Size = new System.Drawing.Size(244, 13);
			this.lblAboveBox.TabIndex = 5;
			this.lblAboveBox.Text = "SVG elements, grouped by matching fill and stroke";
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
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.tableLeft);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.pbSvg);
			this.splitContainer1.Size = new System.Drawing.Size(1273, 556);
			this.splitContainer1.SplitterDistance = 424;
			this.splitContainer1.TabIndex = 3;
			// 
			// SkinRemapperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1273, 556);
			this.Controls.Add(this.splitContainer1);
			this.Name = "SkinRemapperForm";
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
			this.pnlBottomLeft.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
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
		private System.Windows.Forms.Label lblAboveBox;
		private System.Windows.Forms.Panel pnlBottomLeft;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnRevert;
		private System.Windows.Forms.Button btnSave;
	}
}