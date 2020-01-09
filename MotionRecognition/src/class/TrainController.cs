namespace MotionRecognition
{

	public static class TrainController<T> where T : IEncogTrainer
	{

		public static bool Train<T>(
			NetworkContainer NetContainer,
			ITrainContainer trainContainer,
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

			T.Run(trainContainer);

			return false;
		}
	}
}
