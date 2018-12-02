using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using MUNIA.Controllers;
using MUNIA.Util;
using OpenTK.Graphics.OpenGL;

namespace MUNIA.Skinning {
	public class NintendoSpySkin : Skin {
		public List<Button> Buttons = new List<Button>();
		public List<RangeButton> RangeButtons = new List<RangeButton>();
		public List<Stick> Sticks = new List<Stick>();
		public List<Trigger> Triggers = new List<Trigger>();
		public List<Detail> Details = new List<Detail>();
		public Dictionary<string, NSpyBackground> Backgrounds = new Dictionary<string, NSpyBackground>();
		public NSpyBackground SelectedBackground { get; private set; }
		private Size _destSize;
		protected internal string WorkingDir;

		public void Load(string xmlPath) {
			this.Path = xmlPath;
			LoadResult = SkinLoadResult.Fail;
			try {
				XmlDocument xdoc = new XmlDocument();
				xdoc.Load(xmlPath);
				WorkingDir = System.IO.Path.GetDirectoryName(xmlPath);
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
						var bg = new NSpyBackground();
						bg.Name = xnode.Attributes["name"].Value;
						bg.ImagePath = System.IO.Path.Combine(WorkingDir, xnode.Attributes["image"].Value);
						Backgrounds[bg.Name] = bg;
					}

					// read the buttons
					else if (xnode.Name == "button") {
						var btn = new Button();
						btn.Id = mapping.ButtonMap[xnode.Attributes["name"].Value];
						btn.ReadConfig(this, xnode);
						Buttons.Add(btn);
					}

					else if (xnode.Name == "rangebutton") {
						var btn = new RangeButton();
						btn.Id = mapping.ButtonMap[xnode.Attributes["name"].Value];
						btn.ReadConfig(this, xnode);
						btn.VisibleFrom = double.Parse(xnode.Attributes["from"].Value);
						btn.VisibleTo = double.Parse(xnode.Attributes["to"].Value);
						RangeButtons.Add(btn);
					}

					// read the triggers
					else if (xnode.Name == "analog") {
						var trigg = new Trigger();
						trigg.Id = mapping.AxisMap[xnode.Attributes["name"].Value];
						trigg.ReadConfig(this, xnode);
						string direction = xnode.Attributes["direction"].Value.ToLower();

						// nspy reverse/inverse is slightly different, mapping below makes rendering identical to SvgSkin
						trigg.Orientation = direction == "left" || direction == "right" ? TriggerOrientation.Horizontal : TriggerOrientation.Vertical;

						// our reverse means filling from left-to-right or bottom-to-top
						trigg.Reverse = direction == "left" || direction == "up";

						if (xnode.Attributes["reverse"] != null) {
							bool rev = bool.Parse(xnode.Attributes["reverse"].Value);
							// nspy 'reverse' is our inverse, but should be considered as another flip for right-to-left or bottom-to-top
							trigg.Reverse ^= rev;
							trigg.Inverse = rev;
						}
						Triggers.Add(trigg);
					}

					// read the sticks
					else if (xnode.Name == "stick") {
						var stick = new Stick();
						if (mapping.AxisMap.ContainsKey(xnode.Attributes["xname"].Value))
							stick.HorizontalAxis = mapping.AxisMap[xnode.Attributes["xname"].Value];
						if (mapping.AxisMap.ContainsKey(xnode.Attributes["yname"].Value))
							stick.VerticalAxis = mapping.AxisMap[xnode.Attributes["yname"].Value];

						stick.ReadConfig(this, xnode);
						stick.XReverse = bool.Parse(xnode.Attributes["xreverse"]?.Value ?? "false");
						stick.YReverse = bool.Parse(xnode.Attributes["yreverse"]?.Value ?? "false");
						stick.XRange = int.Parse(xnode.Attributes["xrange"].Value);
						stick.YRange = int.Parse(xnode.Attributes["yrange"].Value);
						Sticks.Add(stick);
					}

					// read the detail items
					else if (xnode.Name == "detail") {
						var detail = new Detail();
						detail.ReadConfig(this, xnode);
						detail.Targets = xnode.Attributes["target"].Value.Split(';').ToList();
						Details.Add(detail);
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
				bg.Value.LoadFromBitmap();

			Buttons.ForEach(b => {
				b.LoadFromBitmap();
			});
			RangeButtons.ForEach(b => {
				b.LoadFromBitmap();
			});
			Sticks.ForEach(s => {
				s.LoadFromBitmap();
			});
			Triggers.ForEach(t => {
				t.LoadFromBitmap();
			});
			Details.ForEach(d => {
				d.LoadFromBitmap();
			});
		}

		public override void Deactivate() {
			foreach (var bg in Backgrounds) {
				if (bg.Value.Texture != -1) GL.DeleteTexture(bg.Value.Texture);
				bg.Value.Texture = -1;
			}
			Buttons.ForEach(b => {
				if (b.Texture != -1) GL.DeleteTexture(b.Texture);
				b.Texture = -1;
			});
			RangeButtons.ForEach(b => {
				if (b.Texture != -1) GL.DeleteTexture(b.Texture);
				b.Texture = -1;
			});
			Sticks.ForEach(s => {
				if (s.Texture != -1) GL.DeleteTexture(s.Texture);
				s.Texture = -1;
			});
			Triggers.ForEach(t => {
				if (t.Texture != -1) GL.DeleteTexture(t.Texture);
				t.Texture = -1;
			});
			Details.ForEach(d => {
				if (d.Texture != -1) GL.DeleteTexture(d.Texture);
				d.Texture = -1;
			});
		}

		public override void Render(int width, int height, bool force = false) {
			_destSize = new Size(width, height);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.One, BlendingFactor.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);

			GL.TexParameter(TextureTarget.ProxyTexture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.LinearMipmapNearest);
			GL.TexParameter(TextureTarget.ProxyTexture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapNearest);

			GL.BindTexture(TextureTarget.Texture2D, SelectedBackground.Texture);
			TextureHelper.RenderTexture(0, width, 0, height);

			if (State != null) {
				List<ControllerItem> all = new List<ControllerItem>();
				// add the items, except the ones ignored in current skin
				all.AddRange(Buttons);
				all.AddRange(RangeButtons);
				all.AddRange(Sticks);
				all.AddRange(Triggers);
				// add the details specific for this skin
				all.AddRange(Details.Where(d => d.Targets.Contains(SelectedBackground.Name)));
				// render the items that are not ignored for this background
				foreach (var t in all.Where(ci => ci.IgnoredInSkins.All(s => s != SelectedBackground.Name)))
					RenderItem(t);
			}


			GL.Disable(EnableCap.Blend);
		}

		private void RenderItem(ControllerItem i) {
			if (i is Button b) RenderButton(b);
			if (i is RangeButton rb) RenderRangeButton(rb);
			if (i is Stick s) RenderStick(s);
			if (i is Trigger t) RenderTrigger(t);
			if (i is Detail d) RenderDetail(d);
		}

		private void RenderButton(Button btn) {
			if (btn.Id < State.Buttons.Count) {
				bool pressed = State != null && State.Buttons[btn.Id];
				if (pressed && btn.Texture >= 0) {
					GL.BindTexture(TextureTarget.Texture2D, btn.Texture);
					TextureHelper.RenderTexture(Scale(btn.Location, btn.Size));
				}
			}
		}
		private void RenderRangeButton(RangeButton btn) {
			if (State == null || btn.Texture < 0 || State.Axes.Count <= btn.Id) return;
			double val = State.Axes[btn.Id] * 128.0;
			if (btn.VisibleFrom <= val && val <= btn.VisibleTo) {
				GL.BindTexture(TextureTarget.Texture2D, btn.Texture);
				TextureHelper.RenderTexture(Scale(btn.Location, btn.Size));
			}
		}

		private void RenderStick(Stick stick) {
			if (stick.Texture >= 0) {
				Point loc = stick.Location;
				if (stick.HorizontalAxis != -1 && State.Axes.Count > stick.HorizontalAxis)
					loc.X += (int)(State.Axes[stick.HorizontalAxis] * stick.XRange + 0.5) * (stick.XReverse ? -1 : 1);
				if (stick.VerticalAxis != -1 && State.Axes.Count > stick.VerticalAxis)
					loc.Y += (int)(State.Axes[stick.VerticalAxis] * stick.YRange + 0.5) * (stick.YReverse ? -1 : 1);
				GL.BindTexture(TextureTarget.Texture2D, stick.Texture);
				TextureHelper.RenderTexture(Scale(loc, stick.Size));
			}
		}

		private void RenderTrigger(Trigger trigger) {
			if (trigger.Texture >= 0) {
				var r = Scale(trigger.Location, trigger.Size);
				float o = 0.0f;
				if (State != null && State.Axes.Count > trigger.Id)
					o = (float)State.Axes[trigger.Id];
				o *= 256.0f;
				o = trigger.Range.Clip(o);

				RectangleF crop = new RectangleF(PointF.Empty, new SizeF(1.0f, 1.0f));
				float pressRate = (o - trigger.Range.LowerBound) / (trigger.Range.UpperBound - trigger.Range.LowerBound);
				if (trigger.Inverse) pressRate = 1.0f - pressRate;

				if (trigger.Orientation == TriggerOrientation.Horizontal) {
					crop.X = trigger.Reverse ^ trigger.Inverse ? 1.0f - pressRate : 0;
					crop.Width = pressRate;
				}
				else {
					crop.Y = trigger.Reverse ^ trigger.Inverse ? 1.0f - pressRate : 0;
					crop.Height = pressRate;
				}

				// compensate texture size for crop rate
				r.X += r.Width * crop.X;
				r.Y += r.Height * crop.Y;
				r.Width *= crop.Width;
				r.Height *= crop.Height;

				GL.BindTexture(TextureTarget.Texture2D, trigger.Texture);
				TextureHelper.RenderTexture(crop, r);
			}
		}

		private void RenderDetail(Detail d) {
			if (d.Texture >= 0) {
				GL.BindTexture(TextureTarget.Texture2D, d.Texture);
				TextureHelper.RenderTexture(Scale(d.Location, d.Size));
			}
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
			public List<string> IgnoredInSkins = new List<string>();
			public void LoadFromBitmap() {
				using (var bm = Bitmap.FromFile(ImagePath) as Bitmap) {
					Texture = TextureHelper.CreateTexture(bm);
					// if set, should scale to given dimension(s), else use img dimensions
					if (Size.Width == 0) Size.Width = bm.Size.Width;
					if (Size.Height == 0) Size.Height = bm.Size.Height;
				}
			}
			public void ReadConfig(NintendoSpySkin skin, XmlNode xnode) {
				Location = new Point {
					X = int.Parse(xnode.Attributes["x"].Value),
					Y = int.Parse(xnode.Attributes["y"].Value)
				};
				if (xnode.Attributes["width"] != null) {
					Size.Width = int.Parse(xnode.Attributes["width"].Value);
				}
				if (xnode.Attributes["height"] != null) {
					Size.Height = int.Parse(xnode.Attributes["height"].Value);
				}
				if (xnode.Attributes["ignore"] != null) {
					IgnoredInSkins = xnode.Attributes["ignore"].Value.Split(';').ToList();
				}
				ImagePath = System.IO.Path.Combine(skin.WorkingDir, xnode.Attributes["image"].Value);
			}
		}
		public override string ToString() => Name;

		public class Button : ControllerItem { }

		public class RangeButton : ControllerItem {
			// simple button, but read from analog axis;
			// visible only when axis value within given range
			public double VisibleFrom;
			public double VisibleTo;
		}

		public class Detail : ControllerItem {
			public List<string> Targets;
		}

		public class Stick : ControllerItem {
			public int HorizontalAxis = -1;
			public int VerticalAxis = -1;
			public bool XReverse = false;
			public bool YReverse = false;
			public double XRange = 20;
			public double YRange = 20;
		}

		public enum TriggerOrientation {
			Vertical,
			Horizontal
		}
		public class Trigger : ControllerItem {
			public TriggerOrientation Orientation = TriggerOrientation.Vertical;
			public bool Reverse = false; // applies to 'Bar', normally fills left-to-right; Reverse-->right-to-left
			public bool Inverse = false; // applies to 'Bar', normally fills 0% when not pressed
			public Range Range = new Range(0, 255);
		}

		public class NSpyBackground : ControllerItem {
			public string Name;
		}

		public void SelectBackground(string backgroundName) {
			SelectedBackground = Backgrounds[backgroundName];
		}

		public override void GetNumberOfElements(out int numButtons, out int numAxes) {
			var buttons = Buttons.Where(b => b.Id != -1).Select(b => b.Id).Distinct();
			numButtons = Math.Max(buttons.DefaultIfEmpty(int.MinValue).Max() + 1, buttons.Count());

			var axes = Triggers.Where(t => t.Id != -1).Select(t => t.Id)
				.Union(Sticks.Where(s => s.HorizontalAxis != -1).Select(s => s.HorizontalAxis))
				.Union(Sticks.Where(s => s.VerticalAxis != -1).Select(s => s.VerticalAxis))
				.Distinct();
			numAxes = Math.Max(axes.DefaultIfEmpty(int.MinValue).Max() + 1, axes.Count());
		}

		public override bool GetElementsAtLocation(Point location, Size skinSize,
			List<ControllerMapping.Button> buttons, List<ControllerMapping.Axis[]> axes) {

			if (_destSize != skinSize)
				return false;

			foreach (var btn in Buttons) {
				if (Scale(btn.Location, btn.Size).Contains(location))
					buttons.Add((ControllerMapping.Button)btn.Id);
			}
			foreach (var btn in RangeButtons) {
				if (Scale(btn.Location, btn.Size).Contains(location))
					buttons.Add((ControllerMapping.Button)btn.Id);
			}
			foreach (var trig in Triggers) {
				if (Scale(trig.Location, trig.Size).Contains(location))
					axes.Add(new[] { (ControllerMapping.Axis)trig.Id });
			}
			foreach (var stick in Sticks) {
				if (Scale(stick.Location, stick.Size).Contains(location)) {
					axes.Add(new[] {
						(ControllerMapping.Axis)stick.HorizontalAxis,
						(ControllerMapping.Axis)stick.VerticalAxis
					});
				}
			}

			return true;
		}

	}

}