namespace GoopTest
{
    using Goop.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class EnumerableExTest
    {
        [DataTestMethod]
        [DataRow('b', 'a', 'r')]
        [DataRow('b', 'y', 'r')]
        [DataRow('b', 'y', 'a')]
        [DataRow('b', 'y', 'z')]
        public void BinarySearch(char first, char last, char findThis)
        {
            char[] alphabet = _Alphabet().ToArray();
            int expectedIndex = Array.BinarySearch(alphabet, findThis);
            int actualIndex = alphabet.BinarySearch(findThis);

            Assert.AreEqual(expectedIndex, actualIndex);

            IEnumerable<char> _Alphabet()
            {
                for (char c = first; c <= last; ++c)
                {
                    yield return c;
                }
            }
        }
    }
}
