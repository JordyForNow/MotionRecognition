using System;
using Encog.ML.Data;
using Encog.ML.Train;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks.Layers;
using Encog.Engine.Network.Activation;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Persist;
using System.IO;
using Encog.Neural.Networks;

namespace MotionRecognition
{
	// Layer settings which holds information to generate custom layers.
	public struct EncogLayerSettings
	{
		public IActivationFunction activationFunction;
		public bool hasBias;
		public int neuronCount;
	}

	// Settings which are used while training the network.
	public struct EncogTrainSettings
	{
		public double maxTrainingError;
		public uint maxEpochCount;
		public double[][] dataset;
		public double[][] answers;
	}

	// Settings which are used while predicting with the network.
	public struct EncogPredictSettings
	{
		public double threshold;
		public double[] data;
	}

	public static class EncogWrapper
	{
		// Setup a new network.
		public static bool Instantiate(ref NetworkContainer container)
		{
			if (container.network != null)
				throw new IncorrectActionOrderException("Network has already been instantiated.");

			container.network = new BasicNetwork();

			return true;
		}

		// Add a custom layer to the network.
		public static void AddLayer(ref NetworkContainer container, ref EncogLayerSettings settings)
		{
			if (settings.neuronCount <= 0)
				throw new InvalidNeuronCountException("Neuroncount should be higher than 0.");

			try
			{
				container.network.AddLayer(new BasicLayer(
					settings.activationFunction,
					settings.hasBias,
					settings.neuronCount));
			}
			catch
			{
				throw new EncogException("Adding layer failed.");
			}
		}

		public static void FinalizeNetwork(ref NetworkContainer container)
		{
			try
			{
				container.network.Structure.FinalizeStructure();
				container.network.Reset();
			}
			catch
			{
				throw new EncogException("Failed to finalize network.");
			}
		}

		public static void SaveNetworkToFS(ref NetworkContainer container, string fileName)
		{
			try
			{
				EncogDirectoryPersistence.SaveObject(new FileInfo(fileName), container.network);
			}
			catch
			{
				throw new EncogException("Failed to save network to file system.");
			}
		}

		public static void LoadNetworkFromFS(ref NetworkContainer container, string fileName)
		{
			try
			{
				container.network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(new FileInfo(fileName));
			}
			catch
			{
				throw new EncogException("Failed to load network from file system.");
			}
		}

		// Train network using the according settings.
		public static void Train(ref NetworkContainer container, ref EncogTrainSettings settings)
		{
			if (settings.maxTrainingError <= 0)
				throw new EncogException("Maxtrainingerror should be higher than 0");

			// Create training data.
			IMLDataSet trainingSet = new BasicMLDataSet(settings.dataset, settings.answers);

			// Train the neural network.
			IMLTrain train = new ResilientPropagation(container.network, trainingSet);

			uint epoch = 0;

			do
			{
				train.Iteration();
				if (container.verbose) Console.WriteLine("Epoch # " + epoch + " Error: " + train.Error);
				epoch++;
			} while (train.Error > settings.maxTrainingError && (epoch < settings.maxEpochCount && settings.maxEpochCount > 0));
		}

		// Predict data using a network.
		public static bool Predict(ref NetworkContainer container, ref EncogPredictSettings settings)
		{
			IMLData output = container.network.Compute(new BasicMLData(settings.data));

			if (container.verbose) Console.WriteLine("Raw output: " + output[0]);

			// Return true or false according to threshold.
			return (1 - output[0] < settings.threshold);
		}
	}
}
