using System;
using System.Collections.Generic;
using System.Text;
using Keras;
using Numpy;

namespace MotionRecognition
{
	class NetworkTrainer
	{

		private NDarray trainingData;
		private NDarray trainingAnswers;

		public NetworkTrainer(ref int[,,] _trainingData, bool[] _trainingAnswers)
		{
			trainingData = new NDarray(_trainingData);
			trainingAnswers = new NDarray(_trainingAnswers);
		}

		public void Run()
		{

			int batch_size = 1;
			int num_classes = 2;
			int epochs = 1;

			// input image dimensions
			int img_rows = 500, img_cols = 500;

			Shape input_shape = null;

			input_shape = (1, img_rows, img_cols);

			trainingData = trainingData.astype(np.float32);
			
			//x_train /= 255;
			Console.WriteLine($"trainingData shape: {trainingData.shape}");
			Console.WriteLine($"{trainingData.shape[0]} train samples");

			//// convert class vectors to binary class matrices
			//trainingAnswers = Util.ToCategorical(trainingAnswers, num_classes);
			//y_test = Util.ToCategorical(y_test, num_classes);

			//// Build CNN model
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
			//model.Add(new Dense(num_classes, activation: "softmax"));

			//model.Compile(loss: "categorical_crossentropy",
			//	optimizer: new Adadelta(), metrics: new string[] { "accuracy" });

			//model.Fit(trainingData, trainingAnswers,
			//			batch_size: batch_size,
			//			epochs: epochs,
			//			verbose: 1,
			//			validation_data: new NDarray[] { x_test, y_test });
			//var score = model.Evaluate(x_test, y_test, verbose: 0);
			//Console.WriteLine($"Test loss: {score[0]}");
			//Console.WriteLine($"Test accuracy: {score[1]}");

			////Save model and weights
			//string json = model.ToJson();
			//File.WriteAllText("model.json", json);
			//model.SaveWeight("model.h5");

		}
	}
}
