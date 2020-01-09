namespace MotionRecognition
{
	public interface ICSVFilter
	{
		// True means that the column needs to be interpreted.
		bool Use(ref string[] row, uint index);
	}
}
