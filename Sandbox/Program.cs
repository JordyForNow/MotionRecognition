using System;
using System.IO;
using System.Collections.Generic;
using Encog.Engine.Network.Activation;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			EncogLayerSettings inputLayerSettings = new EncogLayerSettings();
			inputLayerSettings.activationFunction = null;
			inputLayerSettings.hasBias = true;
			inputLayerSettings.neuronCount = 100;			

			NetworkContainer network = new NetworkContainer();
			EncogWrapper.Instantiate(ref network);
			EncogWrapper.AddLayer(ref network, ref inputLayerSettings);

			EncogLayerSettings hiddenLayerOneSettings = new EncogLayerSettings();
			hiddenLayerOneSettings.activationFunction = new ActivationElliott();
			hiddenLayerOneSettings.hasBias = true;
			hiddenLayerOneSettings.neuronCount = 100;

			EncogWrapper.AddLayer(ref network, ref hiddenLayerOneSettings);

			EncogLayerSettings outputLayerSettings = new EncogLayerSettings();
			outputLayerSettings.activationFunction = new ActivationElliott();
			outputLayerSettings.hasBias = false;
			outputLayerSettings.neuronCount = 1;

			EncogWrapper.AddLayer(ref network, ref outputLayerSettings);
			EncogWrapper.finalizeNetwork(ref network);

			EncogTrainSettings trainSettings = new EncogTrainSettings();

			int correctFileCount = getFileCount(_correctTrainingData);
			int incorrectFileCount = getFileCount(_incorrectTrainingData);

			dataset = new double[correctFileCount + incorrectFileCount][];
			trainingAnswers = new double[correctFileCount + incorrectFileCount][];

			// Compute correct training data.
			computeData(
				netContainer.networkInputSize,
				_correctTrainingData,
				ref dataset,
				ref trainingAnswers,
				1.0,
				0);

			// Compute incorrect training data.
			computeData(
				netContainer.networkInputSize,
				_incorrectTrainingData,
				ref dataset,
				ref trainingAnswers,
				0.0,
				correctFileCount);

			private static int getFileCount(string dataDirectory)
			{
				// Get total number of '.csv' files inside Directory.
				return Directory.GetFiles(
					dataDirectory,
					"*.csv*",
					SearchOption.TopDirectoryOnly
				).Length;
			}

			static void computeData(
				int networkInputSize,
				string inputData,
				ref double[][] outputData,
				ref double[][] outputAnswers,
				double outputValue,
				int index)
			{
				DirectoryInfo inputDirectory = new DirectoryInfo(inputData);

				CSVLoaderSettings settings;
				CSVLoader loader<>;
				ArrayCreator creator;

				//// Initialize CountBased Transformer settings.
				//IntervalBasedTransformerSettings countSettings = new IntervalBasedTransformerSettings
				//{
				//	sampleList = data,
				//	count = 10
				//};
				//CountBasedTransformer countTransformer = new CountBasedTransformer();
				//countTransformer.GetNeuralInput(countSettings);

				//// Setup loader.
				//CSVLoaderSettings settings = new CSVLoaderSettings();
				//settings.filePath = dataPath + "data.csv";
				//settings.trimUp = 1;
				//settings.trimDown = 0;

				//var data = CSVLoader<Vector3>.LoadData(ref settings);

				//List<ICSVFilter> filters = new List<ICSVFilter>(1);
				//ICSVFilter quaternions = new CSVEvenColumnFilter();
				//filters.Add(quaternions);
				//settings.filters = filters;

				foreach (var file in inputDirectory.GetFiles("*.csv"))
				{

					//// Declare loader settings.
					//settings = new CSVLoaderSettings
					//{
					//	filepath = file.FullName,
					//	TrimLeft = 1,
					//	TrimRight = 0
					//};

					//// Generate loader.
					//loader = new CSVLoader(settings);

					//// Create array with ArrayCreator from CSVloader.
					//creator = new ArrayCreator();
					//Project1DInto2D(creator.CreateArray(
					//	loader.LoadData(), networkInputSize),
					//	ref outputData,
					//	index);

					//// Set answer to given value.
					//outputAnswers[index] = new[] { outputValue };
					//index++;

				}
			}

			//TrainController.Train(
			//	new NetworkContainer(),
			//	new BaseTrainContainer(),
			//	new NetworkTrainer(),
			//	@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\testCor\",
			//	@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\testIncor\",
			//	@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\",
			//	"testOut");


			//MotionRecognizer recognizer = new MotionRecognizer(
			//	_action: networkActions.TRAIN,
			//	_correctTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectData\",
			//	_incorrectTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectData\",
			//	_outputDirectory: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\",
			//	_outputName: @"modelOne",
			//	_allowFileOverride: true,
			//	_verbose: true
			//);

			//recognizer.Run();

			//int correct = 0;
			//int incorrect = 0;

			//DirectoryInfo correctTestDir = new DirectoryInfo(@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectTestData\");

			//DirectoryInfo incorrectTestDir = new DirectoryInfo(@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectTestData\");

			//foreach (var file in correctTestDir.GetFiles("*.csv"))
			//{
			//	recognizer = new MotionRecognizer(
			//		_action: networkActions.PREDICT,
			//		_trainedNetwork: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\modelOne.eg",
			//		_predictData: file.FullName,
			//		_verbose: true
			//	);

			//	if (recognizer.Run())
			//	{
			//		correct++;
			//	}
			//	else
			//	{
			//		incorrect++;
			//	}
			//}

			//foreach (var file in incorrectTestDir.GetFiles("*.csv"))
			//{
			//	recognizer = new MotionRecognizer(
			//		_action: networkActions.PREDICT,
			//		_trainedNetwork: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\modelOne.eg",
			//		_predictData: file.FullName,
			//		_verbose: true
			//	);

			//	if (recognizer.Run())
			//	{
			//		incorrect++;
			//	}
			//	else
			//	{
			//		correct++;
			//	}
			//}

			//Console.WriteLine("correct: " + correct + " incorrect: " + incorrect);

			//recognizer = new MotionRecognizer(
			//	_action: networkActions.PREDICT,
			//	_trainedNetwork: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\modelOne.eg",
			//	_predictData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectData\Move_13.csv",
			//	_verbose: true
			//);

			//recognizer.Run();

			// Loader settings 
			// CSVLoaderSettings settings = new CSVLoaderSettings();
			// settings.filePath = dataPath + "data.csv";
			// settings.CSVHasHeader = true;
			// settings.trimUp = 0;
			// settings.trimDown = 0;

			// List<ICSVFilter> filters = new List<ICSVFilter>(1);

			// // This filter
			// ICSVFilter quaternions = new CSVEvenColumnFilter();
			// filters.Add(quaternions);
			// settings.filters = filters;

			// var data = CSVLoader<Vector3>.LoadData(ref settings);

			// ImageTransformerSettings transformerSettings = new ImageTransformerSettings();
			// transformerSettings.focusJoints = new LeapMotionJoint[] { LeapMotionJoint.PALM };
			// transformerSettings.samples = data;
			// transformerSettings.size = 10;

			// //Transformer settings
			// ImageTransformer transformer = new ImageTransformer();
			// var arr = transformer.GetNeuralInput(transformerSettings);
			// foreach (var d in arr)
			// 	Console.WriteLine(d);

		}
	}
}
