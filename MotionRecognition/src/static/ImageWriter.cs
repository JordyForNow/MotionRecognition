using System.Drawing;

namespace MotionRecognition
{
	public static class ImageWriter
	{
		public static void WriteMotion3DImage(Motion3DImage motion3DImage, string filename = "test.bmp")
		{
			Bitmap g = new Bitmap(motion3DImage.size, motion3DImage.size);

			for (int y = 0; y < motion3DImage.size; y++)
			{
				for (int x = 0; x < motion3DImage.size; x++)
				{
					g.SetPixel(x, y, Color.White);

					if (motion3DImage.GetData()[y, x] != 0)
					{
						g.SetPixel(x, y, getColor(motion3DImage.GetData()[y, x]));
					}

					if (motion3DImage.GetData()[y + motion3DImage.size, x] != 0)
					{
						g.SetPixel(x, y, getColor(motion3DImage.GetData()[y + motion3DImage.size, x]));
					}
				}
			}

			g.Save(filename);
		}

		private static Color getColor(int color)
		{
			int r = (color >> 16) & 0xff;
			int g = (color >> 8) & 0xff;
			int b = (color) & 0xff;
			int a = (color >> 24) & 0xff;

			return Color.FromArgb(r, g, b);
		}
	}
}
