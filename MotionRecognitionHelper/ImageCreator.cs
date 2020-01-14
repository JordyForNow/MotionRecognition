using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MotionRecognitionHelper
{
    public static class ImageCreator
    {
		// Creates a Bitmap from a double array.
		// If Is3DImage = true then it creates an image that is double the height.
        public static Bitmap CreateNeuralImageFromDoubleArray(ref double[] arr, int size, bool Is3DImage = false)
        {
            int width = size;
            int height = Is3DImage ? size * 2 : size;

            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, (y < size) ? Color.Gray : Color.White); // Use different background color for either halve of the image.

                    if (arr[x + y * width] != 0) // y * width equates to the offset within the array.
                    {
                        bitmap.SetPixel(x, y, (y >= size) ? Color.Blue : Color.Black); // Different foreground pixel color.
                    }
                }
            }
            return bitmap;
        }

        public static void WriteBitmapToFS(Bitmap bitmap, string filePath = "Image.bmp")
        {
			var outputDirectory = Path.GetDirectoryName(filePath);

			if (!Directory.Exists(outputDirectory)) {
				Directory.CreateDirectory(outputDirectory);
			}
			
			bitmap.Save($"{outputDirectory}/{Path.GetFileNameWithoutExtension(filePath)}.bmp", ImageFormat.Bmp);
        }
    }
}
