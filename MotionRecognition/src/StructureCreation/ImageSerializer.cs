//using System;
//using System.IO;
//using System.Text;

//namespace MotionRecognition
//{
//	public static class ImageSerializer
//	{
//		public static void Serialize(Motion3DImage obj, string filePath = "./serializedData")
//		{
//			if (File.Exists(filePath))
//				File.Delete(filePath);

//			int[,] data = obj.GetData();
//			int size = obj.size;

//			using (FileStream fs = File.Create(filePath))
//			{
//				// Write obj size to file.
//				AddText(fs, obj.size + Environment.NewLine);
//				// Write top BitModulator to file.
//				writeBitModulator(fs, ref data, ref size, Motion3DImage.Angle.TOP);
//				// Add seperator between top and side.
//				AddText(fs, "|" + Environment.NewLine);
//				// Add Write side BitModulator to file.
//				writeBitModulator(fs, ref data, ref size, Motion3DImage.Angle.SIDE);
//			}
//		}

//		public static Motion3DImage DeSerialize(string filePath = "./serializedData")
//		{
//			Motion3DImage image = null;

//			if (!File.Exists(filePath))
//				throw new FileNotFoundException();

//			// Index to determine what to read from file 0 = size, 1 = values for Top, 2 = values for side.
//			int index = 0;
//			string line;

//			StreamReader file = new StreamReader(filePath);

//			while ((line = file.ReadLine()) != null)
//			{
//				string[] values = line.Split(",");

//				switch (index)
//				{

//					case 0: // Get size from file.
//						image = new Motion3DImage(int.Parse(line));
//						index = 1;
//						break;

//					case 1: // If seperator, switch to index 2 and start filling side.
//						if (line == "|")
//						{
//							index = 2;
//							break;
//						}

//						// Get values for top BitModulator from file.
//						image.SetPosition(Motion3DImage.Angle.TOP, int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
//						break;
//					case 2: // Get values for side BitModulator from file.
//						image.SetPosition(Motion3DImage.Angle.SIDE, int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
//						break;
//				}
//			}

//			file.Close();

//			return image;
//		}

//		private static void writeBitModulator(FileStream fs, ref int[,] map, ref int size, Motion3DImage.Angle angle)
//		{
//			// Check every y-axis value.
//			for (uint y = 0; y < size; y++)
//			{
//				// Check every x-axis value.
//				for (uint x = 0; x < size; x++)
//				{
//					// If value is not empty, write it to file.
//					if (map[(angle == Motion3DImage.Angle.TOP) ? y : y + size, x] != 0)
//						AddText(fs, $"{y},{x},{map[(angle == Motion3DImage.Angle.TOP) ? y : y + size, x]} {Environment.NewLine}");
//				}
//			}
//		}

//		private static void AddText(FileStream fs, string value)
//		{
//			byte[] info = new UTF8Encoding(true).GetBytes(value);
//			fs.Write(info, 0, info.Length);
//		}
//	}
//}
