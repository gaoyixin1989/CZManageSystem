using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace CZManageSystem.Core.FileManager
{
    /// <summary>
    /// WebDAV 方式的文件服务实现类.
    /// </summary>
    public class WebDAVFileService : IFileService
    {
        /// <summary>
        /// 错误信息.
        /// </summary>
        public string ErrorMessage = "";
        private CredentialCache myCredentialCache;
        private string url;
        private string username;
        private string password;
        private string urlwithoutdirectory;

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WebDAVFileService(string Url,string Directory, string userName, string password)
        {
            this.url = (Url.EndsWith("/") ? Url : Url + "/") + Directory;
            this.username = userName;
            this.password = password;
            this.urlwithoutdirectory = (Url.EndsWith("/") ? Url : Url + "/");
            if (string.IsNullOrEmpty(url))
                throw new ConfigurationErrorsException("WebDAV 文件服务需要提供文件服务器 URL 地址，属性 url.");
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
                this.ErrorMessage = exception.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 指定文件夾是否存在.
        /// </summary>
        /// <param name="fileName">要检查是否存在的文件夾.</param>
        /// <returns></returns>
        public bool Exist(string fileName)
        {
            this.ErrorMessage = "";
            string s = "<?xml version=\"1.0\"?><a:propfind xmlns:a=\"DAV:\"><a:prop><a:getcontentlength/></a:prop></a:propfind>";
            byte[] bytes = new ASCIIEncoding().GetBytes(s);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.urlwithoutdirectory + fileName);
            request.Credentials = new NetworkCredential(this.username, this.password);
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
        /// 新建文件夾
        /// </summary>
        /// <param name="flodername">文件夾名</param>
        /// <returns></returns>
        public bool MakeFileFloder(string flodername)
        {
            try
            {
                HttpWebRequest objRequest = (HttpWebRequest)HttpWebRequest.Create(this.urlwithoutdirectory + flodername);
                objRequest.Credentials = new NetworkCredential(this.username, this.password);
                objRequest.Method = "MKCOL";
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            }
            catch (WebException exception)
            {
                this.ErrorMessage = exception.Message;
                return false;
            }
            return true;
        }

        public bool Download(string file,string Filename)
        {
            try
            {
                //CredentialCache DLCredentialCache = new CredentialCache();
                //DLCredentialCache.Add(new Uri(url), "NTLM", new NetworkCredential(userName, password));
                WebRequest request = WebRequest.Create(file);
                request.Method = "GET";
                request.Timeout = System.Threading.Timeout.Infinite;
                request.Credentials = this.myCredentialCache;//CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                Stream inStream = response.GetResponseStream();
                var ms = new MemoryStream();
                using (response)
                {
                    var stream = response.GetResponseStream();
                    int k = 1024;
                    var buff = new byte[k];
                    while (k > 0)
                    {
                        k = stream.Read(buff, 0, 1024);
                        ms.Write(buff, 0, k);
                    }
                    ms.Flush();
                    ms.Seek(0L, SeekOrigin.Begin);//把指针移动到流的开头
                };
                var Response = System.Web.HttpContext.Current.Response;
                Response.ContentType = "application/octet-stream";//通知浏览器下载文件而不是打开
                Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(Filename, System.Text.Encoding.UTF8));
                Response.BinaryWrite(ms.ToArray());
                Response.Flush();
                Response.End();
                return true;
            }
            catch(Exception ex)
            {

            }
            return false;
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
