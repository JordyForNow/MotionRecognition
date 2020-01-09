using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
    public class InputTransformerTests
    {
        string dataPath = @"../../../Data/";
        IMovementTransformer<IntervalBasedTransformerSettings> intervalTransformer;
        IMovementTransformer<IntervalBasedTransformerSettings> countTransformer;
        IMovementTransformer<ImageTransformerSettings> imageTransformer;
        IntervalBasedTransformerSettings intervalSettings;
        IntervalBasedTransformerSettings countSettings;
        ImageTransformerSettings imageSettings;

        [SetUp]
        public void Setup()
        {
            // Setup loader.
            CSVLoaderSettings settings = new CSVLoaderSettings();
            settings.filePath = dataPath + "data.csv";
            settings.trimUp = 1;
            settings.trimDown = 0;

            List<ICSVFilter> filters = new List<ICSVFilter>(1);
            ICSVFilter quaternions = new CSVEvenColumnFilter();
            filters.Add(quaternions);
            settings.filters = filters;

            var data = CSVLoader<Vector3>.LoadData(ref settings);

            // Initialize IntervalBased Transformer settings.
            intervalSettings = new IntervalBasedTransformerSettings();
            intervalSettings.sampleList = data;
            intervalSettings.interval = 4;
            intervalTransformer = new IntervalBasedTransformer();

            // Initialize CountBased Transformer settings.
            countSettings = new IntervalBasedTransformerSettings();
            countSettings.sampleList = data;
            countSettings.count = 10;
            countTransformer = new CountBasedTransformer();

            // Initialize Image Transformer.
            imageSettings = new ImageTransformerSettings();
            imageSettings.focusJoints = new LeapMotionJoint[] { LeapMotionJoint.PALM };
            imageSettings.samples = data;
            imageSettings.size = 10;
            imageTransformer = new ImageTransformer();
        }

        [Test]
        public void IntervalBasedTransformerReturnsValues()
        {
            Assert.IsNotEmpty(intervalTransformer.GetNeuralInput(intervalSettings));
        }

        [Test]
        public void CountBasedTransformerReturnsValues()
        {
            Assert.IsNotEmpty(countTransformer.GetNeuralInput(countSettings));
        }

        [Test]
        public void ImageTransformerReturnsValues()
        {
            Assert.IsNotEmpty(imageTransformer.GetNeuralInput(imageSettings));
        }

        [Test]
        public void FactoriesReturnDifferentResults()
        {
            Assert.AreNotEqual(intervalTransformer.GetNeuralInput(intervalSettings), countTransformer.GetNeuralInput(countSettings));
        }

        [Test]
        public void FactoriesReturnEqualResults()
        {
            // 59 rows in data file, a run with count 5 should equal a run with an interval of 11 (59 / 5 = 11)
            countSettings.count = 5;
            double[] countTransformerResult = countTransformer.GetNeuralInput(countSettings);

            intervalSettings.interval = 11;
            double[] intervalTransformerResult = intervalTransformer.GetNeuralInput(intervalSettings);

            Assert.AreEqual(countTransformerResult, intervalTransformerResult);
        }

        // ImageFactory
        [Test]
        public void ImageFactoryReturns3DImage()
        {
            var image = imageTransformer.GetNeuralInput(imageSettings);
            int expectedLength = 0;
            // One dimensional Image.
            expectedLength += imageSettings.size * imageSettings.size
                // Top and Front View.
                * 2;
            Assert.IsTrue(image.Length == expectedLength);
        }
    }
}
