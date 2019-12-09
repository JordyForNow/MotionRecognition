using System;
using System.Linq;
namespace MotionRecognition
{
    public struct Vec3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

		public bool parse(string input = "{0.0|0.0|0.0}")
		{
            if(!(input[0] == '{' && input[input.Length - 1] == '}')) return false;
            if(input.Count(o => o == '|') != 2) return false;

			Console.WriteLine(input.Substring(1, input.Length - 2).Split('|'));
            return true;
		}
    }
}
