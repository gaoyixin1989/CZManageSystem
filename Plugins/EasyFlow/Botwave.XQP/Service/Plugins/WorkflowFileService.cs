using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Commons;

namespace Botwave.XQP.Service.Plugins
{
    public class WorkflowFileService : IWorkflowFileService
    {
        #region IWorkflowFileService 成员

        public string UploadFile(System.Web.HttpPostedFile httpFile)
        {
            return UploadHelper.Upload(httpFile);
        }

        #endregion
    }
}
