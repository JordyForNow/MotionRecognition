using System;
using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class Motion3DImageTests
	{
		Table<Measurement> table1;
		Table<Measurement> table2;

		[SetUp]
		public void Setup()
		{
			table1 = new Table<Measurement>();
			table2 = new Table<Measurement>();

			populateTable(ref table1);
			populateTable(ref table2);
		}

		private void populateTable(ref Table<Measurement> table)
		{
			List<Sample<Measurement>> samples = new List<Sample<Measurement>>();
			Random random = new Random();

			for (int x = 0; x < 50; x++)
			{
				List<Measurement> measurements = new List<Measurement>();
				for (int i = 0; i < 21; i++)
				{
					measurements.Add(new Measurement() { pos = new Vec3() { x = random.Next(500), y = random.Next(500), z = random.Next(500) } });
				}
				samples.Add(new Sample<Measurement>() { sampleData = measurements });
			}

			table.samples = samples;
		}

		[Test]
		public void Motion3DImage_Equals_DefaultBothSet()
		{
			Motion3DImage img1 = new Motion3DImage();
			Motion3DImage img2 = new Motion3DImage();

			Assert.IsTrue(img1.Equals(img2));
		}

		[Test]
		public void Motion3DImage_Equals_DefaultOtherNull()
		{
			Motion3DImage img1 = new Motion3DImage();
			Motion3DImage img2 = null;

			Assert.IsFalse(img1.Equals(img2));
		}

		[Test]
		public void Motion3DImage_Equals_TableNotEqual()
		{
			Motion3DImage img1 = new Motion3DImage(ref table1);
			Motion3DImage img2 = new Motion3DImage(ref table2);

			Assert.IsFalse(img1.Equals(img2));
		}

		[Test]
		public void Motion3DImage_Equals_TableEqual()
		{
			Motion3DImage img1 = new Motion3DImage(ref table1);
			Motion3DImage img2 = new Motion3DImage(ref table1);

			Assert.IsTrue(img1.Equals(img2));
		}
	}
}
