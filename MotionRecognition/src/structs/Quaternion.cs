using System.Linq;

namespace MotionRecognition
{
	public struct Quaternion
	{
		public float x { get; set; }
		public float y { get; set; }
		public float z { get; set; }
		public float w { get; set; }

		public bool parse(string input = "(0.0|0.0|0.0|0.0)")
		{
			if (!(input[0] == '(' && input[input.Length - 1] == ')')) return false;

			var points = input.Substring(1, input.Length - 2).Split("| ");

			if (points.Length > 4) return false;
			x = float.Parse(points[0]);
			y = float.Parse(points[1]);
			z = float.Parse(points[2]);
			w = float.Parse(points[3]);
			return true;
		}

		public override string ToString()
		{
			return x + "," + y + "," + z + "," + w;
		}
	}
}
