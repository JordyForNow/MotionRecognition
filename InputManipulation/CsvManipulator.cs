using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace InputManipulation
{
	public struct CsvManipulatorSettings
	{
		public int CopyLines; // how many lines to just copy over
		public int RemoveLines; // how many lines to skip and forget
		public int MutationCount; // amount of mutations
		public float DeviationPercentage; // int 0 - 100
		public float InnerDeviationPercentage; // int 0 - 100 / innerdeviation means the random deviation after the total deviation

		// only one of these needs to be filled, if both are filled then the file will take precedence
		public string DataFile; // csv movement file to copy and mutate
		public string DataFolder; // folder with csv movement files to use and mutate, please end with /

		public string OutputFolder;

		public bool AlterInput; // Alter the input files to have dots instead of commas in the timestamp value
	}

	public static class CsvManipulator
	{
		private static readonly Random random = new Random();

		static public void RunManipulator(ref CsvManipulatorSettings settings)
		{
			if (!Directory.Exists(settings.OutputFolder))
			{
				// Create output directory if necesary
				Directory.CreateDirectory(settings.OutputFolder);
			}
			else
			{
				// Clear otherwise
				ClearOutputFolder(settings.OutputFolder);
			}

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			if (!string.IsNullOrEmpty(settings.DataFile))
			{
				// Mutate a single csv file
				Console.WriteLine($"From: {settings.DataFile} to {settings.OutputFolder}");
				ChangeOriginalFile(ref settings);
				MutateFile(ref settings);
			}
			else if (!string.IsNullOrEmpty(settings.DataFolder))
			{
				// Mutate a foldler with csv files
				Console.WriteLine($"From: {settings.DataFolder} to {settings.OutputFolder}");
				ChangeOriginalBatch(ref settings);
				MutateFolder(ref settings);
			}
			else
			{
				Console.WriteLine("You didnt select any input!");
			}

			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
		}

		static private void ChangeOriginalFile(ref CsvManipulatorSettings settings)
		{
			if (!settings.AlterInput) return;

			var lines = File.ReadAllText(settings.DataFile).Split(Environment.NewLine);

			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].IndexOf(',') == 1)
				{
					ReplaceInString(ref lines[i], 1, '.');
				}
				else if (i > 5)
				{
					return;
				}
			}

			File.WriteAllText(settings.DataFile, string.Join(Environment.NewLine, lines));
		}

		static private void ChangeOriginalBatch(ref CsvManipulatorSettings settings)
		{
			FileInfo[] fileInfo = new DirectoryInfo(settings.DataFolder).GetFiles();

			for (int i = 0; i < fileInfo.Length; i++)
			{
				settings.DataFile = fileInfo[i].FullName;
				ChangeOriginalFile(ref settings);
			}
		}

		static private void MutateFile(ref CsvManipulatorSettings settings, string fileNamePrefix = "")
		{
			// This variable stores the general deviation of each joint per mutated file (i.e. [File1][Joint1])
			Dictionary<string, List<double>> deviations = new Dictionary<string, List<double>>();

			// Local variable to store the amount of lines to copy over
			int CPL = settings.CopyLines;

			// Local variable to store the amount of lines to remove
			int RML = settings.RemoveLines;

			StreamReader reader = new StreamReader(settings.DataFile);
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (line.IndexOf(',') == 1)
				{
					ReplaceInString(ref line, 1, '.');
				}

				if (CPL > 0)
				{
					CPL--;
					for (int i = 0; i < settings.MutationCount; i++)
					{
						using StreamWriter sw = File.AppendText(settings.OutputFolder + fileNamePrefix + "-" + i.ToString() + ".csv");
						sw.WriteLine(line);
						sw.Close();
					}
					continue;
				}

				if (RML > 0)
				{
					RML--;
					continue;
				}

				string[] data = line.Split(',');
				if (data.Length > 1)
				{
					for (int i = 0; i < settings.MutationCount; i++)
					{
						StringBuilder sb = new StringBuilder(data[0]);

						for (int j = 1; j < data.Length; j += 2)
						{ // for each pair of Vector3 and Quarternion edit the Vector 3
							sb.Append($",{MutateVector(data[j], j / 2, fileNamePrefix + i, ref deviations, ref settings)},{ data[j + 1]}");
						}

						File.AppendAllText(settings.OutputFolder + fileNamePrefix + "-" + i.ToString() + ".csv", sb.ToString() + Environment.NewLine);
					}
				}
			}
		}

		static private void MutateFolder(ref CsvManipulatorSettings settings)
		{
			FileInfo[] fileInfo = new DirectoryInfo(settings.DataFolder).GetFiles();

			for (int i = 0; i < fileInfo.Length; i++)
			{
				Console.WriteLine($"Working on {fileInfo[i].Name}");
				settings.DataFile = fileInfo[i].FullName;
				MutateFile(ref settings, i.ToString());
			}
		}

		static private void ReplaceInString(ref string original, int pos, char replacement)
		{
			StringBuilder sb = new StringBuilder(original);
			sb[pos] = replacement;
			original = sb.ToString();
		}

		static private void ClearOutputFolder(string folder)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folder);

			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				file.Delete();
			}
		}

		static private string MutateVector(string vector, int col, string name, ref Dictionary<string, List<double>> deviations, ref CsvManipulatorSettings settings)
		{
			if (!deviations.ContainsKey(name))
			{
				deviations.Add(name, new List<double>());
				deviations[name].Add(CalculateDeviation(settings.DeviationPercentage));
			}
			else
			{
				deviations[name].Add(CalculateDeviation(settings.DeviationPercentage));
			}

			var innerdeviation = CalculateDeviation(settings.InnerDeviationPercentage);

			var points = vector[1..^1].Split("| ");

			string DoDeviation(string data, ref Dictionary<string, List<double>> deviations)
			{
				return (float.Parse(data, CultureInfo.InvariantCulture) * deviations[name][col] * innerdeviation).ToString("0.000000", CultureInfo.InvariantCulture);
			}

			points[0] = DoDeviation(points[0], ref deviations);
			points[1] = DoDeviation(points[1], ref deviations);
			points[2] = DoDeviation(points[2], ref deviations);

			return $"({string.Join("| ", points)})";
		}

		static private double CalculateDeviation(double percentage)
		{
			double positive = random.NextDouble() * percentage;
			double negative = random.NextDouble() * percentage;
			return (positive > negative) ? positive + 1 : 1 - negative;
		}
	}
}
