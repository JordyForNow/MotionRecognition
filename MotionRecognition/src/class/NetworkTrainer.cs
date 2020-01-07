using System;
using System.IO;
using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Persist;

namespace MotionRecognition
{
	class NetworkTrainer
	{
		private double[][] inputData;
		private double[][] inputAnswers;
		private string outputDirectory;
		private string outputName;
		private double maxTrainingError;
		private bool verbose;

		public NetworkTrainer(
			ref double[][] _inputData,
			ref double[][] _inputAnswers,
			string _outputDirectory,
			string _outputName,
			double _maxTrainingError,
			bool _verbose)
		{
			inputData = _inputData;
			inputAnswers = _inputAnswers;
			outputDirectory = _outputDirectory;
			outputName = _outputName;
			maxTrainingError = _maxTrainingError;
			verbose = _verbose;
		}

		public bool Run()
		{
			// create a neural network, without using a factory
			var network = new BasicNetwork();
			network.AddLayer(new BasicLayer(null, true, inputData[0].Length));
			network.AddLayer(new BasicLayer(new ActivationElliott(), true, 100));
			network.AddLayer(new BasicLayer(new ActivationElliott(), false, 1));
			network.Structure.FinalizeStructure();
			network.Reset();

			// Create training data.
			IMLDataSet trainingSet = new BasicMLDataSet(inputData, inputAnswers);

			// Train the neural network.
			IMLTrain train = new ResilientPropagation(network, trainingSet);

			int epoch = 1;

			do
			{
				train.Iteration();
				Console.Write(verbose ? "Epoch # " + epoch + " Error: " + train.Error + "\n": "");
				epoch++;
			} while (train.Error > maxTrainingError);

			// Test the neural network.
			Console.Write(verbose ? "Neural Network Results: \n" : "");
			foreach (IMLDataPair pair in trainingSet)
			{
				IMLData output = network.Compute(pair.Input);
				Console.Write(verbose ? pair.Input[0] + " , " + pair.Input[1] + ", actual= " + output[0] + ", ideal= " + pair.Ideal[0] + "\n": "");
			}

			// The neural network is saved to the specified directory.
			Console.Write(verbose ? "Saving neural network to: " + outputDirectory + "/" + outputName + ".eg" + "\n" : "");
			FileInfo networkFile = new FileInfo(outputDirectory + "/" + outputName + ".eg");
			EncogDirectoryPersistence.SaveObject(networkFile, (BasicNetwork)network);

			return true;
		}

	}
}
