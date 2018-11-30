using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using MUNIA.Controllers;

namespace MUNIA.Forms {
	public sealed class GamepadViewerControl : Control {
		private IController _controller;
		private ControllerState _state;
		public GamepadViewerControl() {
			DoubleBuffered = true;
		}

		public bool StartTesting(IController controller) {
			StopTesting();

			if (controller.RequiresPolling)
				controller = new PollingController(controller, 1000 / 60);
			
			_controller = controller;
			_controller.StateUpdated += OnControllerStateUpdated;

			if (controller.Activate()) {
				OnControllerStateUpdated(null, null); // initial update
				return true;
			}
			return false;
		}
		
		public void StopTesting() {
			if (_controller != null) {
				_controller.StateUpdated -= OnControllerStateUpdated;
				_controller.Deactivate();
				_controller = null;
			}
		}

		private void OnControllerStateUpdated(object sender, EventArgs e) {
			_state = _controller.GetState();
			if (InvokeRequired) BeginInvoke((Action)Invalidate);
			else Invalidate();
		}

		protected override void OnPaint(PaintEventArgs pe) {
			var gfx = pe.Graphics;
			gfx.SmoothingMode = SmoothingMode.AntiAlias;

			gfx.Clear(BackColor);
			if (_state == null) {
				gfx.DrawString("No controller state yet, press some buttons", 
					new Font(DefaultFont.FontFamily, 20, FontStyle.Regular),
					Brushes.Black, 5, 5);
			}
			else {
				const int baseLineY = 4;

				int x = 10;
				int y = baseLineY + 20;
				if (_state.Hats.Any()) {
					gfx.DrawString("Hats", DefaultFont, Brushes.Black, 0, 0);
					for (int i = 0; i < _state.Hats.Count; i++) {
						var hat = _state.Hats[i];
						DrawHat(gfx, i, hat, x, y);
						x += 80;
					}
				}

				int btnX = x;
				int btnY = 20;
				const int maxButtonsPerRow = 8;
				gfx.DrawString("Buttons", DefaultFont, Brushes.Black, x, 0);
				for (int i = 0; i < _state.Buttons.Count;) {
					DrawButton(gfx, i, _state.Buttons[i], btnX, btnY);
					btnX += 40;
					++i;
					if (i % maxButtonsPerRow == 0) {
						btnX = x;
						btnY += 35;
					}
				}

				x += Math.Min(maxButtonsPerRow, _state.Buttons.Count) * 40;
				x += 10; // spacing
				y = 12;
				gfx.DrawString("Axes", DefaultFont, Brushes.Black, x, 0);
				for (int i = 0; i < _state.Axes.Count; i++) {
					DrawAxis(gfx, i, _state.Axes[i], _controller != null && _controller.IsAxisTrigger(i), x, y);
					x += 30;
				}
			}
		}

		private void DrawHat(Graphics gfx, int hatNum, Hat hat, int x, int y) {
			// set x,y to center of hat
			x += 25; y += 25;

			// construct right-pointing arrow
			var area = new Rectangle(0, 0, 24, 14);
			float bodyHeight = (area.Height / 3);
			float headWidth = (area.Width / 3);
			float bodyWidth = area.Width - headWidth;
			PointF[] points = new PointF[7];
			points[0] = new PointF(area.Left, area.Top + bodyHeight);
			points[1] = new PointF(area.Left + bodyWidth, area.Top + bodyHeight);
			points[2] = new PointF(area.Left + bodyWidth, area.Top);
			points[3] = new PointF(area.Right, area.Top + (area.Height / 2));
			points[4] = new PointF(area.Left + bodyWidth, area.Bottom);
			points[5] = new PointF(area.Left + bodyWidth, area.Bottom - bodyHeight);
			points[6] = new PointF(area.Left, area.Bottom - bodyHeight);

			for (int i = 0; i < 8; i++) {
				// 8 possible directions
				GraphicsPath path = new GraphicsPath();
				path.AddPolygon(points);

				var transform = new Matrix();
				transform.RotateAt(45.0f * i, new PointF(area.Left, area.Top + area.Height / 2));
				transform.Translate(12.0f, 0.0f); // move away from center

				transform.Translate(x, y, MatrixOrder.Append);
				path.Transform(transform);

				Hat current = ControllerState.HatLookup[(byte)((i + 2) % 8)];
				using (var pen = new Pen(Color.Black, 1.0f)) {
					gfx.DrawPath(pen, path);
				}
				gfx.FillPath(hat == current ? Brushes.Green : Brushes.Red, path);
			}
		}

		private void DrawButton(Graphics gfx, int buttonNum, bool buttonState, int x, int y) {
			var rect = new RectangleF(x, y, 26f, 26f);
			gfx.FillEllipse(buttonState ? Brushes.Green : Brushes.Red, rect);
			using (var pen = new Pen(Color.Black, 2.0f))
				gfx.DrawEllipse(pen, rect);

			var sf = new StringFormat(StringFormatFlags.FitBlackBox);
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Center;
			gfx.DrawString($"{buttonNum}", DefaultFont, Brushes.White, rect, sf);
		}

		private void DrawAxis(Graphics gfx, int axisNum, double axisValue, bool isTrigger, int x, int y) {
			var rect = new Rectangle(x, y, 12, 72);
			RectangleF fill;
			if (isTrigger) {
				fill = RectangleF.FromLTRB(rect.Left, (float)(rect.Bottom - axisValue * rect.Height), rect.Right, rect.Bottom);
			}
			else {
				float mid = rect.Top + rect.Height / 2.0f;
				if (axisValue < 0)
					fill = RectangleF.FromLTRB(rect.Left, (float)(mid - rect.Height / 2.0f * -axisValue), rect.Right, mid);
				else
					fill = RectangleF.FromLTRB(rect.Left, mid, rect.Right, (float)(mid + rect.Height / 2.0f * axisValue));
			}

			gfx.FillRectangle(Brushes.White, rect);
			gfx.FillRectangle(Brushes.Red, fill);
			using (var pen = new Pen(Color.Black, 1.0f))
				gfx.DrawRectangle(pen, rect);
			var r = new RectangleF(x, rect.Bottom + 3, rect.Width, 15);
			var sf = new StringFormat();
			sf.Alignment = StringAlignment.Center;
			sf.LineAlignment = StringAlignment.Near;
			gfx.DrawString(axisNum.ToString(), DefaultFont, Brushes.Black, r, sf);
		}

	}
}