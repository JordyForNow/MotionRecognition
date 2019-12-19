using System;
namespace MotionRecognition
{
	public class Sample<T>
	{
		Single timestamp;
		T[] vectorArr;
		public Sample(Single _timestamp, T[] _vecArr)
		{
			this.timestamp = _timestamp;
			this.vectorArr = _vecArr;
		}
		public Single GetTimeStamp() => timestamp;
		public T[] GetVectorArr() => vectorArr;
		public int GetVectorCount() => vectorArr.Length;
	}
}
