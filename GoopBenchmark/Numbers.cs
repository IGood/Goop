namespace GoopBenchmark
{
    using BenchmarkDotNet.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class Numbers
    {
        // From SourceOf.Net ...
        //
        // The standard CLR double.IsNaN() function is approximately 100 times slower than our own wrapper,
        // so please make sure to use DoubleUtil.IsNaN() in performance sensitive code.
        // PS item that tracks the CLR improvement is DevDiv Schedule : 26916.
        // IEEE 754 : If the argument is any value in the range 0x7ff0000000000001L through 0x7fffffffffffffffL 
        // or in the range 0xfff0000000000001L through 0xffffffffffffffffL, the result will be NaN.         
        public static bool IsNaN(double value)
        {
            NanUnion t = new NanUnion { DoubleValue = value };
            UInt64 exp = t.UintValue & 0xfff0000000000000;
            UInt64 man = t.UintValue & 0x000fffffffffffff;
            return (exp == 0x7ff0000000000000 || exp == 0xfff0000000000000) && (man != 0);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)] internal double DoubleValue;
            [FieldOffset(0)] internal UInt64 UintValue;
        }
    }

    public class NumbersUtilities
    {
        [Benchmark]
        [ArgumentsSource(nameof(Doubles))]
        public bool UsingClr(double value)
        {
            return double.IsNaN(value);
        }

        [Benchmark]
        [ArgumentsSource(nameof(Doubles))]
        public bool UsingCustom(double value)
        {
            return Numbers.IsNaN(value);
        }

        public IEnumerable<object[]> Doubles()
        {
            yield return new object[] { 1.0 };
            yield return new object[] { 2.0 };
            yield return new object[] { 4.0 };
            yield return new object[] { 10.0 };
            yield return new object[] { double.NaN };
            yield return new object[] { double.Epsilon };
        }
    }
}
