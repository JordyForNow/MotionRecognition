using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class MotionRecognizerTests
	{
		private MotionRecognizer recognizer;
//		public void Setup1()
//		{
//			recognizer = new MotionRecognizer(
//				_action: networkActions.PREDICT,
//				_predictData: @"D:\GitProjects\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataCorrect\Move_26.csv",
//				_networkLayers: @"D:\GitProjects\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\movementOne.json",
//				_networkWeights: @"..\testdata\"
//			);
//		}

		[Test]
		public void MotionRecognizerRun_WrongWeightsFile()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\testdata\DataOut\movementOne.json",
				_networkWeights: @"..\testdata\DataOut\movementOne.json"
			);

			Assert.Throws<WrongFileTypeException>(
				() => recognizer.Run());
		}
		
		[Test]
		public void MotionRecognizerRun_WightsFileNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\testdata\DataOut\movementOne.json",
				_networkWeights: @"..\testdata\movementOne.h5"
			);

			Assert.Throws<FileNotFoundException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerRun_WrongLayersFile()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\testdata\DataOut\movementOne.h5",
				_networkWeights: @"..\testdata\DataOut\movementOne.json"
			);

			Assert.Throws<WrongFileTypeException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerRun_LayersFileNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\testdata\movementOne.json",
				_networkWeights: @"..\testdata\movementOne.h5"
			);

			Assert.Throws<FileNotFoundException>(
				() => recognizer.Run());
		}





	}
}
