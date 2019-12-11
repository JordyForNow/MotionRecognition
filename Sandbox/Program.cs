using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			CSVLoader loader = new CSVLoader("data.csv", 21);

			var table = loader.GetData();

			Motion3DImage image = new Motion3DImage(ref table);

			image.Serialize();
			image.DeSerialize();
		}
	}
}
