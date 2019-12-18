using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	class WrongFileTypeException : Exception
	{

		public WrongFileTypeException(String Message) : base(Message) { }

	}
}
