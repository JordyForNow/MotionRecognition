using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
		private int CSVSize;
		private int epochs;
		private int batchSize;

		private int[,,] dataset;
		private int[] trainingAnswers;

		public MotionRecognizer(networkActions _action,
								string _predictData = null,
								string _correctTrainingData = null,
								string _incorrectTrainingData = null,
								string _outputDirectory = null,
								string _outputName = null,
								string _networkWeights = null,
								string _networkLayers = null,
								int _networkInputSize = 100,
								int _CSVSize = 21,
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
			CSVSize = _CSVSize;
			epochs = _epochs;
			batchSize = _batchSize;

			SetPathVariables();
		}

		public bool Run()
		{
			switch(action)
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

			if (File.Exists(outputDirectory + outputName + ".h5"))
				throw new FileAlreadyExistsException("The file: " + outputDirectory + outputName + ".h5 already exists.");

			if (File.Exists(outputDirectory + outputName + ".json"))
				throw new FileAlreadyExistsException("The file: " + outputDirectory + outputName + ".json already exists.");

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

			dataset = new int[fileCount, networkInputSize, networkInputSize * 2];
			trainingAnswers = new int[fileCount];

			DirectoryInfo inputDirectory = new DirectoryInfo(correctTrainingData);

			CSVLoader loader;
			List<Sample<JointMeasurement>> table;
			Motion3DImage image;
			int index = 0;

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{

				loader = new CSVLoader(file.FullName, CSVSize);

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

				loader = new CSVLoader(file.FullName, CSVSize);

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

			predictor = new NetworkPredictor(
				_networkWeights: networkWeights,
				_networkLayers: networkLayers,
				_inputData: predictData);

			return predictor.Run();
		}

		public void Project2DInto3D(int[,] source, int index)
		{
			for (int i = 0; i < networkInputSize; i++)
			{
				for (int j = 0; j < networkInputSize * 2; j++)
				{
					dataset[index, i, j] = source[i, j];
				}
			}
		}

		private void SetPathVariables()
		{
			string path = "";
			
			// Check if current platform is windows.
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				// Get user directory.
				path = Directory.GetParent(
						Environment.GetFolderPath(
						Environment.SpecialFolder.ApplicationData)
					).FullName;

				if (Environment.OSVersion.Version.Major >= 6)
				{
					path = Directory.GetParent(path).ToString();
				}
				
				// Add Python36 folder to path.
				path += @"\AppData\Local\Programs\Python\Python36";
				Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);

				// Add python excecutable to path.
				path += @"\python.exe";
				Environment.SetEnvironmentVariable("PYTHONHOME", path, EnvironmentVariableTarget.Process);
			} else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				// Implement Linux.
				Console.WriteLine("Linux");
			} else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				// Implement MacOs.
				Console.WriteLine("OSX");
			}
		}

	}
}
