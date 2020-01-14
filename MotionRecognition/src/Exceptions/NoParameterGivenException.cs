using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	public class NoParameterGivenException : Exception
	{

		public NoParameterGivenException(String Message) : base(Message) { }

	}
}
