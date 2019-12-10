using System;

namespace MotionRecognition
{
	[Serializable]
	public class Motion3DImage : IImage
	{
		public BitModulator[,] top = new BitModulator[500, 500];
		public BitModulator[,] side = new BitModulator[500,500];
	}
}
