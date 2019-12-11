namespace MotionRecognition
{
	// this the base class for a CSV file cell.
	// it has a parse function and a tostring function.
	public interface CSVCell
	{
		public bool parse(string input);
		public string ToString();
	}
}
