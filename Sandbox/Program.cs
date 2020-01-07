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
			settings.CSVHasHeader = true;
			settings.TrimLeft = 0;
			settings.TrimRight = 0;

			List<CSVColumnFilter> filters = new List<CSVColumnFilter>(1);

			// This filter
			CSVColumnFilter quaternions = new CSVEvenColumnsFilter();
			filters.Add(quaternions);

			CSVLoader<Vec3> loader = new CSVLoader<Vec3>(ref settings, ref filters);

			var data = loader.LoadData();

			//Factory settings
			IntervalBasedFactorySettings factorySettings = new IntervalBasedFactorySettings();
			factorySettings.sampleList = data;
			factorySettings.interval = 10;

			IntervalBasedFactory factory = new IntervalBasedFactory();

			double[] val = factory.GetNeuralInput(factorySettings);

			foreach(double d in val) 
			{
				Console.WriteLine(d);
			}

			Console.ReadLine();
		}
	}
}
