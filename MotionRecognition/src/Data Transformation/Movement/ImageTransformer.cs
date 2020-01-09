using System;
using System.Linq;

namespace MotionRecognition
{
    public enum LeapMotionJoint
    {
        PALM = 0,
        THUMB_0 = 1,
        THUMB_1 = 2,
        THUMB_2 = 3,
        THUMB_3 = 4,
        INDEX_0 = 5,
        INDEX_1 = 6,
        INDEX_2 = 7,
        INDEX_3 = 8,
        MIDDLE_0 = 9,
        MIDDLE_1 = 10,
        MIDDLE_2 = 11,
        MIDDLE_3 = 12,
        RING_0 = 13,
        RING_1 = 14,
        RING_2 = 15,
        RING_3 = 16,
        LITTLE_0 = 17,
        LITTLE_1 = 18,
        LITTLE_2 = 19,
        LITTLE_3 = 20
    }

    // Settings struct with properties needed for the functions in the transformer
    public struct ImageTransformerSettings
    {
        public int size;
        public Sample<Vector3>[] samples;

        // Which items are used of the sample column list.
        public LeapMotionJoint[] focusJoints;
    }

	// Transforms sample list to a 3D-matrix.
    public class ImageTransformer : IMovementTransformer<ImageTransformerSettings>
    {
        // Remap a value to another range.
        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public double[] GetNeuralInput(ImageTransformerSettings settings)
        {
            // Create a double[] which contains two images, the front view and a base x,y top view.
            double[] dField = new double[settings.size * settings.size * 2];

            // Find the differential value for the neural network to make out a difference.
            int incr = (int)Math.Floor((decimal)int.MaxValue / settings.focusJoints.Length);

            // Find the min and the maximum value this forms the base range.
            Vector3 vecMin = new Vector3();
            Vector3 vecMax = new Vector3();
            foreach (var s in settings.samples)
            {
                for (int i = 0; i < s.values.Length; i++)
                {
                    if (settings.focusJoints.Count(o => (int)o == i) > 0)
                    {
                        vecMin.x = s.values[i].x < vecMin.x ? s.values[i].x : vecMin.x;
                        vecMin.y = s.values[i].y < vecMin.y ? s.values[i].y : vecMin.y;
                        vecMin.z = s.values[i].z < vecMin.z ? s.values[i].z : vecMin.z;

                        vecMax.y = s.values[i].y > vecMax.y ? s.values[i].y : vecMax.y;
                        vecMax.x = s.values[i].x > vecMax.x ? s.values[i].x : vecMax.x;
                        vecMax.z = s.values[i].z > vecMax.z ? s.values[i].z : vecMax.z;
                    }
                }

            }

            // Remap all sample vectors to a map in a range from 0 to the specified (settings size).
            foreach (var sample in settings.samples)
            {
                for (int i = 0; i < sample.values.Length; i++)
                {
                    if (settings.focusJoints.Count(o => (int)o == i) > 0)
                    {
                        int x = (int)Math.Round(Remap(sample.values[i].x, vecMin.x, vecMax.x, 0, settings.size - 1));
                        int y = (int)Math.Round(Remap(sample.values[i].y, vecMin.y, vecMax.y, 0, settings.size - 1));
                        int z = (int)Math.Round(Remap(sample.values[i].z, vecMin.z, vecMax.z, 0, settings.size - 1));

                        // Assign to the map.
                        dField[(settings.size * y) + x] += incr;
                        dField[(settings.size * settings.size) + (settings.size * z) + x] += incr;
                    }
                }
            }

            return dField;
        }
    }
}
