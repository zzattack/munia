using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MUNIA.Controllers;
using MUNIA.Skinning;
using MUNIA.Util;

namespace MUNIA.Forms {
	public partial class SkinPreviewWindow : UserControl {
		public Skin Skin;

		internal SkinPreviewWindow() {
			InitializeComponent();
		}

		public SkinPreviewWindow(Skin skin) : this() {
			ChangeSkin(skin);
		}

		public bool DescriptionPanelVisible {
			get => pnlTop.Visible;
			set => pnlTop.Visible = value;
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

		public void RenderSkin() {
			pbPreview.BackColor = GetContrastingColor(ConfigManager.BackgroundColor);
			pbPreview.Image = SkinToBitmapRenderer.Render(Skin, this.Size, pbPreview.BackColor);
			pbPreview.Refresh();
		}

		private Color GetContrastingColor(Color color) {
			int r = color.R + (color.R < 128 ? 15 : -15);
			int g = color.G + (color.G < 128 ? 15 : -15);
			int b = color.B + (color.B < 128 ? 15 : -15);
			return Color.FromArgb(color.A, r, g, b);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing)
				components?.Dispose();

			base.Dispose(disposing);
		}

	}
}
