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

		public void TrainNetwork()
		{
			if (network == null)
			{
				throw new NoNetworkAvailableException("No network was found.");
			}

			trainer = new NetworkTrainer();
			trainer.Run();
		}

		public bool PredictNetwork()
		{
			predictor = new NetworkPredictor(network);
			return predictor.Run();
		}

	}
}
