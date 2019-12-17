using System;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class Motion3DImage : ISampledImage<JointMeasurement>
	{
		public enum Angle : byte
		{
			TOP = 0,
			SIDE = 1
		}

		// Size is the maximum field size of Top and Side: 2*(sizeˆsize).
		public readonly int size;
		// Top and Side are the 3D composited images.
		private int[,] movementMap;

		public Motion3DImage(int _size = 500)
		{
			this.size = _size;
			movementMap = new int[size * 2, size];
		}

		public Motion3DImage(ref List<Sample<JointMeasurement>> _table) : this()
		{
			CreateImageFromTable(ref _table);
		}

		public void SetPosition(Angle a, int y, int x, int val)
		{
			switch (a)
			{
				case Angle.TOP:
					movementMap[y, x] = val;
					break;
				case Angle.SIDE:
					movementMap[y + size, x] = val;
					break;
			}
		}

		public int[,] GetData()
		{
			return movementMap;
		}

		#region PublicFunctions 

		// Create a 3DImage from a table of measurements.
		public void CreateImageFromTable(ref List<Sample<JointMeasurement>> _sampleList)
		{
			// Find the current base range with the minimum and the maximum.
			Vec3 vecMin = new Vec3();
			Vec3 vecMax = new Vec3();
			foreach (var s in _sampleList)
			{
				foreach (var m in s.sampleData)
				{
					vecMin.x = m.pos.x < vecMin.x ? m.pos.x : vecMin.x;
					vecMin.y = m.pos.y < vecMin.y ? m.pos.y : vecMin.y;
					vecMin.z = m.pos.z < vecMin.z ? m.pos.z : vecMin.z;

					vecMax.y = m.pos.y > vecMax.y ? m.pos.y : vecMax.y;
					vecMax.x = m.pos.x > vecMax.x ? m.pos.x : vecMax.x;
					vecMax.z = m.pos.z > vecMax.z ? m.pos.z : vecMax.z;
				}
			}

			// Remap all sample vectors to a map in a range from 0 -> 499 (500).
			int x, y, z;
			foreach (var sample in _sampleList)
			{
				for (int i = 0; i < sample.sampleData.Count; i++)
				{
					x = (int)Math.Round(Remap(sample.sampleData[i].pos.x, vecMin.x, vecMax.x, 0, size - 1));
					y = (int)Math.Round(Remap(sample.sampleData[i].pos.y, vecMin.y, vecMax.y, 0, size - 1));
					z = (int)Math.Round(Remap(sample.sampleData[i].pos.z, vecMin.z, vecMax.z, 0, size - 1));
					BitModulator.SetIndex(ref this.movementMap[x, y], i, true);
					BitModulator.SetIndex(ref this.movementMap[size + z, y], i, true);
				}
			}
		}
		#endregion

		#region PrivateFunctions

		// This function remaps floats to a given range.
		private float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		#endregion

		#region BaseFunctions
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType()) // If the other object is not a Motion3DImage return false.
			{
				return false;
			}

			var other = (Motion3DImage)obj;

			if (size != other.size) return false; // If the sizes dont match return false.

			for (int y = 0; y < size * 2; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (movementMap[y, x] != other.movementMap[y, x]) // If both cells are not equal return false.
					{
						return false;
					}
				}
			}

			return true;
		}
		#endregion
	}
}
