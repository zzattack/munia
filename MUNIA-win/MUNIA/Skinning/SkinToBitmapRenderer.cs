using System;
using System.Drawing;
using System.Drawing.Imaging;
using MUNIA.Util;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace MUNIA.Skinning {
	public static class SkinToBitmapRenderer {
		public static void Render(Skin skin, Size size, Color background, Bitmap buffer) {
			SetupFrameBuffer(size, out int rgbBuffer);
			GL.PushAttrib(AttribMask.ViewportBit);

			if (rgbBuffer != -1) {
				GL.Viewport(0, 0, buffer.Width, buffer.Height);
				GL.ClearColor(background);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

				GL.MatrixMode(MatrixMode.Modelview);
				GL.LoadIdentity();

				GL.MatrixMode(MatrixMode.Projection);
				GL.LoadIdentity();
				GL.Scale(1.0, -1.0, 1.0); // invert now because readpixels reads rows in flipped vertical order
				GL.Ortho(0, buffer.Width, buffer.Height, 0, 0.0, 4.0);

				// draw the preview
				skin.Activate();
				skin.Render(buffer.Width, buffer.Height);

				// read pixels back to surface
				var bitmapData = buffer.LockBits(new Rectangle(0, 0, buffer.Width, buffer.Height),
					ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				GL.ReadPixels(0, 0, bitmapData.Width, bitmapData.Height,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
				buffer.UnlockBits(bitmapData);

				GL.Ext.DeleteFramebuffer(rgbBuffer);
			}
			// deactivate framebuffer again
			GL.PopAttrib();
			GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // disable rendering into the FBO
		}

		private static bool SetupFrameBuffer(Size size, out int rgbBuffer) {
			try {
				int fbo;
				GL.Ext.GenFramebuffers(1, out fbo);
				GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fbo);
				GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
				GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
			}
			catch (Exception exc) {
				rgbBuffer = -1;
				return false;
			}

			GL.Ext.GenRenderbuffers(1, out rgbBuffer);
			GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, rgbBuffer);
			GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, RenderbufferStorage.Rgba8, size.Width, size.Height);
			GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext,
				RenderbufferTarget.RenderbufferExt, rgbBuffer);

			return GL.CheckFramebufferStatus(FramebufferTarget.FramebufferExt) == FramebufferErrorCode.FramebufferCompleteExt;
		}

	}
}
