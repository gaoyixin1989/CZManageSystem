using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Botwave.Crypto
{
    /// <summary>
    /// SHA1 散列算法密码服务实现类.
    /// </summary>
    public class SHA1Service : ICryptoService
    {
        private SHA1 sha1;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public SHA1Service()
        {
            this.sha1 = new SHA1CryptoServiceProvider();
        }

        #region ICryptoService 成员

        /// <summary>
        /// 加密指定字节数组.
        /// </summary>
        /// <param name="plainBytes"></param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] plainBytes)
        {
            return sha1.ComputeHash(plainBytes);
        }

        /// <summary>
        /// 加密指定字符串.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public string Encrypt(string plainText)
        {
            byte[] data = sha1.ComputeHash(Encoding.Default.GetBytes(plainText));

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
