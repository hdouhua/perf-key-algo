using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace perf_key_algo
{
    static class Program
    {
        public static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            // // debug
            // var algo = new AlgoTest();
            // algo.Test1();
            // algo.Test2();
            // algo.Test3();

            // benchmark
            BenchmarkRunner.Run<AlgoTest>();
            BenchmarkRunner.Run<CryptoTest>();
        }
    }
}
