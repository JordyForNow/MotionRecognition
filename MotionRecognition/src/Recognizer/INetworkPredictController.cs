namespace MotionRecognition
{
	public interface INetworkPredictController<T>
	{

		public static void preparePredictor(ref T settings, ref NetworkContainer container) { }
		
		public static bool predict(ref T settings, ref NetworkContainer container) { return false; }

	}
}
