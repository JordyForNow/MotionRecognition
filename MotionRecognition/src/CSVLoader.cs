using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class CSVLoader : IDataLoader<JointMeasurement>
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
		private Table<JointMeasurement> parseFile(bool hasHeader = true)
		{
			// create a new table.
			var table = new Table<JointMeasurement>();
			// if the file has a header then skip it.
			var rows = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path)).Select(line => line.Split(','));
			// for each row we create a sample
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				Sample<JointMeasurement> sample = new Sample<JointMeasurement>();
				sample.timestamp = float.Parse(row[0]);
				sample.sampleData = new List<JointMeasurement>(measurementsize);
				for (uint i = 1; i < row.Count(); i += 2)
				{
					JointMeasurement m = new JointMeasurement();
					m.parse(row[i], row[i + 1]);
					sample.sampleData.Add(m);
				}
				table.samples.Add(sample);
			}
			return table;
		}
		#endregion
		#region PublicFunc

		public Table<JointMeasurement> GetData()
		{
			return parseFile();
		}
		#endregion
	}
}
