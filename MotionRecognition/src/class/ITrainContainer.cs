using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{

	public interface ITrainContainer
	{

		public void Run(
				NetworkContainer container,
				string correctTrainingData,
				string incorrectTrainingData,
				string _outputDirectory,
				string _outputName);
	}
}
