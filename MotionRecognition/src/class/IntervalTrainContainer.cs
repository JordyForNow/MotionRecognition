using System.IO;

namespace MotionRecognition
{
	public class IntervalTrainContainer : ITrainContainer
	{
		public NetworkContainer netContainer;
		public double[][] dataset;
		public double[][] trainingAnswers;
		public string outputDirectory;
		public string outputName;

		public void Run(
				NetworkContainer _netContainer,
				string _correctTrainingData,
				string _incorrectTrainingData,
				string _outputDirectory,
				string _outputName)
		{
			verifyInput(
				_netContainer,
				_correctTrainingData,
				_incorrectTrainingData,
				_outputDirectory,
				_outputName);

			netContainer = _netContainer;
			outputDirectory = _outputDirectory;
			outputName = _outputName;

			int correctFileCount = getFileCount(_correctTrainingData);
			int incorrectFileCount = getFileCount(_incorrectTrainingData);

			dataset = new double[correctFileCount + incorrectFileCount][];
			trainingAnswers = new double[correctFileCount + incorrectFileCount][];

			// Compute correct training data.
			computeData(
				netContainer.networkInputSize,
				_correctTrainingData,
				ref dataset,
				ref trainingAnswers,
				1.0,
				0);

			// Compute incorrect training data.
			computeData(
				netContainer.networkInputSize,
				_incorrectTrainingData,
				ref dataset,
				ref trainingAnswers,
				0.0,
				correctFileCount);
		}

		private static void verifyInput(
				NetworkContainer netContainer,
				string correctTrainingData,
				string incorrectTrainingData,
				string outputDirectory,
				string outputName)
		{
			if (!Directory.Exists(correctTrainingData))
				throw new DirectoryNotFoundException("Correct input data directory was not found.");

			if (!Directory.Exists(incorrectTrainingData))
				throw new DirectoryNotFoundException("Incorrect input data directory was not found.");

			if (correctTrainingData == incorrectTrainingData)
				throw new DataCrossoverException("Correct and incorrect data point to the same directory.");

			if (!Directory.Exists(outputDirectory))
				throw new DirectoryNotFoundException("Output directory was not found.");

			if (File.Exists(outputDirectory + outputName + ".eg") && !netContainer.allowFileOverride)
				throw new FileAlreadyExistsException("The file: " + outputDirectory + outputName + ".eg already exists, set NetworkContainer.allowFileOverride to 'TRUE' to skip this check.");

			if (outputName == null)
				throw new NoParameterGivenException("No output name was given.");
		}

		private static int getFileCount(string dataDirectory)
		{
			// Get total number of '.csv' files inside Directory.
			return Directory.GetFiles(
				dataDirectory,
				"*.csv*",
				SearchOption.TopDirectoryOnly
			).Length;
		}

		private static void computeData(
			int networkInputSize,
			string inputData,
			ref double[][] outputData,
			ref double[][] outputAnswers,
			double outputValue,
			int index)
		{
			DirectoryInfo inputDirectory = new DirectoryInfo(inputData);

			CSVLoaderSettings settings;
			CSVLoader loader;
			ArrayCreator creator;

			foreach (var file in inputDirectory.GetFiles("*.csv"))
			{

				// Declare loader settings.
				settings = new CSVLoaderSettings
				{
					filepath = file.FullName,
					TrimLeft = 1,
					TrimRight = 0
				};

				// Generate loader.
				loader = new CSVLoader(settings);

				// Create array with ArrayCreator from CSVloader.
				creator = new ArrayCreator();
				Project1DInto2D(creator.CreateArray(
					loader.LoadData(), networkInputSize),
					ref outputData,
					index);

				// Set answer to given value.
				outputAnswers[index] = new[] { outputValue };
				index++;

			}
		}

		private static void Project1DInto2D(double[] source, ref double[][] dest, int index)
		{
			double[] temp = new double[source.Length];

			for (int i = 0; i < source.Length; i++)
			{
				temp[i] = source[i];
			}

			dest[index] = temp;
		}
	}
}
