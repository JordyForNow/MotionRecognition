using System;
using System.Collections.Generic;
using System.Text;

namespace MotionRecognition
{

	public interface IEncogTrainer
	{

		public bool Run(ITrainContainer trainContainer);

	}
}
