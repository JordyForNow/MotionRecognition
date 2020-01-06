using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
	public enum networkActions
	{
		TRAIN,
		PREDICT
	}

	// This class serves as a facade to handle the complete process of recognizing hand gestures.
	public class MotionRecognizer
	{

		private NetworkTrainer trainer;
		private NetworkPredictor predictor;

		private networkActions action;
		private string predictData;
		private string correctTrainingData;
		private string incorrectTrainingData;
		private string outputDirectory;
		private string outputName;
		private string trainedNetwork;
		private int networkInputSize;
		private bool allowFileOverride;

		private double[][] dataset;
		private double[][] trainingAnswers;

		public MotionRecognizer(
			networkActions _action,
			string _predictData = null,
			string _correctTrainingData = null,
			string _incorrectTrainingData = null,
			string _outputDirectory = null,
			string _outputName = null,
			string _trainedNetwork = null,
			bool _allowFileOverride = false,
			int _networkInputSize = 10)
		{
			action = _action;
			predictData = _predictData;
			correctTrainingData = _correctTrainingData;
			incorrectTrainingData = _incorrectTrainingData;
			outputDirectory = _outputDirectory;
			outputName = _outputName;
			trainedNetwork = _trainedNetwork;
			networkInputSize = _networkInputSize;
			allowFileOverride = _allowFileOverride;
		}

		public bool Run()
		{
			switch (action)
			{
				case networkActions.TRAIN:
					return Train();
				case networkActions.PREDICT:
					return Predict();
				default:
					return false;
			}
		}

		private bool Train()
		{
			if (!Directory.Exists(correctTrainingData))
				throw new DirectoryNotFoundException("Correct input data directory was not found.");

			if (!Directory.Exists(incorrectTrainingData))
				throw new DirectoryNotFoundException("Incorrect input data directory was not found.");

			if (correctTrainingData == incorrectTrainingData)
				throw new DataCrossoverException("Correct and incorrect data point to the same directory.");

			if (!Directory.Exists(outputDirectory))
				throw new DirectoryNotFoundException("Output directory was not found.");

			if (File.Exists(outputDirectory + outputName + ".h5") && !allowFileOverride)
				throw new FileAlreadyExistsException("The file: " + outputDirectory + outputName + ".h5 already exists, set _allowFileOverride to 'TRUE' to skip this check.");

			if (File.Exists(outputDirectory + outputName + ".json") && !allowFileOverride)
				throw new FileAlreadyExistsException("The file: " + outputDirectory + outputName + ".json already exists, set _allowFileOverride to 'TRUE' to skip this check.");

			if (outputName == null)
				throw new NoParameterGivenException("No output name was given.");

			// Get total number of '.csv' files inside Directory.
			int fileCount = Directory.GetFiles(
				correctTrainingData,
				"*.csv*",
				SearchOption.TopDirectoryOnly
			).Length;

			fileCount += Directory.GetFiles(
				incorrectTrainingData,
				"*.csv*",
				SearchOption.TopDirectoryOnly
			).Length;

			dataset = new double[fileCount][];
			trainingAnswers = new double[fileCount][];

			DirectoryInfo inputDirectory = new DirectoryInfo(correctTrainingData);

			//CSVLoader loader;
			//List<Sample<JointMeasurement>> table;
			//Motion3DImage image;

			CSVLoaderSettings settings;
			CSVLoader loader;
			ArrayCreator creator;

			int index = 0;

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{

				// Declare loader settings.
				settings = new CSVLoaderSettings
				{
					filepath = file.FullName,
					TrimLeft = 1,
					TrimRight = 0
				};

				// Generate loader.
				loader = new CSVLoader(settings);

				// Create array with the ArrayCreator from CSVloader.
				creator = new ArrayCreator();
				Project1DInto2D(creator.CreateArray(loader.LoadData(), networkInputSize), index);

				// Set answer to true.
				trainingAnswers[index] = new[] { 1.0 };
				index++;

			}

			inputDirectory = new DirectoryInfo(incorrectTrainingData);

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{

				// Declare loader settings.
				settings = new CSVLoaderSettings
				{
					filepath = file.FullName,
					TrimLeft = 1,
					TrimRight = 0
				};

				// Generate loader.
				loader = new CSVLoader(settings);

				// Create array with ArrayCreator from CSVloader.
				creator = new ArrayCreator();
				Project1DInto2D(creator.CreateArray(loader.LoadData(), networkInputSize), index);

				// Set answer to false.
				trainingAnswers[index] = new[] { 0.0 };
				index++;

			}

			trainer = new NetworkTrainer(
				_inputData: ref dataset,
				_inputAnswers: ref trainingAnswers,
				_outputDirectory: outputDirectory,
				_outputName: outputName,
				_inputSize: networkInputSize);

			return trainer.Run();
		}

		private bool Predict()
		{
			if (!File.Exists(trainedNetwork))
				throw new FileNotFoundException("Trained network was not found.");

			if (!Regex.IsMatch(trainedNetwork, @"(\.eg$)"))
				throw new WrongFileTypeException("Wrong network location given.");

			if (!File.Exists(predictData))
				throw new FileNotFoundException("Network input was not found.");

			if (!Regex.IsMatch(predictData, @"(\.csv$)"))
				throw new WrongFileTypeException("Wrong network input given.");

			// Declare loader settings.
			CSVLoaderSettings settings = new CSVLoaderSettings
			{
				filepath = predictData,
				TrimLeft = 1,
				TrimRight = 0
			};

			// Generate loader.
			CSVLoader loader = new CSVLoader(settings);

			// Create array with the ArrayCreator from CSVloader.
			ArrayCreator creator = new ArrayCreator();
			double[] predictorInput = creator.CreateArray(loader.LoadData(), networkInputSize);

			// load CSV file and create image
			//CSVLoader loader;
			//List<Sample<JointMeasurement>> table;
			//Motion3DImage image;

			//loader = new CSVLoader(predictData);

			//table = loader.GetData();

			//image = new Motion3DImage(100);
			//image.CreateImageFromTable(ref table);

			//int[,] data = image.GetData();

			predictor = new NetworkPredictor(
				_trainedNetwork: trainedNetwork,
				_inputData: predictorInput,
				_networkInputSize: networkInputSize
			);

			return predictor.Run();
		}

		public void Project1DInto2D(double[] source, int index)
		{
			double[] temp = new double[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				temp[i] = source[i];
			}

			dataset[index] = temp;
		}
	}
}
