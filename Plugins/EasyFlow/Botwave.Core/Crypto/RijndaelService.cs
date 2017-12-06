using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Botwave.Crypto
{
    /// <summary>
    /// Rijndael 算法密码服务实现类.
    /// </summary>
    public class RijndaelService : ICryptoService
    {
        private Rijndael rijndael;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public RijndaelService()
            : this("")
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="key"></param>
        public RijndaelService(string key)
            : this(key, "")
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public RijndaelService(string key, string iv)
        {
            rijndael = Rijndael.Create();
            rijndael.Key = GetLegalKey(key);
            rijndael.IV = GetLegalIV(iv);
        }

        #region ICryptoService 成员

        /// <summary>
        /// 加密指定字节数组.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] plainBytes)
        {
            try
            {
                byte[] inputBytes = plainBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    ICryptoTransform encryptor = rijndael.CreateEncryptor();
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.FlushFinalBlock();
                        ms.Close();
                        byte[] outputBytes = ms.ToArray();
                        return outputBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("在文件加密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 加密指定字符串.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            try
            {
                byte[] inputBytes = UTF8Encoding.UTF8.GetBytes(plainText);
                using (MemoryStream stream = new MemoryStream())
                {
                    ICryptoTransform encryptor = rijndael.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(stream, encryptor, CryptoStreamMode.Write);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    stream.Close();
                    byte[] outputBytes = stream.ToArray();
                    return Convert.ToBase64String(outputBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("在加密过程中出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密指定字节数组.
        /// </summary>
        /// <param name="cryptoBytes"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] cryptoBytes)
        {
            try
            {
                byte[] inputBytes = cryptoBytes;
                using (MemoryStream ms = new MemoryStream(inputBytes, 0, inputBytes.Length))
                {
                    ICryptoTransform encryptor = rijndael.CreateDecryptor();
                    CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read);
                    StreamReader sr = new StreamReader(cs);
                    return UTF8Encoding.UTF8.GetBytes(sr.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("在文件解密的时候出现错误！错误提示： \n" + ex.Message);
            }
        }

        /// <summary>
        /// 解密指定字符串.
        /// </summary>
        /// <param name="cryptoText"></param>
        /// <returns></returns>
        public string Decrypt(string cryptoText)
        {
            try
            {
                byte[] inputBytes = Convert.FromBase64String(cryptoText);
                using (MemoryStream ms = new MemoryStream(inputBytes, 0, inputBytes.Length))
                {
                    ICryptoTransform decryptor = rijndael.CreateDecryptor();
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        StreamReader reader = new StreamReader(cs);
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("在解密过程中出现错误！错误提示： \n" + ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// 获取密钥.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private byte[] GetLegalKey(string key)
        {
            string result = key;
            rijndael.GenerateKey();
            byte[] keyBytes = rijndael.Key;
            int keyLength = keyBytes.Length;
            if (result.Length > keyLength)
                result = result.Substring(0, keyLength);
            else if (result.Length < keyLength)
                result = result.PadRight(keyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(result);
        }

        /// <summary>
        ///  获得初始向量 IV.
        /// </summary>
        /// <param name="iv"></param>
        /// <returns></returns>
        private byte[] GetLegalIV(string iv)
        {
            string result = iv;
            rijndael.GenerateIV();
            byte[] ivBytes = rijndael.IV;
            int ivLength = ivBytes.Length;
            if (result.Length > ivLength)
                result = result.Substring(0, ivLength);
            else if (result.Length < ivLength)
                result = result.PadRight(ivLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(result);
        }
    }
}
