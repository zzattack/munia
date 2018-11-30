namespace MUNIA.Forms {
	partial class ControllerMapperForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControllerMapperForm));
			this.mapperManager = new MUNIA.Forms.ControllerMapperManager();
			this.SuspendLayout();
			// 
			// mapperManager
			// 
			this.mapperManager.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mapperManager.Location = new System.Drawing.Point(0, 0);
			this.mapperManager.MinimumSize = new System.Drawing.Size(775, 329);
			this.mapperManager.Name = "mapperManager";
			this.mapperManager.Size = new System.Drawing.Size(813, 574);
			this.mapperManager.TabIndex = 0;
			// 
			// ControllerMapperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(813, 574);
			this.Controls.Add(this.mapperManager);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ControllerMapperForm";
			this.Text = "Controller mapper";
			this.ResumeLayout(false);

		}

		#endregion

		private ControllerMapperManager mapperManager;
	}
}