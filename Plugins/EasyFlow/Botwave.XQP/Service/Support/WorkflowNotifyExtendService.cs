using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Support
{
    public class WorkflowNotifyExtendService : IWorkflowNotifyExtendService
    {

        #region IWorkflowNotifyExtendService 成员

        public IList<WorkflowNotifyActor> GetNotifyActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<WorkflowNotifyActor>("xqp_WorkflowNotifyActors_Select_Current", activityInstanceId);
        }

        public IList<WorkflowNotifyActor> GetNextNotifyActors(Guid activityInstanceId)
        {
            return IBatisMapper.Select<WorkflowNotifyActor>("xqp_WorkflowNotifyActors_Select_Next", activityInstanceId);
        }

        #endregion
    }
}
