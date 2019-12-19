using System.IO;

namespace MotionRecognition
{
	class NetworkPredictor
	{
		private int networkInputSize;

		public NetworkPredictor(string _networkWeights,
								string _networkLayers,
								int[,] _inputData,
								int _networkInputSize)
		{
			//Load model and weight
			networkInputSize = _networkInputSize;
		}

		public bool Run()
		{
			return false;
		}

	}
}
