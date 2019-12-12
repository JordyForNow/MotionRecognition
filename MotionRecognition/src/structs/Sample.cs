using System;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class Sample<T>
	{
		public Single Timestamp;
		public List<T> sampleData;

		public Sample() {}
	}
}
