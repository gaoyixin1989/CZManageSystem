using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Crypto
{
    /// <summary>
    /// 加密解密管理类.
    /// </summary>
    public sealed class CryptoManager
    {
        public static readonly ICryptoService Service = new TripleDESService("{A090CB24-AF38-4544-92F8-A5B9F1A36ABD}");
    }
}
