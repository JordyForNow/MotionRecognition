using System;
using System.Collections.Generic;
using System.IO;
using MotionRecognition;

namespace InputManipulation
{
	class Program
	{
		public static void Main(string[] args)
		{
			CsvManipulator manipulator = new CsvManipulator();

			//manipulator.Run();

			FileInfo[] di = new DirectoryInfo(manipulator.outputFolder).GetFiles();

			CSVLoaderSettings settings = new CSVLoaderSettings
			{
				CSVHasHeader = true,
				trimUp = 0,
				trimDown = 0
			};

			List<ICSVFilter> filters = new List<ICSVFilter>(1);

			// This filter
			ICSVFilter quaternions = new CSVEvenColumnFilter();
			filters.Add(quaternions);
			settings.filters = filters;

			ImageTransformerSettings transformerSettings = new ImageTransformerSettings
			{
				focusJoints = (LeapMotionJoint[])Enum.GetValues(typeof(LeapMotionJoint)),
				size = 100
			};

			foreach (var file in di)
			{
				settings.filePath = file.FullName;

				var data = CSVLoader<Vector3>.LoadData(ref settings);

				transformerSettings.samples = data;

				ImageTransformer transformer = new ImageTransformer();
				var arr = transformer.GetNeuralInput(transformerSettings);

				ImageCreator.WriteBitmapToFS(ImageCreator.CreateNeuralImageFromDoubleArray(ref arr, transformerSettings.size, true), "./IMG/" + file.Name);
			}
		}
	}
}
