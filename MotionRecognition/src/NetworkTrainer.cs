using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Keras;
using Keras.Datasets;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Keras.Utils;
using Numpy;

namespace MotionRecognition
{
	class NetworkTrainer
	{

		private NDarray trainingData;
		private NDarray trainingAnswers;

		private string outputDirectory;
		private string outputName;
		private int inputSize;
		private int epochs;
		private int batchSize;

		public NetworkTrainer(
			ref int[,,] _inputData,
			ref int[] _inputAnswers,
			string _outputDirectory,
			string _outputName,
			int _inputSize,
			int _epochs,
			int _batchSize)
		{
			trainingData = new NDarray(_inputData);
			trainingAnswers = new NDarray(_inputAnswers);
			outputDirectory = _outputDirectory;
			outputName = _outputName;
			inputSize = _inputSize;
			epochs = _epochs;
			batchSize = _batchSize;
		}

		public bool Run()
		{
			try
			{
				// Input image dimensions.
				int imgRows = inputSize * 2;
				int imgCols = inputSize;
				int numClasses = 2;

				Shape input_shape = null;

				if (Backend.ImageDataFormat() == "channels_first")
				{
					trainingData = trainingData.reshape(trainingData.shape[0], 1, imgRows, imgCols);
					input_shape = (1, imgRows, imgCols);
				}
				else
				{
					trainingData = trainingData.reshape(trainingData.shape[0], imgRows, imgCols, 1);
					input_shape = (imgRows, imgCols, 1);
				}

				trainingData = trainingData.astype(np.float32);
				trainingData /= 255;
				Console.WriteLine($"trainingData shape: {trainingData.shape}");
				Console.WriteLine($"{trainingData.shape[0]} train samples");

				// Convert class vectors to binary class matrices.
				trainingAnswers = Util.ToCategorical(trainingAnswers, numClasses);

				// Build CNN model.
				var model = new Sequential();
				model.Add(new Conv2D(32, kernel_size: (3, 3).ToTuple(),
										activation: "relu",
										input_shape: input_shape));
				model.Add(new Conv2D(64, (3, 3).ToTuple(), activation: "relu"));
				model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));
				model.Add(new Dropout(0.25));
				model.Add(new Flatten());
				model.Add(new Dense(128, activation: "relu"));
				model.Add(new Dropout(0.5));
				model.Add(new Dense(numClasses, activation: "softmax"));

				model.Compile(loss: "categorical_crossentropy",
					optimizer: new Adadelta(), metrics: new string[] { "accuracy" });

				model.Fit(trainingData, trainingAnswers,
							batch_size: batchSize,
							epochs: epochs,
							verbose: 1);

				// Save model and weights.
				string json = model.ToJson();
				File.WriteAllText(outputDirectory + outputName + ".json", json);
				model.SaveWeight(outputDirectory + outputName + ".h5");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return false;
			}

			return true;
		}

	}
}
