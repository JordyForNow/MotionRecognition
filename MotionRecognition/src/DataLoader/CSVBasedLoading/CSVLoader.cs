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
		public int trimLeft, trimRight;
	}

	public class CSVLoader<T> : IDataLoader<Sample<T>> where T : IParseable, new()
	{
		private List<CSVColumnFilter> filters;
		// Path points to the CSV file that is to be loaded.
		private CSVLoaderSettings settings;

		public CSVLoader(ref CSVLoaderSettings _settings, ref List<CSVColumnFilter> _filters)
		{
			this.settings = _settings;
			this.filters = _filters;
		}

		private uint ColumnCount(ref string[] _row)
		{
			var row = _row;
			uint count = 0;
			for (uint i = 0; i < row.Length; i++)
			if (filters.Exists(o => o.UseColumn(ref row, i)))
					count++;
			return count;
		}

		public Sample<T>[] LoadData()
		{
			// Create a new Table.
			var sampleList = new List<Sample<T>>();
			// If the file has a header then skip it.
			string[][] rows = File.ReadAllLines(this.settings.filepath)
				.Select(line => line.Split(',')).ToArray();
			if (this.settings.CSVHasHeader)
				rows = rows.Skip(1).ToArray();
			if (this.settings.trimLeft > 0 || this.settings.trimRight > 0)
				rows = rows.Skip(this.settings.trimLeft)
					.Take(rows.Count() - this.settings.trimLeft - this.settings.trimRight).ToArray();

			if (rows.Count() == 0) return null;

			uint columnCount = ColumnCount(ref rows[0]);

			// For each row a sample is created.
			for (uint row_index = 0; row_index < rows.Count(); row_index++)
			{
				// Check for unspecified end of file.
				if (string.IsNullOrEmpty(rows[row_index][0]))
					continue;

				// Create new Sample.
				Sample<T> sample = new Sample<T>();
				// Parse rquired timestamp.
				sample.timestamp = float.Parse(rows[row_index][0]);
				sample.vectorArr = new T[columnCount];
				uint vectorArrIndex = 0;
				for (uint i = 0; i < rows[row_index].Count(); i++)
				{
					// Check if a filter is blocking the column.
					if (filters.Exists(o => !o.UseColumn(ref rows[row_index], i))) continue;

					T vec = new T();
					vec.parse(rows[row_index][i]);
					sample.vectorArr[vectorArrIndex] = vec;
					vectorArrIndex += 1;
				}
				sampleList.Add(sample);
			}
			return sampleList.ToArray();
		}
	}
}
