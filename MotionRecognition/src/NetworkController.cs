using NeuralNetworkNET.APIs.Interfaces;
using System;


namespace MotionRecognition
{
	public class NetworkController
	{

		private INeuralNetwork Network;
		private NetworkTrainer Trainer;
		private NetworkPredictor Predictor;

		public NetworkController()
		{
			
		}

		public NetworkController(INeuralNetwork network)
		{
			this.Network = network;
		}

		public void TrainNetwork()
		{
			if (Network == null)
			{
				throw new NoNetworkAvailableException("No network was found.");
			}

			Trainer = new NetworkTrainer(Network);
			Trainer.Run();
		}

		public bool PredictNetwork()
		{
			return false;
		}

	}
}
