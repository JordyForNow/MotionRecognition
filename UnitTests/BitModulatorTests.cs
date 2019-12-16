using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class BitModulatorTests
	{
		[Test]
		public void BitModulator_SetIndex()
		{
			int val = 0;
			BitModulator.SetIndex(ref val, 0, true);
			Assert.IsTrue(val == 1);
		}

		[Test]
		public void BitModulator_ExceedsBuffer()
		{
			int val = 0;
			Assert.IsFalse(BitModulator.SetIndex(ref val, 32, true));
		}

		[Test]
		public void BitModulator_ToString_Bits()
		{
			int val = 0;
			BitModulator.SetIndex(ref val, 0, true);
			Assert.IsTrue(BitModulator.ToString(ref val) == "11111111111111111111111111111110");
		}
	}
}
