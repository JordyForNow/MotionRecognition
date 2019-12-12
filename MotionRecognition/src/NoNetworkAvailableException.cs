using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	public class NoNetworkAvailableException : Exception
	{

		public NoNetworkAvailableException(String Message) : base(Message) { }

	}
}
