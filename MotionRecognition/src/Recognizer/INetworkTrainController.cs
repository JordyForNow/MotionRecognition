namespace MotionRecognition
{
	public interface INetworkTrainController<T>
	{
		static void prepareData(ref T settings, ref NetworkContainer container) { }

		static void prepareNetwork(ref T settings, ref NetworkContainer container) { }

		static void train(ref T settings, ref NetworkContainer container) { }

	}
}
