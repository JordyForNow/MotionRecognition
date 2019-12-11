using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
    [Serializable]
    public class Motion3DImage : IImage
    {
        public int size;
        public BitModulator[,] top, side;

        public Motion3DImage(int s = 500)
        {
            size = s;
            top = new BitModulator[size, size];
            side = new BitModulator[size, size];
        }

        public Motion3DImage(ref Table<Measurement> t) : this()
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
                    x = (int)Math.Round(Remap(sample.sampleData[i].pos.x, vecMin.x, vecMax.x, 0, size - 1));
                    y = (int)Math.Round(Remap(sample.sampleData[i].pos.y, vecMin.y, vecMax.y, 0, size - 1));
                    z = (int)Math.Round(Remap(sample.sampleData[i].pos.z, vecMin.z, vecMax.z, 0, size - 1));
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

        public void Serialize(string filePath = "./data")
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            using (FileStream fs = File.Create(filePath))
            {
                AddText(fs, size + "\n");
                writeBitModulator(fs, top);
                AddText(fs, "|");
                writeBitModulator(fs, side);
            }
        }

        public Motion3DImage DeSerialize(string filePath = "./data")
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException();

            Motion3DImage image = new Motion3DImage();

            string file = File.ReadAllText(filePath);

			var d = file.Split("\n");
			this.size = int.Parse(d[0]);
			
			string[] data = d[1].Split("|");
            string[] top = data[0].Split("-");
            string[] side = data[1].Split("-");

			this.top = createBitModulator(top);
			this.side = createBitModulator(side);

            return image;
        }

        private BitModulator[,] createBitModulator(string[] arr)
        {
            BitModulator[,] bm = new BitModulator[size, size];

            foreach (string val in arr)
            {
                string v = Regex.Replace(val, "({|})", "");

                string[] values = v.Split(",");
				int x = int.Parse(values[0]);
				int y = int.Parse(values[1]);

                bm[x, y] = new BitModulator();
                bm[x, y].SetVal(uint.Parse(values[2]));
            }

            return bm;
        }

        private void writeBitModulator(FileStream fs, BitModulator[,] b)
        {
			bool first = true;

            for (uint x = 0; x < size; x++)
            {
                for (uint y = 0; y < size; y++)
                {
                    if (b[x, y] == null) continue;

                    if (first)
                    {
                        AddText(fs, $"{{{x},{y},{b[x, y].GetVal()}}}");
						first = false;
                    }
                    else
                    {
                        AddText(fs, $"-{{{x},{y},{b[x, y].GetVal()}}}");
                    }

                }
            }
        }

        private void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
		
		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			return true;
		}
    }
}
