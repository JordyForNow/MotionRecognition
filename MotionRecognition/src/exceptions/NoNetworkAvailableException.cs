using System;

namespace MotionRecognition
{
	public class NoNetworkAvailableException : Exception
	{

		public NoNetworkAvailableException(String Message) : base(Message) { }

	}
}
