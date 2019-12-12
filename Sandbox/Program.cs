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
		}
	}
}
