using System;
using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class Motion3DImageTests
	{
		Table<JointMeasurement> table1;
		Table<JointMeasurement> table2;

		[SetUp]
		public void Setup()
		{
			table1 = new Table<JointMeasurement>();
			table2 = new Table<JointMeasurement>();

			populateTable(ref table1);
			populateTable(ref table2);
		}

		private void populateTable(ref Table<JointMeasurement> table)
		{
			List<Sample<JointMeasurement>> samples = new List<Sample<JointMeasurement>>();
			Random random = new Random();

			for (int x = 0; x < 50; x++)
			{
				List<JointMeasurement> measurements = new List<JointMeasurement>();
				for (int i = 0; i < 21; i++)
				{
					measurements.Add(new JointMeasurement() { pos = new Vec3() { x = random.Next(500), y = random.Next(500), z = random.Next(500) } });
				}
				samples.Add(new Sample<JointMeasurement>() { sampleData = measurements });
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
