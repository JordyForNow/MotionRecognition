namespace MotionRecognition
{
	public struct Measurement : ISerializable<string[]>
	{
		public Vec3 pos { get; set; }
		public Quaternion angle { get; set; }

		public void parse(string pos, string angle)
		{
			pos 
		}
	}
}
