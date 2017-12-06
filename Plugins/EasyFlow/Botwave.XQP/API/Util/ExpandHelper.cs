using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Botwave.XQP.API.Enums;
using System.Xml.Linq;
using System.Data;
using Botwave.XQP.API.Service;
using Botwave.Easyflow.API.Entity;
using Botwave.XQP.API.Interface;
using Spring.Context.Support;

namespace Botwave.XQP.API.Util
{
    /// <summary>
    /// 处理扩展类
    /// </summary>
    public class ExpandHelper
    {
        private ISearchAPIService searchAPIService;
        private SearchHelper searchHelper;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExpandHelper()
        {
            searchAPIService =(ISearchAPIService) ContextRegistry.GetContext().GetObject("SearchAPIService"); 
            //searchAPIService = WebApplicationContext.GetRootContext()["SearchAPIService"] as ISearchAPIService;//注入名称目前有重复,稍后改正
            searchHelper = new SearchHelper();
        }

        /// <summary>
        /// 查询类返回值扩展方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="search"></param>
        /// <param name="xContent"></param>
        /// <returns></returns>
        public XElement GetExpandSearchXML(DataTable dt, SearchEntity search, XElement xContent)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            object obj = Enum.Parse(typeof(SearchCommandEnum), search.Action);
            SearchCommandEnum command = (SearchCommandEnum)obj;
            XElement item = null;
            string WorkflowInstanceId = string.Empty, ActivityInstanceId = string.Empty, action = search.Action;
            switch (command)
            {
                case SearchCommandEnum.detail:
                    #region

                    WorkflowInstanceId = dt.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                    DataTable FieldDt = searchAPIService.GetWorkflowFieldList(WorkflowInstanceId);//表单列表
                    DataTable ActivitysDt = searchAPIService.GetWorkflowActivitysList(WorkflowInstanceId);//步骤列表
                    DataTable AttachmentDt = searchAPIService.GetWorkflowAttachmentList(WorkflowInstanceId);//附件列表
                    item = new XElement(action);
                    item = GetNodeXml(dt, item, dt.Rows[0]);
                    if (FieldDt != null)
                    {
                        foreach (DataRow row in FieldDt.Rows)
                        {
                            XElement FieldItem = new XElement("Field");
                            FieldItem = GetNodeXml(FieldDt, FieldItem, row);
                            item.Add(FieldItem);
                        }
                    }
                    if (ActivitysDt != null)
                    {
                        foreach (DataRow row in ActivitysDt.Rows)
                        {
                            XElement ActivityItem = new XElement("Activitys");
                            ActivityItem = GetNodeXml(ActivitysDt, ActivityItem, row);
                            item.Add(ActivityItem);
                        }
                    }
                    if (AttachmentDt != null)
                    {
                        foreach (DataRow row in AttachmentDt.Rows)
                        {
                            XElement AttachmentItem = new XElement("Attachment");
                            AttachmentItem = GetNodeXml(AttachmentDt, AttachmentItem, row);
                            item.Add(AttachmentItem);
                        }
                    }
                    xContent.Add(item);

                    #endregion
                    break;

                case SearchCommandEnum.infolist:
                    #region

                    if (!string.IsNullOrEmpty(search.ActivityInstanceId))//已经调用了获取下一步处理人
                    {
                        action = "NextActivity";
                        if (!string.IsNullOrEmpty(search.WorkflowAlias) || !string.IsNullOrEmpty(search.Workflows))
                        {
                            DataTable fieldDt = searchAPIService.GetFieldInfoList(search.WorkflowAlias, search.Workflows);
                            foreach (DataRow row in fieldDt.Rows)
                            {
                                item = new XElement("FieldInfo");
                                item = GetNodeXml(fieldDt, item, row);
                                xContent.Add(item);
                            }
                        }

                    }
                    else
                    {
                        action = "FieldInfo";
                        if (!string.IsNullOrEmpty(search.ActivityInstanceId))
                        {
                            DataTable nextActivityDt = searchAPIService.GetNextActivityInfoList(search.UserName, search.ActivityInstanceId);
                            foreach (DataRow row in nextActivityDt.Rows)
                            {
                                item = new XElement("NextActivity");
                                item = GetNodeXml(nextActivityDt, item, row);
                                xContent.Add(item);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(search.WorkflowAlias) || !string.IsNullOrEmpty(search.Workflows))
                    {
                        DataTable activityDt = searchAPIService.GetActivityInfoList(search.UserName, search.WorkflowAlias,search.Workflows);
                        foreach (DataRow row in activityDt.Rows)
                        {
                            item = new XElement("Activity");
                            item = GetNodeXml(activityDt, item, row);
                            xContent.Add(item);
                        }

                        //抄送--
                        DataTable profileDt = searchAPIService.GetActivitiesProfileInfoList(search.UserName, search.WorkflowAlias, search.Workflows);
                        foreach (DataRow row in profileDt.Rows)
                        {
                            item = new XElement("ActivitiesProfile");
                            item = GetNodeXml(profileDt, item, row);
                            xContent.Add(item);
                        }
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                    }

                    #endregion
                    break;

                case SearchCommandEnum.recordlist:
                    #region

                    WorkflowInstanceId = dt.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                    DataTable AssignmentDt = searchAPIService.GetWorkflowRecordAssignmentList(WorkflowInstanceId);//转交列表

                    foreach (DataRow row in dt.Rows)
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);

                        ActivityInstanceId = Botwave.Commons.DbUtils.ToString(row["ActivityInstanceId"]);

                        #region 转交
                        if (AssignmentDt != null)
                        {
                            XElement AssignmentItem = null;
                            foreach (DataRow row1 in AssignmentDt.Rows)
                            {
                                string AssignmentActivityInstanceId = row1["ActivityInstanceId"].ToString().Trim();
                                if (AssignmentActivityInstanceId == ActivityInstanceId)
                                {
                                    AssignmentItem = new XElement(action);
                                    GetNodeXml(AssignmentDt, AssignmentItem, row1);
                                    xContent.Add(AssignmentItem);
                                }
                            }
                        }
                        #endregion

                        #region 会签
                        DataTable CountersignedDt = searchAPIService.GetWorkflowRecordCountersignedList(ActivityInstanceId);//会签列表
                        if (CountersignedDt != null)
                        {
                            XElement CountersignedItem = null;
                            foreach (DataRow row2 in CountersignedDt.Rows)
                            {
                                CountersignedItem = new XElement(action);
                                GetNodeXml(CountersignedDt, CountersignedItem, row2);
                                xContent.Add(CountersignedItem);
                            }
                        }

                        #endregion
                    }
                    #endregion
                    break;

                case SearchCommandEnum.searchquery:
                    #region

                    foreach (DataRow row in dt.Rows)//流程
                    {
                        item = new XElement("Workflow");
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                    }
                    if (!string.IsNullOrEmpty(search.Workflows))
                    {
                        DataTable ActivityDt = searchAPIService.GetSearchQueryList(search.UserName, search.Workflows);//流程步骤
                        foreach (DataRow row in ActivityDt.Rows)//流程
                        {
                            item = new XElement("Activity");
                            item = GetNodeXml(ActivityDt, item, row);
                            xContent.Add(item);
                        }
                    }

                    #endregion
                    break;

                case SearchCommandEnum.commentlist:
                    #region

                    foreach (DataRow row in dt.Rows)//评论列表
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        string id = row["id"].ToString();
                        DataTable CommentAttachemntDt = searchAPIService.GetCommentAttachemntList(new Guid(id), search.WorkflowInstanceId);

                        if (!Object.Equals(CommentAttachemntDt, null))
                        {
                            foreach (DataRow dr in CommentAttachemntDt.Rows)//附件信息
                            {
                                XElement itemAttachemnt = new XElement(action);
                                item.Add(GetNodeXml(CommentAttachemntDt, itemAttachemnt, dr));
                            }
                        }

                        xContent.Add(item);
                    }

                    #endregion
                    break;

                default:
                    foreach (DataRow row in dt.Rows)
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                    }
                    break;
            }
            return xContent;
        }

        /// <summary>
        /// 查询类返回值扩展方法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="search"></param>
        /// <param name="xContent"></param>
        /// <returns></returns>
        public XElement GetExpandSearchXML(DataTable dt, SearchEntity search, XElement xContent, IDictionary<string, object> formVariables)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            object obj = Enum.Parse(typeof(SearchCommandEnum), search.Action);
            SearchCommandEnum command = (SearchCommandEnum)obj;
            XElement item = null;
            string WorkflowInstanceId = string.Empty, ActivityInstanceId = string.Empty, action = search.Action;
            switch (command)
            {
                case SearchCommandEnum.detail:
                    #region

                    WorkflowInstanceId = dt.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                    DataTable FieldDt = searchHelper.GetWorkflowFieldList(WorkflowInstanceId, formVariables);//表单列表
                    DataTable ActivitysDt = searchHelper.GetWorkflowActivitiesList(WorkflowInstanceId);//步骤列表
                    //DataTable PreActivitysDt = searchAPIService.GetWorklfowPreActivitysList(WorkflowInstanceId);//上行步骤列表
                    DataTable NextActivitysDt = searchHelper.GetWorkflowNextActivitysList(WorkflowInstanceId);//下行步骤列表
                    DataTable AttachmentDt = searchHelper.GetWorkflowAttachmentList(WorkflowInstanceId);//附件列表
                    item = new XElement(action);
                    item = GetNodeXml(dt, item, dt.Rows[0]);
                    if (FieldDt != null)
                    {
                        foreach (DataRow row in FieldDt.Rows)
                        {
                            XElement FieldItem = new XElement("Field");
                            FieldItem = GetNodeXml(FieldDt, FieldItem, row);
                            item.Add(FieldItem);
                        }
                    }
                    if (ActivitysDt != null)
                    {
                        foreach (DataRow row in ActivitysDt.Rows)
                        {
                            XElement ActivityItem = new XElement("Activitys");
                            ActivityItem = GetNodeXml(ActivitysDt, ActivityItem, row);
                            item.Add(ActivityItem);
                        }
                    }
                    if (NextActivitysDt != null)
                    {
                        foreach (DataRow row in NextActivitysDt.Rows)
                        {
                            XElement ActivityItem = new XElement("NextActivitys");
                            ActivityItem = GetNodeXml(NextActivitysDt, ActivityItem, row);
                            item.Add(ActivityItem);
                        }
                    }
                    if (AttachmentDt != null)
                    {
                        foreach (DataRow row in AttachmentDt.Rows)
                        {
                            XElement AttachmentItem = new XElement("Attachment");
                            AttachmentItem = GetNodeXml(AttachmentDt, AttachmentItem, row);
                            item.Add(AttachmentItem);
                        }
                    }
                    xContent.Add(item);

                    #endregion
                    break;

                case SearchCommandEnum.activitieslist:
                    #region

                    WorkflowInstanceId = dt.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                    ActivityInstanceId = dt.Rows[0]["ActivityInstanceId"].ToString().Trim();
                    DataTable ActivitiesDt = searchHelper.GetWorkflowActivitiesList(WorkflowInstanceId);//步骤列表
                    DataTable PreActivityDt = searchHelper.GetWorklfowPreActivityList(ActivityInstanceId);//上行步骤列表
                    DataTable NextActivitiesDt = searchHelper.GetWorkflowNextActivitysList(WorkflowInstanceId);//下行步骤列表
                    item = new XElement(action);
                    item = GetNodeXml(dt, item, dt.Rows[0]);
                    if (PreActivityDt != null)
                    {
                        foreach (DataRow row in PreActivityDt.Rows)
                        {
                            XElement ActivityItem = new XElement("PreActivitys");
                            ActivityItem = GetNodeXml(PreActivityDt, ActivityItem, row);
                            item.Add(ActivityItem);
                        }
                    }
                    if (ActivitiesDt != null)
                    {
                        foreach (DataRow row in ActivitiesDt.Rows)
                        {
                            XElement ActivityItem = new XElement("Activitys");
                            ActivityItem = GetNodeXml(ActivitiesDt, ActivityItem, row);
                            item.Add(ActivityItem);
                        }
                    }
                    if (NextActivitiesDt != null)
                    {
                        foreach (DataRow row in NextActivitiesDt.Rows)
                        {
                            XElement ActivityItem = new XElement("NextActivitys");
                            ActivityItem = GetNodeXml(NextActivitiesDt, ActivityItem, row);
                            item.Add(ActivityItem);
                        }
                    }
                    xContent.Add(item);

                    #endregion
                    break;

                case SearchCommandEnum.infolist:
                    #region
                    /*
                    if (!string.IsNullOrEmpty(search.ActivityInstanceId))//已经调用了获取下一步处理人
                    {
                        action = "NextActivity";
                        if (!string.IsNullOrEmpty(search.WorkflowAlias) || !string.IsNullOrEmpty(search.Workflows))
                        {
                            DataTable fieldDt = searchAPIService.GetFieldInfoList(search.WorkflowAlias, search.Workflows);
                            foreach (DataRow row in fieldDt.Rows)
                            {
                                item = new XElement("FieldInfo");
                                item = GetNodeXml(fieldDt, item, row);
                                xContent.Add(item);
                            }
                        }

                    }
                    else
                    {
                        action = "FieldInfo";
                        if (!string.IsNullOrEmpty(search.ActivityInstanceId))
                        {
                            DataTable nextActivityDt = searchAPIService.GetNextActivityInfoList(search.UserName, search.ActivityInstanceId);
                            foreach (DataRow row in nextActivityDt.Rows)
                            {
                                item = new XElement("NextActivity");
                                item = GetNodeXml(nextActivityDt, item, row);
                                xContent.Add(item);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(search.WorkflowAlias) || !string.IsNullOrEmpty(search.Workflows))
                    {
                        DataTable activityDt = searchAPIService.GetActivityInfoList(search.UserName, search.WorkflowAlias,search.Workflows);
                        foreach (DataRow row in activityDt.Rows)
                        {
                            item = new XElement("Activity");
                            item = GetNodeXml(activityDt, item, row);
                            xContent.Add(item);
                        }
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                    }
                    */
                    #endregion
                    break;
                #region
                case SearchCommandEnum.recordlist:
                    #region

                    WorkflowInstanceId = dt.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                    DataTable AssignmentDt = searchAPIService.GetWorkflowRecordAssignmentList(WorkflowInstanceId);//转交列表
                    if (AssignmentDt != null)
                        AssignmentDt.Columns.Add("ActivityName");
                    foreach (DataRow row in dt.Rows)
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                        ActivityInstanceId = Botwave.Commons.DbUtils.ToString(row["ActivityInstanceId"]);

                        #region 转交
                        if (AssignmentDt != null)
                        {
                            XElement AssignmentItem = null;
                            foreach (DataRow row1 in AssignmentDt.Rows)
                            {
                                string AssignmentActivityInstanceId = row1["ActivityInstanceId"].ToString().Trim();
                                if (AssignmentActivityInstanceId == ActivityInstanceId)
                                {
                                    AssignmentItem = new XElement(action);
                                    row1["ActivityName"] = row["ActivityName"];
                                    GetNodeXml(AssignmentDt, AssignmentItem, row1);
                                    xContent.Add(AssignmentItem);
                                }
                            }
                        }
                        #endregion

                        #region 会签
                        DataTable CountersignedDt = searchAPIService.GetWorkflowRecordCountersignedList(ActivityInstanceId);//会签列表
                        if (CountersignedDt != null)
                        {
                            CountersignedDt.Columns.Add("ActivityName");
                            XElement CountersignedItem = null;
                            foreach (DataRow row2 in CountersignedDt.Rows)
                            {
                                CountersignedItem = new XElement(action);
                                row2["ActivityName"] = row["ActivityName"];
                                GetNodeXml(CountersignedDt, CountersignedItem, row2);
                                xContent.Add(CountersignedItem);
                            }
                        }

                        #endregion
                    }
                    #endregion
                    break;

                /*case SearchCommandEnum.searchquery:
                    #region

                    foreach (DataRow row in dt.Rows)//流程
                    {
                        item = new XElement("Workflow");
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                    }
                    if (!string.IsNullOrEmpty(search.Workflows))
                    {
                        DataTable ActivityDt = searchAPIService.GetSearchQueryList(search.UserName, search.Workflows);//流程步骤
                        foreach (DataRow row in ActivityDt.Rows)//流程
                        {
                            item = new XElement("Activity");
                            item = GetNodeXml(ActivityDt, item, row);
                            xContent.Add(item);
                        }
                    }

                    #endregion
                    break;

                case SearchCommandEnum.commentlist:
                    #region

                    foreach (DataRow row in dt.Rows)//评论列表
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        string id = row["id"].ToString();
                        DataTable CommentAttachemntDt = searchAPIService.GetCommentAttachemntList(new Guid(id), search.WorkflowInstanceId);

                        if (!Object.Equals(CommentAttachemntDt, null))
                        {
                            foreach (DataRow dr in CommentAttachemntDt.Rows)//附件信息
                            {
                                XElement itemAttachemnt = new XElement(action);
                                item.Add(GetNodeXml(CommentAttachemntDt, itemAttachemnt, dr));
                            }
                        }

                        xContent.Add(item);
                    }

                    #endregion
                    break;*/
                #endregion
                default:
                    foreach (DataRow row in dt.Rows)
                    {
                        item = new XElement(action);
                        item = GetNodeXml(dt, item, row);
                        xContent.Add(item);
                    }
                    break;
            }
            return xContent;
        }

        /// <summary>
        /// 处理类返回值扩展方法
        /// </summary>
        /// <param name="strResult"></param>
        /// <param name="manage"></param>
        /// <param name="xContent"></param>
        /// <returns></returns>
        public XElement GetExpandManageXML(string strResult, ManageEntity manage, XElement xContent)
        {
            object obj = Enum.Parse(typeof(ManageCommandEnum), manage.Action);
            ManageCommandEnum command = (ManageCommandEnum)obj;
            XElement item = null;
            string WorkflowInstanceId = string.Empty, ActivityInstanceId = string.Empty, action = manage.Action;
            switch (command)
            {
                case ManageCommandEnum.start://发单
                    item = new XElement(action);
                    //XElement x = new XElement("item", new XAttribute("name", "WorkflowInstanceId"), new XAttribute("value", strResult));
                    //item.Add(x);
                    //xContent.Add(item);
                    for (int i = 0; i < 2; i++)
                    {

                        if (i == 0)
                        {
                            XElement x = new XElement("item", new XAttribute("name", "WorkflowInstanceId"), new XAttribute("value", strResult));
                            
                            item.Add(x);

                        }
                        else if (i == 1)
                        {
                            Guid guid = new Guid(strResult);
                            string activityinstanceid = ManageActivityinstance.GetActivityinstanceid(guid);
                            XElement x = new XElement("item", new XAttribute("name", "ActivityinstanceId"), new XAttribute("value", activityinstanceid));
                            item.Add(x);
                        }
                    }
                    xContent.Add(item);
                    break;
                case ManageCommandEnum.execute://处理单
                    item = new XElement(action);
                    XElement xe = new XElement("item", new XAttribute("name", "ActivityinstanceId"), new XAttribute("value", strResult));
                    item.Add(xe);
                    xContent.Add(item);
                    break;
                default:
                    break;
            }
            return xContent;
        }

        private XElement GetNodeXml(DataTable dt, XElement item, DataRow row)
        {
            foreach (DataColumn col in dt.Columns)
            {
                string ColumnValue = string.Empty;
                ColumnValue = row[col.ColumnName] == null ? "" : row[col.ColumnName].ToString();

                XElement x = new XElement("item", new XAttribute("name", col.ColumnName), new XAttribute("value", ColumnValue));
                item.Add(x);
            }
            return item;
        }
    }
}
