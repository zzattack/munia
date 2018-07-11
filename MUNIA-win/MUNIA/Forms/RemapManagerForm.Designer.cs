namespace MUNIA.Forms {
	partial class RemapManagerForm {
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
			this.list = new System.Windows.Forms.ListBox();
			this.bsRemaps = new System.Windows.Forms.BindingSource(this.components);
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.btnFinish = new System.Windows.Forms.Button();
			this.gb = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.bsRemaps)).BeginInit();
			this.gb.SuspendLayout();
			this.SuspendLayout();
			// 
			// list
			// 
			this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.list.DataSource = this.bsRemaps;
			this.list.DisplayMember = "Name";
			this.list.FormattingEnabled = true;
			this.list.Location = new System.Drawing.Point(6, 19);
			this.list.Name = "list";
			this.list.Size = new System.Drawing.Size(306, 264);
			this.list.TabIndex = 0;
			// 
			// bsRemaps
			// 
			this.bsRemaps.DataSource = typeof(MUNIA.Skins.ColorRemap);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(6, 289);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(63, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "New";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.Location = new System.Drawing.Point(75, 289);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(63, 23);
			this.button2.TabIndex = 2;
			this.button2.Text = "Edit";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.Location = new System.Drawing.Point(213, 289);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(63, 23);
			this.button3.TabIndex = 3;
			this.button3.Text = "Delete";
			this.button3.UseVisualStyleBackColor = true;
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnFinish.Location = new System.Drawing.Point(270, 355);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(75, 23);
			this.btnFinish.TabIndex = 5;
			this.btnFinish.Text = "Finished";
			this.btnFinish.UseVisualStyleBackColor = true;
			this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// gb
			// 
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.button4);
			this.gb.Controls.Add(this.list);
			this.gb.Controls.Add(this.button3);
			this.gb.Controls.Add(this.button1);
			this.gb.Controls.Add(this.button2);
			this.gb.Location = new System.Drawing.Point(12, 24);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(334, 318);
			this.gb.TabIndex = 6;
			this.gb.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(27, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Color schemes for {0} skin";
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button4.Location = new System.Drawing.Point(144, 289);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(63, 23);
			this.button4.TabIndex = 4;
			this.button4.Text = "Clone";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// RemapManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(359, 389);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.gb);
			this.Name = "RemapManagerForm";
			this.Text = "Manage remap schemes";
			((System.ComponentModel.ISupportInitialize)(this.bsRemaps)).EndInit();
			this.gb.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox list;
		private System.Windows.Forms.BindingSource bsRemaps;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button btnFinish;
		private System.Windows.Forms.GroupBox gb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button4;
	}
}