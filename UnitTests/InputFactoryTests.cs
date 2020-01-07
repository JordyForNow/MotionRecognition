using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
    public class InputFactoryTests
    {
        string dataPath = @"../../../Data/";
		IMovementFactory<IntervalBasedFactorySettings> intervalFactory;
		IMovementFactory<IntervalBasedFactorySettings> countFactory;
		IntervalBasedFactorySettings intervalSettings;
		IntervalBasedFactorySettings countSettings;

        [SetUp]
        public void Setup()
        {
			// Setup loader 
            CSVLoaderSettings settings = new CSVLoaderSettings();
            settings.filepath = dataPath + "data.csv";
            settings.TrimUp = 1;
            settings.TrimRight = 0;

			List<CSVColumnFilter> filters = new List<CSVColumnFilter>(1);
			CSVColumnFilter quaternions = new CSVEvenColumnsFilter();
			filters.Add(quaternions);

            CSVLoader<Vec3> loader = new CSVLoader<Vec3>(ref settings, ref filters);

			// Init IntervalBased factory settings
			intervalSettings = new IntervalBasedFactorySettings();
			intervalSettings.sampleList = loader.LoadData();
			intervalSettings.interval = 4;
			intervalFactory = new IntervalBasedFactory();

			// Init CountBased factory settings
			countSettings = new IntervalBasedFactorySettings();
			countSettings.sampleList = loader.LoadData();
			countSettings.count = 10;
			countFactory = new CountBasedFactory();

			// Init Image factory

        }

        [Test]
        public void IntervalBasedFactoryReturnsValues()
        {
            Assert.IsNotEmpty(intervalFactory.GetNeuralInput(intervalSettings));
        }

		[Test]
        public void CountBasedFactoryReturnsValues()
        {
            Assert.IsNotEmpty(countFactory.GetNeuralInput(countSettings));
        }

		[Test]
		public void FactoriesReturnDifferentResults()
		{
			Assert.AreNotEqual(intervalFactory.GetNeuralInput(intervalSettings), countFactory.GetNeuralInput(countSettings));
		}
    }
}
