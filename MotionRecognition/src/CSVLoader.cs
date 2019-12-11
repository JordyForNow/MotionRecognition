using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class CSVLoader : IDataLoader<JointMeasurement>
	{
		// Path points to the CSV file that is to be loaded
		private string Path;
		// Holds the amount of joints in the data that is to be loaded
		private int jointsCount;
		public CSVLoader(string _Path, int _jointsCount)
		{
			this.Path = _Path;
			this.jointsCount = _jointsCount;
		}

		#region PrivateFunctions
		private Table<JointMeasurement> parseFile(bool hasHeader = true)
		{
			// create a new Table.
			var Table = new Table<JointMeasurement>();
			// if the file has a header then skip it.
			var Rows = (hasHeader ? File.ReadAllLines(Path).Skip(1) : File.ReadAllLines(Path)).Select(line => line.Split(','));
			// for each row we create a sample
			foreach (var Row in Rows)
			{
				if (string.IsNullOrEmpty(Row[0])) continue;

				Sample<JointMeasurement> Sample = new Sample<JointMeasurement>();
				Sample.Timestamp = float.Parse(Row[0]);
				Sample.sampleData = new List<JointMeasurement>(jointsCount);
				for (uint i = 1; i < Row.Count(); i += 2)
				{
					JointMeasurement m = new JointMeasurement();
					m.parse(Row[i], Row[i + 1]);
					Sample.sampleData.Add(m);
				}
				Table.samples.Add(Sample);
			}
			return Table;
		}
		#endregion

		#region PublicFunctions
		public Table<JointMeasurement> GetData()
		{
			return parseFile();
		}
		#endregion
	}
}
