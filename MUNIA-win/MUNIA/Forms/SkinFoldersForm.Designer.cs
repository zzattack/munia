namespace MUNIA.Forms {
	partial class SkinFoldersForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SkinFoldersForm));
			this.list = new System.Windows.Forms.ListBox();
			this.btnMoveUp = new System.Windows.Forms.Button();
			this.btnMoveDown = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.gb = new System.Windows.Forms.GroupBox();
			this.ckbPadPyght = new System.Windows.Forms.CheckBox();
			this.ckbNintendoSpy = new System.Windows.Forms.CheckBox();
			this.lblSkinTypes = new System.Windows.Forms.Label();
			this.ckbSvgSkins = new System.Windows.Forms.CheckBox();
			this.btnFinish = new System.Windows.Forms.Button();
			this.gb.SuspendLayout();
			this.SuspendLayout();
			// 
			// list
			// 
			this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.list.FormattingEnabled = true;
			this.list.Location = new System.Drawing.Point(19, 27);
			this.list.Name = "list";
			this.list.Size = new System.Drawing.Size(367, 186);
			this.list.TabIndex = 0;
			this.list.SelectedIndexChanged += new System.EventHandler(this.list_SelectedIndexChanged);
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveUp.Location = new System.Drawing.Point(392, 109);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.Size = new System.Drawing.Size(75, 23);
			this.btnMoveUp.TabIndex = 3;
			this.btnMoveUp.Text = "Move up";
			this.btnMoveUp.UseVisualStyleBackColor = true;
			this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveDown.Location = new System.Drawing.Point(392, 138);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.Size = new System.Drawing.Size(75, 23);
			this.btnMoveDown.TabIndex = 4;
			this.btnMoveDown.Text = "Move down";
			this.btnMoveDown.UseVisualStyleBackColor = true;
			this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemove.Location = new System.Drawing.Point(392, 68);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 2;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(392, 39);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// gb
			// 
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.ckbPadPyght);
			this.gb.Controls.Add(this.ckbNintendoSpy);
			this.gb.Controls.Add(this.lblSkinTypes);
			this.gb.Controls.Add(this.ckbSvgSkins);
			this.gb.Controls.Add(this.list);
			this.gb.Controls.Add(this.btnAdd);
			this.gb.Controls.Add(this.btnRemove);
			this.gb.Controls.Add(this.btnMoveUp);
			this.gb.Controls.Add(this.btnMoveDown);
			this.gb.Location = new System.Drawing.Point(12, 12);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(485, 244);
			this.gb.TabIndex = 0;
			this.gb.TabStop = false;
			this.gb.Text = "Folder list in which skins are searched for";
			// 
			// ckbPadPyght
			// 
			this.ckbPadPyght.AutoSize = true;
			this.ckbPadPyght.Location = new System.Drawing.Point(272, 221);
			this.ckbPadPyght.Name = "ckbPadPyght";
			this.ckbPadPyght.Size = new System.Drawing.Size(72, 17);
			this.ckbPadPyght.TabIndex = 8;
			this.ckbPadPyght.Text = "PadPyght";
			this.ckbPadPyght.UseVisualStyleBackColor = true;
			this.ckbPadPyght.CheckedChanged += new System.EventHandler(this.ckbPadPyght_CheckedChanged);
			// 
			// ckbNintendoSpy
			// 
			this.ckbNintendoSpy.AutoSize = true;
			this.ckbNintendoSpy.Location = new System.Drawing.Point(179, 221);
			this.ckbNintendoSpy.Name = "ckbNintendoSpy";
			this.ckbNintendoSpy.Size = new System.Drawing.Size(87, 17);
			this.ckbNintendoSpy.TabIndex = 7;
			this.ckbNintendoSpy.Text = "NintendoSpy";
			this.ckbNintendoSpy.UseVisualStyleBackColor = true;
			this.ckbNintendoSpy.CheckedChanged += new System.EventHandler(this.ckbNintendoSpy_CheckedChanged);
			// 
			// lblSkinTypes
			// 
			this.lblSkinTypes.AutoSize = true;
			this.lblSkinTypes.Location = new System.Drawing.Point(34, 222);
			this.lblSkinTypes.Name = "lblSkinTypes";
			this.lblSkinTypes.Size = new System.Drawing.Size(56, 13);
			this.lblSkinTypes.TabIndex = 5;
			this.lblSkinTypes.Text = "Skin types";
			// 
			// ckbSvgSkins
			// 
			this.ckbSvgSkins.AutoSize = true;
			this.ckbSvgSkins.Location = new System.Drawing.Point(113, 221);
			this.ckbSvgSkins.Name = "ckbSvgSkins";
			this.ckbSvgSkins.Size = new System.Drawing.Size(48, 17);
			this.ckbSvgSkins.TabIndex = 6;
			this.ckbSvgSkins.Text = "SVG";
			this.ckbSvgSkins.UseVisualStyleBackColor = true;
			this.ckbSvgSkins.CheckedChanged += new System.EventHandler(this.ckbSvgSkins_CheckedChanged);
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFinish.Location = new System.Drawing.Point(422, 262);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(75, 23);
			this.btnFinish.TabIndex = 1;
			this.btnFinish.Text = "Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			// 
			// SkinFoldersForm
			// 
			this.AcceptButton = this.btnFinish;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(515, 298);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.gb);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(428, 268);
			this.Name = "SkinFoldersForm";
			this.Text = "Skin folders manager";
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox list;
		private System.Windows.Forms.Button btnMoveUp;
		private System.Windows.Forms.Button btnMoveDown;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox gb;
		private System.Windows.Forms.Button btnFinish;
		private System.Windows.Forms.CheckBox ckbPadPyght;
		private System.Windows.Forms.CheckBox ckbNintendoSpy;
		private System.Windows.Forms.Label lblSkinTypes;
		private System.Windows.Forms.CheckBox ckbSvgSkins;
	}
}