using System.Collections.Generic;

namespace MotionRecognition
{
    // Settings struct with properties needed for the functions in the transformer
    public struct IntervalBasedTransformerSettings
    {
        public Sample<Vector3>[] sampleList;

        public int interval { get; set; }
        public int count
        {
            get
            {
                return this.interval;
            }
            set
            {
                this.interval = value;
            }
        }
    }

    /*
	* Transformer which transforms sample list to downsized list based on given count.
	*/
    public class IntervalBasedTransformer : IMovementTransformer<IntervalBasedTransformerSettings>
    {
        // Returns list of doubles filtered from original sample list on specific interval
        public virtual double[] GetNeuralInput(IntervalBasedTransformerSettings settings)
        {
            List<double> values = new List<double>();

            for (int i = 0; i < settings.sampleList.Length; i++)
            {
                if (i % settings.interval == 0)
                {
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
