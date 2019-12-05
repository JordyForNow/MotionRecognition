using System;
using System.IO;
using System.Linq;

namespace MotionRecognition
{
	public class CSVLoader : IDataLoader
	{
		private string path;
		public CSVLoader(string path)
		{
			this.path = path;
		}
		struct Table
		{

		}
		private Table parseFile(bool hasHeader = true)
		{
			var lines = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path));
			var rawData = lines.Select(line => line.Split(','))
					.Select(item =>
					{
						Console.WriteLine(item);
						return new
						{
						};
					});
			return new Table();
		}

		public MotionImage LoadImage()
		{
			MotionImage image = new MotionImage();
			Table t = parseFile();

			return image;
		}
	}
}
