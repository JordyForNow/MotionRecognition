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
		public List<ICSVFilter> filters;
	}

	public class CSVLoader<T> : IDataLoader<Sample<T>, CSVLoaderSettings> where T : IParseable, new()
	{
		private static int ColumnCount(ref CSVLoaderSettings settings, ref string[] _row)
		{
			var row = _row;
			int count = row.Length;
			for (uint i = 0; i < row.Length; i++)
				if (settings.filters.Exists(o => !o.Use(ref row, i)))
					count--;
			return count;
		}

		public static Sample<T>[] LoadData(ref CSVLoaderSettings settings)
		{
			if (!File.Exists(settings.filepath))
				throw new FileNotFoundException(settings.filepath);

			// Create a new Table.
			var sampleList = new List<Sample<T>>();
			// If the file has a header then skip it.
			string[][] rows = File.ReadAllLines(settings.filepath)
				.Select(line => line.Split(',')).ToArray();
			if (settings.CSVHasHeader)
				rows = rows.Skip(1).ToArray();
			if (settings.trimLeft > 0 || settings.trimRight > 0)
				rows = rows.Skip(settings.trimLeft)
					.Take(rows.Count() - settings.trimLeft - settings.trimRight).ToArray();

			if (rows.Count() == 0) return sampleList.ToArray();

			int columnCount = ColumnCount(ref settings, ref rows[0]);

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
				sample.values = new T[columnCount];
				uint valuesIndex = 0;
				for (uint i = 0; i < rows[row_index].Count(); i++)
				{
					// Check if a filter is blocking the column.
					if (settings.filters.Exists(o => !o.Use(ref rows[row_index], i))) continue;

					T vec = new T();
					vec.parse(rows[row_index][i]);
					sample.values[valuesIndex] = vec;
					valuesIndex += 1;
				}
				sampleList.Add(sample);
			}
			return sampleList.ToArray();
		}
	}
}
