using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class BitModulatorTests
	{
		[Test]
		public void BitModulator_Equals_BothSet()
		{
			BitModulator bm1 = new BitModulator();
			BitModulator bm2 = new BitModulator();

			Assert.AreEqual(bm1, bm2);
		}

		[Test]
		public void BitModulator_Equals_OtherNull()
		{
			BitModulator bm1 = new BitModulator();
			BitModulator bm2 = null;

			Assert.AreNotEqual(bm1, bm2);
		}

		[Test]
		public void BitModulator_Equals_FirstNull()
		{
			BitModulator bm1 = null;
			BitModulator bm2 = new BitModulator();

			Assert.AreNotEqual(bm1, bm2);
		}

		[Test]
		public void BitModulator_Equals_BothNull()
		{
			BitModulator bm1 = null;
			BitModulator bm2 = null;

			Assert.AreEqual(bm1, bm2);
		}
	}
}
