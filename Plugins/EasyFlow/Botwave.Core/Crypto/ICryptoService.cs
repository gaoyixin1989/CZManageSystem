using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Crypto
{
    /// <summary>
    /// 加密解密服务接口.
    /// </summary>
    public interface ICryptoService
    {
        /// <summary>
        /// 加密指定字节数组.
        /// </summary>
        /// <param name="plainBytes">字节数组明文.</param>
        /// <returns></returns>
        byte[] Encrypt(byte[] plainBytes);

        /// <summary>
        /// 加密指定字符串.
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        string Encrypt(string plainText);

        /// <summary>
        /// 解密指定字节数组.
        /// </summary>
        /// <param name="cryptoBytes"></param>
        /// <returns></returns>
        byte[] Decrypt(byte[] cryptoBytes);

        /// <summary>
        /// 解密指定字符串.
        /// </summary>
        /// <param name="cryptoText"></param>
        /// <returns></returns>
        string Decrypt(string cryptoText);
    }
}
