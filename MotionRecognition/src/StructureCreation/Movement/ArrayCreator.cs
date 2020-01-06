using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	public class ArrayCreator
	{
		public double[] CreateArray(Sample<Vec3>[] _datalist, int _dataCount)
		{
			// Count total rows
			float totalSampleCount = _datalist.Length;

			// Calculate step size
			int stepSize = (int)Math.Ceiling((decimal)(totalSampleCount / _dataCount));

			// Count total input value size
			int valuesCount = (_datalist[0].vectorArr.Length * 3) * _dataCount;

			// Create array
			double[] values = new double[valuesCount];

			// Fill array
			int valueIndex = 0;
			for(int step = 0; step < totalSampleCount; step+=stepSize)
			{
				foreach(Vec3 v in _datalist[step].vectorArr)
				{
					values[valueIndex] = v.x;
					valueIndex++;
					values[valueIndex] = v.y;
					valueIndex++;
					values[valueIndex] = v.z;
					valueIndex++;
				}
			}

			return values;
		}
	}
}
