﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MotionRecognition
{
    public class Motion3DImage : ISampledImage<JointMeasurement>
    {
        public enum Angle : byte
        {
            TOP = 0,
            SIDE = 1
        }

        // Size is the maximum field size of Top and Side: 2*(sizeˆsize)
        public readonly int size;
        // Top and Side are the 3D composited images.
        private BitModulator[,] top, side;

        public Motion3DImage(int _size = 500)
        {
            this.size = _size;
            top = new BitModulator[size, size];
            side = new BitModulator[size, size];
        }
        public Motion3DImage(ref List<Sample<JointMeasurement>> _table) : this()
        {
            CreateImageFromTable(ref _table);
        }

        public void SetPosition(Angle a, int x, int y, BitModulator bm)
        {
            switch (a)
            {
                case Angle.TOP:
                    top[x, y] = bm;
                    break;
                case Angle.SIDE:
                    side[x, y] = bm;
                    break;
            }
        }

        public BitModulator[,] GetBitModulator(Angle a)
        {
            switch (a)
            {
                case Angle.TOP:
                    return this.top;
                case Angle.SIDE:
                    return this.side;
                default:
                    return default(BitModulator[,]);
            }
        }

        #region PublicFunctions 
        // Create an 3DImage from a table of measurements
        public void CreateImageFromTable(ref List<Sample<JointMeasurement>> _sampleList)
        {
            // Find the current base range with the minimum and the maximum
            Vec3 vecMin = new Vec3();
            Vec3 vecMax = new Vec3();
            foreach (var s in _sampleList)
            {
                foreach (var m in s.sampleData)
                {
                    vecMin.x = m.pos.x < vecMin.x ? m.pos.x : vecMin.x;
                    vecMin.y = m.pos.y < vecMin.y ? m.pos.y : vecMin.y;
                    vecMin.z = m.pos.z < vecMin.z ? m.pos.z : vecMin.z;

                    vecMax.y = m.pos.y > vecMax.y ? m.pos.y : vecMax.y;
                    vecMax.x = m.pos.x > vecMax.x ? m.pos.x : vecMax.x;
                    vecMax.z = m.pos.z > vecMax.z ? m.pos.z : vecMax.z;
                }
            }

            // Remap all sample vectors to a map in a range from 0 -> 499 (500).
            int x, y, z;
            foreach (var sample in _sampleList)
            {
                for (int i = 0; i < sample.sampleData.Count; i++)
                {
                    x = (int)Math.Round(Remap(sample.sampleData[i].pos.x, vecMin.x, vecMax.x, 0, size - 1));
                    y = (int)Math.Round(Remap(sample.sampleData[i].pos.y, vecMin.y, vecMax.y, 0, size - 1));
                    z = (int)Math.Round(Remap(sample.sampleData[i].pos.z, vecMin.z, vecMax.z, 0, size - 1));
                    if (this.top[x, y] == null) this.top[x, y] = new BitModulator();
                    this.top[x, y].SetIndex(i, true);
                    if (this.side[z, y] == null) this.side[z, y] = new BitModulator();
                    this.side[z, y].SetIndex(i, true);
                }
            }
        }

        #endregion

        #region PrivateFunctions

        // This function remaps floats to a given range.
        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        #endregion

        #region BaseFunctions
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Motion3DImage)obj;

            if (size != other.size) return false;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (top[x, y] == null && other.top[x, y] == null) // Ff both cells are not set they are equal.
                    {
                        continue;
                    }
                    else if ((top[x, y] == null && other.top[x, y] != null) || (top[x, y] != null && other.top[x, y] == null)) // if either cell is not set they are not equal
                    {
                        return false;
                    }
                    else if (top[x, y].Equals(other.top[x, y])) // If equals they are equal.
                    {
                        continue;
                    }
                    else // The cells are not equal.
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        #endregion
    }
}
