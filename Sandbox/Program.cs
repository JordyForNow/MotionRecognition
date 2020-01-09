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
			settings.trimLeft = 0;
			settings.trimRight = 0;

			List<ICSVFilter> filters = new List<ICSVFilter>(1);

			// This filter
			ICSVFilter quaternions = new CSVEvenColumnFilter();
			filters.Add(quaternions);
			settings.filters = filters;

			var data = CSVLoader<Vector3>.LoadData(ref settings);

			ImageTransformerSettings transformerSettings = new ImageTransformerSettings();
			transformerSettings.focus_joints = new LeapMotionJoint[] { LeapMotionJoint.PALM };
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
