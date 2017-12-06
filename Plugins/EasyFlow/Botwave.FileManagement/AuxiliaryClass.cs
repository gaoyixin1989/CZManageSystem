using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.FileManagement
{
    public static class AuxiliaryClass
    {
        public enum UploadType
        {
            /// <summary>
            /// 本地
            /// </summary>
            Localhost,
            /// <summary>
            /// 共享目录
            /// </summary>
            SharedDirectory,
            /// <summary>
            /// web上传
            /// </summary>
            WebDAV,
            /// <summary>
            /// FTP上传
            /// </summary>
            FTP
        }
    }
}
