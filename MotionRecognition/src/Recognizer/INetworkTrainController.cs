namespace MotionRecognition
{
	public interface INetworkTrainController<T>
	{
		static void PrepareData(ref T settings, ref NetworkContainer container) { }

		static void PrepareNetwork(ref T settings, ref NetworkContainer container) { }

		static void Train(ref T settings, ref NetworkContainer container) { }

	}
}
