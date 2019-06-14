using System;
using System.Security;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

namespace Encryptor
{

    public class SigmaEncryptor
    {
        public static byte[] Encrypt(byte[] data, RSAParameters parameters)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportParameters(parameters);
            return provider.Encrypt(data, false);
        }


        public static byte[] Decrypt(byte[] data, RSAParameters parameters)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportParameters(parameters);
            return provider.Decrypt(data, false);
        }

        public static Byte[] CompressString(string value)
        {
            Byte[] byteArray = new byte[0];
            if (!string.IsNullOrEmpty(value))
            {
                byteArray = Encoding.UTF8.GetBytes(value);
                using (MemoryStream stream = new MemoryStream())
                {
                    using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress))
                    {
                        zip.Write(byteArray, 0, byteArray.Length);
                    }
                    byteArray = stream.ToArray();
                }
            }
            return byteArray;
        }

        public static string DecompressString(Byte[] value)
        {
            string resultString = string.Empty;
            if (value != null && value.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(value))
                using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress))
                using (StreamReader reader = new StreamReader(zip))
                {
                    resultString = reader.ReadToEnd();
                }
            }
            return resultString;
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }


        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        public static DateTime GetEncryptedDate(string _FileName)
        {
            if (System.Diagnostics.Debugger.IsAttached) return DateTime.Now;

            List<string> allLinesText = File.ReadAllLines(_FileName).ToList();

            string EncriptedText = allLinesText[0];
            string CompressedRsaKey = allLinesText[1];
            string DeCompressedRsaKey = SigmaEncryptor.DecompressString(SigmaEncryptor.StringToByteArray(CompressedRsaKey));


            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(DeCompressedRsaKey);


            byte[] todecrypt = Convert.FromBase64String(EncriptedText);
            byte[] decrypted = SigmaEncryptor.Decrypt(todecrypt, rsa2.ExportParameters(true));
            string decryptedstring = System.Text.Encoding.Unicode.GetString(decrypted);

            int y = Convert.ToInt32( decryptedstring.Substring(0, 4));
            int m = Convert.ToInt32( decryptedstring.Substring(4, 2));
            int d = Convert.ToInt32( decryptedstring.Substring(6, 2));

            return new DateTime(y, m,d);
        }

        public static DateTime GetEncryptedDate()
        {
            if (System.Diagnostics.Debugger.IsAttached) return DateTime.Now;

            string PathtoKeyFile = ArenaStatic.ArenaStaticProc.GetExecutingFolder() + @"\key.sgm";

            List<string> allLinesText = File.ReadAllLines(PathtoKeyFile).ToList();

            string EncriptedText = allLinesText[0];
            string CompressedRsaKey = allLinesText[1];
            string DeCompressedRsaKey = SigmaEncryptor.DecompressString(SigmaEncryptor.StringToByteArray(CompressedRsaKey));


            RSACryptoServiceProvider rsa2 = new RSACryptoServiceProvider();
            rsa2.FromXmlString(DeCompressedRsaKey);


            byte[] todecrypt = Convert.FromBase64String(EncriptedText);
            byte[] decrypted = SigmaEncryptor.Decrypt(todecrypt, rsa2.ExportParameters(true));
            string decryptedstring = System.Text.Encoding.Unicode.GetString(decrypted);

            int y = Convert.ToInt32(decryptedstring.Substring(0, 4));
            int m = Convert.ToInt32(decryptedstring.Substring(4, 2));
            int d = Convert.ToInt32(decryptedstring.Substring(6, 2));

            return new DateTime(y, m, d);
        }
    }
}
