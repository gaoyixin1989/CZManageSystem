using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程文件上传服务的默认实现类.
    /// </summary>
    public class DefaultWorkflowFileService : IWorkflowFileService
    {
        #region IWorkflowFileService 成员

        /// <summary>
        /// 上传文件.
        /// </summary>
        /// <param name="httpFile"></param>
        /// <returns></returns>
        public string UploadFile(HttpPostedFile httpFile)
        {
            return string.Empty;
        }

        #endregion
    }
}
