using System;
using System.Collections.Generic;

namespace MotionRecognition
{
    public interface IImage<T>
    {
        void CreateImageFromTable(ref List<T> _table);
        bool Equals(object obj);
    }
}
