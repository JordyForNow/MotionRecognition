using System;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{

			MotionRecognizer recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\DataCorrect\",
				_incorrectTrainingData: @"..\..\..\DataIncorrect\",
				_outputDirectory: @"..\..\..\DataOut\",
				_outputName: @"movementOne",
				_allowFileOverride: true
			);

			//MotionRecognizer recognizer = new MotionRecognizer(
			//	_action: networkActions.PREDICT,
			//	_predictData: @"D:\GitProjects\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataCorrect\Move_26.csv",
			//	_networkLayers: @"D:\GitProjects\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\movementOne.json",
			//	_networkWeights: @"D:\GitProjects\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\movementOne.h5"
			//);

			Console.WriteLine("Run Result:" + recognizer.Run());

		}
	}
}
