using MotionRecognition;
using System;
using System.Collections.Generic;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			string dataPath = @"../../../Data/";

			// Loader settings 
			CSVLoaderSettings settings = new CSVLoaderSettings();
			settings.filepath = dataPath + "data.csv";
			settings.TrimUp = 1;
			settings.TrimRight = 0;

			List<CSVColumnFilter> filters = new List<CSVColumnFilter>(1);

			// This filter
			CSVColumnFilter quaternions = new CSVEvenColumnsFilter();
			filters.Add(quaternions);

			CSVLoader<Vec3> loader = new CSVLoader<Vec3>(ref settings, ref filters);

			var data = loader.LoadData();

			Console.Read();


		}
	}
}
