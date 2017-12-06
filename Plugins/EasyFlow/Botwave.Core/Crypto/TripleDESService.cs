using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Botwave.Crypto
{
    /// <summary>
    /// TripleDES 密码服务实现类.
    /// </summary>
    public class TripleDESService : ICryptoService
    {
        private TripleDES tripleDES;
        private string key;
        private string iv;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public TripleDESService()
            : this("{A090CB24-AF38-4544-92F8-A5B9F1A36ABD}")
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="key">密钥.</param>
        public TripleDESService(string key)
            : this(key, "#$^%&&*Yisifhsfjsljfslhgosdshf26382837sdfjskhf97(*&(*")
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="key">密钥.</param>
        /// <param name="iv"></param>
        public TripleDESService(string key, string iv)
        {
            this.key = key;
            this.iv = iv;

            tripleDES = new TripleDESCryptoServiceProvider(); 
            tripleDES.Key = GetLegalKey();
            tripleDES.IV = GetLegalIV();
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
                    ICryptoTransform encryptor = tripleDES.CreateEncryptor();
                    CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();
                    ms.Close();
                    byte[] outputBytes = ms.ToArray();
                    return outputBytes;
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
                    ICryptoTransform encryptor = tripleDES.CreateEncryptor();
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
                    ICryptoTransform encryptor = tripleDES.CreateDecryptor();
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
                    ICryptoTransform decryptor = tripleDES.CreateDecryptor();
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
        /// 获得密钥.
        /// </summary>
        /// <returns>密钥.</returns>
        private byte[] GetLegalKey()
        {
            string result = key;
            tripleDES.GenerateKey();
            byte[] keyBytes = tripleDES.Key;
            int keyLength = keyBytes.Length;
            if (result.Length > keyLength)
                result = result.Substring(0, keyLength);
            else if (result.Length < keyLength)
                result = result.PadRight(keyLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(result);
        }

        /// <summary>
        /// 获得初始向量 IV.
        /// </summary>
        /// <returns>初试向量 IV.</returns>
        private byte[] GetLegalIV()
        {
            string result = iv;
            tripleDES.GenerateIV();
            byte[] ivBytes = tripleDES.IV;
            int ivLength = ivBytes.Length;
            if (result.Length > ivLength)
                result = result.Substring(0, ivLength);
            else if (result.Length < ivLength)
                result = result.PadRight(ivLength, ' ');
            return ASCIIEncoding.ASCII.GetBytes(result);
        }
    }
}
