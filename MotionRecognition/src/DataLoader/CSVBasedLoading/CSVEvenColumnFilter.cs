namespace MotionRecognition
{
    public class CSVEvenColumnFilter : ICSVFilter
    {
        public bool Use(ref string[] row, uint index)
        {
            return index % 2 != 0;
        }
    }
}