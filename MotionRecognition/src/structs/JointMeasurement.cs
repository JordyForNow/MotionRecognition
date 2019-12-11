namespace MotionRecognition
{
	public struct JointMeasurement
	{
		#region Properties
		public Vec3 pos;
		public Quaternion angle;
		#endregion
		#region PublicFunc
		// parse the sub values.
		public bool parse(string pos, string angle)
		{
			this.pos = new Vec3();
			this.angle = new Quaternion();
			if(!this.pos.parse(pos)) return false;
			if(!this.angle.parse(angle)) return false;
			return true;
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
