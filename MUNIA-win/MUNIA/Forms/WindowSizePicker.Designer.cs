namespace MUNIA.Forms {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowSizePicker));
			this.BtnAccept = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblWidth = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.gb = new System.Windows.Forms.GroupBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.lblPx = new System.Windows.Forms.Label();
			this.nudHeight = new System.Windows.Forms.NumericUpDown();
			this.nudWidth = new System.Windows.Forms.NumericUpDown();
			this.gbOBS = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbSceneCollection = new System.Windows.Forms.ComboBox();
			this.cbSource = new System.Windows.Forms.ComboBox();
			this.lblSource = new System.Windows.Forms.Label();
			this.lblScene = new System.Windows.Forms.Label();
			this.cbScene = new System.Windows.Forms.ComboBox();
			this.gb.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
			this.gbOBS.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnAccept
			// 
			this.BtnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnAccept.Location = new System.Drawing.Point(154, 206);
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
			this.btnCancel.Location = new System.Drawing.Point(73, 206);
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
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.btnReset);
			this.gb.Controls.Add(this.label1);
			this.gb.Controls.Add(this.lblPx);
			this.gb.Controls.Add(this.nudHeight);
			this.gb.Controls.Add(this.nudWidth);
			this.gb.Controls.Add(this.label2);
			this.gb.Controls.Add(this.lblWidth);
			this.gb.Location = new System.Drawing.Point(12, 12);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(222, 80);
			this.gb.TabIndex = 0;
			this.gb.TabStop = false;
			this.gb.Text = "Inner window dimensions";
			// 
			// btnReset
			// 
			this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReset.Location = new System.Drawing.Point(184, 36);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(27, 24);
			this.btnReset.TabIndex = 4;
			this.btnReset.Text = "⟳";
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(159, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(18, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "px";
			// 
			// lblPx
			// 
			this.lblPx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPx.AutoSize = true;
			this.lblPx.Location = new System.Drawing.Point(159, 31);
			this.lblPx.Name = "lblPx";
			this.lblPx.Size = new System.Drawing.Size(18, 13);
			this.lblPx.TabIndex = 2;
			this.lblPx.Text = "px";
			// 
			// nudHeight
			// 
			this.nudHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nudHeight.Location = new System.Drawing.Point(72, 52);
			this.nudHeight.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
			this.nudHeight.Name = "nudHeight";
			this.nudHeight.Size = new System.Drawing.Size(81, 20);
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
			this.nudWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nudWidth.Location = new System.Drawing.Point(72, 29);
			this.nudWidth.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
			this.nudWidth.Name = "nudWidth";
			this.nudWidth.Size = new System.Drawing.Size(81, 20);
			this.nudWidth.TabIndex = 1;
			this.nudWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudWidth.Enter += new System.EventHandler(this.nudWidth_Enter);
			// 
			// gbOBS
			// 
			this.gbOBS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbOBS.Controls.Add(this.label3);
			this.gbOBS.Controls.Add(this.cbSceneCollection);
			this.gbOBS.Controls.Add(this.cbSource);
			this.gbOBS.Controls.Add(this.lblSource);
			this.gbOBS.Controls.Add(this.lblScene);
			this.gbOBS.Controls.Add(this.cbScene);
			this.gbOBS.Location = new System.Drawing.Point(12, 98);
			this.gbOBS.Name = "gbOBS";
			this.gbOBS.Size = new System.Drawing.Size(222, 101);
			this.gbOBS.TabIndex = 3;
			this.gbOBS.TabStop = false;
			this.gbOBS.Text = "Grab from obs-studio";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 23);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Collection";
			// 
			// cbSceneCollection
			// 
			this.cbSceneCollection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSceneCollection.FormattingEnabled = true;
			this.cbSceneCollection.Location = new System.Drawing.Point(72, 20);
			this.cbSceneCollection.Name = "cbSceneCollection";
			this.cbSceneCollection.Size = new System.Drawing.Size(141, 21);
			this.cbSceneCollection.TabIndex = 4;
			this.cbSceneCollection.SelectedIndexChanged += new System.EventHandler(this.cbSceneCollection_SelectedIndexChanged);
			// 
			// cbSource
			// 
			this.cbSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSource.FormattingEnabled = true;
			this.cbSource.Location = new System.Drawing.Point(72, 70);
			this.cbSource.Name = "cbSource";
			this.cbSource.Size = new System.Drawing.Size(141, 21);
			this.cbSource.TabIndex = 3;
			this.cbSource.SelectedIndexChanged += new System.EventHandler(this.cbSource_SelectedIndexChanged);
			// 
			// lblSource
			// 
			this.lblSource.AutoSize = true;
			this.lblSource.Location = new System.Drawing.Point(12, 73);
			this.lblSource.Name = "lblSource";
			this.lblSource.Size = new System.Drawing.Size(41, 13);
			this.lblSource.TabIndex = 2;
			this.lblSource.Text = "Source";
			// 
			// lblScene
			// 
			this.lblScene.AutoSize = true;
			this.lblScene.Location = new System.Drawing.Point(12, 48);
			this.lblScene.Name = "lblScene";
			this.lblScene.Size = new System.Drawing.Size(38, 13);
			this.lblScene.TabIndex = 1;
			this.lblScene.Text = "Scene";
			// 
			// cbScene
			// 
			this.cbScene.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbScene.FormattingEnabled = true;
			this.cbScene.Location = new System.Drawing.Point(72, 45);
			this.cbScene.Name = "cbScene";
			this.cbScene.Size = new System.Drawing.Size(141, 21);
			this.cbScene.TabIndex = 0;
			this.cbScene.SelectedIndexChanged += new System.EventHandler(this.cbScene_SelectedIndexChanged);
			// 
			// WindowSizePicker
			// 
			this.AcceptButton = this.BtnAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(241, 241);
			this.Controls.Add(this.gbOBS);
			this.Controls.Add(this.gb);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.BtnAccept);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(257, 280);
			this.Name = "WindowSizePicker";
			this.Text = "Window size";
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
			this.gbOBS.ResumeLayout(false);
			this.gbOBS.PerformLayout();
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
		private System.Windows.Forms.GroupBox gbOBS;
		private System.Windows.Forms.Label lblSource;
		private System.Windows.Forms.Label lblScene;
		private System.Windows.Forms.ComboBox cbScene;
		private System.Windows.Forms.ComboBox cbSource;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbSceneCollection;
		private System.Windows.Forms.Button btnReset;
	}
}