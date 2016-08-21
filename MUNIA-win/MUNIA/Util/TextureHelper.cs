using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace MUNIA.Util {
    public static class TextureHelper {

        public static int CreateTexture(Bitmap bmp) {
            GL.Enable(EnableCap.Texture2D);

            int texture;
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            var data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            if (!GL.GetString(StringName.Extensions).Contains("texture_buffer_object")) {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            }
            else {
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.LinearSharpenAlphaSgis);
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			}

			bmp.UnlockBits(data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);

			GL.Disable(EnableCap.Texture2D);
            return texture;
        }
		
		public static void RenderTexture(RectangleF r) {
			RenderTexture(r.Left, r.Right, r.Top, r.Bottom);
		}
	    public static void RenderTexture(float l, float r, float t, float b) {
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
    }
}
