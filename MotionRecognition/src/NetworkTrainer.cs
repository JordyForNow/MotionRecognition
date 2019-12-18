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
			//Shape input_shape = null;
			//if (Backend.ImageDataFormat() == "channels_first")
			//{
			//	input_shape = (9, inputSize * 2, inputSize);
			//} else
			//{
			//	input_shape = (inputSize * 2, inputSize, 9);
			//}

			//Console.WriteLine(trainingData);

			//Build sequential model
			//var model = new Sequential();
			//model.Add(new Dense(32, activation: "relu", input_shape: new Shape(2)));
			//model.Add(new Dense(64, activation: "relu"));
			//model.Add(new Dense(1, activation: "sigmoid"));

			//var model = new Sequential();
			//model.Add(new Conv2D(32, kernel_size: (3, 3).ToTuple(),
			//						activation: "relu",
			//						input_shape: input_shape));
			//model.Add(new Conv2D(64, (3, 3).ToTuple(), activation: "relu"));
			//model.Add(new MaxPooling2D(pool_size: (2, 2).ToTuple()));
			//model.Add(new Dropout(0.25));
			//model.Add(new Flatten());
			//model.Add(new Dense(128, activation: "relu"));
			//model.Add(new Dropout(0.5));
			//model.Add(new Dense(2, activation: "softmax"));

			////Compile and train
			//model.Compile(optimizer: "sgd", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });
			//model.Fit(trainingData, trainingAnswers, batch_size: batchSize, epochs: epochs, verbose: 1);

			////Save model and weights
			//string json = model.ToJson();
			//File.WriteAllText($"{outputDirectory}model.json", json);
			//model.SaveWeight($"{outputDirectory}model.h5");

			//			//Load model and weight
			//			var loaded_model = Sequential.ModelFromJson(File.ReadAllText("model.json"));
			//			loaded_model.LoadWeight("model.h5");

			// input image dimensions
			int img_rows = inputSize * 2, img_cols = inputSize;
			int num_classes = 2;

			Shape input_shape = null;

			// the data, split between train and test sets
			var x_train = trainingData;
			var y_train = trainingAnswers;
			//Console.WriteLine(x_train);

			if (Backend.ImageDataFormat() == "channels_first")
			{
				x_train = x_train.reshape(x_train.shape[0], 1, img_rows, img_cols);
				//x_test = x_test.reshape(x_test.shape[0], 1, img_rows, img_cols);
				input_shape = (1, img_rows, img_cols);
			}
			else
			{
				x_train = x_train.reshape(x_train.shape[0], img_rows, img_cols, 1);
				//x_test = x_test.reshape(x_test.shape[0], img_rows, img_cols, 1);
				input_shape = (img_rows, img_cols, 1);
			}

			x_train = x_train.astype(np.float32);
			//x_test = x_test.astype(np.float32);
			x_train /= 255;
			//x_test /= 255;
			Console.WriteLine($"x_train shape: {x_train.shape}");
			Console.WriteLine($"{x_train.shape[0]} train samples");
			//Console.WriteLine($"{x_test.shape[0]} test samples");

			// convert class vectors to binary class matrices
			y_train = Util.ToCategorical(y_train, num_classes);
			//y_test = Util.ToCategorical(y_test, num_classes);

			// Build CNN model
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
			model.Add(new Dense(num_classes, activation: "softmax"));

			model.Compile(loss: "categorical_crossentropy",
				optimizer: new Adadelta(), metrics: new string[] { "accuracy" });

			model.Fit(x_train, y_train,
						batch_size: batchSize,
						epochs: epochs,
						verbose: 1);
			//var score = model.Evaluate(x_test, y_test, verbose: 0);
			//Console.WriteLine($"Test loss: {score[0]}");
			//Console.WriteLine($"Test accuracy: {score[1]}");

			//Save model and weights
			string json = model.ToJson();
			File.WriteAllText(outputDirectory + outputName + ".json", json);
			model.SaveWeight(outputDirectory + outputName + ".h5");


			return false;
		}

	}
}
