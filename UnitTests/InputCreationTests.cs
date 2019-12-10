using MotionRecognition;
using NUnit.Framework;
using System.IO;

namespace UnitTests
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		#region ParserTests

		[Test]
		public void MeasurementParserWithCorrectValueReturnsTrue()
		{
			Measurement m = new Measurement();
			bool result = m.parse("(0.3| 0.9| 1.4)", "(9.0| 10.1| 4.3| 2.3)");

			Assert.IsTrue(result);
		}

		[Test]
		public void MeasurementParserWithFaultyValueReturnsFalse()
		{
			Measurement m = new Measurement();
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
    }
}
