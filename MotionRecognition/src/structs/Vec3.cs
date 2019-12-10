using System;
using System.Linq;

namespace MotionRecognition
{
	public struct Vec3 : CSVCell
	{
		#region Properties
		public float x;
		public float y;
		public float z;
		#endregion
		#region PublicFunc
		public bool parse(string input = "(0.0|0.0|0.0)")
		{
			System.Console.WriteLine(input);
			if (!(input[0] == '(' && input[input.Length - 1] == ')')) return false;

			var points = input.Substring(1, input.Length - 2).Split("| ");

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
