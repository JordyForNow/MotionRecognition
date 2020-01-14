using System;
using System.Collections.Generic;
using System.IO;
using Encog.Engine.Network.Activation;

namespace MotionRecognition
{

	public struct ImageNetworkTrainSettings
	{
		public EncogTrainSettings trainSettings;
		public string correctInputDirectory;
		public string incorrectInputDirectory;

		public string outputDirectory;
		public string outputName;

		public uint sampleCount;
	}

	public class ImageNetworkTrainController : INetworkTrainController<ImageNetworkTrainSettings>
	{

		// Prepare data which is used to train the network.
		public static void PrepareData(ref NetworkContainer container, ref ImageNetworkTrainSettings settings)
		{
			if (settings.trainSettings.dataset != null)
				throw new IncorrectActionOrderException("This action has already been completed.");

			TestForErrors(ref settings);

			settings.trainSettings = new EncogTrainSettings
			{
				maxTrainingError = 0.02,
				maxEpochCount = 200
			};

			int correctFileCount = BaseTrainHelper.GetFileCount(settings.correctInputDirectory);
			int incorrectFileCount = BaseTrainHelper.GetFileCount(settings.incorrectInputDirectory);

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

		// Prepare network for training, this is mainly setting up the layers and activation functions.
		public static void PrepareNetwork(ref NetworkContainer container, ref ImageNetworkTrainSettings settings)
		{
			if (settings.trainSettings.dataset == null)
				throw new IncorrectActionOrderException("Prepare data before preparing network.");

			EncogWrapper.Instantiate(ref container);

			EncogLayerSettings inputLayerSettings = new EncogLayerSettings
			{
				activationFunction = null,
				hasBias = true,
				neuronCount = settings.trainSettings.dataset[0].Length
			};

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

		// Start actual training of the network.
		public static void Train(ref NetworkContainer container, ref ImageNetworkTrainSettings settings)
		{
			if (settings.trainSettings.dataset == null)
				throw new IncorrectActionOrderException("Prepare data before training network.");

			if (container == null)
				throw new IncorrectActionOrderException("Prepare network before training network.");

			EncogWrapper.Train(ref container, ref settings.trainSettings);
			if (settings.outputDirectory[settings.outputDirectory.Length - 1].Equals("/"))
			{
				EncogWrapper.SaveNetworkToFS(ref container, settings.outputDirectory + settings.outputName + ".eg");
				return;
			}

			EncogWrapper.SaveNetworkToFS(ref container, settings.outputDirectory + "/" + settings.outputName + ".eg");
		}

		// Convert data from a CSV file to the actual input array for the network.
		private static void ComputeData(
			uint sampleCount,
			string inputData,
			ref double[][] outputData,
			ref double[][] outputAnswers,
			double outputValue,
			int index)
		{
			DirectoryInfo inputDirectory = new DirectoryInfo(inputData);

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

				// Initialize image Transformer.
				ImageTransformerSettings imageSettings = new ImageTransformerSettings
				{
					focusJoints = (LeapMotionJoint[])Enum.GetValues(typeof(LeapMotionJoint)),
					samples = data,
					size = sampleCount
				};
				ImageTransformer imageTransformer = new ImageTransformer();

				BaseTrainHelper.Project1DInto2D(
					imageTransformer.GetNeuralInput(imageSettings),
					ref outputData,
					index);

				// Set answer to given value.
				outputAnswers[index] = new double[] { outputValue };
				index++;
			}
		}

		// Test if all inputs are valid.
		public static void TestForErrors(ref ImageNetworkTrainSettings settings)
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
