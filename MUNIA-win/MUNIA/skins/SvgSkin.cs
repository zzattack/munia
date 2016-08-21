using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using MUNIA.Controllers;
using OpenTK.Graphics.OpenGL;
using Svg;

namespace MUNIA.Skins {
	public class SvgSkin : Skin {
		private SvgDocument _svgDocument;
		public List<Button> Buttons = new List<Button>();
		public List<Stick> Sticks = new List<Stick>();
		public List<Trigger> Triggers = new List<Trigger>();
		private SizeF _dimensions;
		private int _baseTexture;

		public void Load(string svgPath) {
			try {
				Path = svgPath;
				_svgDocument = SvgDocument.Open(svgPath);
				_dimensions = _svgDocument.GetDimensions();

				// cleanup
				Buttons.ForEach(b => {
					if (b.PressedTexture != -1) GL.DeleteTexture(b.PressedTexture);
					if (b.Texture != -1) GL.DeleteTexture(b.Texture);
				});
				Sticks.ForEach(s => { if (s.Texture != -1) GL.DeleteTexture(s.Texture); });
				Triggers.ForEach(t => { if (t.Texture != -1) GL.DeleteTexture(t.Texture); });
				Buttons.Clear();
				Sticks.Clear();
				Triggers.Clear();

				// load button/stick/trigger mapping from svg
				RecursiveGetElements(_svgDocument);
				LoadResult = Controllers.Any() ? SkinLoadResult.Ok : SkinLoadResult.Fail;
			}
			catch { LoadResult = SkinLoadResult.Fail; }
		}

		private void RecursiveGetElements(SvgElement e) {
			foreach (var c in e.Children) {
				if (c.ElementName == "info") {
					if (c.CustomAttributes.ContainsKey("device-name")) {
						string devName = c.CustomAttributes["device-name"];
						// match on name for compatibility reasons
						if (devName == "NinHID SNES") Controllers.Add(ControllerType.SNES);
						else if (devName == "NinHID N64") Controllers.Add(ControllerType.N64);
						else if (devName == "NinHID NGC") Controllers.Add(ControllerType.NGC);
					}

					if (c.CustomAttributes.ContainsKey("device-type")) {
						string devType = c.CustomAttributes["device-type"];
						ControllerType type;
						if (Enum.TryParse(devType, true, out type))
							Controllers.Add(type);
					}

					Name = c.CustomAttributes["skin-name"];
				}

				if (c.ContainsAttribute("button-id")) {
					var b = c as SvgVisualElement;
					int id = int.Parse(c.CustomAttributes["button-id"]);
					bool pressed = c.CustomAttributes["button-state"] == "pressed";
					Buttons.EnsureSize(id);

					if (c.ContainsAttribute("z-index"))
						Buttons[id].Z = int.Parse(c.CustomAttributes["z-index"]);

					if (pressed)
						Buttons[id].Pressed = b;
					else
						Buttons[id].Element = b;
				}

				else if (c.ContainsAttribute("stick-id")) {
					var s = c as SvgVisualElement;
					int id = int.Parse(c.CustomAttributes["stick-id"]);
					Sticks.EnsureSize(id);
					Sticks[id].Element = s;
					Sticks[id].HorizontalAxis = int.Parse(c.CustomAttributes["axis-h"]);
					Sticks[id].VerticalAxis = int.Parse(c.CustomAttributes["axis-v"]);
					Sticks[id].OffsetScale = float.Parse(c.CustomAttributes["offset-scale"], CultureInfo.InvariantCulture);

					if (c.ContainsAttribute("z-index"))
						Sticks[id].Z = int.Parse(c.CustomAttributes["z-index"]);
					else
						Sticks[id].Z = 1;
				}

				else if (c.ContainsAttribute("trigger-id")) {
					var t = c as SvgVisualElement;
					int id = int.Parse(c.CustomAttributes["trigger-id"]);
					Triggers.EnsureSize(id);
					Triggers[id].Element = t;
					Triggers[id].Axis = int.Parse(c.CustomAttributes["trigger-axis"]);
					Triggers[id].OffsetScale = float.Parse(c.CustomAttributes["offset-scale"], CultureInfo.InvariantCulture);
					if (c.ContainsAttribute("z-index"))
						Triggers[id].Z = int.Parse(c.CustomAttributes["z-index"]);
					else
						Triggers[id].Z = -1;
				}
				RecursiveGetElements(c);
			}
		}

		public override void Render(int width, int height) {
			if (_svgDocument == null || width == 0 || height == 0) return;
			if (_svgDocument.Height != height || _svgDocument.Width != width) {
				RenderBase(width, height);
			}
			Render();
		}

		public void Render() {
			List<Tuple<ControllerItem, int>> all = new List<Tuple<ControllerItem, int>>();
			all.AddRange(Buttons.Select((b, idx) => Tuple.Create((ControllerItem)b, idx)));
			all.AddRange(Sticks.Select((b, idx) => Tuple.Create((ControllerItem)b, idx)));
			all.AddRange(Triggers.Select((b, idx) => Tuple.Create((ControllerItem)b, idx)));
			all.Sort((tuple, tuple1) => tuple.Item1.Z.CompareTo(tuple1.Item1.Z));

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);

			foreach (var ci in all.Where(x => x.Item1.Z < 0))
				RenderItem(ci.Item1, ci.Item2);

			GL.BindTexture(TextureTarget.Texture2D, _baseTexture);
			RenderTexture(0, _svgDocument.Width, 0, _svgDocument.Height);

			foreach (var ci in all.Where(x => x.Item1.Z >= 0))
				RenderItem(ci.Item1, ci.Item2);
			
			GL.Disable(EnableCap.Blend);
		}

		private void RenderItem(ControllerItem i, int itemidx) {
			if (i is Button) RenderButton(itemidx);
			if (i is Stick) RenderStick(itemidx);
			if (i is Trigger) RenderTrigger(itemidx);
		}
		private void RenderButton(int i) {
			var btn = Buttons[i];
			bool pressed = State != null && State.Buttons[i];
			if (pressed && btn.Pressed != null) {
				GL.BindTexture(TextureTarget.Texture2D, btn.PressedTexture);
				RenderTexture(btn.PressedBounds);
			}
			else if (!pressed && btn.Element != null) {
				GL.BindTexture(TextureTarget.Texture2D, btn.Texture);
				RenderTexture(btn.Bounds);
			}
		}
		private void RenderStick(int i) {
			var stick = Sticks[i];
			var r = stick.Bounds;
			float x, y;
			if (State != null) {
				x = State.Axes[stick.HorizontalAxis];
				y = State.Axes[stick.VerticalAxis];
			}
			else {
				x = y = 0f;
			}

			SizeF img = GetCorrectedDimensions(new SizeF(_svgDocument.Width, _svgDocument.Height));
			x *= img.Width / _dimensions.Width * stick.OffsetScale;
			y *= img.Height / _dimensions.Height * stick.OffsetScale;
			r.Offset(new PointF(x, y));

			GL.BindTexture(TextureTarget.Texture2D, stick.Texture);
			RenderTexture(r);
		}
		
		private void RenderTrigger(int i) {
			var trigger = Triggers[i];
			var r = trigger.Bounds;
			float o = State?.Axes[trigger.Axis] ?? 0f;

			SizeF img = GetCorrectedDimensions(new SizeF(_svgDocument.Width, _svgDocument.Height));
			o *= img.Height / _dimensions.Height * trigger.OffsetScale;

			r.Offset(new PointF(0, o));
			GL.BindTexture(TextureTarget.Texture2D, trigger.Texture);
			RenderTexture(r);
		}

		private void RenderBase(int width, int height) {
			_svgDocument.Height = height;
			_svgDocument.Width = width;

			// hide just the changable elements first
			SetVisibleRecursive(_svgDocument, true);
			foreach (var b in Buttons) {
				if (b.Pressed != null) b.Pressed.Visible = false;
				if (b.Element != null) b.Element.Visible = false;
			}
			foreach (var s in Sticks) {
				s.Element.Visible = false;
			}
			foreach (var t in Triggers) {
				t.Element.Visible = false;
			}

			var baseImg = _svgDocument.Draw();
			_baseTexture = TextureHelper.CreateTexture(baseImg);

			// System.IO.Directory.CreateDirectory(Name);
			// baseImg.Save(Name + "/base_texture.png");

			// hide everything
			SetVisibleRecursive(_svgDocument, false);
			var work = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			foreach (var btn in Buttons) {
				if (btn.Pressed != null) {
					var tb = SvgElementToTexture(work, btn.Pressed);
					btn.PressedTexture = tb.Item1;
					btn.PressedBounds = tb.Item2;
				}
				if (btn.Element != null) {
					var tb = SvgElementToTexture(work, btn.Element);
					btn.Texture = tb.Item1;
					btn.Bounds = tb.Item2;
				}
			}

			foreach (var trig in Triggers) {
				if (trig.Element != null) {
					var tb = SvgElementToTexture(work, trig.Element);
					trig.Texture = tb.Item1;
					trig.Bounds = tb.Item2;
				}
			}

			foreach (var stick in Sticks) {
				if (stick.Element != null) {
					var tb = SvgElementToTexture(work, stick.Element);
					stick.Texture = tb.Item1;
					stick.Bounds = tb.Item2;
				}
			}
		}

		private Tuple<int, RectangleF> SvgElementToTexture(Bitmap work, SvgVisualElement e) {
			var bounds = CalcBounds(e);
			var l = Unproject(bounds.Location);
			var s = Unproject(new PointF(bounds.Right, bounds.Bottom));
			var boundsScaled = RectangleF.FromLTRB(l.X, l.Y, s.X, s.Y);
			boundsScaled.Inflate(5f, 5f); // small margins
			boundsScaled.Intersect(new RectangleF(0f, 0f, work.Width, work.Height));
			int ret = -1;
			if (!boundsScaled.IsEmpty) {
				// unhide temporarily
				SetVisibleToRoot(e, true);
				SetVisibleRecursive(e, true);

				_svgDocument.Draw(work);
				// work.Clone(boundsScaled, work.PixelFormat).Save(Name + "/" + e.ID + ".png");

				ret = TextureHelper.CreateTexture(work.Clone(boundsScaled, work.PixelFormat));
				using (Graphics g = Graphics.FromImage(work))
					g.Clear(Color.Transparent);

				SetVisibleRecursive(e, false);
				SetVisibleToRoot(e, false);
			}
			return Tuple.Create(ret, boundsScaled);
		}

		private void SetVisibleRecursive(SvgElement e, bool visible) {
			if (e is SvgVisualElement) {
				((SvgVisualElement)e).Visible = visible;
			}
			foreach (var c in e.Children)
				SetVisibleRecursive(c, visible);
		}

		private void SetVisibleToRoot(SvgElement e, bool visible) {
			if (e is SvgVisualElement)
				((SvgVisualElement)e).Visible = visible;
			if (e.Parent != null)
				SetVisibleToRoot(e.Parent, visible);
		}

		private void RenderTexture(RectangleF r) {
			RenderTexture(r.Left, r.Right, r.Top, r.Bottom);
		}

		private static void RenderTexture(float l, float r, float t, float b) {
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex2(l, t);
			GL.TexCoord2(1, 0);
			GL.Vertex2(r, t);
			GL.TexCoord2(1, 1);
			GL.Vertex2(r, b);
			GL.TexCoord2(0, 1);
			GL.Vertex2(l, b);
			GL.End();
		}

		private PointF Project(PointF p, SizeF dim) {
			float svgAR = _dimensions.Width / _dimensions.Height;
			float imgAR = dim.Width / dim.Height;
			if (svgAR > imgAR) {
				// compensate for black box
				p.Y -= ((dim.Height - dim.Width / svgAR) / 2f);
				// adjust ratio
				dim.Height = dim.Width / svgAR;
			}
			else {
				// compensate for black box
				p.X -= ((dim.Width - dim.Height * svgAR) / 2f);
				// adjust ratio
				dim.Width = dim.Height * svgAR;
			}

			var x = p.X / dim.Width * _dimensions.Width;
			var y = p.Y / dim.Height * _dimensions.Height;
			return new PointF(x, y);
		}

		private SizeF GetCorrectedDimensions(SizeF dim) {
			// find real width/height, compensating for black box
			float svgAR = _dimensions.Width / _dimensions.Height;
			float imgAR = dim.Width / dim.Height;
			if (svgAR > imgAR)
				dim.Height = dim.Width / svgAR;
			else
				dim.Width = dim.Height * svgAR;
			return dim;
		}

		internal PointF Unproject(PointF p) {
			float svgAR = _dimensions.Width / _dimensions.Height;
			float imgAR = _svgDocument.Width / _svgDocument.Height;
			float width = _svgDocument.Width;
			float height = _svgDocument.Height;
			if (svgAR > imgAR)
				height = width / svgAR;
			else
				width = height * svgAR;

			var x = p.X / _dimensions.Width * width;
			var y = p.Y / _dimensions.Height * height;

			if (svgAR > imgAR)
				y += (_svgDocument.Height - _svgDocument.Width / svgAR) / 2f;
			else
				x += (_svgDocument.Width - _svgDocument.Height * svgAR) / 2f;

			return new PointF(x, y);
		}

		public RectangleF CalcBounds(SvgElement x) {
			var b = (x as ISvgBoundable).Bounds;
			var points = new PointF[4];
			points[0] = new PointF(b.Left, b.Top);
			points[1] = new PointF(b.Right, b.Top);
			points[2] = new PointF(b.Left, b.Bottom);
			points[3] = new PointF(b.Right, b.Bottom);

			// x = x.Parent;
			while (x is SvgVisualElement) {
				var m = x.Transforms.GetMatrix();
				m.TransformPoints(points);
				x = x.Parent;
			}
			float minX = points.Min(p => p.X);
			float minY = points.Min(p => p.Y);
			float maxX = points.Max(p => p.X);
			float maxY = points.Max(p => p.Y);
			return RectangleF.FromLTRB(minX, minY, maxX, maxY);
		}

	}


	public class ControllerItem {
		public SvgVisualElement Element;
		public RectangleF Bounds;
		public int Z;
		public int Texture = -1;
	}
	public class Button : ControllerItem {
		public SvgVisualElement Pressed;
		public int PressedTexture = -1;
		public RectangleF PressedBounds;
	}
	public class Stick : ControllerItem {
		public float OffsetScale;
		public int HorizontalAxis;
		public int VerticalAxis;
	}
	public class Trigger : ControllerItem {
		public float OffsetScale;
		public int Axis;
	}

	static class ExtensionMethods {
		public static void EnsureSize<T>(this List<T> list, int count) {
			while (list.Count <= count) list.Add(Activator.CreateInstance<T>());
		}
	}

}
