using System;
namespace MotionRecognition
{
	public interface CSVCell
	{
		public bool parse(string input);
		public string ToString();
	}
}
