using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{
	public class FileAlreadyExistsException : Exception
	{

		public FileAlreadyExistsException(String Message) : base(Message) { }

	}
}
