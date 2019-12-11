using MotionRecognition;
using NUnit.Framework;

namespace UnitTests
{
	class Motion3DImageTests
	{
		[Test]
		public void Motion3DImage_Equals_DefaultBothSet()
		{
			Motion3DImage bm1 = new Motion3DImage();
			Motion3DImage bm2 = new Motion3DImage();

			Assert.AreEqual(bm1, bm2);
		}

		[Test]
		public void Motion3DImage_Equals_DefaultOtherNull()
		{
			Motion3DImage bm1 = new Motion3DImage();
			Motion3DImage bm2 = null;

			Assert.AreNotEqual(bm1, bm2);
		}

		[Test]
		public void Motion3DImage_Equals_DefaultFirstNull()
		{
			Motion3DImage bm1 = null;
			Motion3DImage bm2 = new Motion3DImage();

			Assert.AreNotEqual(bm1, bm2);
		}

		[Test]
		public void Motion3DImage_Equals_DefaultBothNull()
		{
			Motion3DImage bm1 = null;
			Motion3DImage bm2 = null;

			Assert.AreEqual(bm1, bm2);
		}
	}
}
