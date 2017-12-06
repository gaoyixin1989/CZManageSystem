using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Commons;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 流程公告服务.
    /// </summary>
    public class WorkflowNoticeService : IWorkflowNoticeService
    {
        private INoticeService noticeService;

        public INoticeService NoticeService
        {
            get { return noticeService; }
            set { noticeService = value; }
        }

        #region IWorkflowNoticeService 成员

        public void PopupWorkflowNotices(System.Web.UI.Page ownerPager, string workflowName, bool isDirectOutput)
        {
            IList<Notice> notices = noticeService.GetNotices(XQPHelper.EntityType_WorkflowNotice, workflowName);
            Notice.RenderPopupNotices(ownerPager, XQPHelper.Url_PopupNotice, notices, isDirectOutput);
        }

        #endregion
    }
}
