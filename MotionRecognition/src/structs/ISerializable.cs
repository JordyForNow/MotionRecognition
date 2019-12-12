using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
	public static class ImageSerializer 
	{
        public static void Serialize(Motion3DImage obj, string filePath = "./data")
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

			BitModulator[,] top = obj.GetBitModulator(Motion3DImage.Angle.TOP);
			BitModulator[,] side = obj.GetBitModulator(Motion3DImage.Angle.SIDE);
			int size = obj.size;

            using (FileStream fs = File.Create(filePath))
            {
                AddText(fs, obj.size + Environment.NewLine);
                writeBitModulator(fs, ref top, ref size);
                AddText(fs, "|" + Environment.NewLine);
                writeBitModulator(fs, ref side, ref size);
            }
        }

        public static Motion3DImage DeSerialize(string filePath = "./data")
        {
			Motion3DImage image = null;

            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException();

            //Index 0 = size, 1 = values for Top, 2 = values for Side
            int index = 0;
			int x = 0, y = 0;
            string line;

            StreamReader file = new StreamReader(filePath);

            while ((line = file.ReadLine()) != null)
            {
                string[] values = line.Split(",");
				

                switch (index)
                {
                    case 0:
                        image = new Motion3DImage(int.Parse(line));
                        index = 1;
                        break;
                    case 1:
                        //If seperator, switch to index 2 
                        if (line == "|")
                        {
                            index = 2;
                            break;
                        }
						x = int.Parse(values[0]); 
						y = int.Parse(values[1]);
						image.SetPosition(Motion3DImage.Angle.TOP, x, y, new BitModulator(values[2]));
                        break;
                    case 2:
						x = int.Parse(values[0]); 
						y = int.Parse(values[1]);
						image.SetPosition(Motion3DImage.Angle.SIDE, x, y, new BitModulator(values[2]));
                        break;
                }
            }

            file.Close();

            return image;
        }

        private static void writeBitModulator(FileStream fs, ref BitModulator[,] map, ref int Size)
        {
            for (uint x = 0; x < Size; x++)
            {
                for (uint y = 0; y < Size; y++)
                {
                    if (map[x, y] != null)
                        AddText(fs, $"{x},{y},{map[x, y].GetVal()} {Environment.NewLine}");
                }
            }
        }

        private static void AddText(FileStream fs, string Value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(Value);
            fs.Write(info, 0, info.Length);
        }


	}
}
