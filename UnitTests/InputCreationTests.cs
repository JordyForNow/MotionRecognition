using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	public class Tests
	{
		Table<Measurement> table;

		[SetUp]
		public void Setup()	
		{
			CSVLoader loader = new CSVLoader("./data.csv", 21);
			table = loader.GetData();
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

		[Test]
		public void BitModulator()
		{
			BitModulator b = new BitModulator();
			b.SetIndex(0, true);
			Assert.IsTrue(b.GetVal() == 1);
		}

		[Test]
		public void BitModulatorExceedsBuffer()
		{
			BitModulator b = new BitModulator();
			Assert.IsFalse(b.SetIndex(32, true));
		}

		[Test]
		public void BitModulatorToStringBits()
		{
			BitModulator b = new BitModulator();
			b.SetIndex(0, true);
			Assert.IsTrue(b.ToString(true) == "11111111111111111111111111111110");
		}
		#endregion
<<<<<<< Updated upstream

		#region SerializingTests

		[Test]
		public void SerializeImage()
		{			
			Motion3DImage image = new Motion3DImage(ref table);
			image.Serialize("./testdata");

			Assert.AreEqual(image.DeSerialize("./testdata"), image);
		}
		
		#endregion
=======
>>>>>>> Stashed changes
	}
}
