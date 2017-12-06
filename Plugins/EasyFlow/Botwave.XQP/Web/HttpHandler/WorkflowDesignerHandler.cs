using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Spring.Context.Support;

namespace Botwave.XQP.Web.HttpHandler
{
    /// <summary>
    /// 流程可视化设计器获取流程定义的处理类.
    /// </summary>
    public class WorkflowDesignerHandler : IHttpHandler
    {
        private static IWorkflowDefinitionService workflowDefinitionService;
        private static IActivityDefinitionService activityDefinitionService;
        private static IActivitySetService activitySetService;

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static WorkflowDesignerHandler()
        {
            workflowDefinitionService = WebApplicationContext.Current["workflowDefinitionService"] as IWorkflowDefinitionService;
            activityDefinitionService = WebApplicationContext.Current["activityDefinitionService"] as IActivityDefinitionService;
            activitySetService = WebApplicationContext.Current["activitySetService"] as IActivitySetService;
        }
    
        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Clear();
            context.Response.ContentType = "text/xml";
            string key = context.Request.QueryString.Count > 0 ? context.Request.QueryString[0] : "";
            Guid workflowId = Guid.Empty;
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    workflowId = new Guid(key);
                }
                catch
                {
                    context.Response.Write("<null />");
                    context.Response.End();
                    return;
                }
            }
            if (workflowId == Guid.Empty) // 表示新增流程定义
            {
                context.Response.Write("<workflow name=\"流程示例\" owner=\"admin\"></workflow>");
            }
            else
            {
                string xml = Output(workflowId);
                if (string.IsNullOrEmpty(xml))
                    context.Response.Write("<null />");
                else
                    context.Response.Write(xml);
            }
            context.Response.End();
        }

        #endregion

        #region output xml

        protected static string Output(Guid workflowId)
        {
            WorkflowDefinition workflow = workflowDefinitionService.GetWorkflowDefinition(workflowId);
            IList<ActivityDefinition> activities = activityDefinitionService.GetSortedActivitiesByWorkflowId(workflowId);
            if (workflow == null || activities.Count == 0)
                return string.Empty;

            return Output(workflow, activities);
        }

        protected static string Output(WorkflowDefinition workflow, IList<ActivityDefinition> activities)
        {
            StringBuilder builder = new StringBuilder();

            // 输出流程节点.
            builder.AppendFormat("<workflow name=\"{0}\" owner=\"{1}\">", workflow.WorkflowName, workflow.Owner);

            // 流程步骤名称字典.
            IDictionary<Guid, string> dictActivityName = ToAcitvityNameDictionary(activities);

            // 输出流程步骤(活动).
            int index = 0;
            int columnIndex = 0;
            foreach (ActivityDefinition activity in activities)
            {
                Guid activityId = activity.ActivityId;
                int state = activity.State;
                string prevActivityName = "";
                string nextActivityName = "";
                if (columnIndex >= 5)
                    columnIndex = 0;
                int x = 0 + columnIndex * 130;
                int y = 0 + (index / 5) * 80;

                if (state == 0) // start-activity
                {
                    nextActivityName = GetActivityNames(dictActivityName, activity.NextActivitySetId);
                    builder.AppendFormat("<start-activity name=\"{0}\" nextActivity=\"{1}\" x=\"{2}\" y =\"{3}\" />", activity.ActivityName, nextActivityName, x, y);
                }
                else if (state == 2)    // end-activity
                {
                    prevActivityName = GetActivityNames(dictActivityName, activity.PrevActivitySetId);
                    builder.AppendFormat("<end-activity name=\"{0}\" prevActivity=\"{1}\" x=\"{2}\" y =\"{3}\" />", activity.ActivityName, prevActivityName, x, y);
                }
                else // activity
                {
                    prevActivityName = GetActivityNames(dictActivityName, activity.PrevActivitySetId);
                    nextActivityName = GetActivityNames(dictActivityName, activity.NextActivitySetId);

                    builder.AppendFormat("<activity name=\"{0}\" prevActivity=\"{1}\" nextActivity= \"{2}\" x=\"{3}\" y =\"{4}\" />", activity.ActivityName, prevActivityName, nextActivityName, x, y);
                }
                columnIndex++;
                index++;
            }
            builder.Append("</workflow>");
            return builder.ToString();
        }

        protected static IDictionary<Guid, string> ToAcitvityNameDictionary(IList<ActivityDefinition> activities)
        {
            IDictionary<Guid, string> dict = new Dictionary<Guid, string>();
            if (activities == null || activities.Count == 0)
                return dict;
            foreach (ActivityDefinition item in activities)
            {
                if (!dict.ContainsKey(item.ActivityId))
                    dict.Add(item.ActivityId, item.ActivityName);
            }
            return dict;
        }

        /// <summary>
        /// 获取活动名称字符串（以","隔开）.
        /// </summary>
        /// <param name="dictActivityName"></param>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        protected static string GetActivityNames(IDictionary<Guid, string> dictActivityName, Guid activitySetId)
        {
            if (activitySetId == Guid.Empty)
                return string.Empty;

            IList<Guid> activityIds = activitySetService.GetActivityIdSets(activitySetId);
            if (activityIds == null || activityIds.Count == 0)
                return string.Empty;

            return GetActivityNames(dictActivityName, activityIds);
        }

        /// <summary>
        /// 获取活动名称字符串（以","隔开）.
        /// </summary>
        /// <param name="dictActivityName"></param>
        /// <param name="activityIds"></param>
        /// <returns></returns>
        private static string GetActivityNames(IDictionary<Guid, string> dictActivityName, IList<Guid> activityIds)
        {
            StringBuilder nameBuilder = new StringBuilder();
            foreach (Guid activityId in activityIds)
            {
                if (dictActivityName.ContainsKey(activityId))
                {
                    nameBuilder.AppendFormat(",{0}", dictActivityName[activityId]);
                }
            }
            if (nameBuilder.Length > 1)
                nameBuilder.Remove(0, 1);
            return nameBuilder.ToString();
        }
        
        #endregion
    }
}
