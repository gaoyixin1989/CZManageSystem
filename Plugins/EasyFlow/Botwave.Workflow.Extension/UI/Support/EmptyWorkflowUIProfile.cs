using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.UI.Support
{
    /// <summary>
    /// 流程界面个性化的空实现类.
    /// </summary>
    public class EmptyWorkflowUIProfile : IWorkflowUIProfile
    {
        #region IWorkflowUIProfile 成员

        public string BuildHandlerOpinionHtml(System.Web.HttpContext context, object user)
        {
            return string.Empty;
        }

        #endregion
    }
}
