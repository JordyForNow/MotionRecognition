using System.Collections.Generic;

namespace MotionRecognition
{
	public interface IDataLoader<T>
	{
		List<Sample<T>> GetData();
	}
}
