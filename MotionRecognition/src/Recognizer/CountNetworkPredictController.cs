using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MotionRecognition
{

	public struct CountNetworkPredictSettings
	{
		public EncogPredictSettings predictSettings;

		public string trainedNetwork;
		public string predictData;

		public uint networkInputSize;
	};

	public class CountNetworkPredictController : INetworkPredictController<CountNetworkPredictSettings>
	{

		// Prepare network predictor to predict the output of a dataset.
		public static void preparePredictor(ref CountNetworkPredictSettings settings, ref NetworkContainer container)
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

			// Initialize CountBased Transformer settings.
			IntervalBasedTransformerSettings countSettings = new IntervalBasedTransformerSettings
			{
				sampleList = data,
				count = settings.networkInputSize
			};
			CountBasedTransformer countTransformer = new CountBasedTransformer();

			settings.predictSettings = new EncogPredictSettings
			{
				threshold = 0.5,
				data = countTransformer.GetNeuralInput(countSettings)
			};

		}

		// Predict the output of a dataset using an existing network.
		public static bool predict(ref CountNetworkPredictSettings settings, ref NetworkContainer container)
		{

			if (settings.predictSettings.data == null)
				throw new IncorrectActionOrderException("Prepare predictor before predicting.");

			return EncogWrapper.Predict(ref container, ref settings.predictSettings);

		}

		// Verify that all inputs are valid.
		private static void verifyData(ref CountNetworkPredictSettings settings)
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
