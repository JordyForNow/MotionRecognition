using System;

namespace MotionRecognition
{
	public class WrongFileTypeException : Exception
	{

		public WrongFileTypeException(String Message) : base(Message) { }

	}
}
