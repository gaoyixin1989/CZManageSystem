namespace Botwave.Easyflow.Commons
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class DESUtils
    {
        private static byte[] DESKey = new byte[] { 
            0x81, 0xbb, 0xa1, 0x6a, 0xf5, 0x87, 0x3b, 230, 0x59, 0x6a, 50, 100, 0x7f, 0x3a, 0x2a, 0xbb, 
            0x2b, 0x68, 0xe2, 0x5f, 4, 0xfb, 0xb8, 0x2d, 0x67, 0xb3, 0x56, 0x19, 0x4e, 0xb8, 0xbf, 0xdd
         };

        public static string DESDecrypt(string strSource)
        {
            return DESDecrypt(strSource, DESKey);
        }

        private static string DESDecrypt(string strSource, byte[] key)
        {
            SymmetricAlgorithm algorithm = Rijndael.Create();
            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.Zeros;
            ICryptoTransform transform = algorithm.CreateDecryptor();
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(strSource));
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2, Encoding.Unicode);
            return reader.ReadToEnd();
        }

        public static string DESEncrypt(string strSource)
        {
            return DESEncrypt(strSource, DESKey);
        }

        private static string DESEncrypt(string strSource, byte[] key)
        {
            SymmetricAlgorithm algorithm = Rijndael.Create();
            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.Zeros;
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, algorithm.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] bytes = Encoding.Unicode.GetBytes(strSource);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            return Convert.ToBase64String(stream.ToArray());
        }
    }
}

