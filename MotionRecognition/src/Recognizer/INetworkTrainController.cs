namespace MotionRecognition
{
	public interface INetworkTrainController<T>
	{
		void PrepareData(ref NetworkContainer container, ref T settings);

		void PrepareNetwork(ref NetworkContainer container, ref T settings);

		void Train(ref NetworkContainer container, ref T settings);

	}
}
