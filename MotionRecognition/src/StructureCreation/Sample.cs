using System;
namespace MotionRecognition
{
	public struct Sample<T>
	{
		public Single timestamp;
		public T[] vectorArr;
	}
}
