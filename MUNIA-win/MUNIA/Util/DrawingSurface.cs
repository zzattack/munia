using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MUNIA.Util {
	public class DrawingSurface : IDisposable {
		public BitmapData BitmapData { get; private set; }
		public Bitmap Bitmap { get; private set; }
		public int Width { get; private set; } // prevents repeated (slow) lookups in bm.Width
		public int Height { get; private set; } // prevents repeated (slow) lookups in bm.Width

		public DrawingSurface(int width, int height, PixelFormat pixelFormat) {
			Bitmap = new Bitmap(width, height, pixelFormat);
			Width = width;
			Height = height;
		}

		public void Lock(PixelFormat pixelFormat = PixelFormat.Format32bppArgb) {
			if (BitmapData == null)
				BitmapData = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadWrite, pixelFormat);
		}

		public void Unlock() {
			if (BitmapData != null) {
				Bitmap.UnlockBits(BitmapData);
				BitmapData = null;
			}
		}
		
		public void Dispose() {
			Unlock();
			Bitmap.Dispose();
		}
	}
}
