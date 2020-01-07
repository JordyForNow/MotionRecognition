using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MotionRecognition
{
	public class CSVLoaderSettings
	{
		public string filepath;
		public bool CSVHasHeader;
		public int TrimLeft = 0, TrimRight = 0;
	}
	public class CSVLoader : IDataLoader<Sample<Vec3>>
	{
		// Path points to the CSV file that is to be loaded.
		private CSVLoaderSettings settings;

		public CSVLoader(CSVLoaderSettings _settings)
		{
			this.settings = _settings;
		}

		public Sample<Vec3>[] LoadData()
		{
			// Create a new Table.
			var sampleList = new List<Sample<Vec3>>();
			
			// If the file has a header then skip it.
			var rows = (this.settings.CSVHasHeader ? File.ReadAllLines(this.settings.filepath).Skip(1) : File.ReadAllLines(this.settings.filepath)).Select(line => line.Split(','));

			rows = rows.Skip(this.settings.TrimLeft).Take(rows.Count() - this.settings.TrimLeft - this.settings.TrimRight);

			// For each row a sample is created.
			int j = 0;
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				Sample<Vec3> sample = new Sample<Vec3>();
				sample.timestamp = float.Parse(row[0]);
				sample.vectorArr = new Vec3[row.Count() / 2];
				uint vectorArrIndex = 0;
				for (uint i = 1; i < row.Count(); i += 2)
				{
					Vec3 vec = new Vec3();
					vec.parse(row[i]);
					sample.vectorArr[vectorArrIndex] = vec;
					vectorArrIndex += 1;
				}
				sampleList.Add(sample);
				j++;
			}
			return sampleList.ToArray();
		}
	}
}
