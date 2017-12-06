using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Botwave.Commons;
using Botwave.FileManager;

namespace Botwave.XQP.Commons
{
    /// <summary>
    /// 文件上传辅助类.
    /// </summary>
    public class UploadHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(UploadHelper));

        public static string UploadRootDirectory = "";

        public static string UploadDirectory = "";

        static UploadHelper()
        {
            UploadRootDirectory = FileManagerHelper.FileService.UploadRootPath;
            UploadDirectory = UploadRootDirectory;
        }

        /// <summary>
        /// 上传文件，返回保存的文件名.
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        public static string Upload(HttpPostedFile postedFile)
        {
            if (postedFile.ContentLength > 0)
            {
                string fileName = FileUtils.GetUniqueFileName(postedFile.FileName);
                if (FileManagerHelper.FileService.Upload(postedFile.InputStream, System.IO.Path.GetFileName(fileName)))
                {
                    return fileName;
                }
            }
            return null;
        }

        /// <summary>
        /// 上传文件到文件服务器.返回上传文件的字典(key:文件保存路径).
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static IDictionary<string, HttpPostedFile> Upload(HttpFileCollection files)
        {
            if (files == null || files.Count == 0)
                return new Dictionary<string, HttpPostedFile>();

            IDictionary<string, HttpPostedFile> results = new Dictionary<string, HttpPostedFile>();
            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];
                if (file.ContentLength > 0)
                {
                    string fileName = FileUtils.GetUniqueFileName(file.FileName);
                    if (FileManagerHelper.FileService.Upload(file.InputStream, System.IO.Path.GetFileName(fileName)))
                    {
                        results[fileName] = file;
                    }
                }
            }
            return results;
        }

        public static string Upload(string path, string fileName)
        {
            //上传文件到文件服务器
            if (path.Length > 0)
            {
                fileName = FileUtils.GetUniqueFileName(fileName);
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    if (FileManagerHelper.FileService.Upload(stream, fileName))
                    {
                        return fileName;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 上传指定文件流，并保存为指定文件名.
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Upload(Stream inputStream, string fileName)
        {
            return FileManagerHelper.FileService.Upload(inputStream, fileName);
        }

        /// <summary>
        /// 删除指定文件.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool Delete(string fileName)
        {
            return FileManagerHelper.FileService.Delete(fileName);
        }

        /// <summary>
        /// 从远程服务器上下载文件到本地临时目录.
        /// </summary>
        /// <param name="requestPath">请求下载的文件 URL.</param>
        /// <param name="saveDirectory">下载文件的保存目录.</param>
        /// <param name="fileName">下载文件的保存文件名.</param>
        /// <returns></returns>
        public static string Download(string requestPath, string saveDirectory, string fileName)
        {
            try
            {
                if (!Directory.Exists(saveDirectory))
                    Directory.CreateDirectory(saveDirectory);

                System.Net.WebClient client = new System.Net.WebClient();

                string path = Path.Combine(saveDirectory, fileName);
                if (File.Exists(path))
                    File.Delete(path);

                client.DownloadFile(requestPath, path);

                return path;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return null;
        }
    }
}
