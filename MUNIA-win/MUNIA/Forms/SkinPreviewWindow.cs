using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Skinning;
using MUNIA.Util;

namespace MUNIA.Forms {
	public partial class SkinPreviewWindow : UserControl {
		public SkinPreviewWindow() {
			InitializeComponent();
			BackColor = GetContrastingColor(ConfigManager.BackgroundColor);
		}

		public Skin Skin;

		public SkinPreviewWindow(Skin skin) : this() {
			ChangeSkin(skin);
		}

		public bool DescriptionPanelVisible {
			get => pnlTop.Visible;
			set => pnlTop.Visible = value;
		}

		public bool ShowClickedElement {
			get => lblClickedItems.Visible;
			set {
				lblClickedItems.Visible = value;

				if (value) pbPreview.MouseDown += OnMouseDown;
				else pbPreview.MouseDown -= OnMouseDown;
			}
		}

		private void tmrHideLabel_Tick(object sender, System.EventArgs e) {
			lblClickedItems.Visible = false;
			tmrHideLabel.Stop();
		}

		private void OnMouseDown(object sender, MouseEventArgs e) {
			var buttons = new List<ControllerMapping.Button>();
			var axes = new List<ControllerMapping.Axis[]>();
			if (Skin.GetElementsAtLocation(e.Location, pbPreview.Size, buttons, axes)) {
				var sb = new StringBuilder();
				// show which element was just clicked
				foreach (var btn in buttons) {
					if (btn != ControllerMapping.Button.Unmapped) {
						sb.Append(btn);
						sb.Append(", ");
					}
				}
				// show which element was just clicked
				foreach (var axis in axes) {
					if (axis.Length == 1 && axis[0] != ControllerMapping.Axis.Unmapped) {
						sb.Append($"the trigger on axis {axis}, ");
					}
					else if (axis.Length == 2) {
						sb.Append($"a stick (horizontal axis: {axis[0]}, vertical axis: {axis[1]}), ");
					}
				}

				if (sb.Length > 0) {
					sb.Remove(sb.Length - 2, 2); // remove trailing ", "
					sb.Insert(0, "You clicked on ");
					lblClickedItems.Text = sb.ToString();
				}
				else {
					lblClickedItems.Text = "Mouse click did not match any elements!";
				}


				lblClickedItems.ForeColor = Color.Red;
				lblClickedItems.Font = new Font(lblClickedItems.Font.FontFamily, 12);
				lblClickedItems.Visible = true;
				tmrHideLabel.Stop(); // reset timer
				tmrHideLabel.Start();
			}

		}
		private Color GetContrastingColor(Color color) {
			int r = color.R + (color.R < 128 ? 15 : -15);
			int g = color.G + (color.G < 128 ? 15 : -15);
			int b = color.B + (color.B < 128 ? 15 : -15);
			return Color.FromArgb(color.A, r, g, b);
		}


		public void ChangeSkin(Skin skin) {
			if (Skin != null && Skin != ConfigManager.ActiveSkin) {
				// cleanup temporary skin but not if this one is active in main window!
				Skin?.Deactivate();
			}

			Skin = skin;
			lblSkinPath.Text = skin.Name + " - " + skin.Path;

			// create a dummy state with just a bunch of non-pressed buttons, axes and hats
			var controllerState = new ControllerState(new List<double>(), new List<bool>(), new List<Hat>());
			controllerState.Buttons.EnsureSize(32);
			controllerState.Axes.EnsureSize(16);
			controllerState.Hats.EnsureSize(4);
			Skin.UpdateState(controllerState);
		}

		public void RenderSkin(ControllerState state) {
			Skin.UpdateState(state);
			if (pbPreview.Image?.Size != pbPreview.Size) {
				pbPreview.Image?.Dispose();
				pbPreview.Image = new Bitmap(pbPreview.Width, pbPreview.Height, PixelFormat.Format32bppArgb);
			}
			SkinToBitmapRenderer.Render(Skin, pbPreview.Size, pbPreview.BackColor, pbPreview.Image as Bitmap);
			pbPreview.Invalidate();
		}

	}
}
