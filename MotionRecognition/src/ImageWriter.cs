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
					g.SetPixel(x, y, Color.Black);

					if (motion3DImage.GetData()[y, x] != 0)
					{
						g.SetPixel(x, y, Color.White);
					}

					if (motion3DImage.GetData()[y + motion3DImage.size, x] != 0)
					{
						g.SetPixel(x, y, Color.Green);
					}

				}
			}

			g.Save(filename);

		}
	}
}
