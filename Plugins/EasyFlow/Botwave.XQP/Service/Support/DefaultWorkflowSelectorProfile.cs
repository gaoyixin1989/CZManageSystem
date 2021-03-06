﻿using System;
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
    public class DefaultWorkflowSelectorProfile : IWorkflowSelectorProfile
    {
        /// <summary>
        /// 新增属性委托.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="builder"></param>
        public delegate void AddPropertiesHandler(WorkflowSelectorContext.ActivityActor item, StringBuilder builder);

        #region field
        
        /// <summary>
        /// 选中上限值.当处理用户数小于等于上限值时，则将用户标志为选中状态.
        /// </summary>
        protected const int SelectedBound = 3;
        /// <summary>
        /// 重复列数.
        /// </summary>
        protected const int repeatRows = 8;
        /// <summary>
        /// 重复列模值.
        /// </summary>
        protected const int repeatModValue = repeatRows - 1;
        /// <summary>
        /// 显示操作人的模板.
        /// </summary>
        protected const string Template_DisplayActor = "<span tooltip=\"{0}\">{1}</span>";
        /// <summary>
        /// 显示操作委托人的模板.
        /// </summary>
        protected const string Template_DisplayActor_Proxy = "(<span tooltip=\"{0}\">{1}</span>)";
        /// <summary>
        /// 操作人选择框模板.
        /// 选中的操作人值格式："流程步骤编号$操作人用户名$委托人用户名".
        /// </summary>
        protected const string Template_ActorInputHtml = "<input type=\"checkbox\" id=\"activityAllocatee_{0}_{1}\" name=\"activityAllocatee\" value=\"{2}${3}${4}\" onclick=\"onPreSelectAllocatee('activityOption_{0}', this, {5});\" {6}/><span{7}>{8}</span>\r\n";
        /// <summary>
        /// 无操作人时的提示信息.
        /// </summary>
        protected static readonly string NotActorMessage = "该步骤未设置处理人，如需使用，请联系流程管理员。";

        protected IDictionary<Guid, ActivityProfile> activityProfiles = new Dictionary<Guid, ActivityProfile>();
        #endregion

        #region interface properties

        /// <summary>
        /// 流程还多任务分派服务接口.
        /// </summary>
        protected IActivityAllocationService activityAllocationService;
        /// <summary>
        /// 流程用户服务接口.
        /// </summary>
        protected IWorkflowUserService workflowUserService;

        /// <summary>
        /// 新增属性事件.
        /// </summary>
        protected event AddPropertiesHandler AddProperties;

        /// <summary>
        /// 流程还多任务分派服务接口.
        /// </summary>
        public virtual IActivityAllocationService ActivityAllocationService
        {
            set { activityAllocationService = value; }
        }

        /// <summary>
        /// 流程用户服务接口.
        /// </summary>
        public virtual IWorkflowUserService WorkflowUserService
        {
            set { workflowUserService = value; }
        }
        #endregion

        #region IWorkflowSelectorProfile 成员

        /// <summary>
        /// 生成指定流程活动的活动选择器 Html.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        public virtual string BuildActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            activityProfiles = ActivityProfile.GetProfileDictionary(selecotrContext.WorkflowId);
            return BuildDefaultActivitySelectorHtml(webContext, selecotrContext);
        }

        #endregion

        #region bild default profile

        string WorkflowName = string.Empty;
        /// <summary>
        /// 生成指定流程活动默认的活动选择器 Html.
        /// </summary>
        /// <param name="webContext"></param>
        /// <param name="selecotrContext"></param>
        /// <returns></returns>
        protected virtual string BuildDefaultActivitySelectorHtml(HttpContext webContext, WorkflowSelectorContext selecotrContext)
        {
            Guid workflowInstanceId = selecotrContext.WorkflowInstanceId;
            string currentActor = selecotrContext.Actor;
            string splitCondition = selecotrContext.SplitCondition;
            WorkflowName = this.GetWorkflowName(selecotrContext.WorkflowId);
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
                #region 增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
                string extendAllocatorArgs=dataItem.ExtendAllocatorArgs;
                string actor = Botwave.XQP.Domain.CZActivityInstance.GetPssorActor(workflowInstanceId, extendAllocatorArgs);
                #endregion
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, string.IsNullOrEmpty(actor) ? currentActor : actor.ToString(), true);
                //IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowInstanceId, dataItem, currentActor, true);
                
                activityActors.Add(new WorkflowSelectorContext.ActivityActor(dataItem.ActivityId, dataItem.ActivityName, dict));
            }

            string activityItemTemplate = null;
            if (isCheckBox)
            {
                activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5},'{7}',{8});\" {4} /><span{6}>{2}</span>";
                //activityItemTemplate = "<input type=\"checkbox\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleCheckBoxAllocator('activityAllocatee{3}',this,'actors_{0}',{5});\" {4} /><span{6}>{2}</span>";
                return BuildDefaultCheckBoxActivities(activityActors, selectAll, activityItemTemplate, Template_ActorInputHtml);
            }
            else
            {
                activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5},'{7}',{8});\" {4} /><span{6}>{2}</span>";
                //activityItemTemplate = "<input type=\"radio\" id=\"activityOption_{0}\" name=\"activityOption\" value=\"{1}\" activity=\"{2}\" onclick=\"toggleRadioAllocator('activityAllocatee{3}', this, 'actors_{0}',{5});\" {4} checkLimit=\"{7}\" /><span{6}>{2}</span>";
                return BuildDefaultRadioActivities(activityActors, activityItemTemplate, Template_ActorInputHtml);
            }
        }

        /// <summary>
        /// 以单选框形式呈现默认活动选择器 Html.
        /// </summary>
        /// <param name="activityActors"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string BuildDefaultRadioActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, string activityItemTemplate, string actorItemTemplate)
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
                //if (!string.IsNullOrEmpty(WorkflowName))
                //{
                //    DataTable dt = GetWorkflowManage(WorkflowName, item.ActivityName);
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                //        isSelectedActor = Botwave.Commons.DbUtils.ToBoolean(dt.Rows[0]["managevisible"], false);
                //        Selected = Botwave.Commons.DbUtils.ToInt32(dt.Rows[0]["manageactorcount"]);
                //    }
                //}
                if (activityProfiles.ContainsKey(item.ActivityId))
                {
                    ActivityProfile activityProfile = activityProfiles[item.ActivityId];
                    isSelectedActor = activityProfile.ManageVisible;
                    Selected = activityProfile.ManageActorCount;
                }
                //Selected = Selected < 0 ? SelectedBound : Selected;//-1时则按默认选中数量
                htmlBuilder.AppendFormat(activityItemTemplate, i, item.ActivityId, item.ActivityName, "_" + i, propeties, selectDisable, GetSelectedClass(isActivityChecked), isSelectedActor,Selected);
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

                    //string actorCheckText = GetCheckedAttribute(isSelectedActor);
                    //if (!isSelectedActor)
                    //{
                    //    isSelectedActor = (actorCount <= SelectedBound && isActivityChecked);
                    //    actorCheckText = GetCheckedAttribute(isSelectedActor);
                    //}
                    //string selectedClassName = GetSelectedClass(isSelectedActor);
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

                            //if (Selected >= 0 && isSelectedActor)
                            //{
                            //    if (keyIndex >= Selected)
                            //    {
                            //        actorCheckText = GetCheckedAttribute(false);
                            //    }
                            //}
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
                            actorsBuilder.AppendFormat(actorItemTemplate,
                                i, keyIndex, item.ActivityId, key, proxyUser,
                                selectDisable, actorCheckText, selectedClassName, displayActorName);

                            keyIndex++;
                        }
                        #region 全公司人员判断
                        htmlBuilder.Append(ChooseFromCompanyHtml);
                        #endregion
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
        /// <param name="selectAll"></param>
        /// <param name="activityItemTemplate"></param>
        /// <param name="actorItemTemplate"></param>
        /// <returns></returns>
        protected virtual string BuildDefaultCheckBoxActivities(IList<WorkflowSelectorContext.ActivityActor> activityActors, bool selectAll, string activityItemTemplate, string actorItemTemplate)
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
                            if (Selected >= 0 && isSelectedActor)
                            {
                                if (keyIndex >= Selected)
                                {
                                    actorCheckText = GetCheckedAttribute(false);
                                    selectedClassName = GetSelectedClass(false);
                                }
                            }
                            string key = detail.UserName;
                            if (!actors.ContainsKey(key)) // 用户不存在.
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
                        #region 全公司人员判断
                        htmlBuilder.Append(ChooseFromCompanyHtml);
                        #endregion
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
                            if (Selected >= 0 && isSelectedActor)
                            {
                                if (keyIndex >= Selected)
                                {
                                    actorCheckText = GetCheckedAttribute(false);
                                    selectedClassName = GetSelectedClass(false);
                                }
                            }
                            string key = detail.UserName;
                            if (!actors.ContainsKey(key)) // 用户不存在.
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
                        #region 全公司人员判断
                        //string ChooseFromCompanyHtml =GetChooseFromCompanyHtml(item.ActivityId, i);
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

        protected virtual void OnAddProperties(WorkflowSelectorContext.ActivityActor item, StringBuilder builder)
        {
            if (this.AddProperties != null)
            {
                AddProperties.Invoke(item, builder);
            }
        }

        /// <summary>
        /// 获取处理姓名字典.
        /// </summary>
        /// <param name="actors"></param>
        /// <param name="sortActors"></param>
        /// <returns></returns>
        protected virtual IDictionary<string, string>  OnGetActorNames(ICollection<string> actors, out IList<string> sortActors)
        {
            sortActors = new List<string>();
            IDictionary<string, string> detail = workflowUserService.GetActorRealNames(actors);

            return detail;
        }

        /// <summary>
        /// 获取被选中的 html 属性文本.
        /// </summary>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        protected static string GetCheckedAttribute(bool isChecked)
        {
            return (isChecked ? " checked=\"checked\"" : "");
        }

        /// <summary>
        /// 获取被选中的样式.
        /// </summary>
        /// <param name="isSelected"></param>
        /// <returns></returns>
        protected static string GetSelectedClass(bool isSelected)
        {
            return (isSelected ? " class=\"spanFocus\"" : "");
        }

        protected string GetWorkflowName(Guid workflowId)
        {
            return IBatisMapper.Mapper.QueryForObject<string>("bwwf_Workflows_Select_WfName_ByWorkflowId", workflowId);
        }

        protected DataTable GetWorkflowManage(string workflowName, string activityname)
        {
            string strsql = string.Format(@"select workflowname,activityname,managevisible,manageactorcount from xqp_Activities_Profile where workflowname ='{0}' and activityname='{1}'", workflowName, activityname);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, strsql).Tables[0];
        }

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
        #endregion
    }
}
