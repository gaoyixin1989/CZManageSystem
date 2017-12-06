using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程公告服务的空实现类.
    /// </summary>
    public class EmptyWorkflowNoticeService : IWorkflowNoticeService
    {
        #region IWorkflowNoticeService 成员

        /// <summary>
        /// 弹出流程公告.
        /// </summary>
        /// <param name="ownerPager"></param>
        /// <param name="workflowName"></param>
        /// <param name="isDirectOutput"></param>
        public void PopupWorkflowNotices(Page ownerPager, string workflowName, bool isDirectOutput)
        {

        }

        #endregion
    }
}
