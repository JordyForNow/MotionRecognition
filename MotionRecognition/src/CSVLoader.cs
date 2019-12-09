using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class CSVLoader : IDataLoader
	{
		private string path;
		private int measurementsize;
		public CSVLoader(string path, int measurementsize)
		{
			this.path = path;
			this.measurementsize = measurementsize;
		}
		class Table<T>
		{
			public List<Sample<T>> samples = new List<Sample<T>>();
		}
		private Table<Measurement> parseFile(bool hasHeader = true)
		{
			var table = new Table<Measurement>();
			var rows = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path)).Select(line => line.Split(','));
			foreach(var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				Sample<Measurement> sample = new Sample<Measurement>();
				sample.timestamp = float.Parse(row[0]);
				sample.sampleData = new List<Measurement>();
				for (uint i = 1; i < row.Count(); i++)
				{
					Measurement m = new Measurement();
					m.parse(row[i], row[i + 1]);
					sample.sampleData.Add(m);
					i++;
				}
				table.samples.Add(sample);
			}
			return table;
		}

		public MotionImage LoadImage()
		{
			MotionImage image = new MotionImage();
			Table<Measurement> t = parseFile();
			foreach (var item in t.samples)
				Console.WriteLine(item.sampleData[0].pos.x);
			return image;
		}
	}
}
