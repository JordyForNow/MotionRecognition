namespace MotionRecognition
{
    public class CSVEvenColumnFilter : ICSVFilter
    {
        // True means that the column at the given index should be used.
        public bool UseColumn(uint columnIndex)
        {
            bool b = columnIndex % 2 != 0;
            return b;
        }
    }
}