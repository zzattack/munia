using System;
using System.Drawing;
using MUNIA.Util;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace MUNIA.Skinning {
	public class SkinToBitmapRenderer :IDisposable {
		private readonly DrawingSurface _surface;


		public SkinToBitmapRenderer(int width, int height) {
			_surface = new DrawingSurface(width, height, PixelFormat.Format32bppArgb);
		}

		public Bitmap Render(Skin skin, Color background) {
			SetupFrameBuffer();
			GL.PushAttrib(AttribMask.ViewportBit);
			{
				GL.Viewport(0, 0, _surface.Width, _surface.Height);
				GL.ClearColor(background);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadIdentity();

				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Scale(1.0, -1.0, 1.0); // invert now because readpixels reads rows in flipped vertical order
				GL.Ortho(0, _surface.Width, _surface.Height, 0, 0.0, 4.0);

				// draw the preview
				skin.Activate();
				skin.Render(_surface.Width, _surface.Height, true);

				// read pixels back to surface
				_surface.Lock();
				GL.ReadPixels(0, 0, _surface.BitmapData.Width, _surface.BitmapData.Height, 
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, _surface.BitmapData.Scan0);
				_surface.Unlock();
			}
			// deactivate framebuffer again
			GL.PopAttrib();
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO

			return _surface.Bitmap;
		}

		private bool SetupFrameBuffer() {
			try {
				int fbo;
				GL.Ext.GenFramebuffers(1, out fbo);
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fbo);
				GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
				GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
			}
			catch (Exception exc) {
				return false;
			}
			int depthbuffer;
			GL.Ext.GenRenderbuffers(1, out depthbuffer);
			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, depthbuffer);
			GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, RenderbufferStorage.DepthComponent32, _surface.Width, _surface.Height);
			GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, depthbuffer);

			int rgb_rb;
			GL.Ext.GenRenderbuffers(1, out rgb_rb);
			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, rgb_rb);
			GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, RenderbufferStorage.Rgba8, _surface.Width, _surface.Height);
			GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, RenderbufferTarget.RenderbufferExt, rgb_rb);

			return GL.CheckFramebufferStatus(FramebufferTarget.FramebufferExt) == FramebufferErrorCode.FramebufferCompleteExt;
		}

		public void Dispose() {
			_surface?.Dispose();
		}
	}
}
