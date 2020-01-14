using System;
using System.Collections.Generic;
using System.IO;
using Encog.Engine.Network.Activation;

namespace MotionRecognition
{

	public struct CountNetworkTrainSettings
	{
		public EncogTrainSettings trainSettings;
		public string correctInputDirectory;
		public string incorrectInputDirectory;

		public string outputDirectory;
		public string outputName;

		public uint sampleCount;
	}

	public class CountNetworkTrainController : INetworkTrainController<CountNetworkTrainSettings>
	{

		public static void PrepareData(ref NetworkContainer container, ref CountNetworkTrainSettings settings)
		{
			if (settings.trainSettings.dataset != null)
				throw new IncorrectActionOrderException("This action has already been completed.");

			TestForErrors(ref settings);

			settings.trainSettings = new EncogTrainSettings
			{
				maxTrainingError = 0.01
			};

			int correctFileCount = BaseTrainHelper.GetFileCount(settings.correctInputDirectory);
			int incorrectFileCount = BaseTrainHelper.GetFileCount(settings.incorrectInputDirectory);

			if (correctFileCount <= 0)
				throw new FileNotFoundException("correctFiles not found.");

			if (incorrectFileCount <= 0)
				throw new FileNotFoundException("incorretFiles not found.");

			settings.trainSettings.dataset = new double[correctFileCount + incorrectFileCount][];
			settings.trainSettings.answers = new double[correctFileCount + incorrectFileCount][];

			// Compute correct training data.
			ComputeData(
				settings.sampleCount,
				settings.correctInputDirectory,
				ref settings.trainSettings.dataset,
				ref settings.trainSettings.answers,
				1.0,
				0);

			// Compute incorrect training data.
			ComputeData(
				settings.sampleCount,
				settings.incorrectInputDirectory,
				ref settings.trainSettings.dataset,
				ref settings.trainSettings.answers,
				0.0,
				correctFileCount);
		}

		public static void PrepareNetwork(ref NetworkContainer container, ref CountNetworkTrainSettings settings)
		{
			if (settings.trainSettings.dataset == null)
				throw new IncorrectActionOrderException("Prepare data before preparing network.");

			EncogLayerSettings inputLayerSettings = new EncogLayerSettings
			{
				activationFunction = null,
				hasBias = true,
				neuronCount = settings.trainSettings.dataset[0].Length
			};

			EncogWrapper.Instantiate(ref container);
			EncogWrapper.AddLayer(ref container, ref inputLayerSettings);

			EncogLayerSettings hiddenLayerOneSettings = new EncogLayerSettings
			{
				activationFunction = new ActivationElliott(),
				hasBias = true,
				neuronCount = 100
			};

			EncogWrapper.AddLayer(ref container, ref hiddenLayerOneSettings);

			EncogLayerSettings outputLayerSettings = new EncogLayerSettings
			{
				activationFunction = new ActivationElliott(),
				hasBias = false,
				neuronCount = 1
			};

			EncogWrapper.AddLayer(ref container, ref outputLayerSettings);
			EncogWrapper.FinalizeNetwork(ref container);
		}

		public static void Train(ref NetworkContainer container, ref CountNetworkTrainSettings settings)
		{
			if (settings.trainSettings.dataset == null)
				throw new IncorrectActionOrderException("Prepare data before training network.");

			if (container == null)
				throw new IncorrectActionOrderException("Prepare network before training network.");

			EncogWrapper.Train(ref container, ref settings.trainSettings);
			EncogWrapper.SaveNetworkToFS(ref container, settings.outputDirectory + settings.outputName + ".eg");
		}

		private static void ComputeData(
			uint networkInputSize,
			string inputDataDirectory,
			ref double[][] outputData,
			ref double[][] outputAnswers,
			double outputValue,
			int index)
		{
			DirectoryInfo inputDirectory = new DirectoryInfo(inputDataDirectory);

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{
				List<ICSVFilter> baseFilters = new List<ICSVFilter>(1);
				ICSVFilter quaternions = new CSVEvenColumnFilter();
				baseFilters.Add(quaternions);

				// Setup loader.
				CSVLoaderSettings settings = new CSVLoaderSettings
				{
					filePath = file.FullName,
					trimUp = 1,
					trimDown = 0,
					filters = baseFilters
				};

				var data = CSVLoader<Vector3>.LoadData(ref settings);

				// Initialize CountBased Transformer settings.
				IntervalBasedTransformerSettings countSettings = new IntervalBasedTransformerSettings
				{
					sampleList = data,
					count = networkInputSize
				};
				CountBasedTransformer countTransformer = new CountBasedTransformer();

				BaseTrainHelper.Project1DInto2D(
					countTransformer.GetNeuralInput(countSettings),
					ref outputData,
					index);

				// Set answer to given value.
				outputAnswers[index] = new double[] { outputValue };
				index++;
			}
		}

		// Test if input is valid.
		public static void TestForErrors(ref CountNetworkTrainSettings settings)
		{
			if (!Directory.Exists(settings.correctInputDirectory))
				throw new DirectoryNotFoundException("Correct data directory was not found.");

			if (!Directory.Exists(settings.incorrectInputDirectory))
				throw new DirectoryNotFoundException("Incorrect data directory was not found.");

			if (!Directory.Exists(settings.outputDirectory))
				throw new DirectoryNotFoundException("Output data directory was not found.");
		}
	}
}
