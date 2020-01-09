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
			settings.filePath = dataPath + "data.csv";
			settings.CSVHasHeader = true;
			settings.trimUp = 0;
			settings.trimDown = 0;

			List<ICSVFilter> filters = new List<ICSVFilter>(1);

			// This filter
			ICSVFilter quaternions = new CSVEvenColumnFilter();
			filters.Add(quaternions);
			settings.filters = filters;

			var data = CSVLoader<Vector3>.LoadData(ref settings);

			ImageTransformerSettings transformerSettings = new ImageTransformerSettings();
			transformerSettings.focusJoints = new LeapMotionJoint[] { LeapMotionJoint.PALM };
			transformerSettings.samples = data;
			transformerSettings.size = 10;

			//Transformer settings
			ImageTransformer transformer = new ImageTransformer();
			var arr = transformer.GetNeuralInput(transformerSettings);
			foreach (var d in arr)
				Console.WriteLine(d);

		}
	}
}
