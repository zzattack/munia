using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using MUNIA.Util;
using OpenTK.Graphics.OpenGL;

namespace MUNIA.Skinning {
	public class NintendoSpySkin : Skin {
		public List<Button> Buttons = new List<Button>();
		public List<Stick> Sticks = new List<Stick>();
		public List<Trigger> Triggers = new List<Trigger>();
		public Dictionary<string, NspyBackground> Backgrounds = new Dictionary<string, NspyBackground>();
		public NspyBackground SelectedBackground { get; private set; }
		private Size _destSize;
		protected string _workingDir;

		public void Load(string xmlPath) {
			this.Path = xmlPath;
			LoadResult = SkinLoadResult.Fail;
			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(xmlPath);
				_workingDir = System.IO.Path.GetDirectoryName(xmlPath);
				var xroot = xdoc["skin"];

				this.Name = xroot.Attributes["name"].Value;

				string cTypeStr = xroot.Attributes["type"].Value;
				// see if a mapping is defined for this controller type
				if (!NintendoSpyMapping.TypeMap.ContainsKey(cTypeStr))
					return;

				// retrieve the mapping to munia indices
				var cType = NintendoSpyMapping.TypeMap[cTypeStr];
				Controllers.Add(cType);
				var mapping = NintendoSpyMapping.ControllerMaps[cType];

				foreach (XmlNode xnode in xroot) {
					// read the background
					if (xnode.Name == "background") {
						var bg = new NspyBackground();
						bg.Name = xnode.Attributes["name"].Value;
						bg.ImagePath = System.IO.Path.Combine(_workingDir, xnode.Attributes["image"].Value);
						Backgrounds[bg.Name] = bg;
					}

					// read the buttons
					else if (xnode.Name == "button") {
						var btn = new Button();
						btn.Id = mapping.ButtonMap[xnode.Attributes["name"].Value];
						btn.ReadLocation(xnode);
						btn.ImagePath = System.IO.Path.Combine(_workingDir, xnode.Attributes["image"].Value);
						Buttons.Add(btn);
					}

					// read the triggers
					else if (xnode.Name == "analog") {
						var trigg = new Trigger();
						trigg.Id = mapping.TriggerMap[xnode.Attributes["name"].Value];
						trigg.ReadLocation(xnode);
						trigg.ImagePath = System.IO.Path.Combine(_workingDir, xnode.Attributes["image"].Value);
						Triggers.Add(trigg);
					}

					// read the sticks
					else if (xnode.Name == "stick") {
						var stick = new Stick();
						stick.HorizontalAxis = mapping.AxisMap[xnode.Attributes["xname"].Value];
						stick.VerticalAxis = mapping.AxisMap[xnode.Attributes["yname"].Value];
						stick.ReadLocation(xnode);
						stick.XRange = int.Parse(xnode.Attributes["xrange"].Value);
						stick.YRange = int.Parse(xnode.Attributes["yrange"].Value);
						stick.ImagePath = System.IO.Path.Combine(_workingDir, xnode.Attributes["image"].Value);
						Sticks.Add(stick);
					}
				}

				SelectedBackground = Backgrounds.First().Value;

				LoadResult = SkinLoadResult.Ok;
			}
			catch {
			}
		}

		public override void Activate() {
			// texture resources are created here
			foreach (var bg in Backgrounds)
				bg.Value.LoadFromBitmap(bg.Value.ImagePath);

			Buttons.ForEach(b => {
				b.LoadFromBitmap(b.ImagePath);

			});
			Sticks.ForEach(s => {
				s.LoadFromBitmap(s.ImagePath);
			});
			Triggers.ForEach(t => {
				t.LoadFromBitmap(t.ImagePath);
			});
		}

		public override void Cleanup() {
			// cleanup
			Buttons.ForEach(b => {
				if (b.Texture != -1) GL.DeleteTexture(b.Texture);
			});
			Sticks.ForEach(s => {
				if (s.Texture != -1) GL.DeleteTexture(s.Texture);
			});
			Triggers.ForEach(t => { if (t.Texture != -1) GL.DeleteTexture(t.Texture); });
			Buttons.Clear();
			Sticks.Clear();
			Triggers.Clear();
		}

		public override void Render(int width, int height, bool force = false) {
			_destSize = new Size(width, height);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);

			GL.TexParameter(TextureTarget.ProxyTexture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.LinearMipmapNearest);
			GL.TexParameter(TextureTarget.ProxyTexture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapNearest);

			GL.BindTexture(TextureTarget.Texture2D, SelectedBackground.Texture);
			TextureHelper.RenderTexture(0, width, 0, height);

			if (State != null) {
				List<ControllerItem> all = new List<ControllerItem>();
				all.AddRange(Buttons);
				all.AddRange(Sticks);
				all.AddRange(Triggers);

				for (int i = 0; i < all.Count; i++)
					RenderItem(all[i]);
			}


			GL.Disable(EnableCap.Blend);
		}

		private void RenderItem(ControllerItem i) {
			if (i is Button b) RenderButton(b);
			if (i is Stick s) RenderStick(s);
			if (i is Trigger t) RenderTrigger(t);
		}

		private void RenderButton(Button btn) {
			bool pressed = State != null && State.Buttons[btn.Id];
			if (pressed && btn.Texture >= 0) {
				GL.BindTexture(TextureTarget.Texture2D, btn.Texture);
				TextureHelper.RenderTexture(Scale(btn.Location, btn.Size));
			}
		}
		
		private void RenderStick(Stick stick) {
			if (stick.Texture >= 0) {
				Point loc = stick.Location;
				loc.X += (int)(State.Axes[stick.HorizontalAxis] / 128.0 * stick.XRange + 0.5);
				loc.Y += (int)(State.Axes[stick.VerticalAxis] / 128.0 * stick.YRange + 0.5);
				GL.BindTexture(TextureTarget.Texture2D, stick.Texture);
				TextureHelper.RenderTexture(Scale(loc, stick.Size));
			}
		}

		private void RenderTrigger(Trigger trigger) {
			// todo
		}

		private RectangleF Scale(Point location, Size size) {
			RectangleF dest = new RectangleF();
			float scaleX = (float)_destSize.Width / SelectedBackground.Size.Width;
			float scaleY = (float)_destSize.Height / SelectedBackground.Size.Height;
			dest.X = location.X * scaleX;
			dest.Y = location.Y * scaleY;
			dest.Width = size.Width * scaleX;
			dest.Height = size.Height * scaleY;
			return dest;
		}
		
		public class ControllerItem {
			public int Id; // stick, button or trigger Id on controller
			public int Texture = -1;
			public Point Location = Point.Empty;
			public Size Size = Size.Empty;
			public string ImagePath;
			public void LoadFromBitmap(string bitmapPath) {
				using (var bm = Bitmap.FromFile(ImagePath) as Bitmap) {
					Texture = TextureHelper.CreateTexture(bm);
					Size = bm.Size;
				}
			}
			public void ReadLocation(XmlNode xnode) {
				Location = new Point {
					X = int.Parse(xnode.Attributes["x"].Value),
					Y = int.Parse(xnode.Attributes["y"].Value)
				};
			}

		}
		public class Button : ControllerItem {
		}
		public class Stick : ControllerItem {
			public float OffsetScale;
			public int HorizontalAxis;
			public int VerticalAxis;

			public double XRange;
			public double YRange;
		}

		public class Trigger : ControllerItem {
			public float OffsetScale;
			public int Axis;
		}

		public class NspyBackground : ControllerItem {
			public string Name;
		}

		public void SelectBackground(string backgroundName) {
			SelectedBackground = Backgrounds[backgroundName];
		}
	}

}