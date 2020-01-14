﻿using System;
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

		public static void prepareData(ref ImageNetworkTrainSettings settings, ref NetworkContainer container)
		{
			if (settings.trainSettings.dataset != null)
				throw new IncorrectActionOrderException("This action has already been completed.");

			settings.trainSettings = new EncogTrainSettings
			{
				maxTrainingError = 0.02
			};

			int correctFileCount = getFileCount(settings.correctInputDirectory);
			int incorrectFileCount = getFileCount(settings.incorrectInputDirectory);

			settings.trainSettings.dataset = new double[correctFileCount + incorrectFileCount][];
			settings.trainSettings.answers = new double[correctFileCount + incorrectFileCount][];

			// Compute correct training data.
			computeData(
				settings.sampleCount,
				settings.correctInputDirectory,
				ref settings.trainSettings.dataset,
				ref settings.trainSettings.answers,
				1.0,
				0);

			// Compute incorrect training data.
			computeData(
				settings.sampleCount,
				settings.incorrectInputDirectory,
				ref settings.trainSettings.dataset,
				ref settings.trainSettings.answers,
				0.0,
				correctFileCount);
		}

		public static void prepareNetwork(ref ImageNetworkTrainSettings settings, ref NetworkContainer container)
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
			EncogWrapper.finalizeNetwork(ref container);
		}

		public static void train(ref ImageNetworkTrainSettings settings, ref NetworkContainer container)
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

		private static int getFileCount(string dataDirectory)
		{
			// Get total number of '.csv' files inside Directory.
			return Directory.GetFiles(
				dataDirectory,
				"*.csv*",
				SearchOption.TopDirectoryOnly
			).Length;
		}

		private static void computeData(
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

				Project1DInto2D(
					imageTransformer.GetNeuralInput(imageSettings),
					ref outputData,
					index);

				// Set answer to given value.
				outputAnswers[index] = new double[] { outputValue };
				index++;
			}
		}

		private static void Project1DInto2D(double[] source, ref double[][] dest, int index)
		{
			double[] temp = new double[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				temp[i] = source[i];
			}

			dest[index] = temp;
		}

	}
}
