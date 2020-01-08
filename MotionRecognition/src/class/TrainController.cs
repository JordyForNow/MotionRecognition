namespace MotionRecognition
{

	public static class TrainController
	{

		public static bool Train(
			NetworkContainer NetContainer,
			ITrainContainer trainContainer,
			IEncogTrainer trainer,
			string correctInputDirectory,
			string incorrectInputDirectory,
			string outputDirectory,
			string outputName)
		{

			trainContainer.Run(
				NetContainer,
				correctInputDirectory,
				incorrectInputDirectory,
				outputDirectory,
				outputName);

			trainer.Run(trainContainer);

			return false;
		}
	}
}
