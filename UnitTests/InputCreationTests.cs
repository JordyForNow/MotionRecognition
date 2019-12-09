using MotionRecognition;
using NUnit.Framework;
using System.IO;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLoader()
        {
			CSVLoader ldr = new CSVLoader(Directory.GetCurrentDirectory() + "data.csv");
        }
    }
}
