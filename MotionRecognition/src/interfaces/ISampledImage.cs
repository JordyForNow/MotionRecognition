using System;
using System.Collections.Generic;

namespace MotionRecognition
{
	public interface ISampledImage<T>
	{
		void CreateImageFromTable(ref List<Sample<T>> _table);
		bool Equals(object obj);
	}
}
