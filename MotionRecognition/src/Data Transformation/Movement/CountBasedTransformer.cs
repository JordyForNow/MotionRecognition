using System;

namespace MotionRecognition
{
	// Transformer which transforms sample list to downsized sample list based on specific interval.
	public class CountBasedTransformer : IntervalBasedTransformer
	{
		// Returns a specific count of values from the original sample list, count is used to calculate an interval which is used to retrieve samples from the original list.
		public override double[] GetNeuralInput(IntervalBasedTransformerSettings settings)
		{
			settings.interval = settings.sampleList.Length / settings.count;
			return base.GetNeuralInput(settings);
		}
	}
}
