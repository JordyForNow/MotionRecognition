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
			NetworkController control = new NetworkController();
			uint[,,] d = new uint[1,1,1];
			bool[] b = new bool[1];
			control.TrainNetwork(ref d, b);
			control.Run();
		}
	}
}
