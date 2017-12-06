using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 文件上传下载服务接口.
    /// </summary>
    public interface IWorkflowFileService
    {
        /// <summary>
        /// 上传指定 HTTP 文件到文件服务器，并返回上传文件所保存的新文件名.
        /// </summary>
        /// <param name="httpFile">要上传 HTTP 文件对象.</param>
        /// <returns>返回上传文件所保存的新文件名.</returns>
        string UploadFile(HttpPostedFile httpFile);
    }
}
