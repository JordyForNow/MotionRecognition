using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			CSVLoader loader = new CSVLoader("data.csv", 21);

			var table = loader.GetData();

			Motion3DImage image = new Motion3DImage(100);
			image.CreateImageFromTable(ref table);
			ImageSerializer.Serialize(image);
			Motion3DImage image2 = ImageSerializer.DeSerialize();
			ImageSerializer.Serialize(image2, "./data2");

			image.toImage("img.bmp");
		}
	}
}
