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

namespace Botwave.XQP.Designer
{
    /// <summary>
    /// 流程可视化设计器获取流程定义的处理类.
    /// </summary>
    public class WorkflowDesignerHandler : IHttpHandler
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowDesignerHandler));

        private const string EmptyWorkflow = "<workflow name=\"流程示例\" owner=\"admin\"></workflow>";
        private static IActivitySetService activitySetService;

        /// <summary>
        /// 静态构造方法.
        /// </summary>
        static WorkflowDesignerHandler()
        {
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
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
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
                context.Response.Write(EmptyWorkflow);
            }
            else
            {
                string xml = null;
                try
                {
                    xml = Output(workflowId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    xml = null;
                }
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
            WorkflowComponent workflow = WorkflowComponent.GetWorkflow(workflowId);
            IList<WorkflowActivity> activities = WorkflowActivity.GetActivities(workflowId);

            if (workflow == null || activities.Count == 0)
                return string.Empty;

            return Output(workflow, activities, workflowId);
        }

        protected static string Output(WorkflowComponent workflow, IList<WorkflowActivity> activities, Guid workflowId)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = workflow.CreateNode(doc);

            // 流程步骤名称字典.
            IDictionary<Guid, string> dictActivityName = ToAcitvityNameDictionary(activities);
            IDictionary<Guid, AllocatorOption> activityAssignments = WorkflowActivity.GetAssignments(workflowId);

            // 输出流程步骤(活动).
            int index = 0;
            int columnIndex = 0;
            foreach (WorkflowActivity activity in activities)
            {
                Guid activityId = activity.ActivityId;
                string prevActivityName = "";
                string nextActivityName = "";
                if (columnIndex >= 5)
                    columnIndex = 0;
                int x = (activity.X <= -1 ? 0 + columnIndex * 130 : activity.X);
                int y = (activity.Y <= -1 ? 0 + (index / 5) * 80 : activity.Y);

                prevActivityName = GetActivityNames(dictActivityName, activity.PrevActivitySetId);
                nextActivityName = GetActivityNames(dictActivityName, activity.NextActivitySetId);

                activity.X = x;
                activity.Y = y;
                activity.PrevActivity = prevActivityName;
                activity.NextActivity = nextActivityName;

                XmlNode activityNode = activity.CreateNode(doc);
                if (activityAssignments.ContainsKey(activityId))
                {
                    AllocatorOption assignmentOption = activityAssignments[activityId];
                    if (!WorkflowActivity.IsEmptyAllocator(assignmentOption))
                    {
                        XmlNode assignmentNode = WorkflowActivity.CreateAllocatorNode(doc, assignmentOption, "assignmentAllocator");
                        activityNode.AppendChild(assignmentNode);
                    }
                }
                root.AppendChild(activityNode);

                columnIndex++;
                index++;
            }

            doc.AppendChild(root);
            return doc.OuterXml;
        }

        protected static IDictionary<Guid, string> ToAcitvityNameDictionary(IList<WorkflowActivity> activities)
        {
            IDictionary<Guid, string> dict = new Dictionary<Guid, string>();
            if (activities == null || activities.Count == 0)
                return dict;
            foreach (WorkflowActivity item in activities)
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
