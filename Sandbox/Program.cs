using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			IDataLoader<JointMeasurement> loader = new CSVLoader("data.csv");

			var table = loader.GetData();

			Motion3DImage image = new Motion3DImage(100);
			image.CreateImageFromTable(ref table);

			ImageSerializer.Serialize(image);
			Motion3DImage image2 = ImageSerializer.DeSerialize();
			ImageSerializer.Serialize(image2, "./data2");

			ImageWriter.WriteMotion3DImage(image);
		}
	}
}
