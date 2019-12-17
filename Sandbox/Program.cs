using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			//if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			//{
			//	string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
			//	if (Environment.OSVersion.Version.Major >= 6)
			//	{
			//		path = Directory.GetParent(path).ToString();
			//	}

			//	path += "\\AppData\\Local\\Programs\\Python\\Python36";
			//	Environment.SetEnvironmentVariable("PATH", @path, EnvironmentVariableTarget.Process);
			//	path += "\\python.exe";
			//	Environment.SetEnvironmentVariable("PYTHONHOME", @path, EnvironmentVariableTarget.Process);
			//} else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			//{
			//	Console.WriteLine("Linux");
			//} else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			//{
			//	Console.WriteLine("OSX");
			//}

			//// TODO: make relative
			//Environment.SetEnvironmentVariable("PATH", @"C:\Users\Jordy\AppData\Local\Programs\Python\Python36", EnvironmentVariableTarget.Process);
			//Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\Jordy\AppData\Local\Programs\Python\Python36\python.exe", EnvironmentVariableTarget.Process);
			//NetworkController control = new NetworkController();

			//CSVLoader loader = new CSVLoader("data.csv", 21);

			//List<Sample<JointMeasurement>> table = loader.GetData();

			//Motion3DImage image = new Motion3DImage(100);
			//image.CreateImageFromTable(ref table);

			//int[,] d = image.GetData();
			//int[,,] neuralInput;
			//neuralInput[0] = d;
			//bool[] answers = new bool[1] { true };
			//control.TrainNetwork(ref neuralInput, answers);
			//Neural.Run2();


			/*MotionRecognizer recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_inputData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\Input\",
				_networkWeights: "modelforgesture1.h5",
				_networkLayers: "model.json");

			recognizer.Run();*/
			
		}
	}
}
