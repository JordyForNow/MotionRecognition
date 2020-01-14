using System.Globalization;

namespace MotionRecognition
{
    /* 
	* Vector3 contains an x, y and z coordinate which are used to display a 3D position.
	* It inherits IParseable because the object can be parsed from a CSV value,
	* ITransformerValue is also inherited so the factories can read the coordinates.
	*/
    public class Vector3 : IParseable<Vector3>, ITransformerValue
    {
        public float x, y, z;

        public static Vector3 Parse(string input)
        {
            Vector3 vec3 = new Vector3();
            // Check if the input is a CSVCell.
            if (!(input[0] == '(' && input[input.Length - 1] == ')'))
                throw new FormatIncorrectException("The given input has an incorrect format.");

            // Split the string into the subvalues.
            var points = input.Substring(1, input.Length - 2).Split("| ");

            // Check if points has 3 values.
            if (points.Length > 3)
                throw new InputTooLargeException("Too many arguments at parsing Vector3.");

            vec3.x = float.Parse(points[0], CultureInfo.InvariantCulture);
            vec3.y = float.Parse(points[1], CultureInfo.InvariantCulture);
            vec3.z = float.Parse(points[2], CultureInfo.InvariantCulture);
            return vec3;
        }

        public void parse(string input)
        {
            var vec3 = Vector3.Parse(input);
            this.x = vec3.x;
            this.y = vec3.y;
            this.z = vec3.z;
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
