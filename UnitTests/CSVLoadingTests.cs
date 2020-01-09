using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	public class CSVLoadingTests
	{
		CSVLoaderSettings settings;
		List<ICSVFilter> filters;
		string dataPath = @"../../../Data/";
		[SetUp]
		public void Setup()
		{
			// Setup loader 
			settings = new CSVLoaderSettings();
			settings.filepath = dataPath + "data.csv";
			settings.CSVHasHeader = true;
			settings.trimLeft = 0;
			settings.trimRight = 0;

			filters = new List<ICSVFilter>(1);
			ICSVFilter quaternions = new CSVEvenColumnFilter();
			filters.Add(quaternions);
			settings.filters = filters;
		}

		[Test]
		public void CSVLoaderLoadsData()
		{
			var data = CSVLoader<Vector3>.LoadData(ref settings);
			Assert.IsTrue(data.Length > 0);
		}

		[Test]
		public void CSVLoaderReturnsNoNulls()
		{
			var data = CSVLoader<Vector3>.LoadData(ref settings);
			bool b = false;
			foreach (var sample in data)
				foreach (var col in sample.values)
					if (col == null)
						b = true;
			Assert.IsFalse(b);
		}

		[Test]
		public void CSVLoaderFailWhenFileHasHeader()
		{
			bool bFailed = false;
			settings.CSVHasHeader = false;
			try
			{
				CSVLoader<Vector3>.LoadData(ref settings);
			}
			catch
			{
				bFailed = true;
			}
			Assert.IsTrue(bFailed);
		}

		[Test]
		public void TrimTest()
		{

		}
	}
}
