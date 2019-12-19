using MotionRecognition;
using System;


namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			string dataPath = @"../../../Data/";

			// Loader settings 
			CSVLoaderSettings settings = new CSVLoaderSettings();
			settings.filepath = dataPath + "data.csv";
			settings.TrimLeft = 1;
			settings.TrimRight = 0;

			CSVLoader loader = new CSVLoader(settings);

			// Create array wit ArrayCreator from CSVloader 
			ArrayCreator creator = new ArrayCreator();
			double[] test = creator.CreateArray(loader.LoadData(), 10);


			// FF printen natuurlijk
			foreach(double d in test)
			{
				Console.WriteLine(d);
			}

			Console.Read();



		}
	}
}
