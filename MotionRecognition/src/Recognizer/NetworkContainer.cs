using System;
using Encog.Neural.Networks;

namespace MotionRecognition
{
	public class NetworkContainer
	{
		public BasicNetwork network;
		public bool verbose;

		public NetworkContainer()
		{
			verbose = false;
		}
	}
}
