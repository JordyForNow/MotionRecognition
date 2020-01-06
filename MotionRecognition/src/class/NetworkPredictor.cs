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

		public NetworkPredictor(
			string _trainedNetwork,
			double[] _inputData)
		{
			// Load trained network.
			FileInfo trainedNetworkFile = new FileInfo(_trainedNetwork);
			trainedNetwork = (BasicNetwork)EncogDirectoryPersistence.LoadObject(trainedNetworkFile);
			inputData = _inputData;

		}

		public bool Run()
		{
			// Predict if a movement is correct.
			IMLData output = trainedNetwork.Compute(new BasicMLData(inputData));
			double prediction = output[0];

			// Return prediction.
			return (1 - prediction) > 0.5 ? true : false;
		}
	}
}
