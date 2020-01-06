using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MotionRecognition
{
	public struct CSVLoaderSettings
	{
		public string filepath;
		public bool CSVHasHeader;
		public int TrimLeft, TrimRight;
	}
	public class CSVLoader<T> : IDataLoader<Sample<T>> where T : IParseable, new()
	{
		// Path points to the CSV file that is to be loaded.
		private CSVLoaderSettings settings;

		public CSVLoader(CSVLoaderSettings _settings)
		{
			this.settings = _settings;
		}

		public Sample<T>[] LoadData()
		{
			// Create a new Table.
			var sampleList = new List<Sample<T>>();
			// If the file has a header then skip it.
			var rows = (this.settings.CSVHasHeader ? File.ReadAllLines(this.settings.filepath).Skip(1) :
				File.ReadAllLines(this.settings.filepath)).Select(line => line.Split(','));
			rows = rows.Skip(this.settings.TrimLeft)
				.Take(rows.Count() - this.settings.TrimLeft - this.settings.TrimRight);
			// For each row a sample is created.
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				Sample<T> sample = new Sample<T>();
				sample.timestamp = float.Parse(row[0]);
				sample.vectorArr = new T[row.Count() / 2];
				uint vectorArrIndex = 0;
				for (uint i = 1; i < row.Count(); i += 2)
				{
					T vec = new T();
					vec.parse(row[i]);
					sample.vectorArr[vectorArrIndex] = vec;
					vectorArrIndex += 1;
				}
				sampleList.Add(sample);
			}
			return sampleList.ToArray();
		}
	}
}
