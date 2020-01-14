namespace MotionRecognition
{
	public interface INetworkPredictController<T>
	{

		void PreparePredictor(ref NetworkContainer container, ref T settings);

		bool Predict(ref NetworkContainer container, ref T settings);

	}
}
