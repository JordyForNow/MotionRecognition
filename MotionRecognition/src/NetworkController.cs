//using NeuralNetworkNET.APIs.Interfaces;
//using System;
//
//
//namespace MotionRecognition
//{
//	public class NetworkController
//	{
//
//		private INeuralNetwork network;
//		private NetworkTrainer trainer;
//		private NetworkPredictor predictor;
//
//		public NetworkController()
//		{
//			
//		}
//
//		public NetworkController(INeuralNetwork _network)
//		{
//			network = _network;
//		}
//
//		public void TrainNetwork()
//		{
//			if (network == null)
//			{
//				throw new NoNetworkAvailableException("No network was found.");
//			}
//
//			trainer = new NetworkTrainer(network);
//			trainer.Run();
//		}
//
//		public bool PredictNetwork()
//		{
//			predictor = new NetworkPredictor(network);
//			return predictor.Run();
//		}
//
//	}
//}
