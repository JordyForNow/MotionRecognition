namespace MotionRecognition
{
	public struct JointMeasurement
	{
		public Vec3 pos;
		public Quaternion angle;

		// Parse the sub values.
		public bool parse(string pos, string angle)
		{
			this.pos = new Vec3();
			this.angle = new Quaternion();
			if (!this.pos.parse(pos)) return false;
			if (!this.angle.parse(angle)) return false;
			return true;
		}

		public override string ToString()
		{
			return "{" + this.pos + "|" + this.angle + "}";
		}
	}
}
