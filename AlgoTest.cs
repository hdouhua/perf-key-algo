using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace perf_key_algo
{
    [MemoryDiagnoser]
    [MarkdownExporterAttribute.GitHub]
    [RankColumn]
    public class AlgoTest
    {
        [Benchmark]
        public void Test1()
        {
            // 4387721608c0138f61eb52ea27b53bae
            string key = "AGMzNmMyNTcxMDcxNDZkYWZSUlwBAwMGBgEPVgJSBQsFUglQVgNUA1UDBVIAVwAFVWQ4NTQ2ZmY0 MTIwNWRiZDA=";
            var bytes = Convert.FromBase64String(key);

            var md5 = Decrypt(ref bytes);
            Debug.WriteLine(md5);
        }

        [Benchmark]
        public void Test2()
        {
            // 4387721608c0138f61eb52ea27b53bae
            string key = "AlBWWQZVCwNXVF5WCARRCFUCVVYBAAADAFZSVAcCV1gBIgRjcz46Si5bL2AcIjVbQApbfk9qSwMr";
            var bytes = Convert.FromBase64String(key);

            var (botFlag, timestamp, md5) = Decrypt2(ref bytes);
            Debug.WriteLine("{0}, {1}, {2}", botFlag, timestamp, md5);
        }

        [Benchmark]
        public void Test3()
        {
            string key = "AgdZBFFWCAcJU1IFBwxXVwYOB1ZcDgBQCAEGVg0BVFIFEHpPdAt0NycXUGkAEEt3Rz8WDhYnNF5h";
            var bytes = Convert.FromBase64String(key);

            var (botFlag, timestamp, md5) = Decrypt3(ref bytes);
            Debug.WriteLine("{0}, {1}, {2}", botFlag, timestamp, md5);
            Debug.Assert("6a7e41697ed64dcd76f89aa022440d62" == md5, "decrypt failed !");
        }

        private static string Decrypt(ref byte[] bytes)
        {
            if (bytes.Length != 65)
            {
                throw new ArgumentException("wrong length");
            }

            var x = bytes[17..49];
            var y = bytes[1..17].Reverse().Concat(bytes[49..65]).ToArray();
            var z = Xor(x, y);
            return Encoding.ASCII.GetString(z);
        }

        private static (byte, long, string) Decrypt2(ref byte[] bytes)
        {
            const int PrefixLength = 33;
            var p2Length = bytes.Length - PrefixLength;
            if (p2Length > 0 && p2Length % 2 != 0)
            {
                throw new ArgumentException("wrong length");
            }

            var p2xEnd = PrefixLength + p2Length / 2;
            var x = bytes[PrefixLength..p2xEnd];
            var y = bytes[p2xEnd..];
            var p2 = Xor(x, y);
            var botFlag = p2[0];
            var timestamp = Encoding.ASCII.GetString(p2[1..]);

            //7a92b31cda790c35e6bea0c776eaef1f
            var p1 = Xor(bytes[1..PrefixLength], Utils.GetMd5Bytes(timestamp));
            return (botFlag, long.Parse(timestamp, System.Globalization.NumberStyles.HexNumber), Encoding.ASCII.GetString(p1));
        }

        private static (byte, long, string) Decrypt3(ref byte[] bytes)
        {
            const int PrefixLength = 33;
            var p2Length = bytes.Length - PrefixLength;
            if (p2Length > 0 && p2Length % 2 != 0)
            {
                throw new ArgumentException("wrong length");
            }

            var p2xEnd = PrefixLength + p2Length / 2;
            var x = bytes[PrefixLength..p2xEnd];
            var y = bytes[p2xEnd..];
            var p2 = Xor(x, y);
            var botFlag = p2[0];
            var timestamp = Encoding.ASCII.GetString(p2[1..]);

            var p1 = Xor(bytes[1..PrefixLength], Fill(ref timestamp));
            return (botFlag, long.Parse(timestamp, System.Globalization.NumberStyles.HexNumber), Encoding.ASCII.GetString(p1));
        }

        private static byte[] Fill(ref string timestamp, int targetLength = 32)
        {
            var bytes = Encoding.ASCII.GetBytes(timestamp);

            if (targetLength <= bytes.Length) return bytes;

            var newLength = (targetLength % bytes.Length == 0 ? targetLength / bytes.Length : targetLength / bytes.Length + 1) * bytes.Length;
            var newBytes = new byte[newLength];

            var i = 0;
            while (i < targetLength)
            {
                bytes.CopyTo(newBytes, i);
                i += bytes.Length;
            }

            return newBytes;
        }

        private static byte[] Xor(byte[] x, byte[] y)
        {
            var z = new byte[x.Length];
            for (var i = 0; i < x.Length; ++i)
            {
                z[i] = (byte)(x[i] ^ y[i]);
            }

            return z;
        }
    }
}