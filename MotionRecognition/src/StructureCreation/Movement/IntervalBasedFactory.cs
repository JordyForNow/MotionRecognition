using System.Collections.Generic;

namespace MotionRecognition
{
	public struct IntervalBasedFactorySettings
    {
        public Sample<Vec3>[] sampleList;

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

    public class IntervalBasedFactory : IMovementFactory<IntervalBasedFactorySettings>
    {
        public virtual double[] GetNeuralInput(IntervalBasedFactorySettings settings)
        {
            List<double> values = new List<double>();

			for (int i = 0; i < settings.sampleList.Length; i++)
            {
               if (i % settings.interval == 0)
			   {
				   foreach(Vec3 v in settings.sampleList[i].vectorArr){
					   values.AddRange(v.GetFactoryValue());

				   }
			   }
            }

            return values.ToArray();
        }
    }
}
