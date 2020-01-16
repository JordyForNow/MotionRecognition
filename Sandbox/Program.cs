using MotionRecognition;
using MotionRecognitionHelper;
using System;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			// Main program.
			// A Network container is used to control and hold a network
            NetworkContainer container = new NetworkContainer
            {
                verbose = true
            };


            // Used to declare settings for a particular network
            CountNetworkTrainSettings trainSettings = new CountNetworkTrainSettings
            {
                correctInputDirectory = @"./Data_Correct",
                incorrectInputDirectory = @"./Data_Incorrect",
                outputDirectory = @"./Data_Out",
                outputName = "Handleiding",

                sampleCount = 20
            };

            CountNetworkTrainController.PrepareData(ref container, ref trainSettings);
            CountNetworkTrainController.PrepareNetwork(ref container, ref trainSettings);
            CountNetworkTrainController.Train(ref container, ref trainSettings);

            CountNetworkPredictSettings predictSettings = new CountNetworkPredictSettings
            {

                trainedNetwork = @"./Data_Out/Handleiding.eg",
                predictData = @"./Data_Predict/data.csv",
                sampleCount = 20,

				predictSettings = new EncogPredictSettings 
				{
					threshold = 0.94
				}
            };

            CountNetworkPredictController.PreparePredictor(ref container, ref predictSettings);
            Console.Write(CountNetworkPredictController.Predict(ref container, ref predictSettings));

            Console.WriteLine(predictSettings.predictSettings.threshold);

            predictSettings.trainedNetwork = @"./Data_Out/Handleiding.eg";
            predictSettings.predictData = @"./Data_Predict/0-4.csv";

            CountNetworkPredictController.PreparePredictor(ref container, ref predictSettings);
            Console.Write(CountNetworkPredictController.Predict(ref container, ref predictSettings));
            Console.WriteLine(predictSettings.predictSettings.threshold);
		}
	}
}

//using System;
//using System.IO;
//using MotionRecognition;

//namespace Sandbox
//{
//	class Program
//	{
//		static void Main(string[] args)
//		{
//			NetworkContainer container = new NetworkContainer
//			{
//				verbose = true
//			};

//			CountNetworkTrainSettings trainSettings = new CountNetworkTrainSettings
//			{
//				correctInputDirectory = @"../../../CorrectTestData",
//				incorrectInputDirectory = @"../../../IncorrectTestData",

//				outputDirectory = @"../../../DataOut",
//				outputName = "ModelOne",

//				sampleCount = 10
//			};

//			CountNetworkTrainController.prepareData(ref trainSettings, ref container);
//			CountNetworkTrainController.prepareNetwork(ref trainSettings, ref container);
//			CountNetworkTrainController.train(ref trainSettings, ref container);

//			CountNetworkPredictSettings predictSettings = new CountNetworkPredictSettings
//			{
//				trainedNetwork = @"../../../DataOut/ModelOne.eg",
//				predictData = @"../../../TestCor/0-2.csv",

//				networkInputSize = 10
//			};

//			CountNetworkPredictController.preparePredictor(ref predictSettings, ref container);
//			Console.WriteLine(CountNetworkPredictController.predict(ref predictSettings, ref container));
//		}
//	}
//}

