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

			ImageTransformerSettings transformerSettings = new ImageTransformerSettings();
			transformerSettings.focus_joints = new LeapMotionJoint[] { LeapMotionJoint.PALM, LeapMotionJoint.BABY_0 };
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
