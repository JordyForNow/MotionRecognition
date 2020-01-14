using System;
using System.Collections.Generic;
using System.IO;
using Encog.Engine.Network.Activation;
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
				correctInputDirectory = @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectTestData\",
				incorrectInputDirectory = @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectTestData\",

				outputDirectory = @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\",
				outputName = "ModelOne",
				
				sampleCount = 10
			};

			CountNetworkTrainController.prepareData(ref trainSettings, ref container);
			CountNetworkTrainController.prepareNetwork(ref trainSettings, ref container);
			CountNetworkTrainController.train(ref trainSettings, ref container);

			CountNetworkPredictSettings predictSettings = new CountNetworkPredictSettings
			{
				trainedNetwork = @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\ModelOne.eg",
				predictData = @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\testCor\0-2.csv",

				networkInputSize = 10
			};

			CountNetworkPredictController.preparePredictor(ref predictSettings, ref container);
			Console.WriteLine(CountNetworkPredictController.predict(ref predictSettings, ref container));

		}
	}
}
