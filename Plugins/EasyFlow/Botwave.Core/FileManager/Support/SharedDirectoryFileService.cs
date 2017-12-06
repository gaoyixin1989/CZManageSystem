using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Botwave.FileManager.Support
{
    /// <summary>
    /// 共享目录方式的文件服务实现类.
    /// </summary>
    public class SharedDirectoryFileService : IFileService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SharedDirectoryFileService));
        private string sharedDirectory;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public SharedDirectoryFileService()
            : this(FileManagerHelper.Properties["path"])
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="saveDirectory">保存目录.</param>
        public SharedDirectoryFileService(string saveDirectory)
        {
            this.sharedDirectory = saveDirectory;
            if (string.IsNullOrEmpty(sharedDirectory))
                throw new System.Configuration.ConfigurationErrorsException("使用 SharedDirectory 文件服务需要提供文件存储路径，属性 path。");

            if (!this.sharedDirectory.Contains(":/"))
            {
                if (HttpContext.Current != null)
                {
                    if (this.sharedDirectory.StartsWith("/"))
                        this.sharedDirectory = "~" + this.sharedDirectory;
                    else if (!this.sharedDirectory.StartsWith("~/"))
                        this.sharedDirectory = "~/" + this.sharedDirectory;
                    this.sharedDirectory = HttpContext.Current.Server.MapPath(this.sharedDirectory);
                }
                else
                {
                    this.sharedDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.sharedDirectory);
                }
            }
            this.sharedDirectory = (this.sharedDirectory.EndsWith("\\") ? this.sharedDirectory : this.sharedDirectory + "\\");
        }

        #region IFileService 成员

        /// <summary>
        /// 将上传输入流保存为指定文件.
        /// </summary>
        /// <param name="inputStream">要保存的输入流.</param>
        /// <param name="fileName">保存文件名.</param>
        /// <returns></returns>
        public bool Upload(Stream inputStream, string fileName)
        {
            if (inputStream == null || string.IsNullOrEmpty(fileName))
                return false;

            fileName = Path.Combine(this.sharedDirectory, fileName);
            using (FileStream writer = File.Create(fileName))
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    while (inputStream.Read(buffer, 0, buffer.Length) > 0)
                    {
                        writer.Write(buffer, 0, buffer.Length);
                    }
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    if (writer != null)
                        writer.Close();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 指定文件是否存在.
        /// </summary>
        /// <param name="fileName">要检查是否存在的文件名.</param>
        /// <returns></returns>
        public bool Exist(string fileName)
        {
            fileName = Path.Combine(this.sharedDirectory, fileName);
            return File.Exists(fileName);
        }

        /// <summary>
        /// 删除指定文件.
        /// </summary>
        /// <param name="fileName">要删除的文件名.</param>
        /// <returns></returns>
        public bool Delete(string fileName)
        {
            fileName = Path.Combine(this.sharedDirectory, fileName);
            if (File.Exists(fileName))
            {
                try
                {
                    File.Delete(fileName);
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    return false;
                }
            }
            return true;
        }

        public string UploadRootPath
        {
            get { return this.sharedDirectory; ; }
        }

        #endregion
    }
}
