namespace MotionRecognition
{
	public interface INetworkTrainController<T>
	{
		static void PrepareData(ref NetworkContainer container, ref T settings) { }

		static void PrepareNetwork(ref NetworkContainer container, ref T settings) { }

		static void Train(ref NetworkContainer container, ref T settings) { }

	}
}
