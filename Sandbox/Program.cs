using System;
using MotionRecognition;
using System.IO;

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
                outputName = "DemoNetwork",

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
                    threshold = 0.92
                }
            };

            CountNetworkPredictController.PreparePredictor(ref container, ref predictSettings);
            Console.WriteLine("The given file is: " + CountNetworkPredictController.Predict(ref container, ref predictSettings));

        }
    }
}
