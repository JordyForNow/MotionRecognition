namespace MotionRecognition
{
	// Basic interface used to translate Vector3 to values used for transformers
	public interface ITransformerValue
	{
		// Returns a value a neural network supports.
		double[] GetTransformerValue();
	}
}
