using System;
using System.IO;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Persist;

namespace MotionRecognition
{
	class NetworkPredictor
	{
		private BasicNetwork trainedNetwork;
		private double[] inputData;
		private bool verbose;

		public NetworkPredictor(
			string _trainedNetwork,
			double[] _inputData,
			bool _verbose)
		{
			// Load trained network.
			FileInfo trainedNetworkFile = new FileInfo(_trainedNetwork);
			trainedNetwork = (BasicNetwork)EncogDirectoryPersistence.LoadObject(trainedNetworkFile);
			inputData = _inputData;
			verbose = _verbose;
		}

		public bool Run()
		{
			// Predict if a movement is correct.
			Console.Write(verbose ? "Predicting movement.\n" : "");
			IMLData output = trainedNetwork.Compute(new BasicMLData(inputData));
			double prediction = output[0];
			Console.Write(verbose ? "Prediction: " + prediction + "\n": "");

			// Return prediction.
			return (1 - prediction) > 0.5;
		}
	}
}
