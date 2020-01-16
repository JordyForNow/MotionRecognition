using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MotionRecognition
{

    public struct CountNetworkPredictSettings
    {
        // Filled by the preparePredictor Function.
        public EncogPredictSettings predictSettings;
        // Location of the to be loaded network.
        public string trainedNetwork;
        // Location of the to predict data.
        public string predictData;

        public uint sampleCount;
    };

    public class CountNetworkPredictController : INetworkPredictController<CountNetworkPredictSettings>
    {

        public static void PreparePredictor(ref NetworkContainer container, ref CountNetworkPredictSettings settings)
        {
            TestForErrors(ref settings);

            EncogWrapper.LoadNetworkFromFS(ref container, settings.trainedNetwork);

            List<ICSVFilter> baseFilters = new List<ICSVFilter>(1);
            ICSVFilter quaternions = new CSVEvenColumnFilter();
            baseFilters.Add(quaternions);

            // Setup loader.
            CSVLoaderSettings CSVSettings = new CSVLoaderSettings
            {
                filePath = settings.predictData,
                trimUp = 1,
                trimDown = 0,
                filters = baseFilters
            };

            var data = CSVLoader<Vector3>.LoadData(ref CSVSettings);

            // Initialize CountBased Transformer settings.
            IntervalBasedTransformerSettings countSettings = new IntervalBasedTransformerSettings
            {
                sampleList = data,
                count = settings.sampleCount
            };
            CountBasedTransformer countTransformer = new CountBasedTransformer();

            if (settings.predictSettings == null) settings.predictSettings = new EncogPredictSettings();
            
            settings.predictSettings.data = countTransformer.GetNeuralInput(countSettings);

            if (settings.predictSettings.data.Length != container.network.InputCount)
                throw new NoNetworkMatchException("Sample count doesn't match network input count.");

        }

        // Predict the output of a dataset using an existing network.
        public static bool Predict(ref NetworkContainer container, ref CountNetworkPredictSettings settings)
        {

            if (settings.predictSettings.data == null)
                throw new IncorrectActionOrderException("Prepare predictor before predicting.");

            return EncogWrapper.Predict(ref container, ref settings.predictSettings);

        }

        // Test if all inputs are valid.
        private static void TestForErrors(ref CountNetworkPredictSettings settings)
        {

            if (!File.Exists(settings.trainedNetwork))
                throw new FileNotFoundException("Trained network was not found.");

            if (!Regex.IsMatch(settings.trainedNetwork, @"(\.eg$)"))
                throw new WrongFileTypeException("Wrong network location given.");

            if (!File.Exists(settings.predictData))
                throw new FileNotFoundException("Network input was not found.");

            if (!Regex.IsMatch(settings.predictData, @"(\.csv$)"))
                throw new WrongFileTypeException("Wrong network input given.");
        }
    }
}
