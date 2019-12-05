namespace MotionRecognition
{
	public struct Measurement : ISerializable<string[]>
	{
		public Vec3 pos { get; set; }
		Quaternion angle { get; set; }

		public void parse(string[] str)
		{

		}
	}
}
