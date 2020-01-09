using System.Collections.Generic;
using System.Linq;
using MotionRecognition;
using NUnit.Framework;
using System;

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
			// Setup loader.
			settings = new CSVLoaderSettings();
			settings.filePath = dataPath + "data.csv";
			settings.CSVHasHeader = true;
			settings.trimUp = 0;
			settings.trimDown = 0;

			// Setup filters for loader.
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
			settings.CSVHasHeader = false;

			Assert.Throws<System.FormatException>(() => {CSVLoader<Vector3>.LoadData(ref settings);});
		}

		[Test]
		public void Trim()
		{
			var data = CSVLoader<Vector3>.LoadData(ref settings);
			settings.trimUp = 1;

			var trimmedData = CSVLoader<Vector3>.LoadData(ref settings);

			Assert.IsTrue(data.Skip(1).Count() == trimmedData.Count());
		} 

		[Test]
		public void Vector3Parse()
		{
			Vector3 vec3 = Vector3.Parse("(1.0| 1.0| 1.0)");
			Assert.IsTrue(vec3.x == 1.0 && vec3.y == 1.0 && vec3.z == 1.0);
		}

		[Test]
		public void LoaderThrowsExceptionWhenNoFileGiven()
		{
			settings.filePath = "";

			Assert.Throws<System.IO.FileNotFoundException>(() => {CSVLoader<Vector3>.LoadData(ref settings);});
			
		}
	}
}
