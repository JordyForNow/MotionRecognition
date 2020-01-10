using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace InputManipulation
{
	class CsvManipulator
	{
		public int copyLines = 1; // how many lines to just copy over
		public int removeLines = 1; // how many lines to skip and forget
		public int mutationCount = 10; // amount of mutations
		public float deviationPercentage = 0.05f; // int 0 - 100
		public float innerDeviationPercentage = 0.01f; // int 0 - 100 / innerdeviation means the random deviation after the total deviation

		// only one of these needs to be filled, if both are filled then the file will take precedence
		public string dataFile = ""; // csv movement file to copy and mutate
		public string dataFolder = "./CSV/"; // folder with csv movement files to use and mutate, please end with /

		public string outputFolder = "./mutated/";

		public bool AlterInput = true; // Alter the input files to have dots instead of commas in the timestamp value


		#region runtime varibales
		private Dictionary<string, List<double>> deviations = new Dictionary<string, List<double>>();

		private Random random = new Random();
		#endregion

		public void Run()
		{
			if (!Directory.Exists(outputFolder))
			{
				Directory.CreateDirectory(outputFolder);
			}

			ClearOutputFolder(outputFolder);
			System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
			stopwatch.Start();

			if (!string.IsNullOrEmpty(dataFile))
			{
				Console.WriteLine($"From: {dataFile} to {outputFolder}");
				ChangeOriginalFile(dataFile);
				MutateFile(dataFile, outputFolder);
			}
			else if (!string.IsNullOrEmpty(dataFolder))
			{
				Console.WriteLine($"From: {dataFolder} to {outputFolder}");
				ChangeOriginalBatch(dataFolder);
				MutateFolder(dataFolder, outputFolder);
			}

			stopwatch.Stop();
			TimeSpan ts = stopwatch.Elapsed;
			Console.WriteLine(String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10));

		}

		private void ChangeOriginalFile(string filePath)
		{
			if (!AlterInput) return;

			var lines = File.ReadAllText(filePath).Split(Environment.NewLine);

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

			File.WriteAllText(filePath, string.Join(Environment.NewLine, lines));
		}

		private void ChangeOriginalBatch(string path)
		{
			FileInfo[] di = new DirectoryInfo(path).GetFiles();

			for (int i = 0; i < di.Length; i++)
			{
				ChangeOriginalFile(di[i].FullName);
			}
		}

		private void MutateFile(string filePath, string outputFolder, string fileNamePrefix = "")
		{
			// Local variable to store the amount of lines to copy over
			int CPL = copyLines;

			// Local variable to store the amount of lines to remove
			int RML = removeLines;

			StreamReader reader = new StreamReader(filePath);
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
					for (int i = 0; i < mutationCount; i++)
					{
						using StreamWriter sw = File.AppendText(outputFolder + fileNamePrefix + "-" + i.ToString() + ".csv");
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
					for (int i = 0; i < mutationCount; i++)
					{
						StringBuilder sb = new StringBuilder(data[0]);

						for (int j = 1; j < data.Length; j += 2)
						{ // for each pair of Vector3 and Quarternion edit the Vector 3
							sb.Append($",{MutateVector(data[j], j / 2, fileNamePrefix + i)},{ data[j + 1]}");
						}

						File.AppendAllText(outputFolder + fileNamePrefix + "-" + i.ToString() + ".csv", sb.ToString() + Environment.NewLine);
					}
				}
			}
		}

		private void MutateFolder(string path, string outputFolder)
		{
			FileInfo[] di = new DirectoryInfo(path).GetFiles();

			for (int i = 0; i < di.Length; i++)
			{
				Console.WriteLine($"Working on {di[i].Name}");
				MutateFile(di[i].FullName, outputFolder, i.ToString());
			}
		}

		private void ReplaceInString(ref string original, int pos, char replacement)
		{
			StringBuilder sb = new StringBuilder(original);
			sb[pos] = replacement;
			original = sb.ToString();
		}

		private void ClearOutputFolder(string folder)
		{
			DirectoryInfo di = new DirectoryInfo(folder);

			foreach (FileInfo file in di.GetFiles())
			{
				file.Delete();
			}
		}

		private string MutateVector(string vector, int col, string name)
		{
			if (!deviations.ContainsKey(name))
			{
				deviations.Add(name, new List<double>());
				deviations[name].Add(CalculateDeviation(deviationPercentage));
			}
			else
			{
				deviations[name].Add(CalculateDeviation(deviationPercentage));
			}

			var innerdeviation = CalculateDeviation(innerDeviationPercentage);

			var points = vector.Substring(1, vector.Length - 2).Split("| ");

			string DoDeviation(string data)
			{
				return (float.Parse(data, CultureInfo.InvariantCulture) * deviations[name][col] * innerdeviation).ToString("0.000000", CultureInfo.InvariantCulture);
			}

			points[0] = DoDeviation(points[0]);
			points[1] = DoDeviation(points[1]);
			points[2] = DoDeviation(points[2]);

			return $"({string.Join("| ", points)})";
		}

		private double CalculateDeviation(double percentage)
		{
			double positive = random.NextDouble() * percentage;
			double negative = random.NextDouble() * percentage;
			return (positive > negative) ? positive + 1 : 1 - negative;
		}
	}
}
