using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Web.Controls
{
    /// <summary>
    /// 流程处理历史信息.
    /// </summary>
    public abstract class WorkflowProcessHistoryEditor : WorkflowControlBase
    {
        /// <summary>
        /// 转交表顶部.
        /// </summary>
        protected const string AssignmentHeader = "<th>转交人</th><th>被转交人</th><th>转交时间</th><th></th><th>转交信息</th>";
        /// <summary>
        /// 会签表顶部.
        /// </summary>
        protected const string CountersignedHeader = "<th colspan='2'>会签人</th><th>会签时间</th><th></th><th>会签信息</th>";

        #region properties

        protected IActivityService activityService;
        protected ITaskAssignService taskAssignService;
        protected ICountersignedService countersignedService;
        protected IUserService userService;
        private bool _displayCountersignedHeader = false;
        private string defaultReason = "提交工单";

        public IActivityService ActivityService
        {
            set { activityService = value; }
        }

        public ITaskAssignService TaskAssignService
        {
            set { taskAssignService = value; }
        }

        public ICountersignedService CountersignedService
        {
            set { countersignedService = value; }
        }

        public IUserService UserService
        {
            set { userService = value; }
        }

        public Guid WorkflowInstanceId
        {
            get { return (Guid)(ViewState["WorkflowInstanceId"]); }
            set { ViewState["WorkflowInstanceId"] = value; }
        }

        public bool DisplayCountersignedHeader
        {
            get { return _displayCountersignedHeader; }
            set { _displayCountersignedHeader = value; }
        }

        public string DefaultReason
        {
            set { defaultReason = value; }
        }
        #endregion

        public WorkflowProcessHistoryEditor()
        {
            Spring.Context.IApplicationContext context = Spring.Context.Support.WebApplicationContext.Current;
            if (context == null)
                return;
            this.activityService = context["activityService"] as IActivityService;
            this.taskAssignService = context["taskAssignService"] as ITaskAssignService;
            this.countersignedService = context["countersignedService"] as ICountersignedService;
            this.userService = context["userService"] as IUserService;
        }

        /// <summary>
        /// 初始化控件.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        public virtual void Initialize(Guid workflowInstanceId)
        {
            this.Initialize(string.Empty, workflowInstanceId);
        }

        /// <summary>
        /// 初始化控件.
        /// </summary>
        /// <param name="workflowName">流程名称.</param>
        /// <param name="workflowInstanceId">流程实例编号.</param>
        public virtual void Initialize(string workflowName, Guid workflowInstanceId)
        {
            this.WorkflowInstanceId = workflowInstanceId;
            base.Initialize(workflowName);
            if (this.Visible)
            {
                IList<Assignment> workflowAssignments = taskAssignService.GetAssignments(workflowInstanceId);   // 流程实例的全部转交任务.
                IList<CZActivityInstance> workflowActivities = CZActivityInstance.GetWorkflowActivities(workflowInstanceId); // 流程实例的全部活动实例.

                IList<Countersigned> workflowCountersigneds = new List<Countersigned>();   // 流程实例的全部会签实例.
                if (workflowActivities.Count > 0)
                {
                    workflowActivities = ResortActivities(workflowActivities);
                    this.DataBind(this.BuildProcessHistory(workflowActivities, workflowAssignments, workflowCountersigneds));
                }
                else
                {
                    this.Visible = false;
                }
            }
        }

        /// <summary>
        /// 绑定生成 HTML 数据.
        /// </summary>
        /// <param name="historyHtml"></param>
        protected abstract void DataBind(string historyHtml);

        /// <summary>
        /// 生成处理历史记录 HTML.
        /// </summary>
        /// <param name="activities">流程活动实例列表.</param>
        /// <param name="workflowAssignments">转交信息列表.</param>
        /// <param name="workflowCountersigneds">会签信息列表.</param>
        /// <returns></returns>
        protected virtual string BuildProcessHistory(IList<CZActivityInstance> activities, IList<Assignment> workflowAssignments, IList<Countersigned> workflowCountersigneds)
        {
            bool isOutputUncompleted = false;
            string currentActor = string.Empty;
            int rowIndex = 0;

            this.OnPreBuildProcessHistory(ref isOutputUncompleted, ref activities);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < activities.Count; i++)
            {
                CZActivityInstance item = activities[i];
                //if (i == 0)
                //    item.Reason = defaultReason;

                string itemHtml = BuildActivity(item, currentActor, rowIndex, isOutputUncompleted, workflowAssignments, workflowCountersigneds);
                if (!string.IsNullOrEmpty(itemHtml))
                {
                    builder.AppendLine(itemHtml);
                    rowIndex++;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 预先处理.
        /// </summary>
        /// <param name="isOutputUncompleted"></param>
        /// <param name="workflowActivities"></param>
        protected virtual void OnPreBuildProcessHistory(ref bool isOutputUncompleted, ref IList<CZActivityInstance> workflowActivities)
        { 
            
        }

        #region methods

        /// <summary>
        /// 生成.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currentActor"></param>
        /// <param name="rowIndex"></param>
        /// <param name="isOutputUncompleted"></param>
        /// <param name="workflowAssginments"></param>
        /// <param name="workflowCountersigneds"></param>
        /// <returns></returns>
        protected virtual string BuildActivity(CZActivityInstance item, string currentActor, int rowIndex, bool isOutputUncompleted, IList<Assignment> workflowAssginments, IList<Countersigned> workflowCountersigneds)
        {
            // 流程活动实例的转交信息
            IList<Assignment> activityAssignments = GetActivityAssignments(workflowAssginments, item.ActivityInstanceId);
            // 流程活动实例的会签信息
            IList<Countersigned> activityCountersigneds = (string.IsNullOrEmpty(item.CountersignedCondition) ? new List<Countersigned>() : GetActivityCountersigneds(workflowCountersigneds, item.ActivityInstanceId));

            // 输出流程活动实例 HTML.
            this.OnPreBuildActivity(activityAssignments, activityCountersigneds);
            return OnBuildActivity(item, currentActor, rowIndex, isOutputUncompleted, activityAssignments, activityCountersigneds);
        }

        /// <summary>
        /// 处理.
        /// </summary>
        /// <param name="activityAssginments"></param>
        /// <param name="activityCountersigneds"></param>
        protected virtual void OnPreBuildActivity(IList<Assignment> activityAssginments, IList<Countersigned> activityCountersigneds)
        { 
        
        }

        /// <summary>
        /// 生成.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currentActor"></param>
        /// <param name="rowIndex"></param>
        /// <param name="isOutputUncompleted">是否输出未完成流程活动实例.</param>
        /// <param name="activityAssginments"></param>
        /// <param name="activityCountersigneds"></param>
        /// <returns></returns>
        protected virtual string OnBuildActivity(CZActivityInstance item, string currentActor, int rowIndex, bool isOutputUncompleted, IList<Assignment> activityAssginments, IList<Countersigned> activityCountersigneds)
        {
            bool hasCssClass = (rowIndex % 2 == 1);   // 是否有行样式.
            bool isOutputActivity = isOutputUncompleted ? true : item.IsCompleted;    // 是否输出流出活动信息.

            int assginmentCount = activityAssginments.Count;
            int countersignedCount = activityCountersigneds.Count;
            int rowCount = (isOutputActivity ? 1 : 0) + (assginmentCount > 0 ? assginmentCount + 1 : 0) + (countersignedCount > 0 ? countersignedCount + (this.DisplayCountersignedHeader ? 1 : 0) : 0);   // 显示共所占行数(包括转交、会签等).

            if (rowCount <= 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();

            // activity:开始标签 <tr> 
            builder.AppendLine((hasCssClass ? "<tr class=\"trClass\">" : "<tr>"));
            builder.AppendLine(this.BuildActivityNameCell(item, rowCount));   // 流程活动名称表格单元格.

            if (isOutputActivity)
            {
                builder.AppendLine(this.BuildActivityCells(item, currentActor));
                builder.AppendLine("</tr>");    // activity:结束 </tr>
            }

            // 显示转交记录列表.
            if (assginmentCount > 0)
            {
                builder.AppendLine((isOutputActivity ? "<tr>" : string.Empty) + AssignmentHeader + "</tr>");
                foreach (Assignment assignment in activityAssginments)
                {
                    builder.AppendLine(this.BuildActivityAssginmentCells(assignment));
                }
            }

            // 显示会签记录列表.
            if (countersignedCount > 0)
            {
                if (this.DisplayCountersignedHeader)
                {
                    builder.AppendLine((isOutputActivity || assginmentCount > 0 ? "<tr>" : string.Empty) + CountersignedHeader + "</tr>");
                }
                for (int i = 0; i < countersignedCount; i++)
                {
                    Countersigned countersigned = activityCountersigneds[i];
                    if (this.DisplayCountersignedHeader || i > 0)
                        builder.AppendLine(this.BuildActivityCountersignedCells(countersigned));
                    else
                        builder.AppendLine(this.BuildActivityCountersignedCells(countersigned, (isOutputActivity || assginmentCount > 0)));
                }
            }
            return builder.ToString();
        }

        /// <summary>
        ///  生成流程活动实例的流程名称单元格 HTML(只有流程名称单元格).
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rowCount">显示共所占行数(包括转交、会签等).</param>
        /// <returns></returns>
        protected virtual string BuildActivityNameCell(CZActivityInstance item, int rowCount)
        {
            string rowSpan = (rowCount > 1 ? string.Format(" rowspan=\"{0}\"", rowCount) : string.Empty);  // 流程活动名称单元格的 rowspan 属性.

            // item.OperateType == TodoInfo.OpBack
            string command = (item.Command == null ? string.Empty : item.Command.ToLower());
            if (ActivityCommands.Reject.Equals(command) || ActivityCommands.ReturnToDraft.Equals(command))
                return string.Format("<td{0}><span style=\"color:red;font-weight:bold\">【退还】</span>{1}</td>", rowSpan, item.ActivityName);
            return string.Format("<td{0}>{1}</td>", rowSpan, item.ActivityName);
        }

        /// <summary>
        /// 生成流程活动实例项单元格 HTML(无 tr 标签，也无流程活动名称单元格).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual string BuildActivityCells(CZActivityInstance item, string currentActor)
        {
            StringBuilder builder = new StringBuilder();
            // 处理人
            builder.Append("<td colspan=\"2\">");
            string actor = (null == item.Actor) ? String.Empty : item.Actor;
            if (item.IsCompleted)
            {
                int index = actor.LastIndexOf('/');
                string actorName = string.Empty;
                if (index > 0)
                {
                    actorName = actor.Substring(index + 1);
                    actor = actor.Substring(0, index);
                }
                string actorDescription = item.ActorDescription;
                if (string.IsNullOrEmpty(actorDescription))
                {
                    if (!string.IsNullOrEmpty(actorName))
                        builder.AppendFormat("<span tooltip=\"{0}\"><b>{1}</b></span>", actor, actorName);
                    else
                        builder.AppendFormat("<b>{0}</b>", actor);
                }
                else
                {
                    bool visibleTooltip = (!actor.Equals(currentActor, StringComparison.OrdinalIgnoreCase));
                    builder.Append(Botwave.Workflow.WorkflowHelper.ParserActivityActorDescription(actor, actorDescription, visibleTooltip));
                }
            }
            else
            {
                builder.Append(this.BuildTodoActors(item));
            }
            builder.Append("</td>");

            // 执行时间
            builder.Append(item.IsCompleted ? string.Format("<td>{0:yyyy-MM-dd HH:mm:ss}</td>", item.FinishedTime) : "<td></td>");
            string reason = item.Reason;
            if (!string.IsNullOrEmpty(reason))
            {
                reason = reason.Replace(" ", "&nbsp;");
                reason = reason.Replace("\r\n", "<br />");
            }
            // 处理意见
            builder.AppendFormat("<td><input type=\"checkbox\" name=\"activity_chk_{0}\" title=\"选择删除此项内容。\" style=\"display:none;\"/></td>", item.ActivityInstanceId);
            builder.AppendFormat("<td><textarea id=\"activity_reason_{1}\" name=\"activity_reason_{1}\" style=\"display:none;width:90%\" rowspan=\"3\" >{0}</textarea><span>{0}</span></td>", reason,item.ActivityInstanceId);

            return builder.ToString();
        }

        /// <summary>
        /// 生成转交项单元格 HTML(无 tr 标签).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual string BuildActivityAssginmentCells(Assignment item)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<tr>");
            builder.AppendFormat("<td><b>{0}</b></td>", item.AssigningUser);  // 转交人

            // 被转交人
            string assignedActor = item.AssignedUser;
            int index = assignedActor.LastIndexOf('/');
            builder.Append("<td>");
            if (index > 0 && index < assignedActor.Length - 1)
            {
                builder.AppendFormat("<span tooltip=\"{0}\"><b>{1}</b></span>", assignedActor.Substring(0, index), assignedActor.Substring(index + 1));
            }
            else
            {
                builder.AppendFormat("<b>{0}</b>", assignedActor);
            }
            builder.Append("</td>");

            builder.AppendFormat("<td>{0:yyyy-MM-dd HH:mm:ss}</td>", item.AssignedTime);  // 转交时间.
            builder.AppendFormat("<td><input type=\"checkbox\" name=\"assign_chk_{0}_{1}\" title=\"选择删除此项内容。\"  style=\"display:none;\"/></td>", item.ActivityInstanceId, item.AssignedTime.Value.ToString("yyyyMMddHHmmss"));
            builder.AppendFormat("<td><textarea id=\"assign_reason_{1}_{2}\" name=\"assign_reason_{1}_{2}\" style=\"display:none;width:90%\" rowspan=\"3\" >{0}</textarea><span>{0}</span></td>", item.Message, item.ActivityInstanceId, item.AssignedTime.Value.ToString("yyyyMMddHHmmss"));    // 转交信息
            builder.Append("</tr>");
            return builder.ToString();
        }

        /// <summary>
        /// 生成会签项单元格 HTML.
        /// </summary>
        /// <param name="item"></param>
        protected virtual string BuildActivityCountersignedCells(Countersigned item)
        {
            return this.BuildActivityCountersignedCells(item, true);
        }

        /// <summary>
        /// 生成会签项单元格 HTML.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="rowStartTag">是否带有 "tr" 的起始标签.</param>
        protected virtual string BuildActivityCountersignedCells(Countersigned item, bool rowStartTag)
        {
            StringBuilder builder = new StringBuilder();
            if (rowStartTag)
            {
                builder.Append("<tr>");
            }
            builder.Append("<td colspan=\"2\">"); // 会签人
            UserInfo user = userService.GetUserByUserName(item.UserName);
            builder.AppendFormat("<span tooltip=\"{0}\"><b>{1}</b></span>", user.UserName, user.RealName);
            builder.Append("</td>");
            builder.AppendFormat("<td>{0:yyyy-MM-dd HH:mm:ss}</td>", item.CreatedTime);    // 会签时间
            builder.AppendFormat("<td><input type=\"checkbox\" name=\"countersigned_chk_{0}_{1}\" title=\"选择删除此项内容。\"  style=\"display:none;\"/></td>", item.ActivityInstanceId, item.UserName);
            builder.AppendFormat("<td><textarea id=\"countersigned_reason_{1}_{2}\" name=\"countersigned_reason_{1}_{2}\" style=\"display:none;width:90%\" rowspan=\"3\" >{0}</textarea><span>{0}</span></td>", item.Message, item.ActivityInstanceId, item.UserName);    //  会签信息

            builder.Append("</tr>");
            return builder.ToString();
        }

        /// <summary>
        /// 生成未完成流程活动实例的待处理用户.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual string BuildTodoActors(CZActivityInstance item)
        {
            string result = string.Empty;
            IList<Botwave.Entities.BasicUser> users = taskAssignService.GetTodoActors(item.ActivityInstanceId);
            if (users != null && users.Count > 0)
            {
                foreach (Botwave.Entities.BasicUser user in users)
                    result += string.Format(",<span tooltip=\"{0}\"><b>{1}</b></span>", user.UserName, user.RealName);
                result = result.Remove(0, 1);
            }
            return result;
        }
        #endregion

        #region util

        /// <summary>
        /// 获取流程活动实例的转交信息列表.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        protected static IList<Assignment> GetActivityAssignments(IList<Assignment> sources, Guid activityInstanceId)
        {
            IList<Assignment> items = new List<Assignment>();
            for (int i = 0; i < sources.Count; i++)
            {
                if (sources[i].ActivityInstanceId == activityInstanceId)
                {
                    items.Add(sources[i]);
                    sources.RemoveAt(i);
                    i--;
                }
            }
            return items;
        }

        /// <summary>
        /// 获取流程活动实例的会签信息列表.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        protected virtual IList<Countersigned> GetActivityCountersigneds(IList<Countersigned> sources, Guid activityInstanceId)
        {
            IList<Countersigned> results = countersignedService.GetCountersignedList(activityInstanceId);
            if (results == null)
                results = new List<Countersigned>();
            return results;
        }

        /// <summary>
        /// 为流程活动实例列表重新排序(按完成时间先后顺序，同时按创建时间先后顺序).
        /// </summary>
        /// <param name="activities"></param>
        /// <returns></returns>
        protected static IList<CZActivityInstance> ResortActivities(IList<CZActivityInstance> activities)
        {
            if (activities == null || activities.Count <= 1)
                return activities;
            DateTime minDate = new DateTime(1900, 1, 1);
            IList<CZActivityInstance> uncompletedActivities = new List<CZActivityInstance>(); // 未完成流程活动实例列表.
            // 算法：插入排序.
            for (int i = 0; i < activities.Count; i++)
            {
                CZActivityInstance current = activities[i];
                if (current.IsCompleted == false)
                {
                    uncompletedActivities.Add(current);
                    activities.RemoveAt(i);
                    i--;
                    continue;
                }
                if (i == 0)
                    continue;
                int index = i;
                while (index > 0 && CompareFinishedTime(current, activities[index - 1], minDate))
                {
                    activities[index] = activities[index - 1];//交换顺序
                    --index;
                }
                activities[index] = current;
            }

            int count = uncompletedActivities.Count;
            if (count > 1)
            {
                // 对未完成活动实例进行排序.
                for (int i = 1; i < count; i++)
                {
                    CZActivityInstance current = uncompletedActivities[i];
                    int index = i;
                    while (index > 0 && CompareCreatedTime(current, uncompletedActivities[index - 1], minDate))
                    {
                        uncompletedActivities[index] = uncompletedActivities[index - 1];//交换顺序
                        --index;
                    }
                    uncompletedActivities[index] = current;
                }
            }
            foreach (CZActivityInstance item in uncompletedActivities)
            {
                activities.Add(item);
            }
            return activities;
        }

        /// <summary>
        /// 比较前一流程活动实例的完成时间是否晚于当前流程活动实例的完成时间.
        /// </summary>
        /// <param name="currentActivity"></param>
        /// <param name="prevActivity"></param>
        /// <param name="minDate"></param>
        /// <returns></returns>
        protected static bool CompareFinishedTime(CZActivityInstance currentActivity, CZActivityInstance prevActivity, DateTime minDate)
        {
            bool prevIsNull = (prevActivity.FinishedTime == null || prevActivity.FinishedTime <= minDate);
            return (prevIsNull || prevActivity.FinishedTime > currentActivity.FinishedTime); //  Current 完成时间小于 Prev 完成时间.
        }

        /// <summary>
        /// 比较前一流程活动实例的创建时间是否晚于当前流程活动实例的创建时间.
        /// </summary>
        /// <param name="currentActivity"></param>
        /// <param name="prevActivity"></param>
        /// <param name="minDate"></param>
        /// <returns></returns>
        protected static bool CompareCreatedTime(CZActivityInstance currentActivity, CZActivityInstance prevActivity, DateTime minDate)
        {
            bool prevIsNull = (prevActivity.CreatedTime == null || prevActivity.CreatedTime <= minDate);
            return (prevIsNull || prevActivity.CreatedTime > currentActivity.CreatedTime); // Current 创建时间小于 Prev 创建时间.
        }

        #endregion
    }
}
