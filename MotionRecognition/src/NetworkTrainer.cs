using NeuralNetworkNET.APIs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	class NetworkTrainer
	{
		INeuralNetwork network;

		public NetworkTrainer(INeuralNetwork _network)
		{
			network = _network;
		}
		
		public void Run()
		{

		}

	}
}
