using BenchmarkDotNet.Running;

namespace perf_key_algo
{
    static class Program
    {
        public static void Main()
        {
            // var algo = new AlgoTest();
            // algo.Test1();
            // algo.Test2();
            // algo.Test3();
            BenchmarkRunner.Run<CryptoTest>();
        }
    }
}
