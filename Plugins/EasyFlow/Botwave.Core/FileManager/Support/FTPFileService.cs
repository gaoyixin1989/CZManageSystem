using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Botwave.FileManager.Support
{
    /// <summary>
    /// FTP 文件服务(未实现).
    /// </summary>
    public class FTPFileService : IFileService
    {
        #region IFileService 成员

        /// <summary>
        /// 将上传输入流保存为指定文件.
        /// </summary>
        /// <param name="inputStream">要保存的输入流.</param>
        /// <param name="fileName">保存文件名.</param>
        /// <returns></returns>
        public bool Upload(Stream inputStream, string fileName)
        {
            return false;
        }

        /// <summary>
        /// 指定文件是否存在.
        /// </summary>
        /// <param name="fileName">要检查是否存在的文件名.</param>
        /// <returns></returns>
        public bool Exist(string fileName)
        {
            return false;
        }

        /// <summary>
        /// 删除指定文件.
        /// </summary>
        /// <param name="fileName">要删除的文件名.</param>
        /// <returns></returns>
        public bool Delete(string fileName)
        {
            return false;
        }

        #endregion

        #region IFileService 成员

        public string UploadRootPath
        {
            get { return string.Empty; }
        }

        #endregion
    }
}
