using System.IO;
using MotionRecognition;
using NUnit.Framework;
using DirectoryNotFoundException = MotionRecognition.DirectoryNotFoundException;

namespace UnitTests
{
	class MotionRecognizerTests
	{
		private MotionRecognizer recognizer;

		[Test]
		public void MotionRecognizerPredictRun_WrongWeightsFile_Exception()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\..\..\testdata\DataOut\movementOne.json",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.json"
			);

			Assert.Throws<WrongFileTypeException>(
				() => recognizer.Run());
		}
		
		[Test]
		public void MotionRecognizerPredictRun_WightsFileNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\..\..\testdata\DataOut\movementOne.json",
				_networkWeights: @"..\..\..\testdata\movementOne.h5"
			);

			Assert.Throws<FileNotFoundException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerPredictRun_WrongLayersFile_Exception()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\..\..\testdata\DataOut\movementOne.h5",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.json"
			);

			Assert.Throws<WrongFileTypeException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerPredictRun_LayersFileNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\..\..\testdata\movementOne.json",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.h5"
			);

			Assert.Throws<FileNotFoundException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerPredictRun_WrongPredictDataFile_Exception()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataCorrect\Move_1.h5",
				_networkLayers: @"..\..\..\testdata\DataOut\movementOne.h5",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.json"
			);

			Assert.Throws<WrongFileTypeException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerPredictRun_PredictDataFileNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\Move_1.csv",
				_networkLayers: @"..\..\..\testdata\movementOne.json",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.h5"
			);

			Assert.Throws<FileNotFoundException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerPredictRun_PredictWithGoodParameters_True()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataCorrect\Move_1.csv",
				_networkLayers: @"..\..\..\testdata\DataOut\movementOne.json",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.h5"
			);

			Assert.AreEqual(true, recognizer.Run());
		}

		[Test]
		public void MotionRecognizerPredictRun_PredictWithGoodParameters_False()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.PREDICT,
				_predictData: @"..\..\..\testdata\DataIncorrect\Move_6.csv",
				_networkLayers: @"..\..\..\testdata\DataOut\movementOne.json",
				_networkWeights: @"..\..\..\testdata\DataOut\movementOne.h5"
			);

			Assert.AreEqual(false, recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_DataCorrectDirectoryNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\WrongDirectory\",
				_incorrectTrainingData: @"..\..\..\testdata\DataIncorrect\",
				_outputDirectory: @"..\..\..\testdata\DataOut\",
				_outputName: @"movementOne"
			);

			Assert.Throws<DirectoryNotFoundException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_DataIncorrectDirectoryNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\DataCorrect\",
				_incorrectTrainingData: @"..\..\..\testdata\WrongDirectory\",
				_outputDirectory: @"..\..\..\testdata\DataOut\",
				_outputName: @"movementOne"
			);

			Assert.Throws<DirectoryNotFoundException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_DataIncorrectAndDataCorrectAreSameDirectory_Exception()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\DataCorrect\",
				_incorrectTrainingData: @"..\..\..\testdata\DataCorrect\",
				_outputDirectory: @"..\..\..\testdata\DataOut\",
				_outputName: @"movementOne"
			);

			Assert.Throws<DataCrossoverException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_OutputDirectoryNotFound()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\DataCorrect\",
				_incorrectTrainingData: @"..\..\..\testdata\DataCorrect\",
				_outputDirectory: @"..\..\..\testdata\WrongDirectory\",
				_outputName: @"movementOne"
			);

			Assert.Throws<DataCrossoverException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_OutputFilealreadyExistsAndFileOverrideIsFalse_Exception()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\DataCorrect\",
				_incorrectTrainingData: @"..\..\..\testdata\DataIncorrect\",
				_outputDirectory: @"..\..\..\testdata\DataOut\",
				_outputName: @"movementOne"
			);

			Assert.Throws<FileAlreadyExistsException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_OutputNameIsNull_Exception()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\DataCorrect\",
				_incorrectTrainingData: @"..\..\..\testdata\DataIncorrect\",
				_outputDirectory: @"..\..\..\testdata\DataOut\"
			);

			Assert.Throws<NoParameterGivenException>(
				() => recognizer.Run());
		}

		[Test]
		public void MotionRecognizerTrainRun_TrainWithGoodParameters_True()
		{
			recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"..\..\..\testdata\DataCorrect",
				_incorrectTrainingData: @"..\..\..\testdata\DataIncorrect",
				_outputDirectory: @"..\..\..\testdata\DataOut",
				_outputName: @"movementOne",
				_allowFileOverride: true,
				_epochs: 1
			);

			Assert.AreEqual(true, recognizer.Run());
		}
	}
}
