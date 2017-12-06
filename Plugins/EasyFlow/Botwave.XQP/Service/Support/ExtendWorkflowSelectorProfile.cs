using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Entities;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.UI;
using Botwave.XQP.Domain;
using Botwave.XQP.Service;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 流程选择器界面的默认实现类.
    /// </summary>
    public class ExtendWorkflowSelectorProfile : DefaultWorkflowSelectorProfile
    {
        /// <summary>
        /// 流程抄送选择器个性化.
        /// </summary>
        protected IReviewSelectorProfile reviewSelectorProfile = null;

        /// <summary>
        /// 流程抄送选择器个性化.
        /// </summary>
        public IReviewSelectorProfile ReviewSelectorProfile
        {
            get { return reviewSelectorProfile; }
            set { reviewSelectorProfile = value; }
        }

        public ExtendWorkflowSelectorProfile()
            : base()
        {
            this.AddProperties += new AddPropertiesHandler(Extend_AddProperties);
        }

        protected ReviewType workflowReviewType = ReviewType.None;
        //protected IDictionary<Guid, ActivityProfile> activityProfiles = new Dictionary<Guid, ActivityProfile>();
        protected IDictionary<Guid, ActivityProfile> reviewProfiles = new Dictionary<Guid, ActivityProfile>();
        protected Guid workflowinstanceid;
        protected string currentUser;

        protected virtual void Extend_AddProperties(WorkflowSelectorContext.ActivityActor item, StringBuilder builder)
        {
            if (workflowReviewType != ReviewType.None && activityProfiles.ContainsKey(item.ActivityId))
            {
                ActivityProfile profile = activityProfiles[item.ActivityId];

                builder.AppendFormat(" isReview=\"{0}\" reviewActorCount=\"{1}\" reviewAllChecked=\"{2}\"", profile.IsReview, profile.ReviewActorCount, profile.ReviewValidateType);
            }
        }

        #region override

        public override string BuildActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            workflowReviewType = ReviewSelectorHelper.GeteviewType(selecotrContext.WorkflowId);
            return BuildActivitySelectorHtml(webContext, selecotrContext, workflowReviewType);
        }
        #endregion

        #region extend build

        /// <summary>
        /// 生成.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        protected virtual string BuildActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext, ReviewType reviewType)
        {
            activityProfiles = ActivityProfile.GetProfileDictionary(selecotrContext.WorkflowId);
            reviewProfiles = (reviewType == ReviewType.None ? new Dictionary<Guid, ActivityProfile>() : ActivityProfile.GetProfileDictionary(selecotrContext.WorkflowId));
            this.workflowReviewType = reviewType;
            if (reviewType == ReviewType.CheckBox)
            {
                return this.BuildExtendActivitySelectorHtmlByCheck(webContext, selecotrContext, reviewType);
            }
            else if (reviewType == ReviewType.Classic)
            {
                return this.BuildExtendActivitySelectorHtmlByClassic(webContext, selecotrContext);
            }
            return base.BuildDefaultActivitySelectorHtml(webContext, selecotrContext);
        }

        /// <summary>
        /// 生成指定流程活动默认的活动选择器 Html，并显示旧有的抄送控件内容.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        protected virtual string BuildExtendActivitySelectorHtmlByClassic(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.BuildDefaultActivitySelectorHtml(webContext, selecotrContext));
            builder.Append(ReviewSelectorHelper.BuildClassicHtml());
            return builder.ToString();
        }

        /// <summary>
        /// 生成指定流程活动默认的活动选择器 Html.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <param name="reviewType"></param>
        /// <returns></returns>
        protected virtual string BuildExtendActivitySelectorHtmlByCheck(HttpContext webContext, WorkflowSelectorContext selecotrContext, ReviewType reviewType)
        {
            Guid workflowId = selecotrContext.WorkflowId;

            Guid workflowInstanceId = selecotrContext.WorkflowInstanceId;
            workflowinstanceid = workflowInstanceId;
            string currentActor =currentUser= selecotrContext.Actor;
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

            for (int i = 0; i < activityCount; i++)
            {
                ActivityDefinition dataItem = nextActivities[i];
                //IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, currentActor, true);
                #region 增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
                string extendAllocatorArgs = dataItem.ExtendAllocatorArgs;
                string actor = Botwave.XQP.Domain.CZActivityInstance.GetPssorActor(workflowInstanceId, extendAllocatorArgs);
                #endregion
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, string.IsNullOrEmpty(actor) ? currentActor : actor.ToString(), true);
                activityActors.Add(new WorkflowSelectorContext.ActivityActor(dataItem.ActivityId, dataItem.ActivityName, dict));
            }

            string activityItemTemplate = null;
            if (isCheckBox)
            {
                //activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5},'{7}',{8});\" {4} /><span{6}>{2}</span>";
                return this.BuildExtendCheckBoxActivities(activityActors, reviewProfiles, reviewType, selectAll, activityItemTemplate, Template_ActorInputHtml);
            }
            else
            {
                //activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5},'{7}',{8});\" {4} /><span{6}>{2}</span>";
                return this.BuildExtendRadioActivities(activityActors, activityProfiles, reviewType, activityItemTemplate, Template_ActorInputHtml);
            }
        }

        /// <summary>
        /// 以单选框形式呈现默认活动选择器 Html.
        /// </summary>
        /// <param name="activityActors"></param>
        /// <param name="reviewProfiles"></param>
        /// <param name="reviewType"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string BuildExtendRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, IDictionary<Guid, ActivityProfile> reviewProfiles, ReviewType reviewType, string activityItemTemplate, string actorItemTemplate)
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

                bool isSelectedActor = false; int Selected = -1;// (actorCount <= SelectedBound && isActivityChecked);
                if (activityProfiles.ContainsKey(item.ActivityId))
                {
                    ActivityProfile activityProfile = activityProfiles[item.ActivityId];
                    isSelectedActor = activityProfile.ManageVisible;
                    Selected = activityProfile.ManageActorCount;
                }
                //Selected = Selected < 0 ? SelectedBound : Selected;//-1时则按默认选中数量
                //htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked));
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked), isSelectedActor, Selected);
                IDictionary<string, string> itemActors = item.Actors;
                string ChooseFromCompanyHtml = GetChooseFromCompanyHtml(item.ActivityId, i);
                if (itemActors != null && itemActors.Count > 0)
                {
                    IList<BasicUser> actorDetails;
                    IDictionary<string, string> actors = WorkflowProfileHelper.GetActorNames(item.Actors.Keys, out actorDetails);
                    //IDictionary<string, string> actors = workflowUserService.GetActorRealNames(itemActors.Keys);
                    IDictionary<string, string> proxyActors = workflowUserService.GetActorRealNames(itemActors.Values); // 委托人字典.
                    int actorCount = actors.Count;
                    int keyIndex = 0;
                    //bool isSelectedActor = (actorCount <= SelectedBound && isActivityChecked);
                    // (actorCount <= SelectedBound && isActivityChecked);
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
                            if (Selected >= 0 && isSelectedActor)
                            {
                                if (keyIndex >= Selected)
                                {
                                    actorCheckText = GetCheckedAttribute(false);
                                    selectedClassName = GetSelectedClass(false);
                                }
                            }
                            else if (actorCount <= SelectedBound && isActivityChecked && !isSelectedActor)//没有设置选中时则按默认数量选中
                            {
                                actorCheckText = GetCheckedAttribute(true);
                                selectedClassName = GetSelectedClass(true);
                            }
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
                        #region 全公司人员判断
                        htmlBuilder.Append(ChooseFromCompanyHtml);
                        #endregion
                        if (reviewType == ReviewType.CheckBox && reviewProfiles.ContainsKey(item.ActivityId))
                        {
                            ActivityProfile reviewProfile = reviewProfiles[item.ActivityId];
                            //actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile));
                            actorsBuilder.Append(ReviewSelectorHelper.BuildProfileItemHtml(reviewProfile, workflowinstanceid, currentUser));
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
                            if (Selected >= 0 && isSelectedActor)
                            {
                                if (keyIndex >= Selected)
                                {
                                    actorCheckText = GetCheckedAttribute(false);
                                    selectedClassName = GetSelectedClass(false);
                                }
                            }
                            else if (actorCount <= SelectedBound && isActivityChecked && !isSelectedActor)//没有设置选中时则按默认数量选中
                            {
                                actorCheckText = GetCheckedAttribute(true);
                                selectedClassName = GetSelectedClass(true);
                            }
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
                        #region 全公司人员判断
                        //string ChooseFromCompanyHtml = GetChooseFromCompanyHtml(item.ActivityId, i);
                        if (!string.IsNullOrEmpty(ChooseFromCompanyHtml))
                        {
                            htmlBuilder.Append("<td>" + ChooseFromCompanyHtml + "</td>");
                            keyIndex++;
                        }
                        #endregion
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
                else if (!string.IsNullOrEmpty(ChooseFromCompanyHtml))
                {
                    actorsBuilder.AppendFormat("<div id=\"actors_{0}\" class=\"blockActors\" style=\"margin-left:30px;display:{1}\">", i, (isActivityChecked ? "block" : "none"));
                    #region 全公司人员判断
                    htmlBuilder.Append(ChooseFromCompanyHtml);
                    #endregion
                    actorsBuilder.AppendLine("</div>");
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
        /// <param name="selectAll"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string BuildExtendCheckBoxActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, IDictionary<Guid, ActivityProfile> reviewProfiles, ReviewType reviewType, bool selectAll, string activityItemTemplate, string actorItemTemplate)
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

                bool isSelectedActor = false; int Selected = -1;// (actorCount <= SelectedBound && isActivityChecked);
                if (activityProfiles.ContainsKey(item.ActivityId))
                {
                    ActivityProfile activityProfile = activityProfiles[item.ActivityId];
                    isSelectedActor = activityProfile.ManageVisible;
                    Selected = activityProfile.ManageActorCount;
                }
                //Selected = Selected < 0 ? SelectedBound : Selected;//-1时则按默认选中数量
                htmlBuilder.AppendLine("<li>");
                //htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isChecked));
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isChecked), isSelectedActor, Selected);
                htmlBuilder.AppendLine("</li>");

                IDictionary<string, string> itemActors = item.Actors;
                string ChooseFromCompanyHtml = GetChooseFromCompanyHtml(item.ActivityId, i);
                if (itemActors != null && itemActors.Count > 0)
                {
                    IList<BasicUser> actorDetails;
                    IDictionary<string, string> actors = WorkflowProfileHelper.GetActorNames(item.Actors.Keys, out actorDetails);
                    //IDictionary<string, string> actors = workflowUserService.GetActorRealNames(itemActors.Keys);
                    IDictionary<string, string> proxyActors = workflowUserService.GetActorRealNames(itemActors.Values); // 委托人字典.
                    int keyIndex = 0;
                    int actorCount = actors.Count;
                    //bool isSelectedActor = (isChecked && actorCount <= SelectedBound);
                    // (actorCount <= SelectedBound && isActivityChecked);
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
                            if (Selected >= 0 && isSelectedActor)
                            {
                                if (keyIndex >= Selected)
                                {
                                    actorCheckText = GetCheckedAttribute(false);
                                    selectedClassName = GetSelectedClass(false);
                                }
                            }
                            htmlBuilder.AppendFormat(actorItemTemplate,
                                i, keyIndex, item.ActivityId, key, proxyUser,
                                selectDisable, actorCheckText, selectedClassName, displayActorName);

                        }
                        #region 全公司人员判断
                        htmlBuilder.Append(ChooseFromCompanyHtml);
                        #endregion
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
                            if (Selected >= 0 && isSelectedActor)
                            {
                                if (keyIndex >= Selected)
                                {
                                    actorCheckText = GetCheckedAttribute(false);
                                    selectedClassName = GetSelectedClass(false);
                                }
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
                        #region 全公司人员判断
                        //string ChooseFromCompanyHtml = GetChooseFromCompanyHtml(item.ActivityId, i);
                        if (!string.IsNullOrEmpty(ChooseFromCompanyHtml))
                        {
                            htmlBuilder.Append("<td>" + ChooseFromCompanyHtml + "</td>");
                            keyIndex++;
                        }
                        #endregion
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
                else if (!string.IsNullOrEmpty(ChooseFromCompanyHtml))
                {
                    htmlBuilder.AppendFormat("<li id=\"actors_{0}\" style=\"margin-left:30px\">", i);
                    #region 全公司人员判断
                    htmlBuilder.Append(ChooseFromCompanyHtml);
                    #endregion
                    htmlBuilder.AppendLine("</li>");
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
        /// 从全公司选择处理人
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected string GetChooseFromCompanyHtml(Guid activityId,int index)
        {
            CZActivityDefinition options = CZActivityDefinition.GetWorkflowActivityByActivityId(activityId);
            string extAllocatorArgs = options.ExtendAllocatorArgs;
            if (!string.IsNullOrEmpty(extAllocatorArgs))
            {

                IList<object> args = new List<object>();
                string[] allocatorArgs = extAllocatorArgs.Replace(" ", "").Split(';', '；');
                foreach (string allocatorArg in allocatorArgs)
                {
                    string[] allocatorArray = allocatorArg.Split(':', '：');
                    int lengthOfAllocatorArray = allocatorArray.Length;
                    if (lengthOfAllocatorArray == 0)
                        continue;
                    else if (allocatorArray[0] != "superior")
                        continue;

                    string expression = lengthOfAllocatorArray > 1 ? allocatorArray[1] : string.Empty;
                    if (expression.Length > 0)
                    {
                        string[] argsArray = expression.Split(',', '，');
                        foreach (string arg in argsArray)
                        {
                            if (!string.IsNullOrEmpty(arg))
                                args.Add(arg);
                        }
                    }
                }
                if (null != args && args.Contains("all"))
                {
                    return string.Format("<div><a href=\"#\" onclick=\"javascrpt:return WorkflowExtension.openPopupUserPick(this);\" index=\"{0}\" aid=\"{1}\">从全公司人员中选择处理人</a></div>", index, activityId);
                }
            }
            return string.Empty;
        }
    }
}
