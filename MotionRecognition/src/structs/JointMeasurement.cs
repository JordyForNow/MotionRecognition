namespace MotionRecognition
{
	public struct JointMeasurement
	{
		public Vec3 Pos;
		public Quaternion Angle;

		#region PublicFunctions
		// parse the sub values.
		public bool parse(string Pos, string Angle)
		{
			this.Pos = new Vec3();
			this.Angle = new Quaternion();
			if(!this.Pos.parse(Pos)) return false;
			if(!this.Angle.parse(Angle)) return false;
			return true;
		}
		#endregion
		
		#region Operations
		public override string ToString()
		{
			return "{" + this.Pos + "|" + this.Angle + "}";
		}
		#endregion
	}
}
