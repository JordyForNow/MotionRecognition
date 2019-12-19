using System.Collections.Generic;

namespace MotionRecognition
{
	public interface IDataLoader<T>
	{
		bool LoadData();
	}
}
