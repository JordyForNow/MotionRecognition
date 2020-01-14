using System;
using System.IO;
using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			NetworkContainer container = new NetworkContainer
			{
				verbose = true
			};

			CountNetworkTrainSettings trainSettings = new CountNetworkTrainSettings
			{
				correctInputDirectory = @"../../../CorrectTestData",
				incorrectInputDirectory = @"../../../IncorrectTestData",

				outputDirectory = @"../../../DataOut",
				outputName = "ModelOne",
				
				sampleCount = 10
			};

			CountNetworkTrainController.prepareData(ref trainSettings, ref container);
			CountNetworkTrainController.prepareNetwork(ref trainSettings, ref container);
			CountNetworkTrainController.train(ref trainSettings, ref container);

			CountNetworkPredictSettings predictSettings = new CountNetworkPredictSettings
			{
				trainedNetwork = @"../../../DataOut\ModelOne.eg",
				predictData = @"../../../testCor\0-2.csv",

				networkInputSize = 10
			};

			CountNetworkPredictController.preparePredictor(ref predictSettings, ref container);
			Console.WriteLine(CountNetworkPredictController.predict(ref predictSettings, ref container));
		}
	}
}
