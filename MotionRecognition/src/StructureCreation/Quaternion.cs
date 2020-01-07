namespace MotionRecognition
{
	/* 
	* Quaternion is a vector which displays a direction, it inherits IParseable because it can be parsed
	* from a CSV value.
	*/
	public class Quaternion : IParseable
	{
		public float x, y, z, w;

		public bool parse(string input)
		{
			// Check if the input is a CSVCell.
			if (!(input[0] == '(' && input[input.Length - 1] == ')')) return false;
			// Split the string into the subvalues.
			var points = input.Substring(1, input.Length - 2).Split("| ");
			// Check if points has 4 values.
			if (points.Length != 4) return false;

			this.x = float.Parse(points[0]);
			this.y = float.Parse(points[1]);
			this.z = float.Parse(points[2]);
			this.w = float.Parse(points[3]);
			return true;
		}

		public override string ToString()
		{
			return this.x + "," + this.y + "," + this.z + ", " + this.w;
		}
	}
}
