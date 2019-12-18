using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Keras;
using Keras.Layers;
using Keras.Models;
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

			//Build sequential model
			var model = new Sequential();
			model.Add(new Dense(32, activation: "relu", input_shape: new Shape(2)));
			model.Add(new Dense(64, activation: "relu"));
			model.Add(new Dense(1, activation: "sigmoid"));

			//Compile and train
			model.Compile(optimizer: "sgd", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });
			model.Fit(trainingData, trainingAnswers, batch_size: batchSize, epochs: epochs, verbose: 1);

			//Save model and weights
			string json = model.ToJson();
			File.WriteAllText($"{outputDirectory}model.json", json);
			model.SaveWeight($"{outputDirectory}model.h5");

//			//Load model and weight
//			var loaded_model = Sequential.ModelFromJson(File.ReadAllText("model.json"));
//			loaded_model.LoadWeight("model.h5");


			return false;
		}

	}
}
