namespace GoopBenchmark
{
    using BenchmarkDotNet.Attributes;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Allocate and return an array?
    /// Yield results?
    /// Use Linq?
    /// </summary>
    public class GetEnumerables
    {
        [Benchmark]
        public IEnumerable<int> Array1()
        {
            return new[] { 1 };
        }

        [Benchmark]
        public IEnumerable<int> Array2()
        {
            return new[] { 1, 2 };
        }

        [Benchmark]
        public IEnumerable<int> Lazy1()
        {
            yield return 1;
        }

        [Benchmark]
        public IEnumerable<int> Lazy2()
        {
            yield return 1;
            yield return 2;
        }

        [Benchmark]
        public IEnumerable<int> LinqRepeat1()
        {
            return Enumerable.Repeat(1, 1);
        }

        [Benchmark]
        public IEnumerable<int> LinqRepeat2()
        {
            return Enumerable.Repeat(1, 2);
        }

        public int Count { get; private set; }

        [Benchmark]
        public void LoopArray1()
        {
            foreach (var x in this.Array1())
            {
                ++this.Count;
            }
        }

        [Benchmark]
        public void LoopLazy1()
        {
            foreach (var x in this.Lazy1())
            {
                ++this.Count;
            }
        }

        [Benchmark]
        public void LoopLinqRepeat1()
        {
            foreach (var x in this.LinqRepeat1())
            {
                ++this.Count;
            }
        }

        [Benchmark]
        public void LoopArray2()
        {
            foreach (var x in this.Array2())
            {
                ++this.Count;
            }
        }

        [Benchmark]
        public void LoopLazy2()
        {
            foreach (var x in this.Lazy2())
            {
                ++this.Count;
            }
        }
    }
}
