namespace MotionRecognition
{
	public interface IDataLoader<T>
	{
		Table<T> GetData();
	}
}
