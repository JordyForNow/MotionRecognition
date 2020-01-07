using System.Collections.Generic;

namespace MotionRecognition
{
    public struct IntervalBasedFactorySettings
    {
        public Sample<Vec3>[] sampleList;
        public int interval;
    }

    public class IntervalBasedFactory : IMovementFactory<IntervalBasedFactorySettings>
    {
        public virtual double[] GetNeuralInput(IntervalBasedFactorySettings settings)
        {
            List<double> values = new List<double>();

			for (int i = 0; i < settings.sampleList.Length; i++) { }
            //{
            //    if (i % settings.interval == 0)
            //        values.AddRange(settings.sampleList[i].GetFactoryValue());
            //}

            return null;
        }
    }
}
