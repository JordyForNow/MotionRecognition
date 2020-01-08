namespace MotionRecognition
{
    public class CSVEvenColumnsFilter : ICSVFilter
    {
        public bool UseColumn(ref string[] row, uint index)
        {
            bool b = index % 2 != 0;
            return b;
        }
    }
}