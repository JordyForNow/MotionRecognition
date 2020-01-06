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
		private int networkInputSize;

		public NetworkPredictor(
			string _trainedNetwork,
			double[] _inputData,
			int _networkInputSize)
		{
			// Load trained network.
			FileInfo trainedNetworkFile = new FileInfo(_trainedNetwork);
			trainedNetwork = (BasicNetwork)EncogDirectoryPersistence.LoadObject(trainedNetworkFile);
			inputData = _inputData;
			networkInputSize = _networkInputSize;

		}

		public bool Run()
		{
			IMLData data = new BasicMLData(inputData);
			IMLData output = trainedNetwork.Compute(data);
			double prediction = output[0];
			Console.WriteLine(prediction);
			return false;
		}

		//public void loadAndEvaluate()
		//{
		//	System.out.println("Loading network");

		//	BasicNetwork network = (BasicNetwork)EncogDirectoryPersistence.loadObject(new File(FILENAME));

		//	MLDataSet trainingSet = new BasicMLDataSet(XOR_INPUT, XOR_IDEAL);
		//	double e = network.calculateError(trainingSet);
		//	System.out
		//		.println("Loaded network's error is(should be same as above): "
		//				+ e);
		//}

	}
}
