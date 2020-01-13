using System;

namespace MotionRecognition
{
	public class IncorrectActionOrderException : Exception
	{

		public IncorrectActionOrderException(String Message) : base(Message) { }

	}
}
