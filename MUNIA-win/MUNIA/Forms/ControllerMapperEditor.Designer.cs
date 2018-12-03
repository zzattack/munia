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
			this.bsAxes = new System.Windows.Forms.BindingSource(this.components);
			this.bsButtons = new System.Windows.Forms.BindingSource(this.components);
			this.btnFinish = new System.Windows.Forms.Button();
			this.gbPreview = new System.Windows.Forms.GroupBox();
			this.btnSeqMapSkip = new System.Windows.Forms.Button();
			this.lblSeqMapHint = new System.Windows.Forms.Label();
			this.btnMapSequentially = new System.Windows.Forms.Button();
			this.tmrSequentialMapFlasher = new System.Windows.Forms.Timer(this.components);
			this.lblHint = new System.Windows.Forms.Label();
			this.tabs = new System.Windows.Forms.TabControl();
			this.tpButtons = new System.Windows.Forms.TabPage();
			this.lblButtons = new System.Windows.Forms.Label();
			this.dgvButtons = new System.Windows.Forms.DataGridView();
			this.dgvcSourceButton = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvcTargetButton = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.tpAxes = new System.Windows.Forms.TabPage();
			this.lblAxes = new System.Windows.Forms.Label();
			this.dgvAxes = new System.Windows.Forms.DataGridView();
			this.dgvcSourceAxis = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.dgvcTargetAxis = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.isTriggerDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.tpButtonsToAxes = new System.Windows.Forms.TabPage();
			this.lblButtonToAxisDescription = new System.Windows.Forms.Label();
			this.gbButtonsToAxes = new System.Windows.Forms.GroupBox();
			this.btnRemoveButtonToAxis = new System.Windows.Forms.Button();
			this.lbButtonsToAxes = new System.Windows.Forms.ListBox();
			this.bsButtonsToAxes = new System.Windows.Forms.BindingSource(this.components);
			this.btnAddButtonToAxis = new System.Windows.Forms.Button();
			this.gbButtonToAxis = new System.Windows.Forms.GroupBox();
			this.lblAxisOffsetDescription = new System.Windows.Forms.Label();
			this.lblAxisOffset = new System.Windows.Forms.Label();
			this.cbButtonToAxisSource = new System.Windows.Forms.ComboBox();
			this.tkbAxisOffset = new System.Windows.Forms.TrackBar();
			this.cbButtonToAxisTarget = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tpAxesToButtons = new System.Windows.Forms.TabPage();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnRemoveAxisToButtonMapping = new System.Windows.Forms.Button();
			this.lbAxesToButtons = new System.Windows.Forms.ListBox();
			this.bsAxesToButtons = new System.Windows.Forms.BindingSource(this.components);
			this.btnAddAxisToButtonMapping = new System.Windows.Forms.Button();
			this.gbAxisToButton = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.cbAxisToButtonSource = new System.Windows.Forms.ComboBox();
			this.lblThresholdValue = new System.Windows.Forms.Label();
			this.cbAxisToButtonTarget = new System.Windows.Forms.ComboBox();
			this.tkbButtonThreshold = new System.Windows.Forms.TrackBar();
			this.lblSourceAxis = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lblAxisToButtonTarget = new System.Windows.Forms.Label();
			this.skinPreview = new MUNIA.Forms.SkinPreviewWindow();
			this.gamepadViewer = new MUNIA.Forms.GamepadViewerControl();
			((System.ComponentModel.ISupportInitialize)(this.bsAxes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsButtons)).BeginInit();
			this.gbPreview.SuspendLayout();
			this.tabs.SuspendLayout();
			this.tpButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvButtons)).BeginInit();
			this.tpAxes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvAxes)).BeginInit();
			this.tpButtonsToAxes.SuspendLayout();
			this.gbButtonsToAxes.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsButtonsToAxes)).BeginInit();
			this.gbButtonToAxis.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tkbAxisOffset)).BeginInit();
			this.tpAxesToButtons.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bsAxesToButtons)).BeginInit();
			this.gbAxisToButton.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tkbButtonThreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// bsAxes
			// 
			this.bsAxes.DataSource = typeof(MUNIA.Controllers.ControllerMapping.AxisMap);
			// 
			// bsButtons
			// 
			this.bsButtons.DataSource = typeof(MUNIA.Controllers.ControllerMapping.ButtonMap);
			// 
			// btnFinish
			// 
			this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnFinish.Location = new System.Drawing.Point(1050, 585);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new System.Drawing.Size(82, 26);
			this.btnFinish.TabIndex = 4;
			this.btnFinish.Text = "Finish";
			this.btnFinish.UseVisualStyleBackColor = true;
			this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// gbPreview
			// 
			this.gbPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbPreview.Controls.Add(this.btnSeqMapSkip);
			this.gbPreview.Controls.Add(this.lblSeqMapHint);
			this.gbPreview.Controls.Add(this.btnMapSequentially);
			this.gbPreview.Location = new System.Drawing.Point(531, 370);
			this.gbPreview.Name = "gbPreview";
			this.gbPreview.Size = new System.Drawing.Size(600, 99);
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
			this.lblHint.Location = new System.Drawing.Point(67, 397);
			this.lblHint.Name = "lblHint";
			this.lblHint.Size = new System.Drawing.Size(347, 25);
			this.lblHint.TabIndex = 7;
			this.lblHint.Text = "Hint: use the sequence mapper!";
			// 
			// tabs
			// 
			this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabs.Controls.Add(this.tpButtons);
			this.tabs.Controls.Add(this.tpAxes);
			this.tabs.Controls.Add(this.tpButtonsToAxes);
			this.tabs.Controls.Add(this.tpAxesToButtons);
			this.tabs.Location = new System.Drawing.Point(3, 3);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(507, 466);
			this.tabs.TabIndex = 8;
			this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
			// 
			// tpButtons
			// 
			this.tpButtons.Controls.Add(this.lblButtons);
			this.tpButtons.Controls.Add(this.lblHint);
			this.tpButtons.Controls.Add(this.dgvButtons);
			this.tpButtons.Location = new System.Drawing.Point(4, 22);
			this.tpButtons.Name = "tpButtons";
			this.tpButtons.Padding = new System.Windows.Forms.Padding(3);
			this.tpButtons.Size = new System.Drawing.Size(499, 440);
			this.tpButtons.TabIndex = 0;
			this.tpButtons.Text = "Buttons";
			this.tpButtons.UseVisualStyleBackColor = true;
			// 
			// lblButtons
			// 
			this.lblButtons.AutoSize = true;
			this.lblButtons.Location = new System.Drawing.Point(5, 4);
			this.lblButtons.Name = "lblButtons";
			this.lblButtons.Size = new System.Drawing.Size(139, 13);
			this.lblButtons.TabIndex = 1;
			this.lblButtons.Text = "Buttons on source controller";
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
			this.dgvButtons.Location = new System.Drawing.Point(1, 22);
			this.dgvButtons.Name = "dgvButtons";
			this.dgvButtons.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgvButtons.Size = new System.Drawing.Size(492, 412);
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
			// tpAxes
			// 
			this.tpAxes.Controls.Add(this.lblAxes);
			this.tpAxes.Controls.Add(this.dgvAxes);
			this.tpAxes.Location = new System.Drawing.Point(4, 22);
			this.tpAxes.Name = "tpAxes";
			this.tpAxes.Padding = new System.Windows.Forms.Padding(3);
			this.tpAxes.Size = new System.Drawing.Size(499, 440);
			this.tpAxes.TabIndex = 1;
			this.tpAxes.Text = "Axes";
			this.tpAxes.UseVisualStyleBackColor = true;
			// 
			// lblAxes
			// 
			this.lblAxes.AutoSize = true;
			this.lblAxes.Location = new System.Drawing.Point(5, 4);
			this.lblAxes.Name = "lblAxes";
			this.lblAxes.Size = new System.Drawing.Size(126, 13);
			this.lblAxes.TabIndex = 2;
			this.lblAxes.Text = "Axes on source controller";
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
			this.dgvAxes.Location = new System.Drawing.Point(1, 22);
			this.dgvAxes.Name = "dgvAxes";
			this.dgvAxes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgvAxes.Size = new System.Drawing.Size(492, 412);
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
			// tpButtonsToAxes
			// 
			this.tpButtonsToAxes.Controls.Add(this.lblButtonToAxisDescription);
			this.tpButtonsToAxes.Controls.Add(this.gbButtonsToAxes);
			this.tpButtonsToAxes.Location = new System.Drawing.Point(4, 22);
			this.tpButtonsToAxes.Name = "tpButtonsToAxes";
			this.tpButtonsToAxes.Size = new System.Drawing.Size(499, 440);
			this.tpButtonsToAxes.TabIndex = 2;
			this.tpButtonsToAxes.Text = "Buttons to axes";
			this.tpButtonsToAxes.UseVisualStyleBackColor = true;
			// 
			// lblButtonToAxisDescription
			// 
			this.lblButtonToAxisDescription.Location = new System.Drawing.Point(39, 22);
			this.lblButtonToAxisDescription.Name = "lblButtonToAxisDescription";
			this.lblButtonToAxisDescription.Size = new System.Drawing.Size(339, 40);
			this.lblButtonToAxisDescription.TabIndex = 4;
			this.lblButtonToAxisDescription.Text = "Here you can define axis movement based on button presses.\r\nIf your source contro" +
    "ller lacks a Control Stick but has a D-pad, \r\nyou can map the D-pad to the Contr" +
    "ol Stick. \r\n";
			// 
			// gbButtonsToAxes
			// 
			this.gbButtonsToAxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gbButtonsToAxes.Controls.Add(this.btnRemoveButtonToAxis);
			this.gbButtonsToAxes.Controls.Add(this.lbButtonsToAxes);
			this.gbButtonsToAxes.Controls.Add(this.btnAddButtonToAxis);
			this.gbButtonsToAxes.Controls.Add(this.gbButtonToAxis);
			this.gbButtonsToAxes.Location = new System.Drawing.Point(10, 86);
			this.gbButtonsToAxes.Name = "gbButtonsToAxes";
			this.gbButtonsToAxes.Size = new System.Drawing.Size(476, 341);
			this.gbButtonsToAxes.TabIndex = 5;
			this.gbButtonsToAxes.TabStop = false;
			this.gbButtonsToAxes.Text = "Button to axis mapping";
			// 
			// btnRemoveButtonToAxis
			// 
			this.btnRemoveButtonToAxis.Location = new System.Drawing.Point(135, 253);
			this.btnRemoveButtonToAxis.Name = "btnRemoveButtonToAxis";
			this.btnRemoveButtonToAxis.Size = new System.Drawing.Size(65, 23);
			this.btnRemoveButtonToAxis.TabIndex = 3;
			this.btnRemoveButtonToAxis.Text = "Remove";
			this.btnRemoveButtonToAxis.UseVisualStyleBackColor = true;
			this.btnRemoveButtonToAxis.Click += new System.EventHandler(this.btnRemoveButtonToAxis_Click);
			// 
			// lbButtonsToAxes
			// 
			this.lbButtonsToAxes.DataSource = this.bsButtonsToAxes;
			this.lbButtonsToAxes.DisplayMember = "Name";
			this.lbButtonsToAxes.FormattingEnabled = true;
			this.lbButtonsToAxes.Location = new System.Drawing.Point(17, 35);
			this.lbButtonsToAxes.Name = "lbButtonsToAxes";
			this.lbButtonsToAxes.Size = new System.Drawing.Size(183, 212);
			this.lbButtonsToAxes.TabIndex = 0;
			this.lbButtonsToAxes.SelectedIndexChanged += new System.EventHandler(this.lbButtonsToAxes_SelectedIndexChanged);
			// 
			// bsButtonsToAxes
			// 
			this.bsButtonsToAxes.DataSource = typeof(MUNIA.Controllers.ControllerMapping.ButtonToAxisMap);
			// 
			// btnAddButtonToAxis
			// 
			this.btnAddButtonToAxis.Location = new System.Drawing.Point(64, 252);
			this.btnAddButtonToAxis.Name = "btnAddButtonToAxis";
			this.btnAddButtonToAxis.Size = new System.Drawing.Size(65, 23);
			this.btnAddButtonToAxis.TabIndex = 2;
			this.btnAddButtonToAxis.Text = "Add";
			this.btnAddButtonToAxis.UseVisualStyleBackColor = true;
			this.btnAddButtonToAxis.Click += new System.EventHandler(this.btnAddButtonToAxis_Click);
			// 
			// gbButtonToAxis
			// 
			this.gbButtonToAxis.Controls.Add(this.lblAxisOffsetDescription);
			this.gbButtonToAxis.Controls.Add(this.lblAxisOffset);
			this.gbButtonToAxis.Controls.Add(this.cbButtonToAxisSource);
			this.gbButtonToAxis.Controls.Add(this.tkbAxisOffset);
			this.gbButtonToAxis.Controls.Add(this.cbButtonToAxisTarget);
			this.gbButtonToAxis.Controls.Add(this.label3);
			this.gbButtonToAxis.Controls.Add(this.label1);
			this.gbButtonToAxis.Controls.Add(this.label2);
			this.gbButtonToAxis.Enabled = false;
			this.gbButtonToAxis.Location = new System.Drawing.Point(206, 29);
			this.gbButtonToAxis.Name = "gbButtonToAxis";
			this.gbButtonToAxis.Size = new System.Drawing.Size(264, 218);
			this.gbButtonToAxis.TabIndex = 15;
			this.gbButtonToAxis.TabStop = false;
			// 
			// lblAxisOffsetDescription
			// 
			this.lblAxisOffsetDescription.AutoSize = true;
			this.lblAxisOffsetDescription.Location = new System.Drawing.Point(8, 171);
			this.lblAxisOffsetDescription.Name = "lblAxisOffsetDescription";
			this.lblAxisOffsetDescription.Size = new System.Drawing.Size(250, 26);
			this.lblAxisOffsetDescription.TabIndex = 12;
			this.lblAxisOffsetDescription.Text = "This represents how far away from its center this \r\naxis is set when its correspo" +
    "nding button is pressed.";
			// 
			// lblAxisOffset
			// 
			this.lblAxisOffset.AutoSize = true;
			this.lblAxisOffset.Location = new System.Drawing.Point(160, 107);
			this.lblAxisOffset.Name = "lblAxisOffset";
			this.lblAxisOffset.Size = new System.Drawing.Size(46, 13);
			this.lblAxisOffset.TabIndex = 11;
			this.lblAxisOffset.Text = "Value: 0";
			// 
			// cbButtonToAxisSource
			// 
			this.cbButtonToAxisSource.FormattingEnabled = true;
			this.cbButtonToAxisSource.Location = new System.Drawing.Point(12, 29);
			this.cbButtonToAxisSource.Name = "cbButtonToAxisSource";
			this.cbButtonToAxisSource.Size = new System.Drawing.Size(157, 21);
			this.cbButtonToAxisSource.TabIndex = 4;
			this.cbButtonToAxisSource.SelectedIndexChanged += new System.EventHandler(this.cbButtonToAxisSource_SelectedIndexChanged);
			// 
			// tkbAxisOffset
			// 
			this.tkbAxisOffset.Location = new System.Drawing.Point(12, 123);
			this.tkbAxisOffset.Maximum = 100;
			this.tkbAxisOffset.Minimum = -100;
			this.tkbAxisOffset.Name = "tkbAxisOffset";
			this.tkbAxisOffset.Size = new System.Drawing.Size(231, 45);
			this.tkbAxisOffset.TabIndex = 10;
			this.tkbAxisOffset.TickFrequency = 10;
			this.tkbAxisOffset.Scroll += new System.EventHandler(this.tkbAxisOffset_Scroll);
			// 
			// cbButtonToAxisTarget
			// 
			this.cbButtonToAxisTarget.FormattingEnabled = true;
			this.cbButtonToAxisTarget.Location = new System.Drawing.Point(12, 76);
			this.cbButtonToAxisTarget.Name = "cbButtonToAxisTarget";
			this.cbButtonToAxisTarget.Size = new System.Drawing.Size(157, 21);
			this.cbButtonToAxisTarget.TabIndex = 5;
			this.cbButtonToAxisTarget.SelectedIndexChanged += new System.EventHandler(this.cbButtonToAxisTarget_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(9, 107);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(106, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Axis offset [-100,100]";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 13);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(74, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Source button";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Target axis";
			// 
			// tpAxesToButtons
			// 
			this.tpAxesToButtons.Controls.Add(this.label4);
			this.tpAxesToButtons.Controls.Add(this.groupBox1);
			this.tpAxesToButtons.Location = new System.Drawing.Point(4, 22);
			this.tpAxesToButtons.Name = "tpAxesToButtons";
			this.tpAxesToButtons.Size = new System.Drawing.Size(499, 440);
			this.tpAxesToButtons.TabIndex = 3;
			this.tpAxesToButtons.Text = "Axes to buttons";
			this.tpAxesToButtons.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(39, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(339, 40);
			this.label4.TabIndex = 6;
			this.label4.Text = "Here you can define axis movement based on button presses.\r\nIf your source contro" +
    "ller lacks a Control Stick but has a D-pad, \r\nyou can map the D-pad to the Contr" +
    "ol Stick. \r\n";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.btnRemoveAxisToButtonMapping);
			this.groupBox1.Controls.Add(this.lbAxesToButtons);
			this.groupBox1.Controls.Add(this.btnAddAxisToButtonMapping);
			this.groupBox1.Controls.Add(this.gbAxisToButton);
			this.groupBox1.Location = new System.Drawing.Point(10, 86);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(476, 341);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Button to axis mapping";
			// 
			// btnRemoveAxisToButtonMapping
			// 
			this.btnRemoveAxisToButtonMapping.Location = new System.Drawing.Point(135, 253);
			this.btnRemoveAxisToButtonMapping.Name = "btnRemoveAxisToButtonMapping";
			this.btnRemoveAxisToButtonMapping.Size = new System.Drawing.Size(65, 23);
			this.btnRemoveAxisToButtonMapping.TabIndex = 3;
			this.btnRemoveAxisToButtonMapping.Text = "Remove";
			this.btnRemoveAxisToButtonMapping.UseVisualStyleBackColor = true;
			this.btnRemoveAxisToButtonMapping.Click += new System.EventHandler(this.btnRemoveAxisToButtonMapping_Click);
			// 
			// lbAxesToButtons
			// 
			this.lbAxesToButtons.DataSource = this.bsAxesToButtons;
			this.lbAxesToButtons.DisplayMember = "Name";
			this.lbAxesToButtons.FormattingEnabled = true;
			this.lbAxesToButtons.Location = new System.Drawing.Point(17, 35);
			this.lbAxesToButtons.Name = "lbAxesToButtons";
			this.lbAxesToButtons.Size = new System.Drawing.Size(183, 212);
			this.lbAxesToButtons.TabIndex = 0;
			this.lbAxesToButtons.SelectedIndexChanged += new System.EventHandler(this.lbAxesToButtons_SelectedIndexChanged);
			// 
			// bsAxesToButtons
			// 
			this.bsAxesToButtons.DataSource = typeof(MUNIA.Controllers.ControllerMapping.AxisToButtonMap);
			// 
			// btnAddAxisToButtonMapping
			// 
			this.btnAddAxisToButtonMapping.Location = new System.Drawing.Point(64, 252);
			this.btnAddAxisToButtonMapping.Name = "btnAddAxisToButtonMapping";
			this.btnAddAxisToButtonMapping.Size = new System.Drawing.Size(65, 23);
			this.btnAddAxisToButtonMapping.TabIndex = 2;
			this.btnAddAxisToButtonMapping.Text = "Add";
			this.btnAddAxisToButtonMapping.UseVisualStyleBackColor = true;
			this.btnAddAxisToButtonMapping.Click += new System.EventHandler(this.btnAddAxisToButtonMapping_Click);
			// 
			// gbAxisToButton
			// 
			this.gbAxisToButton.Controls.Add(this.label5);
			this.gbAxisToButton.Controls.Add(this.cbAxisToButtonSource);
			this.gbAxisToButton.Controls.Add(this.lblThresholdValue);
			this.gbAxisToButton.Controls.Add(this.cbAxisToButtonTarget);
			this.gbAxisToButton.Controls.Add(this.tkbButtonThreshold);
			this.gbAxisToButton.Controls.Add(this.lblSourceAxis);
			this.gbAxisToButton.Controls.Add(this.label7);
			this.gbAxisToButton.Controls.Add(this.lblAxisToButtonTarget);
			this.gbAxisToButton.Location = new System.Drawing.Point(206, 29);
			this.gbAxisToButton.Name = "gbAxisToButton";
			this.gbAxisToButton.Size = new System.Drawing.Size(264, 218);
			this.gbAxisToButton.TabIndex = 13;
			this.gbAxisToButton.TabStop = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 125);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(250, 39);
			this.label5.TabIndex = 12;
			this.label5.Text = "This represents how far away from its center this \r\naxis needs to be moved for it" +
    "s corresponding button\r\nto be pressed.";
			// 
			// cbAxisToButtonSource
			// 
			this.cbAxisToButtonSource.FormattingEnabled = true;
			this.cbAxisToButtonSource.Location = new System.Drawing.Point(12, 29);
			this.cbAxisToButtonSource.Name = "cbAxisToButtonSource";
			this.cbAxisToButtonSource.Size = new System.Drawing.Size(157, 21);
			this.cbAxisToButtonSource.TabIndex = 4;
			this.cbAxisToButtonSource.SelectedIndexChanged += new System.EventHandler(this.cbAxisToButtonSource_SelectedIndexChanged);
			// 
			// lblThresholdValue
			// 
			this.lblThresholdValue.AutoSize = true;
			this.lblThresholdValue.Location = new System.Drawing.Point(160, 61);
			this.lblThresholdValue.Name = "lblThresholdValue";
			this.lblThresholdValue.Size = new System.Drawing.Size(46, 13);
			this.lblThresholdValue.TabIndex = 11;
			this.lblThresholdValue.Text = "Value: 0";
			// 
			// cbAxisToButtonTarget
			// 
			this.cbAxisToButtonTarget.FormattingEnabled = true;
			this.cbAxisToButtonTarget.Location = new System.Drawing.Point(12, 192);
			this.cbAxisToButtonTarget.Name = "cbAxisToButtonTarget";
			this.cbAxisToButtonTarget.Size = new System.Drawing.Size(157, 21);
			this.cbAxisToButtonTarget.TabIndex = 5;
			this.cbAxisToButtonTarget.SelectedIndexChanged += new System.EventHandler(this.cbAxisToButtonTarget_SelectedIndexChanged);
			// 
			// tkbButtonThreshold
			// 
			this.tkbButtonThreshold.Location = new System.Drawing.Point(12, 77);
			this.tkbButtonThreshold.Maximum = 100;
			this.tkbButtonThreshold.Minimum = -100;
			this.tkbButtonThreshold.Name = "tkbButtonThreshold";
			this.tkbButtonThreshold.Size = new System.Drawing.Size(231, 45);
			this.tkbButtonThreshold.TabIndex = 10;
			this.tkbButtonThreshold.TickFrequency = 10;
			this.tkbButtonThreshold.Scroll += new System.EventHandler(this.tkbButtonThreshold_Scroll);
			// 
			// lblSourceAxis
			// 
			this.lblSourceAxis.AutoSize = true;
			this.lblSourceAxis.Location = new System.Drawing.Point(9, 13);
			this.lblSourceAxis.Name = "lblSourceAxis";
			this.lblSourceAxis.Size = new System.Drawing.Size(62, 13);
			this.lblSourceAxis.TabIndex = 6;
			this.lblSourceAxis.Text = "Source axis";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 61);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(105, 13);
			this.label7.TabIndex = 9;
			this.label7.Text = "Threshold [-100,100]";
			// 
			// lblAxisToButtonTarget
			// 
			this.lblAxisToButtonTarget.AutoSize = true;
			this.lblAxisToButtonTarget.Location = new System.Drawing.Point(9, 176);
			this.lblAxisToButtonTarget.Name = "lblAxisToButtonTarget";
			this.lblAxisToButtonTarget.Size = new System.Drawing.Size(71, 13);
			this.lblAxisToButtonTarget.TabIndex = 7;
			this.lblAxisToButtonTarget.Text = "Target button";
			// 
			// skinPreview
			// 
			this.skinPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.skinPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(113)))), ((int)(((byte)(113)))), ((int)(((byte)(113)))));
			this.skinPreview.DescriptionPanelVisible = false;
			this.skinPreview.Location = new System.Drawing.Point(531, 3);
			this.skinPreview.Name = "skinPreview";
			this.skinPreview.ShowClickedElement = true;
			this.skinPreview.Size = new System.Drawing.Size(607, 361);
			this.skinPreview.TabIndex = 5;
			this.skinPreview.Resize += new System.EventHandler(this.skinPreview_Resize);
			// 
			// gamepadViewer
			// 
			this.gamepadViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gamepadViewer.Location = new System.Drawing.Point(17, 478);
			this.gamepadViewer.Name = "gamepadViewer";
			this.gamepadViewer.Size = new System.Drawing.Size(1027, 133);
			this.gamepadViewer.TabIndex = 1;
			this.gamepadViewer.Text = "gamepadViewerControl1";
			// 
			// ControllerMapperEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.gbPreview);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.skinPreview);
			this.Controls.Add(this.gamepadViewer);
			this.MinimumSize = new System.Drawing.Size(775, 329);
			this.Name = "ControllerMapperEditor";
			this.Size = new System.Drawing.Size(1141, 622);
			((System.ComponentModel.ISupportInitialize)(this.bsAxes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsButtons)).EndInit();
			this.gbPreview.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.tpButtons.ResumeLayout(false);
			this.tpButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvButtons)).EndInit();
			this.tpAxes.ResumeLayout(false);
			this.tpAxes.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvAxes)).EndInit();
			this.tpButtonsToAxes.ResumeLayout(false);
			this.gbButtonsToAxes.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bsButtonsToAxes)).EndInit();
			this.gbButtonToAxis.ResumeLayout(false);
			this.gbButtonToAxis.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tkbAxisOffset)).EndInit();
			this.tpAxesToButtons.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bsAxesToButtons)).EndInit();
			this.gbAxisToButton.ResumeLayout(false);
			this.gbAxisToButton.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.tkbButtonThreshold)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private GamepadViewerControl gamepadViewer;
		private System.Windows.Forms.BindingSource bsAxes;
		private System.Windows.Forms.BindingSource bsButtons;
		private System.Windows.Forms.Button btnFinish;
		private SkinPreviewWindow skinPreview;
		private System.Windows.Forms.GroupBox gbPreview;
		private System.Windows.Forms.Button btnMapSequentially;
		private System.Windows.Forms.Timer tmrSequentialMapFlasher;
		private System.Windows.Forms.Label lblSeqMapHint;
		private System.Windows.Forms.Button btnSeqMapSkip;
		private System.Windows.Forms.Label lblHint;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage tpButtons;
		private System.Windows.Forms.Label lblButtons;
		private System.Windows.Forms.DataGridView dgvButtons;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcSourceButton;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcTargetButton;
		private System.Windows.Forms.TabPage tpAxes;
		private System.Windows.Forms.Label lblAxes;
		private System.Windows.Forms.DataGridView dgvAxes;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcSourceAxis;
		private System.Windows.Forms.DataGridViewComboBoxColumn dgvcTargetAxis;
		private System.Windows.Forms.DataGridViewCheckBoxColumn isTriggerDataGridViewCheckBoxColumn;
		private System.Windows.Forms.TabPage tpButtonsToAxes;
		private System.Windows.Forms.TabPage tpAxesToButtons;
		private System.Windows.Forms.ListBox lbButtonsToAxes;
		private System.Windows.Forms.Label lblButtonToAxisDescription;
		private System.Windows.Forms.GroupBox gbButtonsToAxes;
		private System.Windows.Forms.Button btnRemoveButtonToAxis;
		private System.Windows.Forms.Button btnAddButtonToAxis;
		private System.Windows.Forms.TrackBar tkbAxisOffset;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbButtonToAxisTarget;
		private System.Windows.Forms.ComboBox cbButtonToAxisSource;
		private System.Windows.Forms.Label lblAxisOffset;
		private System.Windows.Forms.Label lblAxisOffsetDescription;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblThresholdValue;
		private System.Windows.Forms.TrackBar tkbButtonThreshold;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblAxisToButtonTarget;
		private System.Windows.Forms.Label lblSourceAxis;
		private System.Windows.Forms.ComboBox cbAxisToButtonTarget;
		private System.Windows.Forms.ComboBox cbAxisToButtonSource;
		private System.Windows.Forms.Button btnRemoveAxisToButtonMapping;
		private System.Windows.Forms.ListBox lbAxesToButtons;
		private System.Windows.Forms.Button btnAddAxisToButtonMapping;
		private System.Windows.Forms.GroupBox gbButtonToAxis;
		private System.Windows.Forms.GroupBox gbAxisToButton;
		private System.Windows.Forms.BindingSource bsAxesToButtons;
		private System.Windows.Forms.BindingSource bsButtonsToAxes;
	}
}