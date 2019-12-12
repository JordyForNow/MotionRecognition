using System;
using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class Motion3DImageTests
	{
		List<Sample<JointMeasurement>> Table1;
		List<Sample<JointMeasurement>> Table2;

		[SetUp]
		public void Setup()
		{
			Table1 = new List<Sample<JointMeasurement>>();
			Table2 = new List<Sample<JointMeasurement>>();

			populateTable(ref Table1);
			populateTable(ref Table2);
		}

		private void populateTable(ref List<Sample<JointMeasurement>> SampleList)
		{
			List<Sample<JointMeasurement>> Samples = new List<Sample<JointMeasurement>>();
			Random random = new Random();

			for (int x = 0; x < 50; x++)
			{
				List<JointMeasurement> Measurements = new List<JointMeasurement>();
				for (int i = 0; i < 21; i++)
				{
					Measurements.Add(new JointMeasurement() { pos = new Vec3() { x = random.Next(500), y = random.Next(500), z = random.Next(500) } });
				}
				Samples.Add(new Sample<JointMeasurement>() { sampleData = Measurements });
			}

			SampleList = Samples;
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
			Motion3DImage img1 = new Motion3DImage(ref Table1);
			Motion3DImage img2 = new Motion3DImage(ref Table2);

			Assert.IsFalse(img1.Equals(img2));
		}

		[Test]
		public void Motion3DImage_Equals_TableEqual()
		{
			Motion3DImage img1 = new Motion3DImage(ref Table1);
			Motion3DImage img2 = new Motion3DImage(ref Table1);

			Assert.IsTrue(img1.Equals(img2));
		}
	}
}
