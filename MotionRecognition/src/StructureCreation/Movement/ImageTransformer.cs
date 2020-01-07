using System;
using System.Linq;

namespace MotionRecognition
{
	public enum LeapMotionJoint : int
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
		BABY_0 = 17,
		BABY_1 = 18,
		BABY_2 = 19,
		BABY_3 = 20
	}

	public struct ImageTransformerSettings
	{
		public int size;
		public Sample<Vec3>[] samples;
		public LeapMotionJoint[] focus_joints;
	}

	/*
	* Transforms sample list to 3d-matrix.
	*/
	public class ImageTransformer : IMovementTransformer<ImageTransformerSettings>
	{
		private float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}
		private Sample<Vec3>[] RemapList(ref ImageTransformerSettings settings)
		{
			Sample<Vec3>[] remapped_samples = new Sample<Vec3>[settings.samples.Length];
			Vec3 vecMin = new Vec3();
			Vec3 vecMax = new Vec3();
			foreach (var s in settings.samples)
			{
				foreach (var m in s.vectorArr)
				{
					vecMin.x = m.x < vecMin.x ? m.x : vecMin.x;
					vecMin.y = m.y < vecMin.y ? m.y : vecMin.y;
					vecMin.z = m.z < vecMin.z ? m.z : vecMin.z;

					vecMax.y = m.y > vecMax.y ? m.y : vecMax.y;
					vecMax.x = m.x > vecMax.x ? m.x : vecMax.x;
					vecMax.z = m.z > vecMax.z ? m.z : vecMax.z;
				}
			}

			// Remap all sample vectors to a map in a range from 0 -> 499 (500).
			int sample_index = 0, col_index = 0;
			foreach (var sample in settings.samples)
			{
				Sample<Vec3> new_sample = new Sample<Vec3>();
				new_sample.vectorArr = new Vec3[settings.focus_joints.Length];
				for (int i = 0; i < sample.vectorArr.Length; i++)
				{
					if (settings.focus_joints.Count(o => (int)o == i) > 0)
					{
						var new_vec = new Vec3();
						new_vec.x = (int)Math.Round(Remap(sample.vectorArr[i].x, vecMin.x, vecMax.x, 0, settings.size - 1));
						new_vec.y = (int)Math.Round(Remap(sample.vectorArr[i].y, vecMin.y, vecMax.y, 0, settings.size - 1));
						new_vec.z = (int)Math.Round(Remap(sample.vectorArr[i].z, vecMin.z, vecMax.z, 0, settings.size - 1));
						new_sample.vectorArr[col_index] = new_vec;
						col_index++;
					}
				}
				remapped_samples[sample_index] = new_sample;
				sample_index++;
				col_index = 0;
			}
			return remapped_samples;
		}
		public double[] GetNeuralInput(ImageTransformerSettings settings)
		{
			double[] arr = new double[settings.samples.Length * settings.focus_joints.Length * 3];
			int index = 0;
			foreach(var sample in RemapList(ref settings))
			{
				foreach(var vec in sample.vectorArr)
				{
					arr[index] = vec.x;
					index++;
					arr[index] = vec.y;
					index++;
					arr[index] = vec.z;
					index++;
				}
			}

			return arr;
		}
	}
}
