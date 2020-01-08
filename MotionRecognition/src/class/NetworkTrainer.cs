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
	public class NetworkTrainer : IEncogTrainer
	{
		public bool Run(ITrainContainer iTrainContainer)
		{
			BaseTrainContainer trainContainer = (BaseTrainContainer)iTrainContainer;
			// create a neural network, without using a factory
			var network = new BasicNetwork();
			network.AddLayer(new BasicLayer(null, true, trainContainer.dataset[0].Length));
			network.AddLayer(new BasicLayer(new ActivationElliott(), true, 100));
			network.AddLayer(new BasicLayer(new ActivationElliott(), false, 1));
			network.Structure.FinalizeStructure();
			network.Reset();

			// Create training data.
			IMLDataSet trainingSet = new BasicMLDataSet(trainContainer.dataset, trainContainer.trainingAnswers);

			// Train the neural network.
			IMLTrain train = new ResilientPropagation(network, trainingSet);

			int epoch = 1;

			do
			{
				train.Iteration();
				Console.Write(trainContainer.netContainer.verbose ? "Epoch # " + epoch + " Error: " + train.Error + "\n": "");
				epoch++;
			} while (train.Error > trainContainer.netContainer.maxTrainingError);

			// Test the neural network.
			Console.Write(trainContainer.netContainer.verbose ? "Neural Network Results: \n" : "");
			foreach (IMLDataPair pair in trainingSet)
			{
				IMLData output = network.Compute(pair.Input);
				Console.Write(trainContainer.netContainer.verbose ? pair.Input[0] + " , " + pair.Input[1] + ", actual= " + output[0] + ", ideal= " + pair.Ideal[0] + "\n": "");
			}

			// The neural network is saved to the specified directory.
			Console.Write(trainContainer.netContainer.verbose ? "Saving neural network to: " + trainContainer.outputDirectory + "/" + trainContainer.outputName + ".eg" + "\n" : "");
			FileInfo networkFile = new FileInfo(trainContainer.outputDirectory + "/" + trainContainer.outputName + ".eg");
			EncogDirectoryPersistence.SaveObject(networkFile, (BasicNetwork)network);

			return true;
		}
	}
}
