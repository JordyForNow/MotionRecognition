﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
				if (Environment.OSVersion.Version.Major >= 6)
				{
					path = Directory.GetParent(path).ToString();
				}

				path += "\\AppData\\Local\\Programs\\Python\\Python36";
				Environment.SetEnvironmentVariable("PATH", @path, EnvironmentVariableTarget.Process);
				path += "\\python.exe";
				Environment.SetEnvironmentVariable("PYTHONHOME", @path, EnvironmentVariableTarget.Process);
			} else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				Console.WriteLine("Linux");
			} else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				Console.WriteLine("OSX");
			}

			// TODO: make relative
			Environment.SetEnvironmentVariable("PATH", @"C:\Users\Jordy\AppData\Local\Programs\Python\Python36", EnvironmentVariableTarget.Process);
			Environment.SetEnvironmentVariable("PYTHONHOME", @"C:\Users\Jordy\AppData\Local\Programs\Python\Python36\python.exe", EnvironmentVariableTarget.Process);
			NetworkController control = new NetworkController();
			//uint[,,] d = new uint[1,1,1];
			//bool[] b = new bool[1];
			//control.TrainNetwork(ref d, b);
			Neural.Run2();
			
		}
	}
}
