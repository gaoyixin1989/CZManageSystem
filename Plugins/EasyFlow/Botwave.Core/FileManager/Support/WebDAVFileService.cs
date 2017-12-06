using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Botwave.FileManager.Support
{
    /// <summary>
    /// WebDAV 方式的文件服务实现类.
    /// </summary>
    public class WebDAVFileService : IFileService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WebDAVFileService));

        /// <summary>
        /// 错误信息.
        /// </summary>
        public string ErrorMessage = "";
        private CredentialCache myCredentialCache;
        private string url;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WebDAVFileService()
        {
            this.url = FileManagerHelper.Properties["url"];
            string userName = FileManagerHelper.Properties["userName"];
            string password = FileManagerHelper.Properties["password"];
            if (string.IsNullOrEmpty(url))
                throw new System.Configuration.ConfigurationErrorsException("WebDAV 文件服务需要提供文件服务器 URL 地址，属性 url.");
            this.url = (this.url.EndsWith("/") ? this.url : this.url + "/");

            this.myCredentialCache = new CredentialCache();
            if (string.IsNullOrEmpty(userName))
                this.myCredentialCache = (CredentialCache)CredentialCache.DefaultCredentials;
            else
                this.myCredentialCache.Add(new Uri(this.url), "NTLM", new NetworkCredential(userName, password));
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
            this.ErrorMessage = "";
            WebClient client = new WebClient();
            client.Credentials = this.myCredentialCache;
            int length = (int)inputStream.Length;
            byte[] buffer = new byte[length];
            inputStream.Read(buffer, 0, length);
            Stream stream = client.OpenWrite(this.url + fileName, "PUT");
            stream.Write(buffer, 0, length);
            try
            {
                stream.Close();
            }
            catch (WebException exception)
            {
                log.Error(exception);
                this.ErrorMessage = exception.Message;
                return false;
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
            this.ErrorMessage = "";
            string s = "<?xml version=\"1.0\"?><a:propfind xmlns:a=\"DAV:\"><a:prop><a:getcontentlength/></a:prop></a:propfind>";
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url + fileName);
            request.Credentials = this.myCredentialCache;
            request.Method = "PROPFIND";
            request.ContentType = "text/xml";
            request.Headers.Add("Depth", "0");
            request.Headers.Add("Translate: f");
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException exception)
            {
                log.Error(exception);
                if (exception.Message.IndexOf("404") != -1)
                {
                    return false;
                }
                this.ErrorMessage = exception.Message;
                throw exception;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// 删除指定文件.
        /// </summary>
        /// <param name="fileName">要删除的文件名.</param>
        /// <returns></returns>
        public bool Delete(string fileName)
        {
            this.ErrorMessage = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.url + fileName);
            request.Credentials = this.myCredentialCache;
            request.Method = "DELETE";
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException exception)
            {
                log.Error(exception);
                this.ErrorMessage = exception.Message;
                return false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return true;
        }

        public string UploadRootPath
        {
            get { return this.url; ; }
        }

        #endregion
    }
}
