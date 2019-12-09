namespace MotionRecognition
{
	public struct Measurement
	{
		public Vec3 pos { get; set; }
		public Quaternion angle { get; set; }

		public void parse(string pos, string angle)
		{
			System.Console.WriteLine(pos + ":" + angle);	
			this.pos = new Vec3();
			this.angle = new Quaternion();
			if (!this.pos.parse(pos)) throw new System.Exception("je weet t");
			this.angle.parse(angle);
		}

		public override string ToString()
		{
			return "{" + this.pos + "|" + this.angle + "}";
		}
	}
}
