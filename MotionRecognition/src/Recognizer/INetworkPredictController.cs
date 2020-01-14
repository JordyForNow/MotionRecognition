namespace MotionRecognition
{
	public interface INetworkPredictController<T>
	{

		static void PreparePredictor(ref NetworkContainer container, ref T settings) { }
		
		static bool Predict(ref NetworkContainer container, ref T settings) { return false; }

	}
}
