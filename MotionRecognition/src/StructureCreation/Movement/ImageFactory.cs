namespace MotionRecognition
{
	public struct ImageFactorySettings
	{

	}
	public class ImageFactory : IMovementFactory<ImageFactorySettings>
	{
		public double[] GetNeuralInput(ImageFactorySettings settings)
		{
			return null;
		}
	}
}
