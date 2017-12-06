using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Commons;
using Botwave.Entities;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.UI;
using Botwave.XQP.Service;

namespace Botwave.Workflow.Practices.CZMCC.Support
{
    /// <summary>
    /// 销售精英平台流程步骤选择器类.
    /// </summary>
    public class XSJYWorkflowSelectorProfile : Botwave.Workflow.Extension.UI.Support.DefaultWorkflowSelectorProfile
    {
        /// <summary>
        /// 个性化的流程活动(步骤)名称.
        /// </summary>
        private static string _profileActivityName = "内部处理环节";

        /// <summary>
        /// 个性化的流程活动(步骤)名称.
        /// </summary>
        public string ProfileActivityName
        {
            set { _profileActivityName = value; }
        }

        /// <summary>
        /// 处理销售精英平台流程的步骤选择器.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        public override string BuildActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            string activityName = selecotrContext.ActivityName.ToLower().Trim();
            
            if (_profileActivityName.Equals(activityName, StringComparison.OrdinalIgnoreCase) && selecotrContext.NextActivities != null && selecotrContext.NextActivities.Count > 0)
            {
                // 当步骤为特殊步骤时，进行处理.
                IDictionary<string, string> prevNames = GetPrevActivityNames(selecotrContext.ActivityInstanceId);
                for (int i = 0; i < selecotrContext.NextActivities.Count; i++)
                {
                    string nextName = selecotrContext.NextActivities[i].ActivityName;
                    if (!prevNames.ContainsKey(nextName))
                    {
                        selecotrContext.NextActivities.RemoveAt(i);
                        i--;
                    }
                }
            }
            else
            {
                // 遍历下行步骤，当下行步骤有"内部环节处理"步骤时，进行特殊处理.
                foreach (ActivityDefinition next in selecotrContext.NextActivities)
                {
                    if (next.ActivityName.Equals(_profileActivityName, StringComparison.OrdinalIgnoreCase))
                    {
                        return this.XSJYBuildActivitySelectorHtml(webContext, selecotrContext);
                    }
                }
            }
            return base.BuildActivitySelectorHtml(webContext, selecotrContext);
        }

        #region 特别处理

        /// <summary>
        /// 销售精英.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        protected virtual string XSJYBuildActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {

            Guid workflowInstanceId = selecotrContext.WorkflowInstanceId;
            string currentActor = selecotrContext.Actor;
            string currentActivityName = selecotrContext.ActivityName.ToLower().Trim();
            string splitCondition = selecotrContext.SplitCondition;
            IList<ActivityDefinition> nextActivities = selecotrContext.NextActivities;

            IList<WorkflowSelectorContext.ActivityActor> activityActors = new List<WorkflowSelectorContext.ActivityActor>();

            int activityCount = nextActivities.Count;
            bool isCheckBox = false;
            bool selectAll = false;

            //如果分支条件为空，则表示任选并且只能选一个分支.
            if (!string.IsNullOrEmpty(splitCondition))
            {
                isCheckBox = true;
                if (splitCondition.Equals("all", StringComparison.OrdinalIgnoreCase))
                {
                    selectAll = true;
                }
                else
                {
                    //TODO 检查需要选择的分支数量.
                }
            }

            Guid specileActivityId = Guid.Empty;
            for (int i = 0; i < activityCount; i++)
            {
                ActivityDefinition dataItem = nextActivities[i];
                string nextActivityName = dataItem.ActivityName.ToLower().Trim();
                if (nextActivityName.Equals(_profileActivityName, StringComparison.OrdinalIgnoreCase))
                {
                    specileActivityId = dataItem.ActivityId;
                }
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, currentActor, true);
                activityActors.Add(new WorkflowSelectorContext.ActivityActor(dataItem.ActivityId, dataItem.ActivityName, dict));
            }

            string activityItemTemplate = null;
            if (isCheckBox)
            {
                activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return XSJYBuildDefaultCheckBoxActivities(activityActors, specileActivityId, selectAll, activityItemTemplate, Template_ActorInputHtml);
            }
            else
            {
                activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return XSJYBuildDefaultRadioActivities(activityActors, specileActivityId, activityItemTemplate, Template_ActorInputHtml);
            }
        }
        /// <summary>
        /// 以单选框形式呈现默认活动选择器 Html.
        /// </summary>
        /// <param name="activityActors"></param>
        /// <param name="specialActivityId"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string XSJYBuildDefaultRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, Guid specialActivityId, string activityItemTemplate, string actorItemTemplate)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            StringBuilder actorsBuilder = new StringBuilder();
            int count = activityActors.Count;
            bool isActivityChecked = (count == 1);
            string selectDisable = "false";

            htmlBuilder.AppendLine("<div style=\"margin-left:102px\">\r\n<div>");
            for (int i = 0; i < count; i++)
            {
                // 显示步骤.
                WorkflowSelectorContext.ActivityActor item = activityActors[i];

                // 显示步骤可处理用户.
                string activityCheckText = GetCheckedAttribute(isActivityChecked);
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, activityCheckText, selectDisable, GetSelectedClass(isActivityChecked));
                IDictionary<string, string> itemActors = item.Actors;
                if (itemActors == null)
                    itemActors = new Dictionary<string, string>();


                if (item.ActivityId == specialActivityId)
                {
                    // 显示步骤可处理用户.

                    #region html div
                    actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
                    //foreach (string key in actors.Keys)
                    //{
                    //    if (!actors.ContainsKey(key)) // 用户不存在.
                    //        continue;
                    //    string name = actors[key]; // 真实姓名.
                    //    if (string.IsNullOrEmpty(name)) // 用户不存在.
                    //        continue;

                    //    string proxyUser = String.IsNullOrEmpty(itemActors[key]) ? String.Empty : itemActors[key];
                    //    string displayActorName = string.Format(Template_DisplayActor, key, name);
                    //    if (!string.IsNullOrEmpty(proxyUser))
                    //    {
                    //        string proxyRealName = proxyActors[proxyUser];
                    //        if (!string.IsNullOrEmpty(proxyRealName))
                    //            displayActorName += string.Format(Template_DisplayActor_Proxy, proxyUser, proxyRealName);
                    //    }
                    //    actorsBuilder.AppendFormat(actorItemTemplate,
                    //        i, keyIndex, item.ActivityId, key, proxyUser,
                    //        selectDisable, actorCheckText, selectedClassName, displayActorName);

                    //    if (keyIndex > 0 && keyIndex % repeatRows == 0)
                    //        actorsBuilder.Append("<br />");
                    //    keyIndex++;
                    //}
                    actorsBuilder.AppendLine(WorkflowSelectorProfile.BuildPopupButton());
                    actorsBuilder.AppendLine("</div>");

                    actorsBuilder.AppendLine(WorkflowSelectorProfile.BuildPopupScript(item.ActivityId, i));
                    continue;

                    #endregion
                }

                if (itemActors != null && itemActors.Count > 0)
                {
                    IList<BasicUser> actorDetails;
                    IDictionary<string, string> actors = WorkflowProfileHelper.GetActorNames(item.Actors.Keys, out actorDetails);
                    //IDictionary<string, string> actors = workflowUserService.GetActorRealNames(itemActors.Keys);
                    IDictionary<string, string> proxyActors = workflowUserService.GetActorRealNames(itemActors.Values); // 委托人字典.
                    int actorCount = actors.Count;
                    int keyIndex = 0;
                    bool isSelectedActor = (actorCount <= SelectedBound && isActivityChecked);
                    string actorCheckText = GetCheckedAttribute(isSelectedActor);
                    string selectedClassName = GetSelectedClass(isSelectedActor);
                    // 显示步骤可处理用户.
                    if (actorCount <= repeatRows)
                    {
                        #region html div
                        actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
                        //foreach (string key in actors.Keys)
                        foreach (BasicUser detail in actorDetails)
                        {
                            string key = detail.UserName;
                            if (!actors.ContainsKey(key)) // 用户不存在.
                                continue;
                            string name = actors[key]; // 真实姓名.
                            if (string.IsNullOrEmpty(name)) // 用户不存在.
                                continue;

                            string proxyUser = String.IsNullOrEmpty(itemActors[key]) ? String.Empty : itemActors[key];
                            string displayActorName = string.Format(Template_DisplayActor, key, name);
                            if (!string.IsNullOrEmpty(proxyUser))
                            {
                                string proxyRealName = proxyActors[proxyUser];
                                if (!string.IsNullOrEmpty(proxyRealName))
                                    displayActorName += string.Format(Template_DisplayActor_Proxy, proxyUser, proxyRealName);
                            }
                            actorsBuilder.AppendFormat(actorItemTemplate,
                                i, keyIndex, item.ActivityId, key, proxyUser,
                                selectDisable, actorCheckText, selectedClassName, displayActorName);

                            keyIndex++;
                        }
                        actorsBuilder.AppendLine("</div>");
                        #endregion
                    }
                    else
                    {
                        #region html table

                        actorsBuilder.AppendFormat("<table id=\"actors_{0}\" class=\"blockActors\" style=\"border:0;margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
                        //foreach (string key in actors.Keys)
                        foreach (BasicUser detail in actorDetails)
                        {
                            string key = detail.UserName;
                            if (!actors.ContainsKey(key)) // 用户不存在

                                continue;
                            string name = actors[key]; // 真实姓名.
                            if (string.IsNullOrEmpty(name)) // 用户不存在.
                                continue;

                            if (keyIndex % repeatRows == 0) // 是否新行
                                actorsBuilder.Append("<tr><td>");
                            else
                                actorsBuilder.Append("<td>");

                            string proxyUser = String.IsNullOrEmpty(itemActors[key]) ? String.Empty : itemActors[key];
                            string displayActorName = string.Format(Template_DisplayActor, key, name);
                            if (!string.IsNullOrEmpty(proxyUser))
                            {
                                string proxyRealName = proxyActors[proxyUser];
                                if (!string.IsNullOrEmpty(proxyRealName))
                                    displayActorName += string.Format(Template_DisplayActor_Proxy, proxyUser, proxyRealName);
                            }
                            actorsBuilder.AppendFormat(actorItemTemplate,
                                i, keyIndex, item.ActivityId, key, proxyUser,
                                selectDisable, actorCheckText, selectedClassName, displayActorName);

                            if (keyIndex % repeatRows == repeatModValue)
                                actorsBuilder.Append("</td></tr>");
                            else
                                actorsBuilder.Append("</td>");
                            keyIndex++;
                        }
                        if (keyIndex % repeatRows != 0)
                        {
                            int emptyCount = repeatRows - keyIndex % repeatRows;
                            for (; emptyCount > 0; emptyCount--)
                            {
                                actorsBuilder.Append("<td></td>");
                            }
                            actorsBuilder.Append("</tr>");
                        }
                        actorsBuilder.AppendLine("</table>");
                        #endregion
                    }
                }
                else
                {
                    // 无处理人
                    actorsBuilder.AppendFormat("\r\n<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1};color:red\">", i, (isActivityChecked ? "block" : "none"));
                    actorsBuilder.AppendLine(NotActorMessage);
                    actorsBuilder.AppendLine("</div>");
                }
            }
            htmlBuilder.AppendLine("</div>\r\n<div id=\"divActors\">" + actorsBuilder.ToString() + "\r\n</div>\r\n</div>");
            return htmlBuilder.ToString();
        }

        /// <summary>
        /// 以复选框形式呈现默认活动选择器 Html.
        /// </summary>
        /// <param name="activityActors"></param>
        /// <param name="specialActivityId"></param>
        /// <param name="selectAll"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string XSJYBuildDefaultCheckBoxActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, Guid specialActivityId, bool selectAll, string activityItemTemplate, string actorItemTemplate)
        {
            StringBuilder htmlBuilder = new StringBuilder();
            int count = activityActors.Count;
            bool isChecked = (count <= SelectedBound || selectAll);
            string selectDisable = "false";

            htmlBuilder.AppendLine("<ul id=\"divActors\" style=\"margin-left:102px\">");
            for (int i = 0; i < count; i++)
            {
                // 显示步骤.
                WorkflowSelectorContext.ActivityActor item = activityActors[i];

                // 显示步骤可处理用户.
                string activityCheckText = GetCheckedAttribute(isChecked);
                htmlBuilder.AppendLine("<li>");
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, activityCheckText, selectDisable, GetSelectedClass(isChecked));
                htmlBuilder.AppendLine("</li>");

                IDictionary<string, string> itemActors = item.Actors;
                if (itemActors != null && itemActors.Count > 0)
                {
                    IList<BasicUser> actorDetails;
                    IDictionary<string, string> actors = WorkflowProfileHelper.GetActorNames(item.Actors.Keys, out actorDetails);
                    //IDictionary<string, string> actors = workflowUserService.GetActorRealNames(itemActors.Keys);
                    IDictionary<string, string> proxyActors = workflowUserService.GetActorRealNames(itemActors.Values); // 委托人字典.
                    int keyIndex = 0;
                    int actorCount = actors.Count;
                    bool isSelectedActor = (isChecked && actorCount <= SelectedBound);
                    string actorCheckText = GetCheckedAttribute(isSelectedActor);
                    string selectedClassName = GetSelectedClass(isSelectedActor);

                    if (actorCount <= repeatRows)
                    {
                        #region html li

                        htmlBuilder.AppendFormat("<li id=\"actors_{0}\" style=\"margin-left:30px\">", i);
                        //foreach (string key in actors.Keys)
                        foreach (BasicUser detail in actorDetails)
                        {
                            string key = detail.UserName;
                            if (!actors.ContainsKey(key)) // 用户不存在

                                continue;
                            string name = actors[key]; // 真实姓名.
                            // 用户不存在

                            if (string.IsNullOrEmpty(name))
                                continue;

                            string proxyUser = string.IsNullOrEmpty(itemActors[key]) ? string.Empty : itemActors[key];
                            string displayActorName = string.Format(Template_DisplayActor, key, name);
                            if (!string.IsNullOrEmpty(proxyUser))
                            {
                                string proxyRealName = proxyActors[proxyUser];
                                if (!string.IsNullOrEmpty(proxyRealName))
                                    displayActorName += string.Format(Template_DisplayActor_Proxy, proxyUser, proxyRealName);
                            }
                            htmlBuilder.AppendFormat(actorItemTemplate,
                                i, keyIndex, item.ActivityId, key, proxyUser,
                                selectDisable, actorCheckText, selectedClassName, displayActorName);

                        }
                        htmlBuilder.AppendLine("</li>");
                        #endregion
                    }
                    else
                    {
                        #region html table

                        htmlBuilder.AppendLine("<li>");
                        htmlBuilder.AppendFormat("<table id=\"actors_{0}\" style=\"margin-left:30px\">", i);
                        //foreach (string key in actors.Keys)
                        foreach (BasicUser detail in actorDetails)
                        {
                            string key = detail.UserName;
                            if (!actors.ContainsKey(key)) // 用户不存在

                                continue;
                            string name = actors[key]; // 真实姓名.
                            if (string.IsNullOrEmpty(name))  // 用户不存在

                                continue;

                            if (keyIndex % repeatRows == 0) // 是否新行
                                htmlBuilder.Append("<tr><td>");
                            else
                                htmlBuilder.Append("<td>");

                            string proxyUser = string.IsNullOrEmpty(itemActors[key]) ? string.Empty : itemActors[key];
                            string displayActorName = string.Format(Template_DisplayActor, key, name);
                            if (!string.IsNullOrEmpty(proxyUser))
                            {
                                string proxyRealName = proxyActors[proxyUser];
                                if (!string.IsNullOrEmpty(proxyRealName))
                                    displayActorName += string.Format(Template_DisplayActor_Proxy, proxyUser, proxyRealName);
                            }
                            htmlBuilder.AppendFormat(actorItemTemplate,
                                i, keyIndex, item.ActivityId, key, proxyUser,
                                selectDisable, actorCheckText, selectedClassName, displayActorName);

                            if (keyIndex % repeatRows == repeatModValue)
                                htmlBuilder.Append("</td></tr>");
                            else
                                htmlBuilder.Append("</td>");
                            keyIndex++;
                        }
                        if (keyIndex % repeatRows != 0)
                        {
                            int emptyCount = repeatRows - keyIndex % repeatRows;
                            for (; emptyCount > 0; emptyCount--)
                            {
                                htmlBuilder.Append("<td></td>");
                            }
                            htmlBuilder.Append("</tr>");
                        }
                        htmlBuilder.AppendLine("</table>");
                        htmlBuilder.AppendLine("</li>");

                        #endregion
                    }
                }
                else
                {
                    // 无处理人
                    htmlBuilder.AppendFormat("<li id=\"actors_{0}\" style=\"margin-left:30px;color:red\">{1}</li>", i, NotActorMessage);
                }
            }

            htmlBuilder.AppendLine("</ul>");
            return htmlBuilder.ToString();
        }

        #endregion

        /// <summary>
        /// 获取前续步骤名称.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        protected static IDictionary<string, string> GetPrevActivityNames(Guid activityInstanceId)
        {
            string sql = @"  SELECT DISTINCT ActivityName FROM vw_bwwf_Tracking_Activities_All_Ext
                                    WHERE ActivityInstanceID IN(
	                                    SELECT ActivityInstanceId FROM dbo.bwwf_Tracking_Activities_Set WHERE SetID =(
		                                    SELECT PrevSetID FROM vw_bwwf_Tracking_Activities_All WHERE ActivityInstanceId = '{0}')
                                    )";
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(sql, activityInstanceId)).Tables[0];
            IDictionary<string, string> results = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (resultTable == null || resultTable.Rows.Count == 0)
                return results;
            foreach (DataRow row in resultTable.Rows)
            {
                string activityName = DbUtils.ToString(row[0]);
                if (!results.ContainsKey(activityName))
                    results.Add(activityName, activityName);
            }
            return results;
        }
    }
}
