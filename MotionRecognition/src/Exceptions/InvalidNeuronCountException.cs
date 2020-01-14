using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	public class InvalidNeuronCountException : Exception
	{

		public InvalidNeuronCountException(String Message) : base(Message) { }

	}
}
