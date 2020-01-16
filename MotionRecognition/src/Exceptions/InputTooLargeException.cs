using System;

namespace MotionRecognition
{
	public class InputTooLargeException : Exception
	{
		public InputTooLargeException(String msg) : base(msg) { }
	}
}
