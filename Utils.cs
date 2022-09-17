using System;
using System.Text;
using System.Security.Cryptography;

namespace perf_key_algo
{
    public static class Utils
    {
        public static byte[] GetSha1Bytes(string input)
        {
            var r = new Byte[40];

            using (var sha1 = SHA1Managed.Create())
            {
                byte[] bytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(input));
                for (int i = 0; i < bytes.Length; i++)
                {
                    Encoding.ASCII.GetBytes(bytes[i].ToString("x2")).CopyTo(r, i * 2);
                }
            }

            return r;
        }

        public static string GetSha1(string input)
        {
            StringBuilder sb = new StringBuilder();
            using (var sha1 = SHA1Managed.Create())
            {
                byte[] bytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(input));
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
            }

            return sb.ToString();
        }

        public static byte[] GetMd5Bytes(string input)
        {
            var r = new Byte[32];

            using (var md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                for (int i = 0; i < bytes.Length; i++)
                {
                    Encoding.ASCII.GetBytes(bytes[i].ToString("x2")).CopyTo(r, i * 2);
                }
            }

            return r;
        }

        public static string GetMd5(string input)
        {
            StringBuilder sb = new StringBuilder();

            using (var md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
