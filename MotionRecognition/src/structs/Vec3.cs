using System;
using System.Linq;

namespace MotionRecognition
{
	public struct Vec3 : CSVCell
	{
		public float x;
		public float y;
		public float z;

		#region PublicFunc
		public bool parse(string Input = "(0.0| 0.0| 0.0)")
		{
			// check if the input is a CSVCell
			if (!(Input[0] == '(' && Input[Input.Length - 1] == ')')) return false;
			// split the string into the subvalues.
			var points = Input.Substring(1, Input.Length - 2).Split("| ");
			// check if points has 3 values.
			if (points.Length > 3) return false;

			this.x = float.Parse(points[0]);
			this.y = float.Parse(points[1]);
			this.z = float.Parse(points[2]);
			return true;
		}
		#endregion
		
		#region Operations
		public override string ToString()
		{
			return this.x + "," + this.y + "," + this.z;
		}
		#endregion
	}
}
