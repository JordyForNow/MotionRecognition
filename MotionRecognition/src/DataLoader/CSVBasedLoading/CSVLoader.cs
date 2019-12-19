using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace MotionRecognition
{
	public class CSVLoader : IDataLoader<JointMeasurement>
	{
		// Path points to the CSV file that is to be loaded.
		private string path;

		public CSVLoader(string _path)
		{
			this.path = _path;
		}

		private List<Sample<JointMeasurement>> parseFile(bool hasHeader = true, bool skipFirstDataLine = true)
		{
			// Create a new Table.
			var sampleList = new List<Sample<JointMeasurement>>();
			// If the file has a header then skip it.
			var rows = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path)).Select(line => line.Split(','));
			// For each row a sample is created.
			foreach (var row in rows)
			{
				if (string.IsNullOrEmpty(row[0])) continue;

				if (skipFirstDataLine)
				{
					skipFirstDataLine = false;
					continue;
				}

				Sample<JointMeasurement> sample = new Sample<JointMeasurement>();
				sample.timestamp = float.Parse(row[0]);
				sample.sampleData = new List<JointMeasurement>(row.Count()/2);
				for (uint i = 1; i < row.Count(); i += 2)
				{
					JointMeasurement measurement = new JointMeasurement();
					measurement.parse(row[i], row[i + 1]);
					sample.sampleData.Add(measurement);
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
