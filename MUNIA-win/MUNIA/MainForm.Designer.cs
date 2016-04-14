namespace MUNIA {
    partial class MainForm {
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
            this.glControl = new OpenTK.GLControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.somsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.n64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 24);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(577, 358);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(577, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.somsToolStripMenuItem,
            this.n64ToolStripMenuItem,
            this.gCToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // somsToolStripMenuItem
            // 
            this.somsToolStripMenuItem.Name = "somsToolStripMenuItem";
            this.somsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.somsToolStripMenuItem.Text = "SNES";
            this.somsToolStripMenuItem.Click += new System.EventHandler(this.somsToolStripMenuItem_Click);
            // 
            // gCToolStripMenuItem
            // 
            this.gCToolStripMenuItem.Name = "gCToolStripMenuItem";
            this.gCToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gCToolStripMenuItem.Text = "GC";
            this.gCToolStripMenuItem.Click += new System.EventHandler(this.gCToolStripMenuItem_Click);
            // 
            // n64ToolStripMenuItem
            // 
            this.n64ToolStripMenuItem.Name = "n64ToolStripMenuItem";
            this.n64ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.n64ToolStripMenuItem.Text = "N64";
            this.n64ToolStripMenuItem.Click += new System.EventHandler(this.n64ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 382);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MUNIA";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl glControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem somsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem n64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gCToolStripMenuItem;
    }
}

