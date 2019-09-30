namespace GoopTest
{
    using Goop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class SortFloatsTest
    {
        [TestMethod]
        public void RadixSortFloats()
        {
            float[] values = GetFloats();
            float[] sorted = RadixSort.Sort(values);
            AssertSorted(sorted);
        }

        [TestMethod]
        public void ArraySortFloats()
        {
            float[] values = GetFloats();
            Array.Sort(values);
            AssertSorted(values);
        }

        [TestMethod]
        public void LinqSortFloats()
        {
            float[] values = GetFloats();
            var sorted = values.OrderBy(x => x).ToArray();
            AssertSorted(sorted);
        }

        private static float[] GetFloats()
        {
            return _Generate().ToArray();

            IEnumerable<float> _Generate()
            {
                Random r = new Random();
                for (int i = 0; i < 100000; ++i)
                {
                    double d = r.Next(-10000, 10001) * r.NextDouble();
                    yield return (float)d;
                }
            }
        }

        private static void AssertSorted(float[] values)
        {
            for (int i = 1; i < values.Length; ++i)
            {
                Assert.IsTrue(values[i - 1] <= values[i]);
            }
        }
    }
}
