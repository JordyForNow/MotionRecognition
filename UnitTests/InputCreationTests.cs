using System.Collections.Generic;
using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	public class Tests
	{
		List<Sample<JointMeasurement>> table;
		string dataPath;

		[SetUp]
		public void Setup()
		{
			// Assuming your project is ran from /bin/debug/netcoreapp
			string dataPath = @"../../../testdata/";

			CSVLoader loader = new CSVLoader(dataPath + "data.csv");
			table = loader.GetData();
		}

		#region ParserTests

		[Test]
		public void MeasurementParserWithCorrectValueReturnsTrue()
		{
			JointMeasurement m = new JointMeasurement();
			bool result = m.parse("(0.3| 0.9| 1.4)", "(9.0| 10.1| 4.3| 2.3)");

			Assert.IsTrue(result);
		}

		[Test]
		public void MeasurementParserWithFaultyValueReturnsFalse()
		{
			JointMeasurement m = new JointMeasurement();
			bool result = m.parse("this is", "faulty");

			Assert.IsFalse(result);
		}

		[Test]
		public void QuaternionParserWithCorrectValueReturnsTrue()
		{
			CSVCell q = new Quaternion();
			bool result = q.parse("(1.2| 4.3| 2.5| 3.0)");

			Assert.IsTrue(result);
		}

		[Test]
		public void QuaternionParserWithFaultyValueReturnsFalse()
		{
			CSVCell q = new Quaternion();
			bool result = q.parse("test");

			Assert.IsFalse(result);
		}

		[Test]
		public void Vec3ParserWithCorrectValueReturnsTrue()
		{
			CSVCell v = new Vec3();
			bool result = v.parse("(1.2| 4.3| 2.5)");

			Assert.IsTrue(result);
		}

		[Test]
		public void Vec3ParserWithFaultyValueReturnsFalse()
		{
			CSVCell v = new Vec3();
			bool result = v.parse("test");

			Assert.IsFalse(result);
		}
		#endregion

		#region SerializingTests

		[Test]
		public void SerializeImage()
		{
			Motion3DImage image = new Motion3DImage(ref table), image2;
			ImageSerializer.Serialize(image, dataPath + "testSerializedData");
			image2 = ImageSerializer.DeSerialize(dataPath+ "testSerializedData");
			Assert.IsTrue(image.Equals(image2));
		}

		#endregion
	}
}
