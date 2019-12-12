using System.Linq;

namespace MotionRecognition
{
    public struct Quaternion : CSVCell
    {
        public float x, y, z, w;

        public bool parse(string input = "(0.0| 0.0| 0.0| 0.0)")
        {
            // check whether it is a CSVCell.
            if (!(input[0] == '(' && input[input.Length - 1] == ')')) return false;
            // split the values inside.
            var points = input.Substring(1, input.Length - 2).Split("| ");
            // check if it has 4 values.
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
