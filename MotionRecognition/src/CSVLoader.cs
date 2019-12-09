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
			var rows = (hasHeader ? File.ReadAllLines(path).Skip(1) : File.ReadAllLines(path)).Select(line => line.Split(','));
			foreach(var row in rows)
			{
				Sample<Measurement> sample = new Sample<Measurement>();
				sample.timestamp = float.Parse(row[0]);
				for(uint i = 1; i < row.Count(); i++)
				{
					Measurement m = new Measurement { ;
				}
			}
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
