using System.IO;
using Keras.Models;

namespace MotionRecognition
{
	class NetworkPredictor
	{
		BaseModel model;
		private string inputData;

		public NetworkPredictor(string _networkWeights,
								string _networkLayers,
								string _inputData)
		{
			model = Sequential.ModelFromJson(File.ReadAllText(_networkLayers));
			model.LoadWeight(_networkWeights);
			inputData = _inputData;
		}

		public bool Run()
		{


			return false;
		}

	}
}
