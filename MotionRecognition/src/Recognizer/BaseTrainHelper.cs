using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MotionRecognition
{
	public static class BaseTrainHelper
	{

		// Helper function which returns the total number of files in a directory.
		public static int getFileCount(string dataDirectory)
		{
			// Get total number of '.csv' files inside Directory.
			return Directory.GetFiles(
				dataDirectory,
				"*.csv*",
				SearchOption.TopDirectoryOnly
			).Length;
		}

		// This helper function copies a 1D array into a 2D array.
		public static void Project1DInto2D(double[] source, ref double[][] dest, int index)
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
