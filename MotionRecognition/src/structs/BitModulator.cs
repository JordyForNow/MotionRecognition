﻿using System;
namespace MotionRecognition
{
	[Serializable]
	public class BitModulator
	{
		// this is a base of 32 bits.
		uint arr = 0;

		// create a new bitmodulator or create one from a string.
		public BitModulator() { }
		public BitModulator(string val)
		{
			arr = uint.Parse(val);
		}
		// set the index of the bit within the uint to a given value 1 or 0.
		public bool SetIndex(int i, bool b)
		{
			if (i >= 32) return false;
			if (b)
				arr |= ((uint)1 << i);
			else
				arr &= ~((uint)1 << i);
			return true;
		}

		// get and setters for the base value.
		public uint GetVal() => arr;
		public void SetVal(uint arr)
		{
			this.arr = arr;
		}

		// the uint ToString().
		public override string ToString()
		{
			return ToString(false);
		}
		// the uint ToString() in bit format for example "11111111111111111111111111111111".
		public string ToString(bool bit)
		{
			if (bit)
			{
				uint b = ~arr;
				return Convert.ToString(b, toBase: 2);
			}
			return arr.ToString();
		}

		// checks whether this object holds the same value as the given object.
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
