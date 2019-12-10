using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MotionRecognition
{
	class Table<T>
	{
		public List<Sample<T>> samples = new List<Sample<T>>();
	}

	public class CSVLoader : IDataLoader<Motion3DImage>
	{
		#region Properties
		private string path;
		private int measurementsize;
		#endregion
		#region Object(De)Construction
		public CSVLoader(string path, int measurementsize)
		{
			this.path = path;
			this.measurementsize = measurementsize;
		}
		~CSVLoader()
		{

		}
		#endregion
		#region PrivateFunc
		private Table<Measurement> parseFile(bool hasHeader = true)
		{
			var table = new Table<Measurement>();
			var rows = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path)).Select(line => line.Split(','));
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				Sample<Measurement> sample = new Sample<Measurement>();
				sample.timestamp = float.Parse(row[0]);
				sample.sampleData = new List<Measurement>(measurementsize);
				for (uint i = 1; i < row.Count(); i += 2)
				{
					Measurement m = new Measurement();
					m.parse(row[i], row[i + 1]);
					sample.sampleData.Add(m);
				}
				table.samples.Add(sample);
			}
			return table;
		}
		#endregion
		#region PublicFunc

		public float Remap(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}
		public Motion3DImage LoadImage()
		{
			Motion3DImage image = new Motion3DImage();
			Table<Measurement> t = parseFile();

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
			foreach(var sample in t.samples)
			{
				for(int i = 0; i < sample.sampleData.Count(); i++)
				{
					x = (int)Math.Round(Remap(sample.sampleData[i].pos.x, vecMin.x, vecMax.x, 0, 499));
					y = (int)Math.Round(Remap(sample.sampleData[i].pos.y, vecMin.y, vecMax.y, 0, 499));
					z = (int)Math.Round(Remap(sample.sampleData[i].pos.z, vecMin.z, vecMax.z, 0, 499));
					if (image.top[x, y] == null) image.top[x, y] = new BitModulator();
					image.top[x, y].SetIndex(i, true);
					if (image.side[z, y] == null) image.side[z, y] = new BitModulator();
					image.side[z, y].SetIndex(i, true);
				}
			}

			return image;
		}
		#endregion
	}
}
