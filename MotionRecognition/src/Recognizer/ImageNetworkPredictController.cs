using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
	public struct ImageNetworkPredictSettings
	{
		public EncogPredictSettings predictSettings;

		public string trainedNetwork;
		public string predictData;

		public uint networkInputSize;
	};

	public class ImageNetworkPredictController : INetworkPredictController<ImageNetworkPredictSettings>
	{

		public static void preparePredictor(ref ImageNetworkPredictSettings settings, ref NetworkContainer container)
		{
			verifyData(ref settings);

			EncogWrapper.LoadNetworkFromFS(ref container, settings.trainedNetwork);

			List<ICSVFilter> baseFilters = new List<ICSVFilter>(1);
			ICSVFilter quaternions = new CSVEvenColumnFilter();
			baseFilters.Add(quaternions);

			// Setup loader.
			CSVLoaderSettings CSVSettings = new CSVLoaderSettings
			{
				filePath = settings.predictData,
				trimUp = 1,
				trimDown = 0,
				filters = baseFilters
			};

			var data = CSVLoader<Vector3>.LoadData(ref CSVSettings);

			// Initialize image Transformer.
			ImageTransformerSettings imageSettings = new ImageTransformerSettings
			{
				focusJoints = (LeapMotionJoint[])Enum.GetValues(typeof(LeapMotionJoint)),
				samples = data,
				size = 10
			};
			ImageTransformer imageTransformer = new ImageTransformer();

			settings.predictSettings = new EncogPredictSettings
			{
				threshold = 0.5,
				data = imageTransformer.GetNeuralInput(imageSettings)
			};

		}

		public static bool predict(ref ImageNetworkPredictSettings settings, ref NetworkContainer container)
		{

			return EncogWrapper.Predict(ref container, ref settings.predictSettings);

		}

		private static void verifyData(ref ImageNetworkPredictSettings settings)
		{

			if (!File.Exists(settings.trainedNetwork))
				throw new FileNotFoundException("Trained network was not found.");

			if (!Regex.IsMatch(settings.trainedNetwork, @"(\.eg$)"))
				throw new WrongFileTypeException("Wrong network location given.");

			if (!File.Exists(settings.predictData))
				throw new FileNotFoundException("Network input was not found.");

			if (!Regex.IsMatch(settings.predictData, @"(\.csv$)"))
				throw new WrongFileTypeException("Wrong network input given.");

		}

	}
}
