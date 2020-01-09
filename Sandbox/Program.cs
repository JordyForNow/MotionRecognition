using System;
using System.IO;
using Encog.Engine.Network.Activation;
using MotionRecognition;
using MotionRecognition.src.Recognizer;

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

		}
	}
}
