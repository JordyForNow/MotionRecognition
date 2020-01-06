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
		private int inputSize;

		public NetworkTrainer(
			ref double[][] _inputData,
			ref double[][] _inputAnswers,
			string _outputDirectory,
			string _outputName,
			int _inputSize)
		{
			inputData = _inputData;
			inputAnswers = _inputAnswers;
			outputDirectory = _outputDirectory;
			outputName = _outputName;
			inputSize = _inputSize;
		}

		public bool Run()
		{
			// create a neural network, without using a factory
			var network = new BasicNetwork();
			network.AddLayer(new BasicLayer(null, true, inputData[0].Length));
			network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 100));
			network.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 1));
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
				Console.WriteLine("Epoch # " + epoch + " Error: " + train.Error);
				epoch++;
			} while (train.Error > 0.01);

			// Test the neural network.
			Console.WriteLine("Neural Network Results: ");
			foreach (IMLDataPair pair in trainingSet)
			{
				IMLData output = network.Compute(pair.Input);
				Console.WriteLine(pair.Input[0] + " , " + pair.Input[1] + ", actual= " + output[0] + ", ideal= " + pair.Ideal[0]);
			}

			// The neural network is saved to the specified directory.
			Console.WriteLine("Saving neural network.");
			FileInfo networkFile = new FileInfo(outputDirectory + "/" + outputName + ".eg");
			EncogDirectoryPersistence.SaveObject(networkFile, (BasicNetwork)network);

			return false;
		}

	}
}
