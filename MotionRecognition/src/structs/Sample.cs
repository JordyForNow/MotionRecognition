using System;

namespace MotionRecognition
{
	public struct Sample<T>
	{
		public Single timestamp  { get; set; }
		public T[]    sampleData { get; set; }
	}
}
