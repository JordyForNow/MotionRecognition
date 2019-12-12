using System;

namespace MotionRecognition
{
	public class NetworkController
	{

		private string network;
		private NetworkTrainer trainer;
		private NetworkPredictor predictor;

		public NetworkController()
		{

		}

		public NetworkController(string _network)
		{
			network = _network;
		}

		public void TrainNetwork(ref uint[,,] _trainingData, bool[] _trainingAnswers)
		{
			trainer = new NetworkTrainer(ref _trainingData, _trainingAnswers);
			trainer.Run();
		}

		public bool PredictNetwork()
		{
			if (network == null)
			{
				throw new NoNetworkAvailableException("No network was found.");
			}

			predictor = new NetworkPredictor(network);
			return predictor.Run();
		}
	}
}
