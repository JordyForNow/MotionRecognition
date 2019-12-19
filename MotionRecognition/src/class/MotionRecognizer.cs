using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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
		private string networkWeights;
		private string networkLayers;
		private int networkInputSize;
		private bool allowFileOverride;
		private int epochs;
		private int batchSize;

		private int[,,] dataset;
		private int[] trainingAnswers;

		public MotionRecognizer(
			networkActions _action,
			string _predictData = null,
			string _correctTrainingData = null,
			string _incorrectTrainingData = null,
			string _outputDirectory = null,
			string _outputName = null,
			string _networkWeights = null,
			string _networkLayers = null,
			bool _allowFileOverride = false,
			int _networkInputSize = 100,
			int _epochs = 3,
			int _batchSize = 32)
		{
			action = _action;
			predictData = _predictData;
			correctTrainingData = _correctTrainingData;
			incorrectTrainingData = _incorrectTrainingData;
			outputDirectory = _outputDirectory;
			outputName = _outputName;
			networkWeights = _networkWeights;
			networkLayers = _networkLayers;
			networkInputSize = _networkInputSize;
			allowFileOverride = _allowFileOverride;
			epochs = _epochs;
			batchSize = _batchSize;

			SetPathVariables();
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

			dataset = new int[fileCount, networkInputSize * 2, networkInputSize];
			trainingAnswers = new int[fileCount];

			DirectoryInfo inputDirectory = new DirectoryInfo(correctTrainingData);

			CSVLoader loader;
			List<Sample<JointMeasurement>> table;
			Motion3DImage image;
			int index = 0;

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{

				loader = new CSVLoader(file.FullName);

				table = loader.GetData();

				image = new Motion3DImage(100);
				image.CreateImageFromTable(ref table);

				Project2DInto3D(image.GetData(), index);
				trainingAnswers[index] = 1;
				index++;

			}

			inputDirectory = new DirectoryInfo(incorrectTrainingData);

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{

				loader = new CSVLoader(file.FullName);

				table = loader.GetData();

				image = new Motion3DImage(100);
				image.CreateImageFromTable(ref table);

				Project2DInto3D(image.GetData(), index);
				trainingAnswers[index] = 0;
				index++;

			}

			trainer = new NetworkTrainer(
				_inputData: ref dataset,
				_inputAnswers: ref trainingAnswers,
				_outputDirectory: outputDirectory,
				_outputName: outputName,
				_inputSize: networkInputSize,
				_epochs: epochs,
				_batchSize: batchSize);

			return trainer.Run();
		}

		private bool Predict()
		{
			if (!Regex.IsMatch(networkWeights, @"(\.h5$)"))
				throw new WrongFileTypeException("Wrong network weights location given.");

			if (!File.Exists(networkWeights))
				throw new FileNotFoundException("Network weights were not found.");

			if (!Regex.IsMatch(networkLayers, @"(\.json$)"))
				throw new WrongFileTypeException("Wrong network layers location given.");

			if (!File.Exists(networkLayers))
				throw new FileNotFoundException("Network layers were not found.");

			if (!Regex.IsMatch(predictData, @"(\.csv$)"))
				throw new WrongFileTypeException("Wrong network input given.");

			if (!File.Exists(predictData))
				throw new FileNotFoundException("Network input was not found.");

			// load CSV file and create image
			CSVLoader loader;
			List<Sample<JointMeasurement>> table;
			Motion3DImage image;

			loader = new CSVLoader(predictData);

			table = loader.GetData();

			image = new Motion3DImage(100);
			image.CreateImageFromTable(ref table);

			int[,] data = image.GetData();

			predictor = new NetworkPredictor(
				_networkWeights: networkWeights,
				_networkLayers: networkLayers,
				_inputData: data,
				_networkInputSize: networkInputSize);

			return predictor.Run();
		}

		public void Project2DInto3D(int[,] source, int index)
		{
			for (int i = 0; i < networkInputSize * 2; i++)
			{
				for (int j = 0; j < networkInputSize; j++)
				{
					dataset[index, i, j] = source[i, j];
				}
			}
		}

		private void SetPathVariables()
		{

			StringBuilder pathBuilder = new StringBuilder();

			pathBuilder.Append(System.IO.Path.GetDirectoryName(
				System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase));

			

			pathBuilder.Remove(0, 6);
			pathBuilder.Remove(64, 23);
			
			Console.WriteLine(pathBuilder.ToString());
			pathBuilder.Append(@"vendor\Python\");

			// Set python 3.6.4 home directory.
			Environment.SetEnvironmentVariable("PATH", pathBuilder.ToString() + ";%PATH%", EnvironmentVariableTarget.Process);
			Environment.SetEnvironmentVariable("PATH", pathBuilder.ToString() + @"Lib\site-packages;%PATH%", EnvironmentVariableTarget.Process);

			pathBuilder.Append(@"\python.exe");

			// Set python 3.6.4 excecutable.
			Environment.SetEnvironmentVariable("PYTHONHOME", pathBuilder.ToString(), EnvironmentVariableTarget.Process);

		}
	}
}
