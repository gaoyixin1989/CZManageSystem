using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Web.SessionState;

namespace Botwave.Web.HttpHandler
{
    /// <summary>
    /// 下载处理类.
    /// 传递参数为:
    ///    1. {download.ashx}?id={id};
    ///    2. {download.ashx}?path={path}displayName={displayName};
    ///         例如: {download.ashx}?path=contrib/workflow/res/templates/workflow.xml&amp;displayName=流程配置文件.
    /// <example>下载路径传递参数模式:
    ///    1. {download.ashx}?id={id};
    ///    2. {download.ashx}?path={path}displayName={displayName};
    ///         例如: {download.ashx}?path=contrib/workflow/res/templates/workflow.xml&amp;displayName=流程配置文件.
    /// </example>
    /// </summary>
    public class DownloadHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 下载文件不存在时的提示信息.
        /// </summary>
        protected const string FileNotExistsMessage = "下载文件不存在！";
        /// <summary>
        /// App_Data 目录.
        /// </summary>
        protected const string AppDataDirectory = "~/App_Data/";

        #region IHttpHandler 成员

        /// <summary>
        /// 可以再度使用.
        /// </summary>
        public virtual bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// 处理请求.
        /// </summary>
        /// <param name="context"></param>
        public virtual void ProcessRequest(HttpContext context)
        {
            string id = context.Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id))
            {
                // 获取附件的编号
                this.DownloadAttachment(context, id);
            }
            else
            {
                //下载直接指定路径的本地文件(在App_Data目录中)
                string path = context.Request.QueryString["path"];
                if (!String.IsNullOrEmpty(path))
                {
                    path = path.ToLower();
                    string displayName = context.Request.QueryString["displayName"];

                    string filePath = ((path.StartsWith("/") ? "~" : AppDataDirectory) + path);
                    filePath = context.Server.MapPath(filePath);
                    DownloadFile(context.Response, filePath, displayName);
                }
            }
        }

        #endregion

        /// <summary>
        /// 下载文件.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="filePath"></param>
        /// <param name="displayName"></param>
        protected static void DownloadFile(HttpResponse res, string filePath, string displayName)
        {
            DownloadFile(res, filePath, displayName, false);
        }

        /// <summary>
        /// 下载文件.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="filePath"></param>
        /// <param name="displayName"></param>
        /// <param name="shouldDelete"></param>
        protected static void DownloadFile(HttpResponse res, string filePath, string displayName, bool shouldDelete)
        {
            if (!File.Exists(filePath))
            {
                res.Write(FileNotExistsMessage);
            }
            else
            {
                FileInfo file = new FileInfo(filePath);
                if (String.IsNullOrEmpty(displayName))
                {
                    displayName = HttpUtility.UrlEncode(file.Name);
                }
                else
                {
                    displayName = HttpUtility.UrlEncode(displayName) + file.Extension;
                }

                res.Clear();
                res.AddHeader("Pragma", "public");
                res.AddHeader("Expires", "0");
                res.AddHeader("Cache-Control", "must-revalidate, post-check=0, pre-check=0");
                res.AddHeader("Content-Type", "application/force-download");
                res.AddHeader("Content-Type", "application/octet-stream");
                res.AddHeader("Content-Type", "application/download");
                res.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", displayName));
                res.AddHeader("Content-Transfer-Encoding", "binary");
                res.AddHeader("Content-Length", file.Length.ToString());

                res.WriteFile(filePath);
                res.Flush();

                if (shouldDelete)
                {
                    File.Delete(filePath);
                }
            }
        }

        /// <summary>
        /// 下载远程文件到本地.
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <param name="localPath"></param>
        protected static void DownloadToLocal(string fileUrl, string localPath)
        {
            WebClient downloadClient = new WebClient();
            downloadClient.DownloadFile(fileUrl, localPath);
            downloadClient.Dispose();
        }

        /// <summary>
        /// 下载指定编号的附件.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        protected virtual void DownloadAttachment(HttpContext context, string id)
        { }
    }
}
