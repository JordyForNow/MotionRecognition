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
		IntervalBasedTransformerSettings intervalSettings;
		IntervalBasedTransformerSettings countSettings;

        [SetUp]
        public void Setup()
        {
			// Setup loader 
            CSVLoaderSettings settings = new CSVLoaderSettings();
            settings.filepath = dataPath + "data.csv";
            settings.trimLeft = 1;
            settings.trimRight = 0;

			List<CSVColumnFilter> filters = new List<CSVColumnFilter>(1);
			CSVColumnFilter quaternions = new CSVEvenColumnsFilter();
			filters.Add(quaternions);

            CSVLoader<Vec3> loader = new CSVLoader<Vec3>(ref settings, ref filters);

			// Init IntervalBased Transformer settings
			intervalSettings = new IntervalBasedTransformerSettings();
			intervalSettings.sampleList = loader.LoadData();
			intervalSettings.interval = 4;
			intervalTransformer = new IntervalBasedTransformer();

			// Init CountBased Transformer settings
			countSettings = new IntervalBasedTransformerSettings();
			countSettings.sampleList = loader.LoadData();
			countSettings.count = 10;
			countTransformer = new CountBasedTransformer();

			// Init Image Transformer

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
    }
}
