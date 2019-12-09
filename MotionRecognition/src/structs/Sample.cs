using System;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class Sample<T>
	{
		public Single	timestamp  { get; set; }
		public List<T>	sampleData { get; set; }
	}
}
