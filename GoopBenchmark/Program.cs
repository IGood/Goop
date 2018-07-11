namespace GoopBenchmark
{
    using System;
    using BenchmarkDotNet.Running;

    class Program
    {
        public static int Indent { get; set; }

        static void Main(string[] args)
        {
            //*
            Benchmark<NumbersUtilities>();
            /*/
            var x = new EnumParsing();
            x.UsingMethodInfo();
            x.UsingMethodInfoCached();
            x.UsingDynamicInvokeDelegateCached();
            x.UsingCastedDelegateCachedSlowest();
            x.UsingCastedDelegateCachedSlow();
            x.UsingCastedDelegateCachedFast();
            x.UsingFrameworkMethodSlow();
            x.UsingFrameworkMethodFast();
            //*/
        }

        public static string ConsoleWriteLine(string message)
        {
            string @out = new string('\t', Indent) + message;

            /*
            Console.WriteLine(@out);
            //*/

            return @out;
        }

        private static void Benchmark<T>()
        {
            BenchmarkRunner.Run<T>();
        }
    }
}
