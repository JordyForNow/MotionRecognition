using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Keras.Models;
using Numpy;

namespace MotionRecognition
{
	class NetworkPredictor
	{
		BaseModel model;
		private NDarray inputData;
		private int networkInputSize;

		public NetworkPredictor(string _networkWeights,
								string _networkLayers,
								int[,] _inputData,
								int _networkInputSize)
		{
			//Load model and weight
			model = Sequential.ModelFromJson(File.ReadAllText(_networkLayers));
			model.LoadWeight(_networkWeights);
			inputData = new NDarray(_inputData);
			networkInputSize = _networkInputSize;
		}

		public bool Run()
		{
			var pred = model.Predict(inputData.reshape(-1, networkInputSize*2, networkInputSize, 1));

			return (int)pred.argmax() == 1;
		}

	}
}
