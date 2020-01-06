using System.Collections.Generic;

namespace MotionRecognition
{
    public struct IntervalBasedFactorySettings<T>
    {
        public Sample<Vec3> sampleList;
        public int interval;
    }

    public class IntervalBasedFactory : IMovementFactory
    {
        public virtual double[] GetNeuralInput(IntervalBasedFactorySettings<Vec3> settings)
        {
            List<double> values = new List<double>();

            for (int i = 0; i < setting.sampleList.Length; i++)
            {
                if (i % settings.interval == 0)
                    values.AddRange(input[i].GetFactoryValue());
            }

            return values;
        }
    }
}
