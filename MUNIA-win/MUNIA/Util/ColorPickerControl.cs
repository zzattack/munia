using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MUNIA.Util {
	[DefaultEvent("ColorPicked")]
	public class ColorPickerControl : UserControl {
		private Bitmap _canvas;
		private Graphics _graphicsBuffer;
		private LinearGradientBrush _spectrumGradient, _blackBottomGradient, _whiteTopGradient;
		private Panel pnlPrimary;
		private Panel pnlSecondary;
		private Panel pnlHover;
		private Label lblFill;
		private TextBox tbFill;
		private TextBox tbStroke;
		private Label lblStroke;
		private TextBox tbHover;
		private Label lblHover;
		private bool _primaryEnabled;
		private bool _secondaryEnabled;
		private Color _primaryColor;
		private Color _secondaryColor;

		public event EventHandler PrimaryColorPicked;
		public event EventHandler SecondaryColorPicked;

		public ColorPickerControl() {
			InitializeComponent();
			base.Cursor = Cursors.Hand;
			this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.ResizeRedraw |
						  ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
						  ControlStyles.UserPaint, true);

			this.Size = new Size(200, 100);
			UpdateLinearGradientBrushes();
			UpdateGraphicsBuffer();
		}

		protected virtual void OnPrimaryColorPicked() {
			PrimaryColorPicked?.Invoke(this, EventArgs.Empty);
		}
		protected virtual void OnSecondaryColorPicked() {
			 SecondaryColorPicked?.Invoke(this, EventArgs.Empty);
		}

		private void UpdateLinearGradientBrushes() {
			// Update spectrum gradient
			_spectrumGradient = new LinearGradientBrush(Point.Empty, new Point(this.Width, 0), Color.White, Color.White);
			ColorBlend blend = new ColorBlend();
			blend.Positions = new[] { 0, 1 / 7f, 2 / 7f, 3 / 7f, 4 / 7f, 5 / 7f, 6 / 7f, 1 };
			blend.Colors = new[] { Color.Gray, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
			_spectrumGradient.InterpolationColors = blend;
			// Update greyscale gradient
			RectangleF rect = new RectangleF(0, this.Height * 0.7f, this.Width, this.Height * 0.3F);
			_blackBottomGradient = new LinearGradientBrush(rect, Color.Transparent, Color.Black, 90f);
			rect = new RectangleF(Point.Empty, new SizeF(this.Width, this.Height * 0.3F));
			_whiteTopGradient = new LinearGradientBrush(rect, Color.White, Color.Transparent, 90f);
		}

		private void UpdateGraphicsBuffer() {
			if (this.Width > 0) {
				int pad = Height - pnlPrimary.Top - pnlPrimary.Height;
				_canvas = new Bitmap(this.Width, this.Height - pnlPrimary.Height - 2 * pad);
				_graphicsBuffer = Graphics.FromImage(_canvas);
			}
		}

		protected override void OnSizeChanged(EventArgs e) {
			base.OnSizeChanged(e);
			UpdateLinearGradientBrushes();
			UpdateGraphicsBuffer();
		}


		protected override void OnMouseMove(MouseEventArgs e) {
			base.OnMouseMove(e);

			if (new Rectangle(Point.Empty, _canvas.Size).Contains(e.Location)) {
				pnlHover.BackColor = _canvas.GetPixel(e.X, e.Y);
				tbHover.Text = pnlHover.BackColor.ToHexValue();
				lblHover.Visible = tbHover.Visible = pnlHover.Visible = true;
			}
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			if (new Rectangle(Point.Empty, _canvas.Size).Contains(e.Location)) {
				if (e.Button == MouseButtons.Left) {
					PrimaryColor = _canvas.GetPixel(e.X, e.Y);
				}
				else if (e.Button == MouseButtons.Right) {
					SecondaryColor = _canvas.GetPixel(e.X, e.Y);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e) {
			//base.OnPaint(e);
			_graphicsBuffer.FillRectangle(_spectrumGradient, this.ClientRectangle);
			_graphicsBuffer.FillRectangle(_blackBottomGradient, 0, this.Height * 0.7f + 1, this.Width, this.Height * 0.3f);
			_graphicsBuffer.FillRectangle(_whiteTopGradient, 0, 0, this.Width, this.Height * 0.3f);
			e.Graphics.DrawImageUnscaled(_canvas, Point.Empty);
		}

		public Color PrimaryColor {
			get => _primaryColor;
			set {
				bool unchanged = _primaryColor.Equals(value);
				_primaryColor = value;
				pnlPrimary.BackColor = value;

				if (!tbFill.Focused) tbFill.Text = value.ToHexValue();
				if (!unchanged && _primaryEnabled) OnPrimaryColorPicked();
			}
		}
		public Color SecondaryColor {
			get => _secondaryColor;
			set {
				bool unchanged = _secondaryColor.Equals(value);
				_secondaryColor = value;
				pnlSecondary.BackColor = value;

				pnlSecondary.BackColor = value;
				if (!tbStroke.Focused) tbStroke.Text = value.ToHexValue();
				if (!unchanged && _secondaryEnabled) OnSecondaryColorPicked();
			}
		}

		public bool PrimaryEnabled {
			set => pnlPrimary.Visible = lblFill.Visible = tbFill.Visible = _primaryEnabled = value;
			get => _primaryEnabled;
		}

		public bool SecondaryEnabled {
			set => pnlSecondary.Visible = lblStroke.Visible = tbStroke.Visible = _secondaryEnabled = value;
			get => _secondaryEnabled;
		}

		private void tbFill_TextChanged(object sender, EventArgs e) {
			if (!_primaryEnabled) return;
			try {
				var col = ColorTranslator.FromHtml(tbFill.Text);
				PrimaryColor = col;
			}
			catch { }
		}

		private void tbStroke_TextChanged(object sender, EventArgs e) {
			if (!_secondaryEnabled) return;
			try {
				var col = ColorTranslator.FromHtml(tbStroke.Text);
				SecondaryColor = col;
			}
			catch { }
		}

		private void panel_Paint(object sender, PaintEventArgs e) {
			// give them a border
			var panel = sender as Panel;
			if (panel.BorderStyle == BorderStyle.FixedSingle) {
				int thickness = 3;
				int halfThickness = thickness / 2;
				using (Pen p = new Pen(Color.Black, thickness)) {
					e.Graphics.DrawRectangle(p, new Rectangle(halfThickness,
						halfThickness,
						panel.ClientSize.Width - thickness,
						panel.ClientSize.Height - thickness));
				}
			}

		}

		private void InitializeComponent() {
			this.pnlHover = new System.Windows.Forms.Panel();
			this.pnlSecondary = new System.Windows.Forms.Panel();
			this.pnlPrimary = new System.Windows.Forms.Panel();
			this.lblFill = new System.Windows.Forms.Label();
			this.tbFill = new System.Windows.Forms.TextBox();
			this.tbStroke = new System.Windows.Forms.TextBox();
			this.lblStroke = new System.Windows.Forms.Label();
			this.tbHover = new System.Windows.Forms.TextBox();
			this.lblHover = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pnlHover
			// 
			this.pnlHover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.pnlHover.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlHover.Location = new System.Drawing.Point(330, 101);
			this.pnlHover.Name = "pnlHover";
			this.pnlHover.Size = new System.Drawing.Size(60, 40);
			this.pnlHover.TabIndex = 0;
			this.pnlHover.Visible = false;
			this.pnlHover.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
			// 
			// pnlSecondary
			// 
			this.pnlSecondary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlSecondary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlSecondary.Location = new System.Drawing.Point(196, 101);
			this.pnlSecondary.Name = "pnlSecondary";
			this.pnlSecondary.Size = new System.Drawing.Size(60, 40);
			this.pnlSecondary.TabIndex = 1;
			this.pnlSecondary.Visible = false;
			this.pnlSecondary.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
			// 
			// pnlPrimary
			// 
			this.pnlPrimary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pnlPrimary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlPrimary.Location = new System.Drawing.Point(60, 101);
			this.pnlPrimary.Name = "pnlPrimary";
			this.pnlPrimary.Size = new System.Drawing.Size(60, 40);
			this.pnlPrimary.TabIndex = 2;
			this.pnlPrimary.Visible = false;
			this.pnlPrimary.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
			// 
			// lblFill
			// 
			this.lblFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFill.AutoSize = true;
			this.lblFill.Location = new System.Drawing.Point(13, 101);
			this.lblFill.Name = "lblFill";
			this.lblFill.Size = new System.Drawing.Size(19, 13);
			this.lblFill.TabIndex = 3;
			this.lblFill.Text = "Fill";
			this.lblFill.Visible = false;
			// 
			// tbFill
			// 
			this.tbFill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tbFill.Location = new System.Drawing.Point(3, 117);
			this.tbFill.Name = "tbFill";
			this.tbFill.Size = new System.Drawing.Size(54, 20);
			this.tbFill.TabIndex = 4;
			this.tbFill.Text = "#000000";
			this.tbFill.Visible = false;
			this.tbFill.TextChanged += new System.EventHandler(this.tbFill_TextChanged);
			// 
			// tbStroke
			// 
			this.tbStroke.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tbStroke.Location = new System.Drawing.Point(128, 117);
			this.tbStroke.Name = "tbStroke";
			this.tbStroke.Size = new System.Drawing.Size(62, 20);
			this.tbStroke.TabIndex = 6;
			this.tbStroke.Text = "#000000";
			this.tbStroke.Visible = false;
			this.tbStroke.TextChanged += new System.EventHandler(this.tbStroke_TextChanged);
			// 
			// lblStroke
			// 
			this.lblStroke.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblStroke.AutoSize = true;
			this.lblStroke.Location = new System.Drawing.Point(138, 101);
			this.lblStroke.Name = "lblStroke";
			this.lblStroke.Size = new System.Drawing.Size(38, 13);
			this.lblStroke.TabIndex = 5;
			this.lblStroke.Text = "Stroke";
			this.lblStroke.Visible = false;
			// 
			// tbHover
			// 
			this.tbHover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHover.Enabled = false;
			this.tbHover.Location = new System.Drawing.Point(267, 117);
			this.tbHover.Name = "tbHover";
			this.tbHover.Size = new System.Drawing.Size(62, 20);
			this.tbHover.TabIndex = 8;
			this.tbHover.Text = "#000000";
			this.tbHover.Visible = false;
			// 
			// lblHover
			// 
			this.lblHover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHover.AutoSize = true;
			this.lblHover.Location = new System.Drawing.Point(277, 101);
			this.lblHover.Name = "lblHover";
			this.lblHover.Size = new System.Drawing.Size(36, 13);
			this.lblHover.TabIndex = 7;
			this.lblHover.Text = "Hover";
			this.lblHover.Visible = false;
			// 
			// ColorPickerControl
			// 
			this.Controls.Add(this.tbHover);
			this.Controls.Add(this.lblHover);
			this.Controls.Add(this.tbStroke);
			this.Controls.Add(this.lblStroke);
			this.Controls.Add(this.tbFill);
			this.Controls.Add(this.lblFill);
			this.Controls.Add(this.pnlPrimary);
			this.Controls.Add(this.pnlSecondary);
			this.Controls.Add(this.pnlHover);
			this.MinimumSize = new System.Drawing.Size(393, 144);
			this.Name = "ColorPickerControl";
			this.Size = new System.Drawing.Size(393, 144);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

	}
}
