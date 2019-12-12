using System;
using System.Collections.Generic;

namespace MotionRecognition
{
    public interface ISampledImage<T>
    {
        void CreateImageFromTable(ref List<Sample<T>> _Table);
        bool Equals(object obj);
    }
}
