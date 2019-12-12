using System;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			// TODO: make relative
			Environment.SetEnvironmentVariable("PATH", @"C:\Users\Jordy\AppData\Local\Programs\Python\Python36", EnvironmentVariableTarget.Process);
			Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\Jordy\AppData\Local\Programs\Python\Python36\python.exe", EnvironmentVariableTarget.Process);
			Neural.Run2();
			
			CSVLoader loader = new CSVLoader("data.csv", 21);

			var table = loader.GetData();

			Motion3DImage image = new Motion3DImage(ref table);
			ImageSerializer.Serialize(image);
			Motion3DImage image2 = ImageSerializer.DeSerialize();
			ImageSerializer.Serialize(image2, "./data2");
		}
	}
}
