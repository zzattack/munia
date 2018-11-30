namespace MUNIA.Forms {
	public partial class ControllerMapperEditor {
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
			this.dgvAxes = new System.Windows.Forms.DataGridView();
			this.dgvcSourceAxis = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvcTargetAxis = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.isTriggerDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.bsAxes = new System.Windows.Forms.BindingSource(this.components);
			this.dgvButtons = new System.Windows.Forms.DataGridView();
			this.dgvcSourceButton = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvcTargetButton = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.bsButtons = new System.Windows.Forms.BindingSource(this.components);
			this.btnFinish = new System.Windows.Forms.Button();
			this.layout = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lblButtons = new System.Windows.Forms.Label();
			this.panel2 = new System.Windows.Forms.Panel();
			this.lblAxes = new System.Windows.Forms.Label();
			this.gbPreview = new System.Windows.Forms.GroupBox();
			this.btnSeqMapSkip = new System.Windows.Forms.Button();
			this.lblSeqMapHint = new System.Windows.Forms.Label();
			this.btnMapSequentially = new System.Windows.Forms.Button();
			this.tmrSequentialMapFlasher = new System.Windows.Forms.Timer(this.components);
			this.lblHint = new System.Windows.Forms.Label();
			this.skinPreview = new MUNIA.Forms.SkinPreviewWindow();
			this.gamepadViewer = new MUNIA.Forms.GamepadViewerControl();
			((System.ComponentModel.ISupportInitialize)(this.dgvAxes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsAxes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvButtons)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsButtons)).BeginInit();
			this.layout.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.gbPreview.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvAxes
			// 
			this.dgvAxes.AllowUserToAddRows = false;
			this.dgvAxes.AllowUserToDeleteRows = false;
			this.dgvAxes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvAxes.AutoGenerateColumns = false;
			this.dgvAxes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAxes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcSourceAxis,
            this.dgvcTargetAxis,
            this.isTriggerDataGridViewCheckBoxColumn});
			this.dgvAxes.DataSource = this.bsAxes;
			this.dgvAxes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.dgvAxes.Location = new System.Drawing.Point(3, 23);
			this.dgvAxes.Name = "dgvAxes";
			this.dgvAxes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgvAxes.Size = new System.Drawing.Size(399, 149);
			this.dgvAxes.TabIndex = 1;
			this.dgvAxes.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_CurrentCellDirtyStateChanged);
			this.dgvAxes.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvButtons_EditingControlShowing);
			// 
			// dgvcSourceAxis
			// 
			this.dgvcSourceAxis.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dgvcSourceAxis.DataPropertyName = "Source";
			this.dgvcSourceAxis.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			this.dgvcSourceAxis.HeaderText = "Source";
			this.dgvcSourceAxis.MaxDropDownItems = 1;
			this.dgvcSourceAxis.Name = "dgvcSourceAxis";
			this.dgvcSourceAxis.ReadOnly = true;
			this.dgvcSourceAxis.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvcSourceAxis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// dgvcTargetAxis
			// 
			this.dgvcTargetAxis.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dgvcTargetAxis.DataPropertyName = "Target";
			this.dgvcTargetAxis.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.dgvcTargetAxis.HeaderText = "Target";
			this.dgvcTargetAxis.MaxDropDownItems = 16;
			this.dgvcTargetAxis.Name = "dgvcTargetAxis";
			this.dgvcTargetAxis.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvcTargetAxis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// isTriggerDataGridViewCheckBoxColumn
			// 
			this.isTriggerDataGridViewCheckBoxColumn.DataPropertyName = "IsTrigger";
			this.isTriggerDataGridViewCheckBoxColumn.HeaderText = "IsTrigger";
			this.isTriggerDataGridViewCheckBoxColumn.Name = "isTriggerDataGridViewCheckBoxColumn";
			// 
			// bsAxes
			// 
			this.bsAxes.DataSource = typeof(MUNIA.Controllers.ControllerMapping.AxisMap);
			// 
			// dgvButtons
			// 
			this.dgvButtons.AllowUserToAddRows = false;
			this.dgvButtons.AllowUserToDeleteRows = false;
			this.dgvButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dgvButtons.AutoGenerateColumns = false;
			this.dgvButtons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvButtons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcSourceButton,
            this.dgvcTargetButton});
			this.dgvButtons.DataSource = this.bsButtons;
			this.dgvButtons.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.dgvButtons.Location = new System.Drawing.Point(3, 26);
			this.dgvButtons.Name = "dgvButtons";
			this.dgvButtons.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgvButtons.Size = new System.Drawing.Size(399, 146);
			this.dgvButtons.TabIndex = 0;
			this.dgvButtons.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgv_CurrentCellDirtyStateChanged);
			this.dgvButtons.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvButtons_EditingControlShowing);
			// 
			// dgvcSourceButton
			// 
			this.dgvcSourceButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dgvcSourceButton.DataPropertyName = "Source";
			this.dgvcSourceButton.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
			this.dgvcSourceButton.HeaderText = "Source";
			this.dgvcSourceButton.MaxDropDownItems = 1;
			this.dgvcSourceButton.Name = "dgvcSourceButton";
			this.dgvcSourceButton.ReadOnly = true;
			this.dgvcSourceButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvcSourceButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// dgvcTargetButton
			// 
			this.dgvcTargetButton.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dgvcTargetButton.DataPropertyName = "Target";
			this.dgvcTargetButton.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.dgvcTargetButton.HeaderText = "Target";
			this.dgvcTargetButton.MaxDropDownItems = 16;
			this.dgvcTargetButton.Name = "dgvcTargetButton";
			this.dgvcTargetButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvcTargetButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// bsButtons
			// 
			this.bsButtons.DataSource = typeof(MUNIA.Controllers.ControllerMapping.ButtonMap);
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.Location = new System.Drawing.Point(713, 475);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(82, 26);
			this.btnFinish.TabIndex = 4;
			this.btnFinish.Text = "Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// layout
			// 
			this.layout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.layout.ColumnCount = 1;
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.layout.Controls.Add(this.panel1, 0, 0);
			this.layout.Controls.Add(this.panel2, 0, 1);
			this.layout.Location = new System.Drawing.Point(3, 0);
			this.layout.Name = "layout";
			this.layout.RowCount = 2;
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.layout.Size = new System.Drawing.Size(411, 362);
			this.layout.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lblButtons);
			this.panel1.Controls.Add(this.dgvButtons);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(405, 175);
			this.panel1.TabIndex = 5;
			// 
			// lblButtons
			// 
			this.lblButtons.AutoSize = true;
			this.lblButtons.Location = new System.Drawing.Point(8, 7);
			this.lblButtons.Name = "lblButtons";
			this.lblButtons.Size = new System.Drawing.Size(139, 13);
			this.lblButtons.TabIndex = 1;
			this.lblButtons.Text = "Buttons on source controller";
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.lblAxes);
			this.panel2.Controls.Add(this.dgvAxes);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 184);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(405, 175);
			this.panel2.TabIndex = 6;
			// 
			// lblAxes
			// 
			this.lblAxes.AutoSize = true;
			this.lblAxes.Location = new System.Drawing.Point(8, 7);
			this.lblAxes.Name = "lblAxes";
			this.lblAxes.Size = new System.Drawing.Size(126, 13);
			this.lblAxes.TabIndex = 2;
			this.lblAxes.Text = "Axes on source controller";
			// 
			// gbPreview
			// 
			this.gbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbPreview.Controls.Add(this.btnSeqMapSkip);
			this.gbPreview.Controls.Add(this.lblSeqMapHint);
			this.gbPreview.Controls.Add(this.btnMapSequentially);
			this.gbPreview.Location = new System.Drawing.Point(420, 260);
			this.gbPreview.Name = "gbPreview";
			this.gbPreview.Size = new System.Drawing.Size(374, 99);
			this.gbPreview.TabIndex = 6;
			this.gbPreview.TabStop = false;
			this.gbPreview.Text = "Mapping preview";
			// 
			// btnSeqMapSkip
			// 
			this.btnSeqMapSkip.Location = new System.Drawing.Point(176, 23);
			this.btnSeqMapSkip.Name = "btnSeqMapSkip";
			this.btnSeqMapSkip.Size = new System.Drawing.Size(139, 28);
			this.btnSeqMapSkip.TabIndex = 7;
			this.btnSeqMapSkip.Text = "Skip current button";
			this.btnSeqMapSkip.UseVisualStyleBackColor = true;
			this.btnSeqMapSkip.Visible = false;
			this.btnSeqMapSkip.Click += new System.EventHandler(this.btnSeqMapSkip_Click);
			// 
			// lblSeqMapHint
			// 
			this.lblSeqMapHint.Location = new System.Drawing.Point(16, 61);
			this.lblSeqMapHint.Name = "lblSeqMapHint";
			this.lblSeqMapHint.Size = new System.Drawing.Size(338, 35);
			this.lblSeqMapHint.TabIndex = 6;
			this.lblSeqMapHint.Text = "The button above will start a sequential button mapping sequence,\r\nallowing you t" +
    "o quickly define a mapping for all buttons in the skin.";
			// 
			// btnMapSequentially
			// 
			this.btnMapSequentially.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnMapSequentially.Location = new System.Drawing.Point(8, 23);
			this.btnMapSequentially.Name = "btnMapSequentially";
			this.btnMapSequentially.Size = new System.Drawing.Size(162, 28);
			this.btnMapSequentially.TabIndex = 2;
			this.btnMapSequentially.Text = "Map buttons sequentially";
			this.btnMapSequentially.UseVisualStyleBackColor = true;
			this.btnMapSequentially.Click += new System.EventHandler(this.btnMapSequentially_Click);
			// 
			// tmrSequentialMapFlasher
			// 
			this.tmrSequentialMapFlasher.Interval = 300;
			this.tmrSequentialMapFlasher.Tick += new System.EventHandler(this.tmrSequentialMapFlasher_Tick);
			// 
			// lblHint
			// 
			this.lblHint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
			this.lblHint.AutoSize = true;
			this.lblHint.BackColor = System.Drawing.Color.Red;
			this.lblHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHint.ForeColor = System.Drawing.Color.White;
			this.lblHint.Location = new System.Drawing.Point(434, 108);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(347, 25);
			this.lblHint.TabIndex = 7;
			this.lblHint.Text = "Hint: use the sequence mapper!";
			// 
			// skinPreview
			// 
			this.skinPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skinPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
			this.skinPreview.DescriptionPanelVisible = false;
			this.skinPreview.Location = new System.Drawing.Point(420, 3);
			this.skinPreview.Name = "skinPreview";
			this.skinPreview.ShowClickedElement = true;
			this.skinPreview.Size = new System.Drawing.Size(381, 251);
			this.skinPreview.TabIndex = 5;
			// 
			// gamepadViewer
			// 
			this.gamepadViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gamepadViewer.Location = new System.Drawing.Point(17, 368);
			this.gamepadViewer.Name = "gamepadViewer";
			this.gamepadViewer.Size = new System.Drawing.Size(690, 133);
			this.gamepadViewer.TabIndex = 1;
			this.gamepadViewer.Text = "gamepadViewerControl1";
			// 
			// ControllerMapperEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblHint);
			this.Controls.Add(this.gbPreview);
			this.Controls.Add(this.layout);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.skinPreview);
			this.Controls.Add(this.gamepadViewer);
			this.MinimumSize = new System.Drawing.Size(775, 329);
			this.Name = "ControllerMapperEditor";
			this.Size = new System.Drawing.Size(804, 512);
			((System.ComponentModel.ISupportInitialize)(this.dgvAxes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsAxes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvButtons)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsButtons)).EndInit();
			this.layout.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.gbPreview.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private GamepadViewerControl gamepadViewer;
		private System.Windows.Forms.DataGridView dgvButtons;
		private System.Windows.Forms.DataGridView dgvAxes;
		private System.Windows.Forms.BindingSource bsAxes;
		private System.Windows.Forms.BindingSource bsButtons;
		private System.Windows.Forms.Button btnFinish;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcSourceAxis;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcTargetAxis;
		private System.Windows.Forms.DataGridViewCheckBoxColumn isTriggerDataGridViewCheckBoxColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcSourceButton;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcTargetButton;
		private System.Windows.Forms.TableLayoutPanel layout;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblButtons;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label lblAxes;
		private SkinPreviewWindow skinPreview;
		private System.Windows.Forms.GroupBox gbPreview;
		private System.Windows.Forms.Button btnMapSequentially;
		private System.Windows.Forms.Timer tmrSequentialMapFlasher;
		private System.Windows.Forms.Label lblSeqMapHint;
		private System.Windows.Forms.Button btnSeqMapSkip;
		private System.Windows.Forms.Label lblHint;
	}
}