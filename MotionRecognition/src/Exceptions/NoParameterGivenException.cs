using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	class NoParameterGivenException : Exception
	{

		public NoParameterGivenException(String Message) : base(Message) { }

	}
}
