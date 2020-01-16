namespace MotionRecognition
{
	public interface IParseable<T>
	{
		static T Parse(string input) => default(T);

		public void parse(string input);
	}
}
