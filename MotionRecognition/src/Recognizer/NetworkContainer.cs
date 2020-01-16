using Encog.Neural.Networks;

namespace MotionRecognition
{
	public class NetworkContainer
	{
		// Network
		public BasicNetwork network;
		// Whether Console.WriteLine needs to be executed.
		public bool verbose;

		public NetworkContainer()
		{
			verbose = false;
		}
	}
}
