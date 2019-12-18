using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			//if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			//{
			//	StringBuilder pathBuilder = new StringBuilder();

			//	// Get user directory.
			//	var ApplicationData = Directory.GetParent(
			//			Environment.GetFolderPath(
			//			Environment.SpecialFolder.ApplicationData));

			//	if (Environment.OSVersion.Version.Major >= 6)
			//		pathBuilder.Append(Directory.GetParent(ApplicationData.FullName));
			//	else
			//		pathBuilder.Append(ApplicationData.FullName);

			//	// Add Python36 folder to path.
			//	pathBuilder.Append(@"\AppData\Local\Programs\Python\Python36");
			//	Environment.SetEnvironmentVariable("PATH", pathBuilder.ToString(), EnvironmentVariableTarget.Process);

			//	// Add python excecutable to path.
			//	pathBuilder.Append(@"\python.exe");
			//	Environment.SetEnvironmentVariable("PYTHONHOME", pathBuilder.ToString(), EnvironmentVariableTarget.Process);
			//}
			//else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			//{
			//	Console.WriteLine("Linux");
			//}
			//else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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
			//Console.WriteLine("---------------------");


			MotionRecognizer recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataCorrect\",
				_incorrectTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataIncorrect\",
				_outputDirectory: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\",
				_outputName: @"movementOne",
				_allowFileOverride: true
			);

			recognizer.Run();

		}
	}
}
