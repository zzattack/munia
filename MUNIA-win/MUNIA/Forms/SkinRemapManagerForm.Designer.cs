namespace MUNIA.Forms {
	partial class SkinRemapManagerForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkinRemapManagerForm));
			this.list = new System.Windows.Forms.ListBox();
			this.btnNew = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnFinish = new System.Windows.Forms.Button();
			this.gb = new System.Windows.Forms.GroupBox();
			this.btnExport = new System.Windows.Forms.Button();
			this.btnClone = new System.Windows.Forms.Button();
			this.lblSkinType = new System.Windows.Forms.Label();
			this.sfd = new System.Windows.Forms.SaveFileDialog();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.lblExport = new System.Windows.Forms.Label();
			this.lblHint = new System.Windows.Forms.Label();
			this.gb.SuspendLayout();
			this.SuspendLayout();
			// 
			// list
			// 
			this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.list.FormattingEnabled = true;
			this.list.Location = new System.Drawing.Point(6, 19);
			this.list.Name = "list";
			this.list.Size = new System.Drawing.Size(347, 225);
			this.list.TabIndex = 0;
			this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
			this.list.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.list_MouseDoubleClick);
			// 
			// btnNew
			// 
			this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnNew.Enabled = false;
			this.btnNew.Location = new System.Drawing.Point(6, 265);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(63, 23);
			this.btnNew.TabIndex = 1;
			this.btnNew.Text = "New";
			this.btnNew.UseVisualStyleBackColor = true;
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnEdit.Enabled = false;
			this.btnEdit.Location = new System.Drawing.Point(75, 265);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(63, 23);
			this.btnEdit.TabIndex = 2;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(213, 265);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(63, 23);
			this.btnDelete.TabIndex = 4;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFinish.Location = new System.Drawing.Point(295, 331);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(75, 23);
			this.btnFinish.TabIndex = 3;
			this.btnFinish.Text = "Finished";
			this.btnFinish.UseVisualStyleBackColor = true;
			this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// gb
			// 
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.lblHint);
			this.gb.Controls.Add(this.btnExport);
			this.gb.Controls.Add(this.btnClone);
			this.gb.Controls.Add(this.list);
			this.gb.Controls.Add(this.btnDelete);
			this.gb.Controls.Add(this.btnNew);
			this.gb.Controls.Add(this.btnEdit);
			this.gb.Location = new System.Drawing.Point(12, 24);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(359, 294);
			this.gb.TabIndex = 1;
			this.gb.TabStop = false;
			// 
			// btnExport
			// 
			this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExport.Enabled = false;
			this.btnExport.Location = new System.Drawing.Point(290, 265);
			this.btnExport.Name = "btnExport";
			this.btnExport.Size = new System.Drawing.Size(63, 23);
			this.btnExport.TabIndex = 5;
			this.btnExport.Text = "Export";
			this.btnExport.UseVisualStyleBackColor = true;
			this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
			// 
			// btnClone
			// 
			this.btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClone.Enabled = false;
			this.btnClone.Location = new System.Drawing.Point(144, 265);
			this.btnClone.Name = "btnClone";
			this.btnClone.Size = new System.Drawing.Size(63, 23);
			this.btnClone.TabIndex = 3;
			this.btnClone.Text = "Clone";
			this.btnClone.UseVisualStyleBackColor = true;
			this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
			// 
			// lblSkinType
			// 
			this.lblSkinType.AutoSize = true;
			this.lblSkinType.Location = new System.Drawing.Point(27, 9);
			this.lblSkinType.Name = "lblSkinType";
			this.lblSkinType.Size = new System.Drawing.Size(130, 13);
			this.lblSkinType.TabIndex = 0;
			this.lblSkinType.Text = "Color schemes for {0} skin";
			// 
			// sfd
			// 
			this.sfd.DefaultExt = "svg";
			this.sfd.Filter = "Scalable Vector Graphics (*.svg)|*.svg|All files (*.*)|*";
			// 
			// lblExport
			// 
			this.lblExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblExport.Location = new System.Drawing.Point(27, 321);
			this.lblExport.Name = "lblExport";
			this.lblExport.Size = new System.Drawing.Size(192, 35);
			this.lblExport.TabIndex = 2;
			this.lblExport.Text = "To make your changes permanent, export them to a new SVG file";
			// 
			// lblHint
			// 
			this.lblHint.AutoSize = true;
			this.lblHint.Location = new System.Drawing.Point(6, 247);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(308, 13);
			this.lblHint.TabIndex = 6;
			this.lblHint.Text = "Selected theme is embedded in skin. To customize, clone it first!";
			this.lblHint.Visible = false;
			// 
			// SkinRemapManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 365);
			this.Controls.Add(this.lblExport);
			this.Controls.Add(this.lblSkinType);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.gb);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(400, 386);
			this.Name = "SkinRemapManagerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Manage skin remap schemes";
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox list;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnFinish;
		private System.Windows.Forms.GroupBox gb;
		private System.Windows.Forms.Label lblSkinType;
		private System.Windows.Forms.Button btnClone;
		private System.Windows.Forms.Button btnExport;
		private System.Windows.Forms.SaveFileDialog sfd;
		private System.Windows.Forms.ToolTip tooltip;
		private System.Windows.Forms.Label lblExport;
		private System.Windows.Forms.Label lblHint;
	}
}