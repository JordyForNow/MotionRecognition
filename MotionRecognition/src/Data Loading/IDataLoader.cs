namespace MotionRecognition
{
	public interface IDataLoader<T, I>
	{
		static T[] LoadData(ref I settings) => null;
	}
}
