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
	public struct EncogLayerSettings
	{
		public IActivationFunction activationFunction;
		public bool hasBias;
		public int neuronCount;
	}
	public struct EncogTrainSettings
	{
		public double maxTrainingError;
		public uint maxEpochCount;
		public double[][] dataset;
		public double[][] answers;
	}
	public struct EncogPredictSettings
	{
		public double threshold;
		public double[] data;
	}
	public static class EncogWrapper
	{
		public static bool Instantiate(ref NetworkContainer container)
		{
			if (container.network != null)
				throw new IncorrectActionOrderException("Network has already been instantiated.");

			container.network = new BasicNetwork();

			return true;
		}

		public static bool AddLayer(ref NetworkContainer container, ref EncogLayerSettings settings)
		{
			if (settings.neuronCount <= 0) return false;
			try
			{
				container.network.AddLayer(new BasicLayer(
					settings.activationFunction,
					settings.hasBias,
					settings.neuronCount));
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool finalizeNetwork(ref NetworkContainer container)
		{
			try
			{
				container.network.Structure.FinalizeStructure();
				container.network.Reset();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool SaveNetworkToFS(ref NetworkContainer container, string fileName)
		{
			try
			{
				EncogDirectoryPersistence.SaveObject(new FileInfo(fileName), container.network);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool LoadNetworkFromFS(ref NetworkContainer container, string fileName)
		{
			try
			{
				container.network = (BasicNetwork)EncogDirectoryPersistence.LoadObject(new FileInfo(fileName));
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool Train(ref NetworkContainer container, ref EncogTrainSettings settings)
		{
			if (settings.maxTrainingError <= 0) return false;

			// Create training data.
			IMLDataSet trainingSet = new BasicMLDataSet(settings.dataset, settings.answers);

			// Train the neural network.
			IMLTrain train = new ResilientPropagation(container.network, trainingSet);

			uint epoch = 0;

			do
			{
				train.Iteration();
				if(container.verbose) Console.WriteLine("Epoch # " + epoch + " Error: " + train.Error);
				epoch++;
			} while (train.Error > settings.maxTrainingError && (epoch < settings.maxEpochCount && settings.maxEpochCount > 0));

			return true;
		}

		public static bool Predict(ref NetworkContainer container, ref EncogPredictSettings settings)
		{
			IMLData output = container.network.Compute(new BasicMLData(settings.data));

			if (container.verbose) Console.WriteLine("Raw output: " + output[0]);

			return (1 - output[0] < settings.threshold);
		}
	}
}
