using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Web.SessionState;
using Botwave.Commons;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Web.HttpHandler
{
    /// <summary>
    /// 下载处理类.
    /// </summary>
    public class DownloadHandler : Botwave.Web.HttpHandler.DownloadHandler
    {
        /// <summary>
        /// 下载的临时文件保存的虚拟路径格式.
        /// </summary>
        private static readonly string LocalTempFileNamePattern = "~/" + GlobalSettings.Instance.TemporaryDir + "/{0}";

        protected override void DownloadAttachment(HttpContext context, string id)
        {
            Guid attachmentId = new Guid(id);
            DownloadAttachment(context, attachmentId);
        }

        /// <summary>
        /// 根据附件ID下载远程文件到本地.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attchmentId"></param>
        private static void DownloadAttachment(HttpContext context, Guid attchmentId)
        {
            Attachment attachment = Attachment.LoadById(attchmentId);
            if (attachment != null)
            {
                string filePath = attachment.FileName; //附件的FileName包含路径
                string displayName = attachment.Title; // +attachment.MimeType;

                // 判断是否本地文件，以"http://"开头的都认为非本地文件
                if (filePath.StartsWith("http://"))
                {
                    string fileName = filePath.Substring(filePath.LastIndexOf("/"));//纯粹的文件名(不含路径)
                    filePath = context.Server.MapPath(String.Format(LocalTempFileNamePattern, fileName));

                    //先下载到本地
                    DownloadToLocal(attachment.FileName, filePath);

                    DownloadFile(context.Response, filePath, displayName, true);
                }
                else
                {
                    //对本地文件，约定存储在App_Data中，路径为相对路径

                    //filePath = context.Server.MapPath(String.Format(LOCAL_FILEPATH_FORMAT, filePath));
                    DownloadFile(context.Response, filePath, displayName);
                }
            }
            else
            {
                context.Response.Write(FileNotExistsMessage);
            }
        }
    }
}
