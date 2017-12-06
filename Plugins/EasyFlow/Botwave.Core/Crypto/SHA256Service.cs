using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Botwave.Crypto
{
    /// <summary>
    /// SHA256 散列算法密码服务实现类.
    /// </summary>
    public class SHA256Service : ICryptoService
    {
        private SHA256 sha256;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public SHA256Service()
        {
            this.sha256 = SHA256Managed.Create();
        }

        #region ICryptoService 成员

        /// <summary>
        /// 加密指定字节数组.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] plainBytes)
        {
            return sha256.ComputeHash(plainBytes);
        }

        /// <summary>
        /// 加密指定字符串.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            byte[] data = sha256.ComputeHash(Encoding.Default.GetBytes(plainText));

            StringBuilder resultBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                resultBuilder.Append(data[i].ToString("x2"));
            return resultBuilder.ToString();
        }

        /// <summary>
        /// 不支持解密，返回参数值.
        /// </summary>
        /// <param name="cryptoBytes"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] cryptoBytes)
        {
            return cryptoBytes;
        }

        /// <summary>
        /// 不支持解密，返回参数值.
        /// </summary>
        /// <param name="cryptoText"></param>
        /// <returns></returns>
        public string Decrypt(string cryptoText)
        {
            return cryptoText;
        }

        #endregion
    }
}
