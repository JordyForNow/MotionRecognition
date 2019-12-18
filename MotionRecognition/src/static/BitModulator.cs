using System;
namespace MotionRecognition
{
	public static class BitModulator
	{
		// Set the index of the bit within the uint to a given value 1 or 0.
		public static bool SetIndex(ref int value, int i, bool b)
		{
			if (i >= 32) return false;
			if (b)
				value |= (1 << i);
			else
				value &= ~(1 << i);
			return true;
		}
		public static int SetIndex(int i, bool b)
		{
			int value = 0;
			if (i >= 32) return value;
			if (b)
				value |= (1 << i);
			else
				value &= ~(1 << i);
			return value;
		}
		// The uint ToString() in bit format for example "11111111111111111111111111111111".
		public static string ToString(ref int value)
		{
			int b = ~value;
			return Convert.ToString(b, toBase: 2);
		}
	}
}
