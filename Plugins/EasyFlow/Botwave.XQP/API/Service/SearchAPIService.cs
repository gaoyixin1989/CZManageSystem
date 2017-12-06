using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using Botwave.Commons;
using System.Data.SqlClient;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Service;
using Botwave.XQP.API.Util;
using System.Collections;
using Botwave.XQP.Util;
using Botwave.Easyflow.API;
using Botwave.Workflow.Domain;
using Botwave.XQP.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Routing.Implements;
using Botwave.Workflow.Routing.Domain;
using System.Xml.Linq;
using Botwave.XQP.API.Entity;

namespace Botwave.XQP.API.Service
{
    /// <summary>
    /// 查询类
    /// </summary>
    public class SearchAPIService : ISearchAPIService
    {
        #region 注入

        private IActivityService activityService;
        private IWorkflowPagerService workflowPagerService;
        private Botwave.Workflow.Service.IActivityDefinitionService activityDefinitionService;
        private Botwave.Workflow.Service.IActivityAllocationService activityAllocationService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IWorkflowSearcher workflowSearcher;
        private ICommentService commentService;
        private IActivityRulesService activityRulesService;
        private IWorkflowService workflowService;

        /// <summary>
        /// 待办
        /// </summary>
        public IActivityService ActivityService
        {
            set { activityService = value; }
        }
        /// <summary>
        /// 已办
        /// </summary>
        public IWorkflowPagerService WorkflowPagerService
        {
            set { workflowPagerService = value; }
        }
        /// <summary>
        /// 信息
        /// </summary>
        public Botwave.Workflow.Service.IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }
        /// <summary>
        /// 信息
        /// </summary>
        public Botwave.Workflow.Service.IActivityAllocationService ActivityAllocationService
        {
            set { activityAllocationService = value; }
        }
        /// <summary>
        /// 高级查询条件
        /// </summary>
        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }
        /// <summary>
        /// 高级查询
        /// </summary>
        public IWorkflowSearcher WorkflowSearcher
        {
            set { workflowSearcher = value; }
        }
        /// <summary>
        /// 评论
        /// </summary>
        public ICommentService CommentService
        {
            set { commentService = value; }
        }

        public IActivityRulesService ActivityRulesService
        {
            set { activityRulesService = value; }
        }

        public IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }
        #endregion

        /// 获取指定用户的待办列表
        /// </summary>
        /// <param name="userName">登录用户</param>
        /// <param name="workflowName">指定获取数据的的流程名(多个以,号隔开)</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetTodoList(string userName, string workflowName, string keyWords, string pageIndex, string pageCount, ref int recordCount)
        {
            DataTable dtReturn = null;

            #region 数据过滤

            if (!string.IsNullOrEmpty(workflowName))
            {
                int inta = workflowName.LastIndexOf(',');
                if (inta == workflowName.Length - 1)
                    workflowName = workflowName.Remove(inta, 1);
            }

            #endregion

            dtReturn = activityService.GetTaskListByUserName(userName, workflowName, keyWords, int.Parse(pageIndex), int.Parse(pageCount), ref recordCount);

            return dtReturn;
        }

        /// <summary>
        /// 获取指定用户的已办列表
        /// </summary>
        /// <param name="userName">登录用户</param>
        /// <param name="workflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetDoneList(string userName, string workflowName, string keyWords, string beginTime, string endTime, string pageIndex, string pageCount, ref int recordCount)
        {
            DataTable dtReturn = null;

            #region 数据过滤

            if (!string.IsNullOrEmpty(workflowName))
            {
                int inta = workflowName.LastIndexOf(',');
                if (inta == workflowName.Length - 1)
                    workflowName = workflowName.Remove(inta, 1);
            }

            DateTime minValue = DateTime.Now.AddMonths(-1);
            DateTime maxValue = DateTimeUtils.MaxValue;
            if (!string.IsNullOrEmpty(beginTime))
            {
                minValue = Convert.ToDateTime(beginTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                maxValue = Convert.ToDateTime(endTime);
            }

            #endregion

            dtReturn = workflowPagerService.GetDoneTaskPager(workflowName, userName, keyWords, minValue.ToString("yyyy/MM/dd"), maxValue.ToString("yyyy/MM/dd"), false, int.Parse(pageIndex), int.Parse(pageCount), ref recordCount);

            return dtReturn;
        }

        /// <summary>
        /// 获取指定用户的“我的工单”列表信息
        /// </summary>
        /// <param name="userName">登录用户</param>
        /// <param name="workflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="keyWords">关键字</param>
        /// <param name="state">工单状态(0:所有，1:未完成，2:已完成，3:可撤回)</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetMyTasksList(string userName, string workflowName, string keyWords, string state, string beginTime, string endTime, string pageIndex, string pageCount, ref int recordCount)
        {
            DataTable dtReturn = null;

            #region 数据过滤

            if (!string.IsNullOrEmpty(workflowName))
            {
                int inta = workflowName.LastIndexOf(',');
                if (inta == workflowName.Length - 1)
                    workflowName = workflowName.Remove(inta, 1);
            }

            DateTime minValue = DateTime.Now.AddMonths(-1);
            DateTime maxValue = DateTimeUtils.MaxValue;
            if (!string.IsNullOrEmpty(beginTime))
            {
                minValue = Convert.ToDateTime(beginTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                maxValue = Convert.ToDateTime(endTime);
            }

            if (string.IsNullOrEmpty(state))
            {
                state = "0";
            }

            //pageIndex = (int.Parse(pageIndex) - 1).ToString();//我的工单的分页从0开始

            #endregion

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("WorkflowName", SqlDbType.NVarChar, 200),
                new SqlParameter("State", SqlDbType.Int),
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Keywords", SqlDbType.NVarChar, 200),
                new SqlParameter("StartTime", SqlDbType.VarChar, 50),
                new SqlParameter("EndTime", SqlDbType.VarChar, 50),
                new SqlParameter("PageIndex", SqlDbType.Int),
                new SqlParameter("PageSize", SqlDbType.Int),
                new SqlParameter("RecordCount", SqlDbType.Int)
            };
            parameters[0].Value = workflowName;
            parameters[1].Value = int.Parse(state);
            parameters[2].Value = userName;
            parameters[3].Value = keyWords;
            parameters[4].Value = minValue.ToString("yyyy/MM/dd");
            parameters[5].Value = maxValue.ToString("yyyy/MM/dd");
            parameters[6].Value = int.Parse(pageIndex);
            parameters[7].Value = int.Parse(pageCount);
            parameters[8].Direction = ParameterDirection.Output;

            dtReturn = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "xqp_API_GetMyTask", parameters).Tables[0];
            recordCount = (int)parameters[8].Value;

            return dtReturn;

        }

        #region 获取指定需求单处理列表

        /// <summary>
        /// 处理列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单工单号sheetId</param>
        /// <returns></returns>
        public DataTable GetWorkflowRecordActivityList(string workflowInstanceId, string sheetId)
        {
            DataTable dtReturn = null;
            if (XmlAnalysisHelp.ToGuid(workflowInstanceId)==null && string.IsNullOrEmpty(sheetId))
            {
                throw new WorkflowAPIException(14, "workflowInstanceId,sheetId");
            }

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowInstanceId);
            ha.Add("SheetId", sheetId);
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_ActivityInstance", ha);

            return dtReturn;
        }

        /// <summary>
        /// 转交列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <returns></returns>
        public DataTable GetWorkflowRecordAssignmentList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_Assignment", workflowInstanceId);//转交信息

            return dtReturn;
        }

        /// <summary>
        /// 会签列表
        /// </summary>
        /// <param name="activityInstanceId">需求单工单标识活动实例ID</param>
        /// <returns></returns>
        public DataTable GetWorkflowRecordCountersignedList(string activityInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowRecord_Countersigned", activityInstanceId);//会签信息

            return dtReturn;
        }

        #endregion

        #region 获取详细信息

        /// <summary>
        /// 获取表单详细信息
        /// </summary>
        /// <param name="workflowAlias">流程类别简写</param>
        /// <param name="workflowName">流程类别中文</param>
        /// <returns></returns>
        public DataTable GetFieldInfoList(string workflowAlias, string workflowName)
        {
            DataTable dtReturn = null;
            if (string.IsNullOrEmpty(workflowAlias) && string.IsNullOrEmpty(workflowName))
            {
                throw new WorkflowAPIException(14, "workflowAlias,workflowName");
            }

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowAlias", workflowAlias);
            ha.Add("WorkflowName", workflowName);
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowInfo_Field", ha);

            return dtReturn;
        }

        /// <summary>
        /// 获取流程提单的处理人
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="workflowAlias">流程类别</param>
        /// <param name="workflowName">流程类别中文</param>
        /// <returns></returns>
        public DataTable GetActivityInfoList(string userName, string workflowAlias, string workflowName)
        {
            if (string.IsNullOrEmpty(workflowAlias)) workflowAlias = "###@@@";
            if (string.IsNullOrEmpty(workflowName)) workflowName = "###@@@";

            DataTable dtReturn = null;
            if (string.IsNullOrEmpty(workflowAlias) && string.IsNullOrEmpty(workflowName))
            {
                throw new WorkflowAPIException(14, "workflowAlias,workflowName");
            }

            dtReturn = new DataTable();
            dtReturn.Columns.Add("Name");
            dtReturn.Columns.Add("Actors");

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowAlias", workflowAlias);
            ha.Add("WorkflowName", workflowName);
            DataTable soure = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowInfo_Activity", ha);

            if (soure != null && soure.Rows.Count > 0)
            {
                Guid workflowid = (Guid)soure.Rows[0]["workflowid"];
                IList<Botwave.Workflow.Domain.ActivityDefinition> activities = activityDefinitionService.GetStartActivities(workflowid);
                for (int i = 0; i < activities.Count; i++)
                {
                    Botwave.Workflow.Domain.ActivityDefinition dataItem = activities[i];
                    IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, userName, true);
                    //ArrayList arr = new ArrayList();
                    List<string> arr = new List<string>();
                    foreach (KeyValuePair<string, string> pair in dict)
                    {
                        arr.Add(pair.Key);
                    }
                    DataRow dr = dtReturn.NewRow();
                    dr["Name"] = dataItem.ActivityName;
                    dr["Actors"] = CommonHelper.GetRealNames((string[])arr.ToArray());
                    dtReturn.Rows.Add(dr);
                }
            }

            return dtReturn;
        }

        /// <summary>
        /// 获取流程提单的抄送人
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="workflowAlias">流程类别</param>
        /// <param name="workflowName">流程类别中文</param>
        /// <returns></returns>
        public DataTable GetActivitiesProfileInfoList(string userName, string workflowAlias, string workflowName)
        {
            if (string.IsNullOrEmpty(workflowAlias)) workflowAlias = "###@@@";
            if (string.IsNullOrEmpty(workflowName)) workflowName = "###@@@";

            DataTable dtReturn = null;
            if (string.IsNullOrEmpty(workflowAlias) && string.IsNullOrEmpty(workflowName))
            {
                throw new WorkflowAPIException(14, "workflowAlias,workflowName");
            }

            dtReturn = new DataTable();
            dtReturn.Columns.Add("Name");
            dtReturn.Columns.Add("Actors");
            dtReturn.Columns.Add("ReviewType");//
            dtReturn.Columns.Add("ReviewValidateType");//是否全选
            dtReturn.Columns.Add("ReviewActorCount");//抄送人数限制           

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowAlias", workflowAlias);
            ha.Add("WorkflowName", workflowName);
            DataTable soure = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowInfo_Activity", ha);

            if (soure != null && soure.Rows.Count > 0)
            {
                Guid workflowid = (Guid)soure.Rows[0]["workflowid"];
                Botwave.XQP.Service.ReviewType reviewType = Botwave.XQP.Service.ReviewSelectorHelper.GeteviewType(workflowid);
                if (reviewType != Botwave.XQP.Service.ReviewType.None)//None为不支持抄送
                {
                    IList<Botwave.Workflow.Domain.ActivityDefinition> activities = activityDefinitionService.GetStartActivities(workflowid);
                    for (int i = 0; i < activities.Count; i++)
                    {
                        Hashtable ha2 = new Hashtable();
                        ha2.Add("WorkflowAlias", workflowAlias);
                        ha2.Add("WorkflowName", workflowName);
                        ha2.Add("ActivityName", activities[i].ActivityName);
                        DataTable profile = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowInfo_Activities_Profile", ha2);
                        if (profile == null || profile.Rows.Count == 0 || profile.Rows[0]["IsReview"].ToString() == "False")
                            continue;


                        DataRow dr = dtReturn.NewRow();
                        dr["Name"] = activities[i].ActivityName;
                        dr["ReviewType"] = reviewType;
                        dr["ReviewValidateType"] = profile.Rows[0]["ReviewValidateType"].ToString();
                        dr["ReviewActorCount"] = profile.Rows[0]["ReviewActorCount"].ToString();
                        if (reviewType == Botwave.XQP.Service.ReviewType.Classic)
                        {//旧有方式的抄送
                            dr["Actors"] = "";
                        }
                        else
                        {//复选框方式的抄送
                            string strReviewActors = profile.Rows[0]["ReviewActors"].ToString();
                            string strExtendAllocators = profile.Rows[0]["ExtendAllocators"].ToString();
                            string strExtendAllocatorArgs = profile.Rows[0]["ExtendAllocatorArgs"].ToString();

                            IDictionary<string, string> dict = new Dictionary<string, string>();
                            if (!string.IsNullOrEmpty(strReviewActors))
                            {
                                foreach (string ddtemp in strReviewActors.Split(',', '，'))
                                {
                                    if (!string.IsNullOrEmpty(ddtemp) && !dict.ContainsKey(ddtemp))
                                        dict.Add(ddtemp, null);
                                }
                            }
                            if (!string.IsNullOrEmpty(strExtendAllocators))
                            {
                                AllocatorOption options = new AllocatorOption();
                                options.ExtendAllocators = strExtendAllocators;
                                options.ExtendAllocatorArgs = strExtendAllocatorArgs;
                                IDictionary<string, string> dict2 = activityAllocationService.GetTargetUsers(Guid.Empty, options, userName, true);
                                foreach (var ddtemp in dict2)
                                {
                                    if (!dict.ContainsKey(ddtemp.Key))
                                    {
                                        dict.Add(ddtemp.Key, ddtemp.Value);
                                    }
                                }
                            }

                            List<string> arr = new List<string>();
                            foreach (KeyValuePair<string, string> pair in dict)
                            {
                                arr.Add(pair.Key);
                            }

                            dr["Actors"] = CommonHelper.GetRealNames((string[])arr.ToArray());
                        }

                        dtReturn.Rows.Add(dr);

                    }
                }

            }

            return dtReturn;
        }

        /// <summary>
        /// 获取下一步处理人
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public DataTable GetNextActivityInfoList(string userName, string activityInstanceId)
        {
            DataTable dtReturn = null;

            Guid ActivityInstanceId = new Guid(activityInstanceId);
            dtReturn = new DataTable();
            dtReturn.Columns.Add("Name");
            dtReturn.Columns.Add("Actors");
            ActivityDefinition definition=activityDefinitionService.GetActivityDefinitionByInstanceId(ActivityInstanceId);
            IList<Botwave.Workflow.Domain.ActivityDefinition> activities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(ActivityInstanceId);
            for (int i = 0; i < activities.Count; i++)
            {
                Botwave.Workflow.Domain.ActivityDefinition dataItem = activities[i];
                #region 校验规则
                RulesDetail rule=activityRulesService.GetNextActivityRules(dataItem.WorkflowId.ToString(),definition.ActivityName,dataItem.ActivityName);
                if (rule != null)
                {
                    WorkflowInstance workflowInstance = workflowService.GetWorkflowInstanceByActivityInstanceId(ActivityInstanceId);
                    rule.Workflowinstanceid = workflowInstance.WorkflowInstanceId;
                    DataTable dtPreview = activityRulesService.GetInstanceTable(workflowInstance.WorkflowId, workflowInstance.WorkflowInstanceId, userName, new Dictionary<string, object>(), true);
                    if (!activityRulesService.GetActivityRulesAnalysisResult(rule,dtPreview))
                        continue;
                }
                #endregion
                #region 增加过程控制，解决个性化定制权限角色(针对组织控制类型和历史处理人类型)
                string extendAllocatorArgs = dataItem.ExtendAllocatorArgs;
                string actor = Botwave.XQP.Domain.CZActivityInstance.GetPssorActorByAiid(ActivityInstanceId, extendAllocatorArgs);
                #endregion
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, userName, true);

                DataRow dr = dtReturn.NewRow();
                dr["Name"] = dataItem.ActivityName;
                dr["Actors"] = CommonHelper.GetRealNames((new List<string>(dict.Keys)).ToArray());
                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        #endregion

        #region 高级查询

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <returns></returns>
        public DataTable GetSearchQueryList(string userName)
        {
            DataTable dtReturn = null;

            dtReturn = new DataTable();
            dtReturn.Columns.Add("WorkflowFullName");
            dtReturn.Columns.Add("WorkflowName");

            IDictionary<string, string> Resources = CommonHelper.GetResources(userName);//获取权限

            IList<Botwave.Workflow.Domain.WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
            workflows = CommonHelper.GetAllowedWorkflows(workflows, Resources, "0004");
            foreach (Botwave.Workflow.Domain.WorkflowDefinition wd in workflows)
            {
                DataRow dr = dtReturn.NewRow();
                dr["WorkflowFullName"] = wd.WorkflowName;
                dr["WorkflowName"] = wd.WorkflowName;
                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }
        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="userName">指定用户的Portal 登录账号</param>
        /// <param name="workflowName">流程名</param>
        /// <returns></returns>
        public DataTable GetSearchQueryList(string userName, string workflowName)
        {
            DataTable dtReturn = null;

            dtReturn = new DataTable();
            dtReturn.Columns.Add("ActivityName");

            if (!string.IsNullOrEmpty(workflowName))
            {
                IList<Botwave.Workflow.Domain.WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionListByName(workflowName);
                if (workflows != null && workflows.Count > 0)
                {
                    Guid workflowId = workflows[0].WorkflowId;
                    List<Botwave.Workflow.Domain.ActivityDefinition> Activitys =new List<ActivityDefinition> (activityDefinitionService.GetActivitiesByWorkflowId(workflowId));
                    foreach (Botwave.Workflow.Domain.ActivityDefinition ad in Activitys)
                    {
                        DataRow dr = dtReturn.NewRow();
                        dr["ActivityName"] = ad.ActivityName;
                        dtReturn.Rows.Add(dr);
                    }
                }
            }


            return dtReturn;
        }

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="beginTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="workflowName">流程</param>
        /// <param name="activityName">步骤</param>
        /// <param name="creator">发起人</param>
        /// <param name="actor">当前处理人</param>
        /// <param name="titleKeywords">标题关键字</param>
        /// <param name="contentKeywords">内容关键字</param>
        /// <param name="sheetId">受理号</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetSearchList(string userName, string beginTime, string endTime, string workflowName, string activityName, string creator, string actor, string titleKeywords, string contentKeywords, string sheetId, string pageIndex, string pageCount, ref int recordCount)
        {
            DataTable dtReturn = null;

            //流程若选择全部,则需取出所有流程作为条件查询.
            if (string.IsNullOrEmpty(workflowName))
            {
                activityName = "";

                StringBuilder sbWorkflowNames = new StringBuilder();

                IDictionary<string, string> Resources = CommonHelper.GetResources(userName);

                IList<Botwave.Workflow.Domain.WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
                workflows = CommonHelper.GetAllowedWorkflows(workflows, Resources, "0004");
                foreach (Botwave.Workflow.Domain.WorkflowDefinition entity in workflows)
                {
                    sbWorkflowNames.AppendFormat("'{0}',", entity.WorkflowName);
                }
                if (sbWorkflowNames.Length > 0)
                    workflowName = sbWorkflowNames.ToString().Substring(1, sbWorkflowNames.Length - 3);
            }

            DateTime minValue = DateTime.Now.AddMonths(-1);
            DateTime maxValue = DateTimeUtils.MaxValue;
            if (!string.IsNullOrEmpty(beginTime))
            {
                minValue = Convert.ToDateTime(beginTime);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                maxValue = Convert.ToDateTime(endTime);
            }

            AdvancedSearchCondition condition = new AdvancedSearchCondition();
            condition.BeginTime = minValue.ToString("yyyy/MM/dd");
            condition.EndTime = maxValue.ToString("yyyy/MM/dd");
            condition.ActivityName = activityName;
            condition.CreatorName = creator;
            condition.Keywords = contentKeywords;
            condition.ProcessorName = actor;
            condition.SheetId = sheetId;
            condition.Title = titleKeywords;
            condition.WorkflowName = workflowName;

            dtReturn = workflowSearcher.Search(condition, int.Parse(pageIndex), int.Parse(pageCount), ref recordCount);

            return dtReturn;
        }

        #endregion

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单标识工单号sheetId</param>
        /// <returns></returns>
        public DataTable GetWorkflowState(string workflowInstanceId, string sheetId)
        {
            DataTable dtReturn = null;

            if (XmlAnalysisHelp.ToGuid(workflowInstanceId) == null && string.IsNullOrEmpty(sheetId))
            {
                throw new WorkflowAPIException(14, "workflowInstanceId,sheetId");
            }

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowInstanceId);
            ha.Add("SheetId", sheetId);
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowState", ha);

            return dtReturn;
        }

        /// <summary>
        /// 获取流程分组
        /// </summary>
        /// <returns></returns>
        public DataTable GetMenuGroup()
        {
            DataTable dtReturn = null;
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowMenuGroup", null);

            return dtReturn;
        }

        /// <summary>
        /// 获取流程列表
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="groupName">流程分组名</param>
        /// <returns></returns>
        public DataTable GetWorkflow(string userName, string groupName)
        {
            DataTable dtReturn = null;

            dtReturn = new DataTable();
            dtReturn.Columns.Add("WorkflowName");
            dtReturn.Columns.Add("WorkflowAlias");
            dtReturn.Columns.Add("WorkflowID");
            dtReturn.Columns.Add("GroupName");

            IList<Botwave.XQP.Domain.WorkflowMenuGroup> groups = Botwave.XQP.Domain.WorkflowMenuGroup.Select();
            IList<Botwave.XQP.Domain.WorkflowInMenuGroup> groupWorkflows = Botwave.XQP.Domain.WorkflowInMenuGroup.Select();//所有流程
            IDictionary<string, string> workflowResouces = ResourceHelper.GetResouceByParentId(ResourceHelper.Workflow_ResourceId);//流程权限

            #region 用户权限信息

            IDictionary<string, string> userResources = null; bool isTrue = false;
            if (!string.IsNullOrEmpty(userName))
            {
                userResources = CommonHelper.GetResources(userName);
            }
            else//用户名为空
            {
                isTrue = true;
            }

            #endregion

            if (!string.IsNullOrEmpty(groupName))//流程分组名不为空的情况下
            {
                Botwave.XQP.Domain.WorkflowMenuGroup menuGroups = null;
                foreach (Botwave.XQP.Domain.WorkflowMenuGroup g in groups)
                {
                    if (g.GroupName == groupName)
                    {
                        menuGroups = g;
                        break;
                    }
                }
                for (int i = 0; i < groupWorkflows.Count; i++)
                {
                    Botwave.XQP.Domain.WorkflowInMenuGroup item = groupWorkflows[i];
                    if (item.MenuGroupId == menuGroups.GroupID)
                    {
                        string workflowName = item.WorkflowName.ToLower();
                        if (workflowResouces.ContainsKey(workflowName))
                        {
                            DataRow dr = dtReturn.NewRow();
                            string requireResouceId = workflowResouces[workflowName] + "0000"; // 取得指定流程公用权限.
                            if (!ResourceHelper.VerifyResource(userResources, requireResouceId) && !isTrue)
                                continue;

                            dr["WorkflowName"] = item.WorkflowName;
                            dr["WorkflowAlias"] = item.WorkflowAlias;
                            dr["WorkflowID"] = item.WorkflowId.ToString();
                            dr["GroupName"] = menuGroups.GroupName;
                            dtReturn.Rows.Add(dr);
                            // 移除当前流程子目录，减少循环次数.
                            groupWorkflows.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            else
            {
                foreach (Botwave.XQP.Domain.WorkflowMenuGroup g in groups)
                {
                    for (int i = 0; i < groupWorkflows.Count; i++)
                    {
                        Botwave.XQP.Domain.WorkflowInMenuGroup item = groupWorkflows[i];
                        if (item.MenuGroupId == g.GroupID)
                        {
                            string workflowName = item.WorkflowName.ToLower();
                            if (workflowResouces.ContainsKey(workflowName))
                            {
                                DataRow dr = dtReturn.NewRow();
                                string requireResouceId = workflowResouces[workflowName] + "0000"; // 取得指定流程公用权限.
                                if (!ResourceHelper.VerifyResource(userResources, requireResouceId) && !isTrue)
                                    continue;

                                dr["WorkflowName"] = item.WorkflowName;
                                dr["WorkflowAlias"] = item.WorkflowAlias;
                                dr["WorkflowID"] = item.WorkflowId.ToString();
                                dr["GroupName"] = g.GroupName;
                                dtReturn.Rows.Add(dr);
                                // 移除当前流程子目录，减少循环次数.
                                groupWorkflows.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
            }

            return dtReturn;
        }

        #region 获取指定工单标识的需求单明细信息Detail

        /// <summary>
        /// 获取指定工单标识的明细信息
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID</param>
        /// <param name="sheetId">需求单工单工单号sheetId</param>
        /// <returns></returns>
        public DataTable GetWorkflowDetail(string workflowInstanceId, string sheetId)
        {
            DataTable dtReturn = null;
            
            if (XmlAnalysisHelp.ToGuid(workflowInstanceId) == null && string.IsNullOrEmpty(sheetId))
            {
                throw new WorkflowAPIException(14, "workflowInstanceId,sheetId");
            }

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowInstanceId);
            ha.Add("SheetId", sheetId);
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowDetail", ha);

            return dtReturn;
        }

        public DataTable GetWorkflowInfo(string workflowinstanceid)
        {
            DataTable dtReturn = null;

            if (XmlAnalysisHelp.ToGuid(workflowinstanceid) == null)
            {
                throw new WorkflowAPIException(14, "workflowInstanceId");
            }

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowinstanceid);
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowInfo", ha);

            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单的表单列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowFieldList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowDetail_Field", workflowInstanceId);

            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单的下一步列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowActivitysList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowDetail_Activitys", workflowInstanceId);

            return dtReturn;
        }

        /// <summary>
        /// 获取指定工单的附件列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetWorkflowAttachmentList(string workflowInstanceId)
        {
            DataTable dtReturn = null;

            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowInstanceId);
            ha.Add("EntityType", "W_A");
            dtReturn = APIServiceSQLHelper.QueryForDataSet("API_Select_WorkflowDetail_Attachment", ha);

            return dtReturn;
        }

        #endregion

        #region 获取评论信息

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetCommentList(string workflowInstanceId)
        {
            DataTable dtReturn = null;
            if (XmlAnalysisHelp.ToGuid(workflowInstanceId) == null )
            {
                throw new WorkflowAPIException(13, "workflowInstanceId");
            }
            dtReturn = new DataTable();
            dtReturn.Columns.Add("Id");
            dtReturn.Columns.Add("CreatedTime");
            dtReturn.Columns.Add("Message");
            dtReturn.Columns.Add("Creator");
            IList<Botwave.Workflow.Domain.Comment> comments = commentService.GetWorkflowComments(new Guid(workflowInstanceId));//评论列表

            foreach (Botwave.Workflow.Domain.Comment c in comments)
            {
                DataRow row = dtReturn.NewRow();
                row["Creator"] = c.Creator;
                row["CreatedTime"] = c.CreatedTime.ToString();
                row["CreatedTime"] = c.Message;
                row["Id"] = c.Id.Value.ToString();
                dtReturn.Rows.Add(row);
            }

            return dtReturn;
        }

        /// <summary>
        /// 评论附件信息
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetCommentAttachemntList(Guid commentId, string workflowInstanceId)
        {
            DataTable dtReturn = null;

            dtReturn = new DataTable();
            dtReturn.Columns.Add("Name");
            dtReturn.Columns.Add("Creator");
            dtReturn.Columns.Add("CreatedTime");
            dtReturn.Columns.Add("Url");
            dtReturn.Columns.Add("UserName");
            DataTable attachementTable = CommonHelper.GetAttachemnts(workflowInstanceId);

            if (attachementTable == null || attachementTable.Rows.Count == 0)
                return null;
            DataRow[] rows = attachementTable.Select(string.Format("EntityId = '{0}'", commentId));
            if (rows == null || rows.Length == 0)
                return null;
            foreach (DataRow row in rows)
            {
                DataRow dr = dtReturn.NewRow();
                dr["Name"] = row["Title"] == null ? string.Empty : row["Title"].ToString();
                dr["Creator"] = row["Creator"] == null ? string.Empty : row["Creator"].ToString();
                dr["CreatedTime"] = row["CreatedTime"] == null ? string.Empty : row["CreatedTime"].ToString();
                dr["Url"] = row["FileName"] == null ? string.Empty : row["FileName"].ToString();
                dr["UserName"] = row["Creator"] == null ? string.Empty : CommonHelper.GetRealName(row["Creator"].ToString());
                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        #endregion

        #region 获取步骤和处理人
        /// <summary>
        /// 获取步骤的处理人
        /// </summary>
        /// <param name="workflowname">流程名</param>
        /// <param name="actor">当前登录人</param>
        /// <param name="activityname">要获取处理人的步骤名</param>
        /// <returns></returns>
        public DataTable GetActivityActor(string Workflowinstanceid, string workflowname, string actor, string activityname, string workflowProperties)
        {
            DataTable dtReturn = new DataTable();
            dtReturn.Columns.Add("Actor", typeof(string));
            //Guid activityinstanceid;
            if (string.IsNullOrEmpty(Workflowinstanceid))
            {
                Workflowinstanceid = Guid.Empty.ToString();
            }
            Guid workflowinstanceid = new Guid(Workflowinstanceid);
             string _workflowID = string.Empty, FormDefinitionId = string.Empty;//FormDefinitionId 表单定义ID

            DataTable dt = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_Start_WorkflowId", workflowname);
            if (dt != null && dt.Rows.Count > 0)
            {
                _workflowID = dt.Rows[0][0].ToString();//流程定义ID
            }
            else
            { throw new WorkflowAPIException(9); }
            IList<ActivityDefinition> activities = activityDefinitionService.GetActivitiesByWorkflowId(new Guid(_workflowID)).Where(a=>a.ActivityName==activityname).ToList();
               // CZActivityDefinition.GetActivityDefinitionsByInstanceIdTo(workflowname, activityname);

            //获取步骤的处理人

            for (int i = 0; i < activities.Count; i++)
            {
                ActivityDefinition dataItem = activities[i];

                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(workflowinstanceid, dataItem, actor, true);
                
                foreach (string key in dict.Keys)
                {
                    DataRow row = dtReturn.NewRow();
                    row["Actor"] = key;
                    dtReturn.Rows.Add(row);
                }
            }

            return dtReturn;

        }


        #endregion
    }
}
