using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程提醒用户类.
    /// </summary>
    public class WorkflowNotifyActor : Botwave.Workflow.Extension.Domain.NotifyActor
    {
        private int _operateType;
        private string _workItemTitle;
        private int _notifyType;

        /// <summary>
        /// 流程标题.
        /// </summary>
        public string WorkItemTitle
        {
            get { return _workItemTitle; }
            set { _workItemTitle = value; }
        }

        /// <summary>
        /// 流程步骤实例操作类型.
        /// </summary>
        public int OperateType
        {
            get { return _operateType; }
            set { _operateType = value; }
        }

        /// <summary>
        /// 当前提醒用户的指定流程步骤的流程提醒类型（短信提醒/邮件提醒）.
        /// 0 都禁止提醒(禁止邮件和短信提醒).
        /// 1 都允许提醒(默认).
        /// 2 启用邮件提醒.
        /// 3 启用短信提醒.
        /// (即：1、2表示可以邮件提醒；1、3表示可以短信提醒.)
        /// </summary>
        public int NotifyType
        {
            get { return _notifyType; }
            set { _notifyType = value; }
        }
    }
}
