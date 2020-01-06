namespace MotionRecognition
{
	public struct CountBasedFactorySettings {

	}
	
    public class CountBasedFactory : IMovementFactory
    {
        public override double[] GetNeuralInput(CountBasedFactorySettings settings)
		{
			return base.GetNeuralInput(settings);
		}
    }
}
