namespace MotionRecognition
{
	public interface IDataLoader<T, I>
	{
		// Loads Data given a context.
		static T[] LoadData(ref I settings) => null;
	}
}
