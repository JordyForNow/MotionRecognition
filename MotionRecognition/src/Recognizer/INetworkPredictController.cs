namespace MotionRecognition
{
	public interface INetworkPredictController<T>
	{

		public static void PreparePredictor(ref T settings, ref NetworkContainer container) { }
		
		public static bool Predict(ref T settings, ref NetworkContainer container) { return false; }

	}
}
