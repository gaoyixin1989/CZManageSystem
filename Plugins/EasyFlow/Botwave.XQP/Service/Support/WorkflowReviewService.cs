using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Entities;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 待阅服务实现类.
    /// </summary>
    public class WorkflowReviewService : IWorkflowReviewService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowReviewService));

        private IActivityDefinitionService activityDefinitionService;
        private IActivityService activityService;

        public IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }

        public IActivityService ActivityService
        {
            set { activityService = value; }
        }

        #region IWorkflowReviewService 成员

        public bool PendingReview(WorkflowProfile profile, Guid activityInstanceId, Guid workflowId, string workflowTitle, BasicUser sender)
        {
            if (profile == null || profile.IsReview == false)
                return false;

            string senderName = sender.UserName;
            string senderRealName = sender.RealName;

            // 消息内容.
            string message = profile.ReviewNotifyMessage;
            if (!string.IsNullOrEmpty(message))
            {
                message = message.ToLower().Replace("#title#", workflowTitle).Replace("#from#", senderRealName);
            }

            // 为每个下行步骤设置抄送人(待阅人)
            IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);
            if (nextActivities == null || nextActivities.Count == 0)
            {
                log.Error("未能找到下行步骤列表(发送给当前步骤). ActivityInstanceId:" + activityInstanceId.ToString());
                // 发送给当前步骤.
                return false;
            }

            IDictionary<Guid, string> sended = new Dictionary<Guid, string>();
            foreach (ActivityInstance item in nextActivities)
            {
                Guid nextActivityId = item.ActivityId;
                if (sended.ContainsKey(nextActivityId))
                    continue;
                ToReview.OnPendingReview(item.ActivityInstanceId, message, senderName, workflowId, nextActivityId);
                sended.Add(nextActivityId, string.Empty);
            }

            return true;
        }

        public bool PendingReview(WorkflowProfile profile, ActivityExecutionContext context, Guid workflowId, string workflowTitle, BasicUser sender)
        {
            if (profile == null || profile.IsReview == false)
                return false;

            Guid activityInstanceId = context.ActivityInstanceId;
            string senderName = sender.UserName;
            string senderRealName = sender.RealName;

            // 消息内容.
            string message = profile.ReviewNotifyMessage;
            if (!string.IsNullOrEmpty(message))
            {
                message = message.ToLower().Replace("#title#", workflowTitle).Replace("#from#", senderRealName);
            }

            // 为每个下行步骤设置抄送人(待阅人)
            IList<ActivityInstance> nextActivities = activityService.GetNextActivities(activityInstanceId);
            if (nextActivities == null || nextActivities.Count == 0)
            {
                // 一般出现这种情况，基本上时步骤合并或者会签的原因.
                log.Error("未能找到下行步骤列表(发送给当前步骤)，将取得下行步骤定义 ActivityId. ActivityInstanceId:" + activityInstanceId.ToString());
                IList<Guid> nextActivitiyIdList = new List<Guid>();
                if (context.ActivityAllocatees.Count > 0)
                {
                    foreach (Guid key in context.ActivityAllocatees.Keys)
                        nextActivitiyIdList.Add(key);
                }
                else
                {
                    nextActivitiyIdList = GetNextActivities(activityInstanceId);
                }
                return ToReview.OnPendingReview(activityInstanceId, message, senderName, workflowId, nextActivitiyIdList);
            }

            IDictionary<Guid, string> sended = new Dictionary<Guid, string>();
            foreach (ActivityInstance item in nextActivities)
            {
                Guid nextActivityId = item.ActivityId;
                if (sended.ContainsKey(nextActivityId))
                    continue;
                ToReview.OnPendingReview(item.ActivityInstanceId, message, senderName, workflowId, nextActivityId);
                sended.Add(nextActivityId, string.Empty);
            }

            return true;
        }

        #endregion

        /// <summary>
        /// 获取下行流程定义编号集合.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public IList<Guid> GetNextActivities(Guid activityInstanceId)
        {
            IList<ActivityDefinition> nextActivities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
            if (nextActivities != null && nextActivities.Count > 1)
            {
                ActivityDefinition currentActivity = activityDefinitionService.GetActivityDefinitionByInstanceId(activityInstanceId);
                for (int i = 0; i < nextActivities.Count; i++)
                {
                    if (nextActivities[i].ActivityId == currentActivity.ActivityId)
                    {
                        nextActivities.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (nextActivities == null || nextActivities.Count == 0)
                return new List<Guid>();

            IList<Guid> nextActivityID = new List<Guid>();
            foreach (ActivityDefinition item in nextActivities)
            {
                if (!nextActivityID.Contains(item.ActivityId))
                    nextActivityID.Add(item.ActivityId);
            }
            return nextActivityID;
        }
    }
}
