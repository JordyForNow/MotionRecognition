using System;
namespace MotionRecognition
{
	public class BitModulator
	{
		uint arr = 0;

		public bool SetIndex(byte i, bool b)
		{
			if (i >= 32) return false;
			if(b)
				arr |= ((uint)1 << i);
			else
				arr &= ~((uint)1 << i);
			return true;
		}

		public override string ToString()
		{
			return ToString(false);
		}
		public string ToString(bool bit)
		{
			if(bit)
			{
				uint b = ~arr;
				return Convert.ToString(b, toBase: 2);
			}
			return arr.ToString();
		}
	}
}
