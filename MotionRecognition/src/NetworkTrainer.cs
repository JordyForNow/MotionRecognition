using NeuralNetworkNET.APIs.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	class NetworkTrainer
	{
		INeuralNetwork Network;

		public NetworkTrainer(INeuralNetwork Network)
		{
			this.Network = Network;
		}
		
		public void Run()
		{

		}

	}
}
