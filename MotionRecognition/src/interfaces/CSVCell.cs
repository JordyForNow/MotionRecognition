namespace MotionRecognition
{
	// This the base class for a CSV file cell.
	// It has a parse function and a tostring function.
	public interface CSVCell
	{
		bool parse(string Input);
		string ToString();
	}
}
