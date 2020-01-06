namespace MotionRecognition
{
	public interface CSVColumnFilter
	{
		// True means that the column needs to be interpreted.
		bool UseColumn(ref string[] row, uint index);
	}
	public class CSVEvenColumnsFilter : CSVColumnFilter
	{
		public bool UseColumn(ref string[] row, uint index)
		{
			bool b = index % 2 != 0;
			return b;
		}
	}
}
