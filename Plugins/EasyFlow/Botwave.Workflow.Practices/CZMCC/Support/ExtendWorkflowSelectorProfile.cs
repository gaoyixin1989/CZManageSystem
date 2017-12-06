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
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

namespace Botwave.Workflow.Practices.CZMCC.Support
{
    /// <summary>
    /// 带抄送功能的特殊流程选择器类.
    /// </summary>
    public class ExtendWorkflowSelectorProfile : Botwave.XQP.Service.Support.ExtendWorkflowSelectorProfile
    {
        #region properties

        /// <summary>
        /// 执行当前特定处理的流程步骤列表.
        /// </summary>
        protected IList<string> activities = new List<string>();

        /// <summary>
        /// 执行当前特定处理的流程步骤列表.
        /// </summary>
        public IList<string> Activities
        {
            set
            {
                foreach (string key in value)
                {
                    string item = key.Trim().ToLower();
                    if (!activities.Contains(item))
                        activities.Add(item);
                }
            }
        }
        #endregion

        #region IWorkflowSelectorProfile 成员

        /// <summary>
        /// 生成指定流程活动的活动选择器 Html.
        /// ****
        /// 申请人可提交给任意下一个（或多个）处理人，当前处理人也可再提交给下一个（或多个）处理人，
        /// 当且仅当当前处理人只有申请人时，当前处理人可选择将工单结束，也可提交给其它处理人处理.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        public override string BuildActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            this.workflowReviewType = ReviewSelectorHelper.GeteviewType(selecotrContext.WorkflowId);
            string activityName = selecotrContext.ActivityName.ToLower().Trim();
            string actor = selecotrContext.Actor;
            if (activities.Contains(activityName) && selecotrContext.WorkflowInstance != null && !string.IsNullOrEmpty(actor))
            {
                this.activityProfiles = (workflowReviewType == ReviewType.None ? new Dictionary<Guid, ActivityProfile>() : ActivityProfile.GetProfileDictionary(selecotrContext.WorkflowId));
                if (this.workflowReviewType == ReviewType.CheckBox)
                {
                    return this.CZBuildExtendActivitySelectorHtmlByCheck(webContext, selecotrContext, this.workflowReviewType);
                }
                else if (this.workflowReviewType == ReviewType.Classic)
                {
                    return this.CZBuildExtendActivitySelectorHtmlByClassic(webContext, selecotrContext);
                }
                return this.CZBuildDefaultActivitySelectorHtml(webContext, selecotrContext);
            }

            // 以默认方式显示流程步骤选择器.
            return base.BuildActivitySelectorHtml(webContext, selecotrContext, this.workflowReviewType);
        }

        #endregion

        #region override
        
        protected override string BuildDefaultRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, string activityItemTemplate, string actorItemTemplate)
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
                StringBuilder propeties = new StringBuilder();
                this.OnAddProperties(item, propeties);
                propeties.Append(activityCheckText);

                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked),false,0);
                IDictionary<string, string> itemActors = item.Actors;
                if (itemActors == null)
                    itemActors = new Dictionary<string, string>();
                //if(itemActors.Count > 0)
                //{
                IList<BasicUser> actorDetails;
                IDictionary<string, string> actors = WorkflowProfileHelper.GetActorNames(item.Actors.Keys, out actorDetails);
                //IDictionary<string, string> actors = workflowUserService.GetActorRealNames(itemActors.Keys);
                IDictionary<string, string> proxyActors = workflowUserService.GetActorRealNames(itemActors.Values); // 委托人字典.
                int actorCount = actors.Count;
                int keyIndex = 0;
                bool isSelectedActor = (actorCount <= SelectedBound && isActivityChecked);
                string actorCheckText = GetCheckedAttribute(isSelectedActor);
                string selectedClassName = GetSelectedClass(isSelectedActor);

                if (this.activities.Contains(item.ActivityName.Trim().ToLower()))
                {
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
                    //actorsBuilder.AppendLine(BuildPopupButton());
                    actorsBuilder.AppendLine(BuildPopupButton(item.ActivityId, i));
                    actorsBuilder.AppendLine("</div>");

                    actorsBuilder.AppendLine(BuildPopupScript(item.ActivityId, i));
                    continue;
                }

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
                        if (!actors.ContainsKey(key)) // 用户不存在.
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
                //}
                //else
                //{
                //    // 无处理人
                //    actorsBuilder.AppendFormat("\r\n<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1};color:red\">", i, (isActivityChecked ? "block" : "none"));
                //    actorsBuilder.AppendLine(NotActorMessage);
                //    actorsBuilder.AppendLine("</div>");
                //}
            }
            htmlBuilder.AppendLine("</div>\r\n<div id=\"divActors\">" + actorsBuilder.ToString() + "\r\n</div>\r\n</div>");
            return htmlBuilder.ToString();
        }

        protected override string BuildExtendRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, IDictionary<Guid, ActivityProfile> reviewProfiles, ReviewType reviewType, string activityItemTemplate, string actorItemTemplate)
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
                StringBuilder propeties = new StringBuilder();
                this.OnAddProperties(item, propeties);
                propeties.Append(activityCheckText);

                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked), false, 0);
                IDictionary<string, string> itemActors = item.Actors;
                if (itemActors == null)
                    itemActors = new Dictionary<string, string>();

                IList<BasicUser> actorDetails;
                IDictionary<string, string> actors = WorkflowProfileHelper.GetActorNames(item.Actors.Keys, out actorDetails);
                //IDictionary<string, string> actors = workflowUserService.GetActorRealNames(itemActors.Keys);
                IDictionary<string, string> proxyActors = workflowUserService.GetActorRealNames(itemActors.Values); // 委托人字典.
                int actorCount = actors.Count;
                int keyIndex = 0;
                bool isSelectedActor = (actorCount <= SelectedBound && isActivityChecked);
                string actorCheckText = GetCheckedAttribute(isSelectedActor);
                string selectedClassName = GetSelectedClass(isSelectedActor);

                if (this.activities.Contains(item.ActivityName.Trim().ToLower()))
                {
                    actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
          
                    //actorsBuilder.AppendLine(BuildPopupButton());
                    actorsBuilder.AppendLine(BuildPopupButton(item.ActivityId, i));

                    if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                    {
                        ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                        actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                    }
                    actorsBuilder.AppendLine("</div>");

                    actorsBuilder.AppendLine(BuildPopupScript(item.ActivityId, i));
                    continue;
                }

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
                    if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                    {
                        ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                        actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                    }
                    actorsBuilder.AppendLine("</div>");
                    #endregion
                }
                else
                {
                    #region html table

                    actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"border:0;margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
                    actorsBuilder.AppendLine("<table>");
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

                    if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                    {
                        ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                        actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                    }
                    actorsBuilder.AppendFormat("</div>");
                    #endregion
                }
                //}
                //else
                //{
                //    // 无处理人
                //    actorsBuilder.AppendFormat("\r\n<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1};color:red\">", i, (isActivityChecked ? "block" : "none"));
                //    actorsBuilder.AppendLine(NotActorMessage);
                //    actorsBuilder.AppendLine("</div>");
                //}
            }
            htmlBuilder.AppendLine("</div>\r\n<div id=\"divActors\">" + actorsBuilder.ToString() + "\r\n</div>\r\n</div>");
            return htmlBuilder.ToString();
        }
        #endregion


        #region bild default profile

        /// <summary>
        /// 生成指定流程活动默认的活动选择器 Html.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        protected virtual string CZBuildDefaultActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
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
                if (nextActivityName.Equals(currentActivityName, StringComparison.CurrentCultureIgnoreCase))
                {
                    specileActivityId = dataItem.ActivityId;
                }
                #region 增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
                string extendAllocatorArgs = dataItem.ExtendAllocatorArgs;
                string actor = Botwave.XQP.Domain.CZActivityInstance.GetPssorActor(workflowInstanceId, extendAllocatorArgs);
                #endregion
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, actor == null ? currentActor : actor.ToString(), true);
                activityActors.Add(new WorkflowSelectorContext.ActivityActor(dataItem.ActivityId, dataItem.ActivityName, dict));
            }

            string activityItemTemplate = null;
            if (isCheckBox)
            {
                activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return CZBuildDefaultCheckBoxActivities(activityActors, specileActivityId, selectAll, activityItemTemplate, Template_ActorInputHtml);
            }
            else
            {
                activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return CZBuildDefaultRadioActivities(activityActors, specileActivityId, activityItemTemplate, Template_ActorInputHtml);
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
        protected virtual string CZBuildDefaultRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, Guid specialActivityId, string activityItemTemplate, string actorItemTemplate)
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
                StringBuilder propeties = new StringBuilder();
                this.OnAddProperties(item, propeties);
                propeties.Append(activityCheckText);

                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked));
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
                    //actorsBuilder.AppendLine(BuildPopupButton());
                    actorsBuilder.AppendLine(BuildPopupButton(item.ActivityId, i));
                    actorsBuilder.AppendLine("</div>");

                    actorsBuilder.AppendLine(BuildPopupScript(item.ActivityId, i));
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
        protected virtual string CZBuildDefaultCheckBoxActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, Guid specialActivityId, bool selectAll, string activityItemTemplate, string actorItemTemplate)
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
                StringBuilder propeties = new StringBuilder();
                this.OnAddProperties(item, propeties);
                propeties.Append(activityCheckText);

                htmlBuilder.AppendLine("<li>");
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isChecked));
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

        #region review extend build

        /// <summary>
        /// 生成指定流程活动默认的活动选择器 Html，并显示旧有的抄送控件内容.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        protected virtual string CZBuildExtendActivitySelectorHtmlByClassic(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.CZBuildDefaultActivitySelectorHtml(webContext, selecotrContext));
            builder.Append(ReviewSelectorHelper.BuildClassicHtml());
            return builder.ToString();
        }

        /// <summary>
        /// 生成指定流程活动的活动选择器 Html(带有抄送功能).
        /// ****
        /// 申请人可提交给任意下一个（或多个）处理人，当前处理人也可再提交给下一个（或多个）处理人，
        /// 当且仅当当前处理人只有申请人时，当前处理人可选择将工单结束，也可提交给其它处理人处理.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <param name="reviewType"></param>
        /// <returns></returns>
        protected virtual string CZBuildExtendActivitySelectorHtmlByCheck(HttpContext webContext, WorkflowSelectorContext selecotrContext, ReviewType reviewType)
        {
            Guid workflowId = selecotrContext.WorkflowId;
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
                if (nextActivityName.Equals(currentActivityName, StringComparison.CurrentCultureIgnoreCase))
                {
                    specileActivityId = dataItem.ActivityId;
                }
                #region 增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
                string extendAllocatorArgs = dataItem.ExtendAllocatorArgs;
                string actor = Botwave.XQP.Domain.CZActivityInstance.GetPssorActor(workflowInstanceId, extendAllocatorArgs);
                #endregion
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, actor == null ? currentActor : actor.ToString(), true);
                activityActors.Add(new WorkflowSelectorContext.ActivityActor(dataItem.ActivityId, dataItem.ActivityName, dict));
            }

            string activityItemTemplate = null;
            if (isCheckBox)
            {
                activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return CZBuildExtendCheckBoxActivities(activityActors, this.activityProfiles, reviewType, specileActivityId, selectAll, activityItemTemplate, Template_ActorInputHtml);
            }
            else
            {
                activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return CZBuildExtendRadioActivities(activityActors, this.activityProfiles, reviewType, specileActivityId, activityItemTemplate, Template_ActorInputHtml);
            }
        }

        /// <summary>
        /// 以单选框形式呈现默认活动选择器 Html.
        /// </summary>
        /// <param name="activityActors"></param>
        /// <param name="reviewProfiles"></param>
        /// <param name="reviewType"></param>
        /// <param name="specialActivityId"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string CZBuildExtendRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, IDictionary<Guid, ActivityProfile> reviewProfiles, ReviewType reviewType, Guid specialActivityId, string activityItemTemplate, string actorItemTemplate)
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
                StringBuilder propeties = new StringBuilder();
                this.OnAddProperties(item, propeties);
                propeties.Append(activityCheckText);

                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked));
                IDictionary<string, string> itemActors = item.Actors;
                if (itemActors == null)
                    itemActors = new Dictionary<string, string>();

                if (item.ActivityId == specialActivityId)
                {
                    // 显示步骤可处理用户.

                    #region html div
                    actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));

                    //actorsBuilder.AppendLine(BuildPopupButton());
                    actorsBuilder.AppendLine(BuildPopupButton(item.ActivityId, i));
                    if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                    {
                        ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                        actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                    }
                    actorsBuilder.AppendLine("</div>");

                    actorsBuilder.AppendLine(BuildPopupScript(item.ActivityId, i));
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
                        if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                        {
                            ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                            actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                        }
                        actorsBuilder.AppendLine("</div>");
                        #endregion
                    }
                    else
                    {
                        #region html table

                        actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"border:0;margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
                        actorsBuilder.AppendLine("<table>");
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
                        if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                        {
                            ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                            actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                        }
                        actorsBuilder.AppendFormat("</div>");
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
        /// <param name="reviewProfiles"></param>
        /// <param name="reviewType"></param>
        /// <param name="specialActivityId"></param>
        /// <param name="selectAll"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string CZBuildExtendCheckBoxActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, IDictionary<Guid, ActivityProfile> reviewProfiles, ReviewType reviewType, Guid specialActivityId, bool selectAll, string activityItemTemplate, string actorItemTemplate)
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
                StringBuilder propeties = new StringBuilder();
                this.OnAddProperties(item, propeties);
                propeties.Append(activityCheckText);

                htmlBuilder.AppendLine("<li>");
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isChecked));
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
                        if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                        {
                            ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                            htmlBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                        }
                        htmlBuilder.AppendLine("</li>");
                        #endregion
                    }
                    else
                    {
                        #region html table

                        htmlBuilder.AppendFormat("<li id=\"actors_{0}\" style=\"margin-left:30px\">", i);
                        htmlBuilder.AppendLine("<table>");
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

                        if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                        {
                            ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                            htmlBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                        }
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

        #region 特定实现方式.

        /// <summary>
        /// 弹出按钮.
        /// </summary>
        /// <returns></returns>
        public static string BuildPopupButton()
        {
            return string.Format("<div><a href=\"#\" onclick=\"javascrpt:return openPopupUserPick();\">从全公司人员中选择处理人</a></div>");
        }

        /// <summary>
        /// 弹出按钮.
        /// </summary>
        /// <returns></returns>
        public static string BuildPopupButton(Guid activityId, int index)
        {
            //return string.Format("<div><a href=\"#\" onclick=\"javascrpt:return openPopupUserPick();\">从全公司人员中选择处理人</a></div>");
            return string.Format("<div><a href=\"#\" onclick=\"javascrpt:return WorkflowExtension.openPopupUserPick(this);\" index=\"{0}\" aid=\"{1}\">从全公司人员中选择处理人</a></div>", index, activityId);
        }

        /// <summary>
        /// 脚本代码.
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string BuildPopupScript(Guid activityId, int id)
        {
            string funcName = "oncompleteSelectActors";
            string openPath = string.Format("{0}contrib/security/pages/popupUserPicker2.aspx?func={1}", Botwave.Web.WebUtils.GetAppPath(), funcName);

            /*
                $("#inputDataActors").val()
             
             */
            StringBuilder scriptBuilder = new StringBuilder();
            scriptBuilder.AppendLine("<script type=\"text/javascript\" language=\"javascript\">\r\n<!--//");
            scriptBuilder.AppendLine("function openPopupUserPick(){");
            scriptBuilder.AppendLine("    var h = 500; var w = 700; var iTop = (window.screen.availHeight-30-h)/2; var iLeft = (window.screen.availWidth-10-w)/2; \r\n");
            scriptBuilder.AppendFormat("   window.open('{0}', '', 'height='+ h +', width='+ w+', top='+ iTop +', left='+ iLeft +', toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no');", openPath);
            scriptBuilder.AppendLine("    return false; ");
            scriptBuilder.AppendLine("}\r\n\r\n");

            scriptBuilder.AppendFormat("function {0}(result)", funcName);
            scriptBuilder.Append("{");
            scriptBuilder.AppendLine("\r\n var htmlText = \"\"; var id = \"0\"; ");
            scriptBuilder.AppendLine("var index = $(\"#actors_" + id + "\").children(\"input[name='activityAllocatee']\").length;");
            scriptBuilder.AppendLine("for( var i=0; i<result.length; i++) {");
            scriptBuilder.AppendLine("if($(\"#actors_" + id + "\").children(\"input[name='activityAllocatee'][value*='$\" + result[i].key + \"$']\").length>0){ continue; };");
            scriptBuilder.AppendLine("index++;");
            scriptBuilder.AppendFormat("if(index>0 && index % {0} ==0) ", repeatRows);
            scriptBuilder.AppendLine("{ htmlText += \"<br />\"; } ");
            scriptBuilder.AppendFormat(@"htmlText += ""<input type='checkbox' id='activityAllocatee_{0}_"" + index+""' name='activityAllocatee' value='{1}$"" + result[i].key+ ""$' onclick='onPreSelectAllocatee(\""activityOption_""+ {0} + ""\"", this, false);' checked='checked' /><span><span tooltip='"" + result[i].key+ ""'>"" + result[i].value+'</span></span>';", id, activityId);
            scriptBuilder.AppendLine("}");
            scriptBuilder.AppendFormat(@"$(""#actors_{0}"").prepend(htmlText);", id);
            scriptBuilder.AppendLine("\r\n");
            scriptBuilder.AppendLine("}");

            scriptBuilder.AppendLine("//-->\r\n</script>\r\n");
            return scriptBuilder.ToString();
        }

        #endregion

        /// <summary>
        /// 获取指定流程活动实例编号的待办人列表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        protected static IList<string> GetTodoActors(Guid activityInstanceId)
        {
            if (activityInstanceId == Guid.Empty)
                return new List<string>();

            IList<string> users = new List<string>();
            string sql = string.Format("SELECT UserName FROM bwwf_Tracking_Todo WHERE ActivityInstanceId = '{0}'", activityInstanceId);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            foreach (DataRow row in resultTable.Rows)
            {
                string userName = DbUtils.ToString(row[0], string.Empty).Trim().ToLower();
                if (!users.Contains(userName))
                    users.Add(userName);
            }
            return users;
        }
    }
}
