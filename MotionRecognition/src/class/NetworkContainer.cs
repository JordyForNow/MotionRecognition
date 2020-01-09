using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{

	public class dsNetworkContainer
	{
		public bool allowFileOverride { get; set; } = false;

		private double _maxTrainingError = 0.01;
		public double maxTrainingError
		{
			get
			{
				return _maxTrainingError;
			}
			set
			{
				if (double.IsNaN(value))
					throw new ArgumentNullException("No value given for maxTrainingError.");

				if (value <= 0)
					throw new ArgumentOutOfRangeException("maxTrainingError can't be smaller as 1.");

				_maxTrainingError = value;
			}
		}

		private int _networkInputSize = 10;
		public int networkInputSize
		{
			get
			{
				return _networkInputSize;
			}
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("networkInputSize can't be smaller as 1.");

				_networkInputSize = value;
			}
		}

		public bool verbose { get; set; } = true;

	}
}
