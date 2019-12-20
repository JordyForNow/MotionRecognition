using Encog;
using Encog.App.Analyst;
using Encog.App.Analyst.CSV.Normalize;
using Encog.App.Analyst.Script.Normalize;
using Encog.App.Analyst.Wizard;
using Encog.Util.CSV;
using MotionRecognition;
using System;
using System.IO;
using System.Text;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			//string dataPath = @"../../../Data/";

			//// Loader settings 
			//CSVLoaderSettings settings = new CSVLoaderSettings();
			//settings.filepath = dataPath + "data.csv";
			//settings.TrimLeft = 1;
			//settings.TrimRight = 0;

			//CSVLoader loader = new CSVLoader(settings);

			//// Create array wit ArrayCreator from CSVloader 
			//ArrayCreator creator = new ArrayCreator();
			//double[] test = creator.CreateArray(loader.LoadData(), 10);


			//// FF printen natuurlijk
			//foreach(double d in test)
			//{
			//	Console.WriteLine(d);
			//}

			//Console.Read();


			//MotionRecognizer recognizer = new MotionRecognizer(
			//	_action: networkActions.TRAIN,
			//	_correctTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectData\",
			//	_incorrectTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectData\",
			//	_outputDirectory: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\",
			//	_outputName: @"modelone",
			//	_allowFileOverride: true
			//	);

			//recognizer.Run();
			//vecx1, vecy1, vecz1, quat11, quat21, quat31, qua41,
			//string d = "”time”,";

			//for (int i = 0; i < 21; i++)
			//{
			//	d += "”vecx" + i + "”,”vecy" + i + "”,”vecz" + i + "”,”quat1" + i + "”,”quat2" + i + "”,”quat3" + i + "”,”quat4" + i + "”,";
			//}

			//Console.WriteLine(d);

			//var sourceFile = new FileInfo(@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectData\test.csv");
			//var targetFile = new FileInfo(@"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectData\testout.csv");

			//var analyst = new EncogAnalyst();
			//var wizard = new AnalystWizard(analyst);
			//bool _useHeaders = true;
			//wizard.Wizard(sourceFile, _useHeaders, AnalystFileFormat.DecpntComma);
			//var norm = new AnalystNormalizeCSV();
			//norm.Analyze(sourceFile, _useHeaders, CSVFormat.English, analyst);
			//norm.ProduceOutputHeaders = _useHeaders;
			//norm.Normalize(targetFile);

		}

		//public static void DumpFieldInfo(EncogAnalyst analyst)
		//{
		//	Console.WriteLine(@"Fields found in file:");
		//	foreach (AnalystField field in analyst.Script.Normalize.NormalizedFields)
		//	{
		//		var line = new StringBuilder();
		//		line.Append(field.Name);
		//		line.Append(",action=");
		//		line.Append(field.Action);
		//		line.Append(",min=");
		//		line.Append(field.ActualLow);
		//		line.Append(",max=");
		//		line.Append(field.ActualHigh);
		//		Console.WriteLine(line.ToString());
		//	}
		//}
	}
}
