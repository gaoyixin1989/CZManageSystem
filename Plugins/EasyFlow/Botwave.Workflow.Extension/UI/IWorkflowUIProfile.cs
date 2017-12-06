using System;
using System.Collections.Generic;
using System.Web;

namespace Botwave.Workflow.Extension.UI
{
    /// <summary>
    /// 流程界面个性化接口.
    /// </summary>
    public interface IWorkflowUIProfile
    {
        /// <summary>
        /// 生成指定处理人的处理意见 Html.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user">用户对象.</param>
        /// <returns></returns>
        string BuildHandlerOpinionHtml(HttpContext context, object user);
    }
}
