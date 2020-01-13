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
		public int CopyLines; // how many lines to just copy over.
		public int RemoveLines; // how many lines to skip and forget.
		public int MutationCount; // amount of mutations.
		public float DeviationPercentage; // int 0 - 100.
		public float InnerDeviationPercentage; // int 0 - 100 / innerdeviation means the random deviation after the total deviation.

		// only one of these needs to be filled, if both are filled then the file will take precedence.
		public string DataFile; // csv movement file to copy and mutate.
		public string DataFolder; // folder with csv movement files to use and mutate, please end with /.

		public string OutputFolder;

		public bool AlterInput; // Alter the input files to have dots instead of commas in the timestamp value.
	}

	public static class CsvManipulator
	{
		private static readonly Random random = new Random();

		public static void RunManipulator(ref CsvManipulatorSettings settings)
		{
			if (!Directory.Exists(settings.OutputFolder))
			{
				Directory.CreateDirectory(settings.OutputFolder); // Create output directory if necesary.
			}
			else
			{
				ClearOutputFolder(settings.OutputFolder); // Clear otherwise.
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
				// Mutate a folder with csv files
				Console.WriteLine($"From: {settings.DataFolder} to {settings.OutputFolder}");
				ChangeOriginalBatch(ref settings);
				MutateFolder(ref settings);
			}
			else
			{
				Console.WriteLine("You didn't select any input!");
			}

			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			Console.WriteLine(string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));
		}

		// Method to make sleight changes to the original file,
		// Currently only checks if the second character in the line is a comma and changes it to .
		// This is to make sure the CSV file doesnt break because the comma is the splitter
		private static void ChangeOriginalFile(ref CsvManipulatorSettings settings)
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

		// Gets all files in input directory and does the change function
		private static void ChangeOriginalBatch(ref CsvManipulatorSettings settings)
		{
			FileInfo[] fileInfo = new DirectoryInfo(settings.DataFolder).GetFiles();

			for (int i = 0; i < fileInfo.Length; i++)
			{
				settings.DataFile = fileInfo[i].FullName;
				ChangeOriginalFile(ref settings);
			}
		}

		// fileNamePrefix is only used in batch mutate
		private static void MutateFile(ref CsvManipulatorSettings settings, string fileNamePrefix = "")
		{
			// This variable stores the general deviation of each joint per mutated file (i.e. [OutputFile1][Joint1])
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


				// Copy line to new mutated files
				if (CPL > 0)
				{
					for (int i = 0; i < settings.MutationCount; i++)
					{
						using StreamWriter sw = File.AppendText(settings.OutputFolder + fileNamePrefix + "-" + i.ToString() + ".csv");
						sw.WriteLine(line);
						sw.Close();
					}
					CPL--;
					continue;
				}

				// Remove line
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
						{ // for each pair of Vector3 and Quarternion edit only Vector3 - data[j] = Vector3, data[j + 1] = Quaternion
							sb.Append($",{MutateVector(ref settings, data[j], j / 2, fileNamePrefix + i, ref deviations)},{data[j + 1]}");
						}

						File.AppendAllText(settings.OutputFolder + fileNamePrefix + "-" + i.ToString() + ".csv", sb.ToString() + Environment.NewLine);
					}
				}
			}
		}

		private static void MutateFolder(ref CsvManipulatorSettings settings)
		{
			FileInfo[] fileInfo = new DirectoryInfo(settings.DataFolder).GetFiles();

			for (int i = 0; i < fileInfo.Length; i++) // For each input do settings.MutationCount mutations
			{
				Console.WriteLine($"Working on {fileInfo[i].Name}");
				settings.DataFile = fileInfo[i].FullName;
				MutateFile(ref settings, i.ToString());
			}
		}

		// Extra method to change a char at specified position
		private static void ReplaceInString(ref string original, int pos, char replacement)
		{
			StringBuilder sb = new StringBuilder(original);
			sb[pos] = replacement;
			original = sb.ToString();
		}

		private static void ClearOutputFolder(string folder)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(folder);

			foreach (FileInfo file in directoryInfo.GetFiles())
			{
				file.Delete();
			}
		}

		private static string MutateVector(ref CsvManipulatorSettings settings, string vector, int col, string name, ref Dictionary<string, List<double>> deviations)
		{
			if (!deviations.ContainsKey(name)) // Creates a new List for the joint deviations
			{
				deviations.Add(name, new List<double>());
			}

			if (deviations[name].Count - 1 <= col) // Create new deviation for the joint
			{
				deviations[name].Add(CalculateDeviation(settings.DeviationPercentage));
			}

			var innerdeviation = CalculateDeviation(settings.InnerDeviationPercentage); // Inner deviation is the deviation after the general deviation of the whole joint path

			var points = vector.Substring(1, vector.Length - 2).Split("| ");

			// Method for doing the calculation and rewriting it to the proper string length
			string DoDeviation(string data, ref Dictionary<string, List<double>> deviations)
			{
				return (float.Parse(data, CultureInfo.InvariantCulture) * deviations[name][col] * innerdeviation).ToString("0.000000", CultureInfo.InvariantCulture); // InvariantCulture is used to properly read and write the float data
			}

			points[0] = DoDeviation(points[0], ref deviations);
			points[1] = DoDeviation(points[1], ref deviations);
			points[2] = DoDeviation(points[2], ref deviations);

			return $"({string.Join("| ", points)})";
		}

		// Calculates a random deviation depending on the 
		private static double CalculateDeviation(double percentage)
		{
			double positive = random.NextDouble() * percentage;
			double negative = random.NextDouble() * percentage;
			return 1.0d + ((positive > negative) ? positive : -negative);
		}
	}
}
