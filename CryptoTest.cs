using BenchmarkDotNet.Attributes;

namespace perf_key_algo
{
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [RankColumn]
    public class CryptoTest
    {
        [Benchmark(Description = "md5 - string")]
        public void Test1()
        {
            var _ = Utils.GetMd5("hello world!");
        }

        [Benchmark(Description = "md5 - bytes")]
        public void Test2()
        {
            var _ = Utils.GetMd5Bytes("hello world!");
        }

        [Benchmark(Description = "sha1 - string")]
        public void Test3()
        {
            var _ = Utils.GetSha1("hello world!");
        }
        [Benchmark(Description = "sha1 - bytes")]
        public void Test4()
        {
            var _ = Utils.GetSha1Bytes("hello world!");
        }


    }
}