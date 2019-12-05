namespace MotionRecognition
{
	public interface ISerializable<T>
	{
		public void parse(T str);
	}
}
