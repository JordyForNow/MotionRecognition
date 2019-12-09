namespace MotionRecognition
{
	public interface ISerializable<T>
	{
		public void parse(string pos, string angle);
	}
}
