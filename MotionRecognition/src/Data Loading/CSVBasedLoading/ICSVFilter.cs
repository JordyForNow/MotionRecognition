namespace MotionRecognition
{
	public interface ICSVFilter : IDataFilter
	{
		// True means that the column at the given index should be used.
		bool UseColumn(uint columnIndex);
	}
}
