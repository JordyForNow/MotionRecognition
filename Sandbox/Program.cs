using MotionRecognition;

namespace Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{

			MotionRecognizer recognizer = new MotionRecognizer(
				_action: networkActions.TRAIN,
				_correctTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\CorrectData\",
				_incorrectTrainingData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectData\",
				_outputDirectory: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\",
				_outputName: @"modelOne",
				_allowFileOverride: true,
				_verbose: true
			);

			recognizer.Run();

			//recognizer = new MotionRecognizer(
			//	_action: networkActions.PREDICT,
			//	_trainedNetwork: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\DataOut\modelOne.eg",
			//	_predictData: @"C:\Users\Jordy\Documents\KBS-SE3_VR-Rehabilitation-Data\Sandbox\IncorrectData\Move_13.csv",
			//	_verbose: true
			//);

			//recognizer.Run();

		}
	}
}
