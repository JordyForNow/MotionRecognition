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

			//Transformer settings
			IntervalBasedTransformerSettings TransformerSettings = new IntervalBasedTransformerSettings();
			TransformerSettings.sampleList = data;
			TransformerSettings.interval = 10;

			IntervalBasedTransformer Transformer = new IntervalBasedTransformer();

			double[] val = Transformer.GetNeuralInput(TransformerSettings);

			foreach(double d in val) 
			{
				Console.WriteLine(d);
			}

			Console.ReadLine();
		}
	}
}
