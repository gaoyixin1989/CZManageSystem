using System;
using System.IO;
using System.Text;

namespace Botwave.FileManager
{
    /// <summary>
    /// 文件管理服务接口.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// 上传文件保存的根目录.
        /// </summary>
        string UploadRootPath { get; }

        /// <summary>
        /// 将上传输入流保存为指定文件.
        /// </summary>
        /// <param name="inputStream">要保存的输入流.</param>
        /// <param name="fileName">保存文件名.</param>
        /// <returns></returns>
        bool Upload(Stream inputStream, string fileName);

        /// <summary>
        /// 指定文件是否存在.
        /// </summary>
        /// <param name="fileName">要检查是否存在的文件名.</param>
        /// <returns></returns>
        bool Exist(string fileName);

        /// <summary>
        /// 删除指定文件.
        /// </summary>
        /// <param name="fileName">要删除的文件名.</param>
        /// <returns></returns>
        bool Delete(string fileName);
    }
}
