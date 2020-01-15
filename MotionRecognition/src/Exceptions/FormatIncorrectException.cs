using System;

namespace MotionRecognition
{
    public class FormatIncorrectException : Exception
    {
        public FormatIncorrectException(String msg) : base(msg) { }
    }
}
