using System;
using System.Collections.Generic;

namespace MotionRecognition
{
	// Settings struct with properties needed for the functions in the transformer.
	public struct IntervalBasedTransformerSettings
	{
		public Sample<Vector3>[] sampleList;

		public double interval { get; set; }
		public double count
		{
			get
			{
				return interval;
			}
			set
			{
				interval = value;
			}
		}
	}

	// Transformer which transforms sample list to downsized list based on given count.
	public class IntervalBasedTransformer : IMovementTransformer<IntervalBasedTransformerSettings>
	{
		// Returns list of doubles filtered from original sample list on a specific interval.
		public virtual double[] GetNeuralInput(IntervalBasedTransformerSettings settings)
		{
			List<double> values = new List<double>();
			double inter = settings.interval;
			for (int i = 0; i < settings.sampleList.Length; i++)
			{
				if (i == Math.Round(inter) || i == 0)
				{
					if (i != 0)
						inter += settings.interval;
					foreach (Vector3 v in settings.sampleList[i].values)
					{
						values.AddRange(v.GetTransformerValue());
					}
				}
			}

			return values.ToArray();
		}
	}
}
