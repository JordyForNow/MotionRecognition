namespace MotionRecognition
{
	public interface ICSVFilter : IDataFilter
	{
		// True means that the column needs to be interpreted.
		bool UseColumn(ref string[] row, uint index);
	}
}
