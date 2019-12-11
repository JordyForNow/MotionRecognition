using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
    public class Motion3DImage : IImage
    {
		// Size is the maximum field size of Top and Side: 2*(sizeˆsize)
        public int Size;
		// Top and Side are the 3D composited images.
        public BitModulator[,] Top, Side;

        public Motion3DImage(int _Size = 500)
        {
            SetSize(_Size);
        }

        public Motion3DImage(ref List<Sample<JointMeasurement>> _Table) : this()
        {
            CreateImageFromTable(ref _Table);
        }

		// Create an 3DImage from a table of measurements
        public void CreateImageFromTable(ref List<Sample<JointMeasurement>> _SampleList)
        {
			// find the current base range with the minimum and the maximum
            Vec3 vecMin = new Vec3();
            Vec3 vecMax = new Vec3();
            foreach (var s in _SampleList)
            {
                foreach (var m in s.sampleData)
                {
                    vecMin.x = m.Pos.x < vecMin.x ? m.Pos.x : vecMin.x;
                    vecMin.y = m.Pos.y < vecMin.y ? m.Pos.y : vecMin.y;
                    vecMin.z = m.Pos.z < vecMin.z ? m.Pos.z : vecMin.z;

                    vecMax.y = m.Pos.y > vecMax.y ? m.Pos.y : vecMax.y;
                    vecMax.x = m.Pos.x > vecMax.x ? m.Pos.x : vecMax.x;
                    vecMax.z = m.Pos.z > vecMax.z ? m.Pos.z : vecMax.z;
                }
            }
			// Remap all sample vectors to a map in a range from 0 -> 499 (500).
            int x, y, z;
            foreach (var sample in _SampleList)
            {
                for (int i = 0; i < sample.sampleData.Count; i++)
                {
                    x = (int)Math.Round(Remap(sample.sampleData[i].Pos.x, vecMin.x, vecMax.x, 0, Size - 1));
                    y = (int)Math.Round(Remap(sample.sampleData[i].Pos.y, vecMin.y, vecMax.y, 0, Size - 1));
                    z = (int)Math.Round(Remap(sample.sampleData[i].Pos.z, vecMin.z, vecMax.z, 0, Size - 1));
                    if (this.Top[x, y] == null) this.Top[x, y] = new BitModulator();
                    this.Top[x, y].SetIndex(i, true);
                    if (this.Side[z, y] == null) this.Side[z, y] = new BitModulator();
                    this.Side[z, y].SetIndex(i, true);
                }
            }
        }

		// this function remaps floats to a given range
        private float Remap(float Value, float from1, float to1, float from2, float to2)
        {
            return (Value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

		public void Serialize(string filePath = "./data")
		{
			if (File.Exists(filePath))
				File.Delete(filePath);

            using (FileStream fs = File.Create(filePath))
            {
                AddText(fs, Size + Environment.NewLine);
                writeBitModulator(fs, Top);
                AddText(fs, "|" + Environment.NewLine);
                writeBitModulator(fs, Side);
            }
        }

        public Motion3DImage DeSerialize(string filePath = "./data")
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException();

            //Index 0 = size, 1 = values for Top, 2 = values for Side
            int Index = 0;
            string Line;

            StreamReader File = new StreamReader(filePath);

            while ((Line = File.ReadLine()) != null)
            {
                string[] Values = Line.Split(",");

                switch (Index)
                {
                    case 0:
                        this.Size = int.Parse(Line);
                        Index = 1;
                        break;
                    case 1:
                        //If seperator, switch to index 2 
                        if (Line == "|") {
							Index = 2;
							break;
						}

                        this.Top[int.Parse(Values[0]), int.Parse(Values[1])] = new BitModulator(Values[2]);
                        break;
                    case 2:
                        this.Side[int.Parse(Values[0]), int.Parse(Values[1])] = new BitModulator(Values[2]);
                        break;
                }
            }

            File.Close();

			return this;
		}

        private BitModulator[,] createBitModulator(string[] stringArr)
        {
            BitModulator[,] bm = new BitModulator[Size, Size];

            foreach (string val in stringArr)
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

        private void writeBitModulator(FileStream fs, BitModulator[,] Map)
        {
            for (uint x = 0; x < Size; x++)
            {
                for (uint y = 0; y < Size; y++)
                {
                    if (Map[x, y] != null)
                        AddText(fs, $"{x},{y},{Map[x, y].GetVal()} {Environment.NewLine}");
                }
            }
        }

        private void AddText(FileStream fs, string Value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(Value);
            fs.Write(info, 0, info.Length);
        }

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			var other = (Motion3DImage)obj;

            if (Size != other.Size) return false;

            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (Top[x, y] == null && other.Top[x, y] == null) // if both cells are not set they are equal
                    {
                        continue;
                    }
                    else if ((Top[x, y] == null && other.Top[x, y] != null) || (Top[x, y] != null && other.Top[x, y] == null)) // if either cell is not set they are not equal
                    {
                        return false;
                    }
                    else if (Top[x, y].Equals(other.Top[x, y])) // if equals they are equal
                    {
                        continue;
                    }
                    else // the cells are not equal
                    {
                        return false;
                    }
                }
            }

			return true;
		}

        public void SetSize(int s)
        {
            this.Size = s;
            Top = new BitModulator[Size, Size];
            Side = new BitModulator[Size, Size];
        }
    }
}
