using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程文件服务的空实现类.
    /// </summary>
    public class EmptyWorkflowFileService : IWorkflowFileService
    {
        #region IWorkflowFileService 成员

        /// <summary>
        /// 上传文件.
        /// </summary>
        /// <param name="httpFile"></param>
        /// <returns></returns>
        public string UploadFile(System.Web.HttpPostedFile httpFile)
        {
            return httpFile.FileName;
        }

        #endregion
    }
}
