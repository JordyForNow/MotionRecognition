using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class CSVLoader : IDataLoader<JointMeasurement>
	{
		// Path points to the CSV file that is to be loaded
		private string path;
		// Holds the amount of joints in the data that is to be loaded
		private int jointsCount;
		
		public CSVLoader(string _path, int _jointsCount)
		{
			this.path = _path;
			this.jointsCount = _jointsCount;
		}
		
		private List<Sample<JointMeasurement>> parseFile(bool hasHeader = true)
		{
			// Create a new Table.
			var sampleList = new List<Sample<JointMeasurement>>();
			// If the file has a header then skip it.
			var rows = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path)).Select(line => line.Split(','));
			// For each row we create a sample.
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				Sample<JointMeasurement> sample = new Sample<JointMeasurement>();
				sample.timestamp = float.Parse(row[0]);
				sample.sampleData = new List<JointMeasurement>(jointsCount);
				for (uint i = 1; i < row.Count(); i += 2)
				{
					JointMeasurement m = new JointMeasurement();
					m.parse(row[i], row[i + 1]);
					sample.sampleData.Add(m);
				}
				sampleList.Add(sample);
			}
			return sampleList;
		}

		public List<Sample<JointMeasurement>> GetData()
		{
			return parseFile();
		}
	}
}
