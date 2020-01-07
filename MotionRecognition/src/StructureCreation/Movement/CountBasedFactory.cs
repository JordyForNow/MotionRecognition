namespace MotionRecognition
{
	public struct CountBasedFactorySettings {

	}
	
    public class CountBasedFactory : IntervalBasedFactory
    {
        public override double[] GetNeuralInput(CountBasedFactorySettings settings)
		{
			return base.GetNeuralInput(null);
		}
    }
}
