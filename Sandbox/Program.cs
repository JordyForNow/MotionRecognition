using System;
using System.IO;
using MotionRecognition;
using MotionRecognition.src.Recognizer;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			NetworkContainer network = new NetworkContainer();
			EncogWrapper.Instantiate(ref network);
			
			
			// create a neural network, without using a factory
			var network = new BasicNetwork();
			network.AddLayer(new BasicLayer(null, true, trainContainer.dataset[0].Length));
			network.AddLayer(new BasicLayer(new ActivationElliott(), true, 100));
			network.AddLayer(new BasicLayer(new ActivationElliott(), false, 1));
			network.Structure.FinalizeStructure();
			network.Reset();


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
