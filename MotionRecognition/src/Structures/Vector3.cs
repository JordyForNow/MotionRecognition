namespace MotionRecognition
{
	/* 
	* Vec3 contains an x, y and z coordinate which are used to display a 3d position.
	* It inherits IParseable because the object is can be parsed from a CSV file value,
	* ITransformerValue is also inherited so the factories can read the coordinates.
	*/
    public class Vector3 : IParseable, ITransformerValue
    {
        public float x, y, z;

        public bool parse(string input)
        {
            // Check if the input is a CSVCell.
            if (!(input[0] == '(' && input[input.Length - 1] == ')')) return false;
            // Split the string into the subvalues.
            var points = input.Substring(1, input.Length - 2).Split("| ");
            // Check if points has 3 values.
            if (points.Length > 3) return false;

            this.x = float.Parse(points[0]);
            this.y = float.Parse(points[1]);
            this.z = float.Parse(points[2]);
            return true;
        }

        public double[] GetTransformerValue()
        {
            var returnArray = new double[3];

            returnArray[0] = this.x;
            returnArray[1] = this.y;
            returnArray[2] = this.z;

            return returnArray;
        }

        public override string ToString()
        {
            return this.x + "," + this.y + "," + this.z;
        }
    }
}
