using System;
namespace MotionRecognition
{
	/*
	* Sample is a generic class which holds a timestamp and an array of generic values,
	* it is used to contain timestamp-connected values.
	*/
	public struct Sample<T>
	{
		public Single timestamp;
		public T[] vectorArr;
	}
}
