using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			CSVLoader loader = new CSVLoader("data.csv", 21);
			loader.LoadImage();
		}
	}
}
