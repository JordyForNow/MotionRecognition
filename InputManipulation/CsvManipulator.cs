using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace InputManipulation
{
	public struct CSVManipulatorSettings
	{
		public int copyLines; // How many lines to just copy over.
		public int removeLines; // How many lines to skip and forget.
		public int mutationCount; // Amount of mutations.
		public float deviationPercentage; // Float 0 - 1.
		public float innerDeviationPercentage; // Float 0 - 1 / Innerdeviation means the random deviation after the total deviation.

		// Only one of these needs to be filled, if both are filled then the file will take precedence.
		public string dataFile; // Csv movement file to copy and mutate.
		public string dataFolder; // Folder with csv movement files to use and mutate, please end with /.

		public string outputFolder;

		public bool alterInput; // Alter the input files to have dots instead of commas in the timestamp value.

		public bool verbose;
	}

	public static class CSVManipulator
	{
		private static readonly Random random = new Random();

		public static void RunManipulator(ref CSVManipulatorSettings settings)
		{
			if (!Directory.Exists(settings.outputFolder))
			{
				Directory.CreateDirectory(settings.outputFolder); // Create output directory if necessary.
			}
			else
			{
				ClearOutputFolder(settings.outputFolder); // Clear otherwise.
			}

			if (!string.IsNullOrEmpty(settings.dataFile))
			{
				// Mutate a single csv file.
				if (settings.verbose)
					Console.WriteLine($"From: {settings.dataFile} to {settings.outputFolder}");
				
				ChangeOriginalFile(ref settings);
				MutateFile(ref settings);
			}
			else if (!string.IsNullOrEmpty(settings.dataFolder))
			{
				// Mutate a folder with csv files.
				if (settings.verbose)
					Console.WriteLine($"From: {settings.dataFolder} to {settings.outputFolder}");
				ChangeOriginalBatch(ref settings);
				MutateFolder(ref settings);
			}
			else
			{
				if (settings.verbose)
					Console.WriteLine("You didn't select any input!");
			}

			if (settings.verbose)
				Console.WriteLine("Finished manipulating.");
		}

		// Method to make sleight changes to the original file,
		// Currently only checks if the second character in the line is a comma and changes it to '.'
		// This is to make sure the CSV file doesnt break because the comma is the splitter.
		private static void ChangeOriginalFile(ref CSVManipulatorSettings settings)
		{
			if (!settings.alterInput) return;

			var lines = File.ReadAllText(settings.dataFile).Split(Environment.NewLine);

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

			File.WriteAllText(settings.dataFile, string.Join(Environment.NewLine, lines));
		}

		// Gets all files in input directory and does the change function.
		private static void ChangeOriginalBatch(ref CSVManipulatorSettings settings)
		{
			FileInfo[] fileInfo = new DirectoryInfo(settings.dataFolder).GetFiles();

			for (int i = 0; i < fileInfo.Length; i++)
			{
				settings.dataFile = fileInfo[i].FullName;
				ChangeOriginalFile(ref settings);
			}
		}

		// FileNamePrefix is only used in batch mutate.
		private static void MutateFile(ref CSVManipulatorSettings settings, string fileNamePrefix = "")
		{
			// This variable stores the general deviation of each joint per mutated file (i.e. [OutputFile1][Joint1])
			Dictionary<string, List<double>> deviations = new Dictionary<string, List<double>>();

			// Local variable to store the amount of lines to copy over.
			int CPL = settings.copyLines;

			// Local variable to store the amount of lines to remove.
			int RML = settings.removeLines;

			StreamReader reader = new StreamReader(settings.dataFile);
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				if (line.IndexOf(',') == 1)
				{
					ReplaceInString(ref line, 1, '.');
				}

				// Copy line to new mutated files.
				if (CPL > 0)
				{
					for (int i = 0; i < settings.mutationCount; i++)
					{
						using StreamWriter sw = File.AppendText(settings.outputFolder + fileNamePrefix + "-" + i.ToString() + ".csv");
						sw.WriteLine(line);
						sw.Close();
					}
					CPL--;
					continue;
				}

				// Remove line.
				if (RML > 0)
				{
					RML--;
					continue;
				}

				string[] data = line.Split(',');
				if (data.Length > 1)
				{
					for (int i = 0; i < settings.mutationCount; i++)
					{
						StringBuilder sb = new StringBuilder(data[0]);

						for (int j = 1; j < data.Length; j += 2)
						{ // For each pair of Vector3 and Quarternion edit only Vector3 - data[j] = Vector3, data[j + 1] = Quaternion.
							sb.Append($",{MutateVector(ref settings, data[j], j / 2, fileNamePrefix + i, ref deviations)},{data[j + 1]}");
						}

						File.AppendAllText(settings.outputFolder + fileNamePrefix + "-" + i.ToString() + ".csv", sb.ToString() + Environment.NewLine);
					}
				}
			}
		}

		private static void MutateFolder(ref CSVManipulatorSettings settings)
		{
			FileInfo[] fileInfo = new DirectoryInfo(settings.dataFolder).GetFiles();

			for (int i = 0; i < fileInfo.Length; i++) // For each input do settings.MutationCount mutations.
			{
				if (settings.verbose)
					Console.WriteLine($"Working on {fileInfo[i].Name}");
				settings.dataFile = fileInfo[i].FullName;
				MutateFile(ref settings, i.ToString());
			}
		}

		// Extra method to change a char at specified position.
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

		private static string MutateVector(ref CSVManipulatorSettings settings, string vector, int col, string name, ref Dictionary<string, List<double>> deviations)
		{
			if (!deviations.ContainsKey(name)) // Creates a new List for the joint deviations.
			{
				deviations.Add(name, new List<double>());
			}

			if (deviations[name].Count - 1 <= col) // Create new deviation for the joint.
			{
				deviations[name].Add(CalculateDeviation(settings.deviationPercentage));
			}

			var innerdeviation = CalculateDeviation(settings.innerDeviationPercentage); // Inner deviation is the deviation after the general deviation of the whole joint path.

			var points = vector.Substring(1, vector.Length - 2).Split("| ");

			// Method for doing the calculation and rewriting it to the proper string length.
			string DoDeviation(string data, ref Dictionary<string, List<double>> deviations)
			{
				// InvariantCulture is used to properly read and write the float data.
				return (float.Parse(data, CultureInfo.InvariantCulture) * deviations[name][col] * innerdeviation).ToString("0.000000", CultureInfo.InvariantCulture);
			}

			points[0] = DoDeviation(points[0], ref deviations);
			points[1] = DoDeviation(points[1], ref deviations);
			points[2] = DoDeviation(points[2], ref deviations);

			return $"({string.Join("| ", points)})";
		}

		// Calculates a random deviation depending on the input percentage.
		private static double CalculateDeviation(double percentage)
		{
			double positive = random.NextDouble() * percentage;
			double negative = random.NextDouble() * percentage;
			return 1.0d + ((positive > negative) ? positive : -negative);
		}
	}
}
