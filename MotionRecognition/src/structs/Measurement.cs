namespace MotionRecognition
{
	public struct Measurement
	{
		#region Properties
		public Vec3 pos;
		public Quaternion angle;
		#endregion
		#region PublicFunc
		public void parse(string pos, string angle)
		{
			this.pos = new Vec3();
			this.angle = new Quaternion();
			this.pos.parse(pos);
			this.angle.parse(angle);
		}
		#endregion
		#region Operations
		public override string ToString()
		{
			return "{" + this.pos + "|" + this.angle + "}";
		}
		#endregion
	}
}
