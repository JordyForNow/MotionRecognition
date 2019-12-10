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

	public class CSVLoader : IDataLoader
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
		public MotionImage LoadImage()
		{
			MotionImage image = new MotionImage();
			Table<Measurement> t = parseFile();

			return image;
		}
		#endregion
	}
}
