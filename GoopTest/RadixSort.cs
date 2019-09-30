namespace Goop
{
    public static class RadixSort
    {
        public static unsafe float[] Sort(float[] array)
        {
            int numElements = array.Length;

            float[] result = new float[numElements];

            fixed (float* pinInput = array)
            fixed (float* pinOutput = result)
            {
                uint* original = (uint*)pinInput;
                uint* sorted = (uint*)pinOutput;

                // 3 histograms on the stack:
                const int HistogramSize = 2048;
                uint* b0 = stackalloc uint[HistogramSize * 3];
                uint* b1 = b0 + HistogramSize;
                uint* b2 = b1 + HistogramSize;

                // 1. parallel histogramming pass
                for (int i = 0; i < numElements; ++i)
                {
                    uint fi = FloatFlip(original[i]);
                    ++b0[_0(fi)];
                    ++b1[_1(fi)];
                    ++b2[_2(fi)];
                }

                // 2. Sum the histograms -- each histogram entry records the number of values preceding itself.
                for (int i = 1; i < HistogramSize; ++i)
                {
                    b0[i] += b0[i - 1];
                    b1[i] += b1[i - 1];
                    b2[i] += b2[i - 1];
                }

                // byte 0: floatflip entire value, read/write histogram, write out flipped
                for (int i = numElements - 1; i >= 0; --i)
                {
                    uint fi = FloatFlip(original[i]);
                    uint pos = _0(fi);
                    sorted[--b0[pos]] = fi;
                }

                // byte 1: read/write histogram, copy
                //   sorted -> array
                for (int i = numElements - 1; i >= 0; --i)
                {
                    uint si = sorted[i];
                    uint pos = _1(si);
                    original[--b1[pos]] = si;
                }

                // byte 2: read/write histogram, copy & flip out
                //   array -> sorted
                for (int i = numElements - 1; i >= 0; --i)
                {
                    uint ai = original[i];
                    uint pos = _2(ai);
                    sorted[--b2[pos]] = IFloatFlip(ai);
                }
            }

            return result;

            // ---- utils for accessing 11-bit quantities
            uint _0(uint x) => x & 0x7FF;
            uint _1(uint x) => x >> 11 & 0x7FF;
            uint _2(uint x) => x >> 22;
        }

        /// <summary>
        /// flip a float for sorting
        /// finds SIGN of fp number.
        /// if it's 1 (negative float), it flips all bits
        /// if it's 0 (positive float), it flips the sign only
        /// </summary>
        private static uint FloatFlip(uint f)
        {
            uint mask = (uint)-(f >> 31) | 0x80000000;
            return f ^ mask;
        }

        /// <summary>
        /// flip a float back (invert FloatFlip)
        /// signed was flipped from above, so:
        /// if sign is 1 (negative), it flips the sign bit back
        /// if sign is 0 (positive), it flips all bits back
        /// </summary>
        private static uint IFloatFlip(uint f)
        {
            uint mask = ((f >> 31) - 1) | 0x80000000;
            return f ^ mask;
        }
    }
}
