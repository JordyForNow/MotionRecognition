namespace MotionRecognition
{
	public struct Measurement
	{
		public Vec3 pos;
		public Quaternion angle;

		public void parse(string pos, string angle)
		{
			this.pos = new Vec3();
			this.angle = new Quaternion();
			this.pos.parse(pos);
			this.angle.parse(angle);
		}

		public override string ToString()
		{
			return "{" + this.pos + "|" + this.angle + "}";
		}
	}
}
