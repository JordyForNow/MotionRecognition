using System;

namespace MotionRecognition
{
	[Serializable]
	public class Motion3DImage : IImage
	{
		public BitModulator[,] top = new BitModulator[500, 500];
		public BitModulator[,] side = new BitModulator[500,500];

		public Motion3DImage(ref Table<Measurement> t)
		{
			LoadImage(ref t);
		}

		public void LoadImage(ref Table<Measurement> t)
		{
			Vec3 vecMin = new Vec3();
			Vec3 vecMax = new Vec3();
			foreach (var s in t.samples)
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
			int x, y, z;
			foreach (var sample in t.samples)
			{
				for (int i = 0; i < sample.sampleData.Count; i++)
				{
					x = (int)Math.Round(Remap(sample.sampleData[i].pos.x, vecMin.x, vecMax.x, 0, 499));
					y = (int)Math.Round(Remap(sample.sampleData[i].pos.y, vecMin.y, vecMax.y, 0, 499));
					z = (int)Math.Round(Remap(sample.sampleData[i].pos.z, vecMin.z, vecMax.z, 0, 499));
					if (this.top[x, y] == null) this.top[x, y] = new BitModulator();
					this.top[x, y].SetIndex(i, true);
					if (this.side[z, y] == null) this.side[z, y] = new BitModulator();
					this.side[z, y].SetIndex(i, true);
				}
			}
		}

		private float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}
	}
}
