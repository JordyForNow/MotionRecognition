namespace MotionRecognition
{
	public class CountBasedFactory : IntervalBasedFactory
	{
		public override double[] GetNeuralInput(IntervalBasedFactorySettings settings)
		{
			settings.interval = settings.sampleList.Length / settings.count;
			return base.GetNeuralInput(settings);
		}
	}
}
