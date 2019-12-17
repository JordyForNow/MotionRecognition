using System;
using System.Collections.Generic;
using System.Text;
using Keras;
using Numpy;

namespace MotionRecognition
{
	class NetworkTrainer
	{

		private NDarray trainingData;
		private NDarray trainingAnswers;

		private string outputDirectory;
		private string outputName;
		private int inputSize;
		private int epochs;
		private int batchSize;

		public NetworkTrainer(
			ref int[,,] _inputData,
			ref int[] _inputAnswers,
			string _outputDirectory,
			string _outputName,
			int _inputSize,
			int _epochs,
			int _batchSize)
		{
			trainingData = new NDarray(_inputData);
			trainingAnswers = new NDarray(_inputAnswers);
			outputDirectory = _outputDirectory;
			outputName = _outputName;
			inputSize = _inputSize;
			epochs = _epochs;
			batchSize = _batchSize;
		}

		public bool Run()
		{
			return false;
		}

	}
}
