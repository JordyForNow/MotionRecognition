using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
	public enum networkActions
	{
		TRAIN,
		PREDICT
	}

	// This class serves as a facade to handle the complete process of recognizing hand gestures.
	public class MotionRecognizer
	{

		private NetworkTrainer trainer;
		private NetworkPredictor predictor;

		private networkActions action;
		private string inputData;
		private string networkWeights;
		private string networkLayers;
		private int networkInputSize;
		private int CSVSize;

		private int[,,] dataset;

		public MotionRecognizer(networkActions _action,
								string _inputData,
								string _networkWeights,
								string _networkLayers,
								int _networkInputSize = 100,
								int _CSVSize = 21)
		{
			action = _action;
			inputData = _inputData;
			networkWeights = _networkWeights;
			networkLayers = _networkLayers;
			networkInputSize = _networkInputSize;
			CSVSize = _CSVSize;
			SetPathVariables();
		}

		public void Run()
		{
			switch(action)
			{
				case networkActions.TRAIN:
					if (!Directory.Exists(inputData))
						throw new FolderNotFoundException("Input data folder was not found.");

					int fileCount = Directory.GetFiles(inputData, "*.csv*", SearchOption.TopDirectoryOnly).Length;

					dataset = new int[fileCount, networkInputSize, networkInputSize * 2];

					DirectoryInfo inputDirectory = new DirectoryInfo(inputData);

					CSVLoader loader;
					List<Sample<JointMeasurement>> table;
					Motion3DImage image;
					int index = 0;

					foreach (var file in inputDirectory.GetFiles("*.csv"))
					{

						loader = new CSVLoader(file.FullName, CSVSize);

						table = loader.GetData();

						image = new Motion3DImage(100);
						image.CreateImageFromTable(ref table);

						Project2DInto3D(image.GetData(), index);
						index++;

					}

					break;
				case networkActions.PREDICT:
					if (!Regex.IsMatch(networkWeights, @"(\.h5$)"))
						throw new WrongFileTypeException("Wrong network weights location given.");

					if (!File.Exists(networkWeights))
						throw new FileNotFoundException("Network weights were not found.");

					if (!Regex.IsMatch(networkLayers, @"(\.json$)"))
						throw new WrongFileTypeException("Wrong network layers location given.");

					if (!File.Exists(networkLayers))
						throw new FileNotFoundException("Network layers were not found.");

					if (!Regex.IsMatch(inputData, @"(\.csv$)"))
						throw new WrongFileTypeException("Wrong network input given.");

					if (!File.Exists(inputData))
						throw new FileNotFoundException("Network input was not found.");

					predictor = new NetworkPredictor(
						_networkWeights: networkWeights,
						_networkLayers: networkLayers,
						_inputData: inputData);

					Console.WriteLine(predictor.Run());

					break;
			}
		}
		/*{
		 *	{
		 *		{0,0,0,0,0,,0 },
		 *		{ 0,0,0,0,}
		 *	},
		 *	{
		 *		{ },
		 *		{ }
		 *	}
		 *}
		 */

		public void Project2DInto3D(int[,] source, int index)
		{
			for (int i = 0; i < networkInputSize; i++)
			{
				for (int j = 0; j < networkInputSize * 2; j++)
				{
					dataset[index, i, j] = source[i, j];
				}
			}
		}

		private void SetPathVariables()
		{
			string path = "";
			
			// Check if current platform is windows.
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				// Get user directory.
				path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;

				if (Environment.OSVersion.Version.Major >= 6)
				{
					path = Directory.GetParent(path).ToString();
				}
				
				// Add Python36 folder to path.
				path += @"\AppData\Local\Programs\Python\Python36";
				Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);

				// Add python excecutable to path.
				path += @"\python.exe";
				Environment.SetEnvironmentVariable("PYTHONHOME", path, EnvironmentVariableTarget.Process);
			} else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				// Implement Linux.
				Console.WriteLine("Linux");
			} else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				// Implement MacOs.
				Console.WriteLine("OSX");
			}
		}

	}
}
