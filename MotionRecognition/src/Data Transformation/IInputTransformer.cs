namespace MotionRecognition
{
	// Interface for the transformers which transform a list of samples to a usable format for the neural net.
	public interface IInputTransformer<T> where T : notnull
	{
		// Returns a value of an object in a format a neural network support.
		double[] GetNeuralInput(T settings);
	}
}
