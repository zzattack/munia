namespace MUNIA.Forms {
	partial class ControllerMapperManager {
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
			this.lblHeader = new System.Windows.Forms.Label();
			this.lbControllerSource = new System.Windows.Forms.ListBox();
			this.lbTargetControllerType = new System.Windows.Forms.ListBox();
			this.gb = new System.Windows.Forms.GroupBox();
			this.btnClone = new System.Windows.Forms.Button();
			this.lblPickMapping = new System.Windows.Forms.Label();
			this.lblPickSkin = new System.Windows.Forms.Label();
			this.lblPickTarget = new System.Windows.Forms.Label();
			this.lblPickController = new System.Windows.Forms.Label();
			this.btnRemoveMapping = new System.Windows.Forms.Button();
			this.btnAddMapping = new System.Windows.Forms.Button();
			this.lblStep4 = new System.Windows.Forms.Label();
			this.lbMappings = new System.Windows.Forms.ListBox();
			this.lblStep3 = new System.Windows.Forms.Label();
			this.lbSkins = new System.Windows.Forms.ListBox();
			this.lblStep2 = new System.Windows.Forms.Label();
			this.lblStep1 = new System.Windows.Forms.Label();
			this.btnContinue = new System.Windows.Forms.Button();
			this.lblContinue = new System.Windows.Forms.Label();
			this.lblMappingIncompatible = new System.Windows.Forms.Label();
			this.lblMappingBuiltIn = new System.Windows.Forms.Label();
			this.gb.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblHeader
			// 
			this.lblHeader.AutoSize = true;
			this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHeader.Location = new System.Drawing.Point(12, 20);
			this.lblHeader.Name = "lblHeader";
			this.lblHeader.Size = new System.Drawing.Size(748, 40);
			this.lblHeader.TabIndex = 0;
			this.lblHeader.Text = "Here you can define a mapping between generic, i.e. non-MUNIA controllers, and an" +
    "y of the loaded skins.\r\nThis will allow you to use any skin with any controller," +
    " without modifying the skin itself.";
			// 
			// lbControllerSource
			// 
			this.lbControllerSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbControllerSource.FormattingEnabled = true;
			this.lbControllerSource.Location = new System.Drawing.Point(15, 60);
			this.lbControllerSource.Name = "lbControllerSource";
			this.lbControllerSource.Size = new System.Drawing.Size(132, 238);
			this.lbControllerSource.TabIndex = 1;
			this.lbControllerSource.SelectedIndexChanged += new System.EventHandler(this.lbControllerSource_SelectedIndexChanged);
			this.lbControllerSource.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbControllerSource_MouseDown);
			// 
			// lbTargetControllerType
			// 
			this.lbTargetControllerType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbTargetControllerType.FormattingEnabled = true;
			this.lbTargetControllerType.Location = new System.Drawing.Point(173, 60);
			this.lbTargetControllerType.Name = "lbTargetControllerType";
			this.lbTargetControllerType.Size = new System.Drawing.Size(132, 238);
			this.lbTargetControllerType.TabIndex = 2;
			this.lbTargetControllerType.SelectedIndexChanged += new System.EventHandler(this.lbTargetControllerType_SelectedIndexChanged);
			this.lbTargetControllerType.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbControllerSource_MouseDown);
			// 
			// gb
			// 
			this.gb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gb.Controls.Add(this.btnClone);
			this.gb.Controls.Add(this.lblPickMapping);
			this.gb.Controls.Add(this.lblPickSkin);
			this.gb.Controls.Add(this.lblPickTarget);
			this.gb.Controls.Add(this.lblPickController);
			this.gb.Controls.Add(this.btnRemoveMapping);
			this.gb.Controls.Add(this.btnAddMapping);
			this.gb.Controls.Add(this.lblStep4);
			this.gb.Controls.Add(this.lbMappings);
			this.gb.Controls.Add(this.lblStep3);
			this.gb.Controls.Add(this.lbSkins);
			this.gb.Controls.Add(this.lblStep2);
			this.gb.Controls.Add(this.lblStep1);
			this.gb.Controls.Add(this.lbControllerSource);
			this.gb.Controls.Add(this.lbTargetControllerType);
			this.gb.Location = new System.Drawing.Point(15, 76);
			this.gb.Name = "gb";
			this.gb.Size = new System.Drawing.Size(746, 347);
			this.gb.TabIndex = 3;
			this.gb.TabStop = false;
			this.gb.Text = "Controller and skin selection";
			// 
			// btnClone
			// 
			this.btnClone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClone.Location = new System.Drawing.Point(545, 304);
			this.btnClone.Name = "btnClone";
			this.btnClone.Size = new System.Drawing.Size(54, 23);
			this.btnClone.TabIndex = 15;
			this.btnClone.Text = "Clone";
			this.btnClone.UseVisualStyleBackColor = true;
			this.btnClone.Click += new System.EventHandler(this.btnClone_Click);
			// 
			// lblPickMapping
			// 
			this.lblPickMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPickMapping.AutoSize = true;
			this.lblPickMapping.Location = new System.Drawing.Point(12, 307);
			this.lblPickMapping.Name = "lblPickMapping";
			this.lblPickMapping.Size = new System.Drawing.Size(274, 26);
			this.lblPickMapping.TabIndex = 14;
			this.lblPickMapping.Text = "Finally, choose whether to modify an existing mapping or \r\nclick the button to st" +
    "art defining a new one.";
			// 
			// lblPickSkin
			// 
			this.lblPickSkin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPickSkin.AutoSize = true;
			this.lblPickSkin.Location = new System.Drawing.Point(12, 307);
			this.lblPickSkin.Name = "lblPickSkin";
			this.lblPickSkin.Size = new System.Drawing.Size(465, 26);
			this.lblPickSkin.TabIndex = 13;
			this.lblPickSkin.Text = "Now pick any skin that will be used to preview your mapping.\r\nNote: even though y" +
    "ou select a single skin here, your mapping will work for any of the listed ones." +
    "";
			// 
			// lblPickTarget
			// 
			this.lblPickTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPickTarget.AutoSize = true;
			this.lblPickTarget.Location = new System.Drawing.Point(12, 307);
			this.lblPickTarget.Name = "lblPickTarget";
			this.lblPickTarget.Size = new System.Drawing.Size(333, 26);
			this.lblPickTarget.TabIndex = 12;
			this.lblPickTarget.Text = "Next, pick the controller type for which you want to define a mapping.\r\nThis narr" +
    "ows down the list of skins to choose from.";
			// 
			// lblPickController
			// 
			this.lblPickController.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblPickController.AutoSize = true;
			this.lblPickController.Location = new System.Drawing.Point(12, 307);
			this.lblPickController.Name = "lblPickController";
			this.lblPickController.Size = new System.Drawing.Size(421, 26);
			this.lblPickController.TabIndex = 11;
			this.lblPickController.Text = "First select the (generic) controller for which you wish to create a new mapping." +
    "\r\nNote: in fact, even MUNIA and NintendoSpy controllers can be remapped onto any" +
    " skin";
			// 
			// btnRemoveMapping
			// 
			this.btnRemoveMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveMapping.Location = new System.Drawing.Point(605, 304);
			this.btnRemoveMapping.Name = "btnRemoveMapping";
			this.btnRemoveMapping.Size = new System.Drawing.Size(60, 23);
			this.btnRemoveMapping.TabIndex = 10;
			this.btnRemoveMapping.Text = "Remove";
			this.btnRemoveMapping.UseVisualStyleBackColor = true;
			this.btnRemoveMapping.Click += new System.EventHandler(this.btnRemoveMapping_Click);
			// 
			// btnAddMapping
			// 
			this.btnAddMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddMapping.Location = new System.Drawing.Point(671, 304);
			this.btnAddMapping.Name = "btnAddMapping";
			this.btnAddMapping.Size = new System.Drawing.Size(54, 23);
			this.btnAddMapping.TabIndex = 9;
			this.btnAddMapping.Text = "Add";
			this.btnAddMapping.UseVisualStyleBackColor = true;
			this.btnAddMapping.Click += new System.EventHandler(this.btnAddMapping_Click);
			// 
			// lblStep4
			// 
			this.lblStep4.AutoSize = true;
			this.lblStep4.Location = new System.Drawing.Point(525, 25);
			this.lblStep4.Name = "lblStep4";
			this.lblStep4.Size = new System.Drawing.Size(134, 26);
			this.lblStep4.TabIndex = 8;
			this.lblStep4.Text = "Step 4:\r\nSelect or create a mapping";
			// 
			// lbMappings
			// 
			this.lbMappings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMappings.FormattingEnabled = true;
			this.lbMappings.Location = new System.Drawing.Point(523, 60);
			this.lbMappings.Name = "lbMappings";
			this.lbMappings.Size = new System.Drawing.Size(202, 238);
			this.lbMappings.TabIndex = 7;
			this.lbMappings.SelectedIndexChanged += new System.EventHandler(this.lbMappings_SelectedIndexChanged);
			this.lbMappings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbMappings_MouseDoubleClick);
			this.lbMappings.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbControllerSource_MouseDown);
			// 
			// lblStep3
			// 
			this.lblStep3.AutoSize = true;
			this.lblStep3.Location = new System.Drawing.Point(334, 25);
			this.lblStep3.Name = "lblStep3";
			this.lblStep3.Size = new System.Drawing.Size(103, 26);
			this.lblStep3.TabIndex = 6;
			this.lblStep3.Text = "Step 3: \r\nPick an existing skin";
			// 
			// lbSkins
			// 
			this.lbSkins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbSkins.FormattingEnabled = true;
			this.lbSkins.Location = new System.Drawing.Point(332, 60);
			this.lbSkins.Name = "lbSkins";
			this.lbSkins.Size = new System.Drawing.Size(168, 238);
			this.lbSkins.TabIndex = 5;
			this.lbSkins.SelectedIndexChanged += new System.EventHandler(this.lbSkins_SelectedIndexChanged);
			this.lbSkins.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbControllerSource_MouseDown);
			// 
			// lblStep2
			// 
			this.lblStep2.AutoSize = true;
			this.lblStep2.Location = new System.Drawing.Point(176, 25);
			this.lblStep2.Name = "lblStep2";
			this.lblStep2.Size = new System.Drawing.Size(107, 26);
			this.lblStep2.TabIndex = 4;
			this.lblStep2.Text = "Step 2: \r\nTarget controller type";
			// 
			// lblStep1
			// 
			this.lblStep1.AutoSize = true;
			this.lblStep1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblStep1.Location = new System.Drawing.Point(21, 25);
			this.lblStep1.Name = "lblStep1";
			this.lblStep1.Size = new System.Drawing.Size(100, 26);
			this.lblStep1.TabIndex = 3;
			this.lblStep1.Text = "Step 1: \r\nSelect controller";
			// 
			// btnContinue
			// 
			this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnContinue.Location = new System.Drawing.Point(686, 442);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 23);
			this.btnContinue.TabIndex = 4;
			this.btnContinue.Text = "Continue";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// lblContinue
			// 
			this.lblContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblContinue.AutoSize = true;
			this.lblContinue.Location = new System.Drawing.Point(437, 447);
			this.lblContinue.Name = "lblContinue";
			this.lblContinue.Size = new System.Drawing.Size(228, 13);
			this.lblContinue.TabIndex = 9;
			this.lblContinue.Text = "On the next screen the mapping will be defined";
			// 
			// lblMappingIncompatible
			// 
			this.lblMappingIncompatible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMappingIncompatible.AutoSize = true;
			this.lblMappingIncompatible.Location = new System.Drawing.Point(400, 447);
			this.lblMappingIncompatible.Name = "lblMappingIncompatible";
			this.lblMappingIncompatible.Size = new System.Drawing.Size(265, 13);
			this.lblMappingIncompatible.TabIndex = 11;
			this.lblMappingIncompatible.Text = "Selected mapping does not apply to selected controller";
			this.lblMappingIncompatible.Visible = false;
			// 
			// lblMappingBuiltIn
			// 
			this.lblMappingBuiltIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMappingBuiltIn.AutoSize = true;
			this.lblMappingBuiltIn.Location = new System.Drawing.Point(283, 447);
			this.lblMappingBuiltIn.Name = "lblMappingBuiltIn";
			this.lblMappingBuiltIn.Size = new System.Drawing.Size(382, 13);
			this.lblMappingBuiltIn.TabIndex = 12;
			this.lblMappingBuiltIn.Text = "Select mapping is a built-in mapping and cannot be modified, but may be cloned";
			this.lblMappingBuiltIn.Visible = false;
			// 
			// ControllerMapperManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblMappingBuiltIn);
			this.Controls.Add(this.lblMappingIncompatible);
			this.Controls.Add(this.lblContinue);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.lblHeader);
			this.Controls.Add(this.gb);
			this.MinimumSize = new System.Drawing.Size(775, 329);
			this.Name = "ControllerMapperManager";
			this.Size = new System.Drawing.Size(775, 474);
			this.gb.ResumeLayout(false);
			this.gb.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblHeader;
		private System.Windows.Forms.ListBox lbControllerSource;
		private System.Windows.Forms.ListBox lbTargetControllerType;
		private System.Windows.Forms.GroupBox gb;
		private System.Windows.Forms.Button btnRemoveMapping;
		private System.Windows.Forms.Button btnAddMapping;
		private System.Windows.Forms.Label lblStep4;
		private System.Windows.Forms.ListBox lbMappings;
		private System.Windows.Forms.Label lblStep3;
		private System.Windows.Forms.ListBox lbSkins;
		private System.Windows.Forms.Label lblStep2;
		private System.Windows.Forms.Label lblStep1;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Label lblContinue;
		private System.Windows.Forms.Label lblPickMapping;
		private System.Windows.Forms.Label lblPickSkin;
		private System.Windows.Forms.Label lblPickTarget;
		private System.Windows.Forms.Label lblPickController;
		private System.Windows.Forms.Label lblMappingIncompatible;
		private System.Windows.Forms.Button btnClone;
		private System.Windows.Forms.Label lblMappingBuiltIn;
	}
}