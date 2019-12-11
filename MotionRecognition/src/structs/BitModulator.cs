using System;
namespace MotionRecognition
{
	[Serializable]
	public class BitModulator
	{
		uint arr = 0;

		public BitModulator() {}
		public BitModulator(string val)
		{
			arr = uint.Parse(val);
		}

		public bool SetIndex(int i, bool b)
		{
			if (i >= 32) return false;
			if (b)
				arr |= ((uint)1 << i);
			else
				arr &= ~((uint)1 << i);
			return true;
		}

		public uint GetVal() => arr;
		public void SetVal(uint arr)
		{
			this.arr = arr;
		}

		public override string ToString()
		{
			return ToString(false);
		}
		public string ToString(bool bit)
		{
			if (bit)
			{
				uint b = ~arr;
				return Convert.ToString(b, toBase: 2);
			}
			return arr.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (BitModulator)obj;

			if (GetVal() != other.GetVal()) return false;

			return true;
		}
	}
}
