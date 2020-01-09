using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MotionRecognition
{
	public struct CSVLoaderSettings
	{
		public string filePath;
		public bool CSVHasHeader;
		public int trimUp, trimDown;
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
			if (!File.Exists(settings.filePath))
				throw new FileNotFoundException(settings.filePath);

			// Create a new Table.
			var sampleList = new List<Sample<T>>();

			// If the file has a header then skip it.
			string[][] rows = File.ReadAllLines(settings.filePath)
				.Select(line => line.Split(',')).ToArray();
			if (settings.CSVHasHeader)
				rows = rows.Skip(1).ToArray();
			if (settings.trimUp > 0 || settings.trimDown > 0)
				rows = rows.Skip(settings.trimUp)
					.Take(rows.Count() - settings.trimUp - settings.trimDown).ToArray();

			if (rows.Count() == 0) return sampleList.ToArray();

			int columnCount = ColumnCount(ref settings, ref rows[0]);

			// For each row a sample is created.
			for (uint rowIndex = 0; rowIndex < rows.Count(); rowIndex++)
			{
				// Check for unspecified end of file.
				if (string.IsNullOrEmpty(rows[rowIndex][0]))
					continue;

				// Create new Sample.
				Sample<T> sample = new Sample<T>();

				// Parse required timestamp.
				sample.timestamp = float.Parse(rows[rowIndex][0]);
				sample.values = new T[columnCount];
				uint valuesIndex = 0;

				for (uint i = 0; i < rows[rowIndex].Count(); i++)
				{
					// Check if a filter is blocking the column.
					if (settings.filters.Exists(o => !o.Use(ref rows[rowIndex], i))) continue;

					T vector = new T();
					vector.Parse(rows[rowIndex][i]);
					sample.values[valuesIndex] = vector;
					valuesIndex += 1;
				}
				sampleList.Add(sample);
			}
			return sampleList.ToArray();
		}
	}
}
