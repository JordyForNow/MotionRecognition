namespace MotionRecognition
{
	public interface INetworkPredictController<T>
	{

		public static void PreparePredictor(ref NetworkContainer container, ref T settings) { }
		
		public static bool Predict(ref NetworkContainer container, ref T settings) { return false; }

	}
}
