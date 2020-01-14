using MotionRecognition;
using MotionRecognitionHelper;
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

			//ManipulateAndImageCreate();
		}

		static void ManipulateAndImageCreate()
		{
			#region Csv Manipulation
			CSVManipulatorSettings manipulatorSettings = new CSVManipulatorSettings // Create manipulation settings
			{
				copyLines = 1,
				removeLines = 1,
				mutationCount = 5,
				deviationPercentage = 0.05f,
				innerDeviationPercentage = 0.01f,
				dataFile = "",
				dataFolder = "./CSV/",
				outputFolder = "./mutated/",
				alterInput = true,
				verbose = true
			};

			CSVManipulator.RunManipulator(ref manipulatorSettings);
			#endregion

			#region Image creation related calls
			System.IO.FileInfo[] FileInfo = new System.IO.DirectoryInfo(manipulatorSettings.outputFolder).GetFiles();

			CSVLoaderSettings csvLoaderSettings = new CSVLoaderSettings
			{
				CSVHasHeader = true,
				trimUp = 0,
				trimDown = 0
			};

			List<ICSVFilter> csvFilters = new List<ICSVFilter>(1);

			ICSVFilter evenFilter = new CSVEvenColumnFilter();
			csvFilters.Add(evenFilter);
			csvLoaderSettings.filters = csvFilters;

			ImageTransformerSettings transformerSettings = new ImageTransformerSettings
			{
				focusJoints = (LeapMotionJoint[])Enum.GetValues(typeof(LeapMotionJoint)),
				size = 100
			};

			foreach (var file in FileInfo)
			{
				csvLoaderSettings.filePath = file.FullName;

				var csvData = CSVLoader<Vector3>.LoadData(ref csvLoaderSettings);

				transformerSettings.samples = csvData;

				var outputArr = new ImageTransformer().GetNeuralInput(transformerSettings);

				ImageCreator.WriteBitmapToFS(ImageCreator.CreateNeuralImageFromDoubleArray(ref outputArr, transformerSettings.size, true), "./IMG/" + file.Name);
			}
			#endregion
		}
	}
}
