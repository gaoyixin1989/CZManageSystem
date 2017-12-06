using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程公告服务接口.
    /// </summary>
    public interface IWorkflowNoticeService
    {
        /// <summary>
        /// 在指定页面弹出指定流程的公告信息.
        /// </summary>
        /// <param name="ownerPager"></param>
        /// <param name="workflowName"></param>
        /// <param name="isDirectOutput">是否直接输出弹出公告窗口的脚本.</param>
        void PopupWorkflowNotices(Page ownerPager, string workflowName, bool isDirectOutput);
    }
}
