using System;
using MotionRecognition;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            CSVLoader loader = new CSVLoader("data.csv");
            loader.LoadImage();
        }
    }
}
