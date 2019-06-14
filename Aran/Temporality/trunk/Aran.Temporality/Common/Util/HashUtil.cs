using System;
using System.Security.Cryptography;
using System.Text;

namespace Aran.Temporality.Common.Util
{
    public class HashUtil
    {
        private static readonly MD5 Md5 = new MD5CryptoServiceProvider();
        public static string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input)) return String.Empty;
            var checkSum = Md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            var result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
            return result;
        }
    }
}
