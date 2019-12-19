namespace MotionRecognition
{
	class NetworkTrainer
	{
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
