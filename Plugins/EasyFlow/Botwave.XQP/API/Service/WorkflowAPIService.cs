using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Botwave.Extension.IBatisNet;
using System.Data;
using Botwave.Commons;
using System.Data.SqlClient;
using Botwave.XQP.API.Entity;
using Botwave.XQP.API.Util;
using Botwave.DynamicForm;
using Botwave.XQP.Util;
using Botwave.Workflow.Domain;
using Botwave.Workflow;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.IBatisNet;
using System.Xml;
using Botwave.XQP.Service;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Util;
using System.Collections;
using Botwave.Security.Domain;

namespace Botwave.XQP.API.Service
{
    public class WorkflowAPIService : IWorkflowAPIService
    {
        #region

        private Botwave.Security.Service.IUserService userService;
        private Botwave.Security.Service.IRoleService roleService;
        private Botwave.Workflow.Service.IActivityService activityService;
        private Botwave.DynamicForm.Services.IFormInstanceService formInstanceService;
        private Botwave.Workflow.Service.IWorkflowService workflowService;
        private Botwave.Workflow.IWorkflowEngine workflowEngine;
        private Botwave.Workflow.Service.IActivityDefinitionService activityDefinitionService;
        private Botwave.Workflow.Service.IActivityAllocationService activityAllocationService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IResourceTranslator resourceTranslator;
        private IDeployService deployService;
        private ITaskAssignService taskAssignService;
        private IAdvancedSearcher advancedSearcher;
        private ICommentService commentService;

        public ITaskAssignService TaskAssignService
        {
            set { taskAssignService = value; }
        }
        /// <summary>
        /// 用户服务接口，只写.
        /// </summary>
        public Botwave.Security.Service.IUserService UserService
        {
            set { userService = value; }
        }

        public Botwave.Security.Service.IRoleService RoleService
        {
            set { roleService = value; }
        }

        public Botwave.Workflow.Service.IActivityService ActivityService
        {
            set { activityService = value; }
        }

        public Botwave.DynamicForm.Services.IFormInstanceService FormInstanceService
        {
            set { formInstanceService = value; }
        }

        public Botwave.Workflow.Service.IWorkflowService WorkflowService
        {
            set { workflowService = value; }
        }

        public Botwave.Workflow.IWorkflowEngine WorkflowEngine
        {
            set { workflowEngine = value; }
        }

        public Botwave.Workflow.Service.IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }

        public Botwave.Workflow.Service.IActivityAllocationService ActivityAllocationService
        {
            set { activityAllocationService = value; }
        }

        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }

        public IResourceTranslator ResourceTranslator
        {
            set { resourceTranslator = value; }
        }

        public IDeployService DeployService
        {
            set { deployService = value; }
        }

        public IAdvancedSearcher AdvancedSearcher
        {
            set { advancedSearcher = value; }
        }

        public ICommentService CommentService
        {
            set { commentService = value; }
        }

        #endregion

        /// <summary>
        /// 获取指定用户的待办列表
        /// </summary>
        /// <param name="WorkflowName">指定获取数据的的流程名(多个以,号隔开)</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Activities">指定获取数据的的流程步骤(多个以,号隔开)</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="isOnlyStart">是否本人的数据</param>
        /// <returns></returns>
        public DataTable GetTodoList(string WorkflowName, string UserName, string Activities, string BeginTime, string EndTime, bool isOnlyStart)
        {
            #region 数据过滤

            if (!string.IsNullOrEmpty(WorkflowName))
            {
                int inta = WorkflowName.LastIndexOf(',');
                if (inta == WorkflowName.Length - 1)
                    WorkflowName = WorkflowName.Remove(inta, 1);
            }

            if (!string.IsNullOrEmpty(Activities))
            {
                int inta = Activities.LastIndexOf(',');
                if (inta == Activities.Length - 1)
                    Activities = Activities.Remove(inta, 1);
            }

            DateTime startTime = DateTimeUtils.MinValue;
            DateTime endTime = DateTimeUtils.MaxValue;
            if (!string.IsNullOrEmpty(BeginTime))
                startTime = Convert.ToDateTime(BeginTime);
            if (!string.IsNullOrEmpty(EndTime))
                endTime = Convert.ToDateTime(EndTime);

            #endregion

            #region 拼接sql语句

            string tableName = "vw_bwwf_Tracking_Todo";//表名

            string fieldShow = @"ActivityInstanceId, UserName, State, ProxyName, OperateType, IsCompleted, CreatedTime,FinishedTime, Actor, ActivityName, Title, WorkflowAlias,WorkflowName, 
                          WorkflowInstanceId, SheetId,StartedTime, Urgency,expectFinishedTime, Importance, Creator, CreatorName, AliasImage, TodoActors";

            string fieldOrder = "Urgency DESC, CreatedTime DESC";

            StringBuilder where = new StringBuilder();
            where.AppendFormat("(IsCompleted = 0) ");

            if (isOnlyStart)
                where.AppendFormat("AND (UserName = '{0}')", UserName);

            if (!string.IsNullOrEmpty(WorkflowName))
                where.AppendFormat(" AND (WorkflowName in ('{0}'))", WorkflowName.Replace(",", "','"));

            if (!string.IsNullOrEmpty(Activities))
                where.AppendFormat(" AND (activityName in ('{0}'))", Activities.Replace(",", "','"));

            where.AppendFormat(" AND StartedTime >= '{0}' AND StartedTime <= '{1}' ", startTime, endTime);

            string strSql = string.Format("select {0} from {1} where {2} order by {3}", fieldShow, tableName, where.ToString(), fieldOrder);

            #endregion

            DataTable results = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];

            return results;
        }

        /// <summary>
        /// 获取已办列表
        /// </summary>
        /// <param name="WorkflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="Activities">当前步骤名(多个以,号隔开)</param>
        /// <param name="UserName">用户名</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="isOnlyStart">是否本人的数据</param>
        /// <returns></returns>
        public DataTable GetDoneList(string WorkflowName, string Activities, string UserName, string BeginTime, string EndTime, bool isOnlyStart)
        {
            int num;
            if (!string.IsNullOrEmpty(WorkflowName))
            {
                num = WorkflowName.LastIndexOf(',');
                if (num == (WorkflowName.Length - 1))
                {
                    WorkflowName = WorkflowName.Remove(num, 1);
                }
            }
            if (!string.IsNullOrEmpty(Activities))
            {
                num = Activities.LastIndexOf(',');
                if (num == (Activities.Length - 1))
                {
                    Activities = Activities.Remove(num, 1);
                }
            }
            DateTime minValue = DateTimeUtils.MinValue;
            DateTime maxValue = DateTimeUtils.MaxValue;
            if (!string.IsNullOrEmpty(BeginTime))
            {
                minValue = Convert.ToDateTime(BeginTime);
            }
            if (!string.IsNullOrEmpty(EndTime))
            {
                maxValue = Convert.ToDateTime(EndTime);
            }
            SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("WorkflowName", SqlDbType.NVarChar, 200),
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Activities", SqlDbType.NVarChar, 200),
                new SqlParameter("StartTime", SqlDbType.VarChar, 50), 
                new SqlParameter("EndTime", SqlDbType.VarChar, 50), 
                new SqlParameter("IsStart", SqlDbType.Bit) };
            parameterArray[0].Value = WorkflowName;
            parameterArray[1].Value = UserName;
            parameterArray[2].Value = Activities;
            parameterArray[3].Value = minValue;
            parameterArray[4].Value = maxValue;
            parameterArray[5].Value = isOnlyStart;
            return IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "xqp_API_GetDoneList", parameterArray).Tables[0];
        }

        /// <summary>
        /// 获取指定用户的“我的工单”列表信息
        /// </summary>
        /// <param name="WorkflowName">当前流程名(多个以,号隔开)</param>
        /// <param name="Activities">当前步骤名(多个以,号隔开)</param>
        /// <param name="UserName">用户名</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="State">工单状态</param>
        /// <returns></returns>
        public DataTable GetMyTasksList(string WorkflowName, string Activities, string UserName, string BeginTime, string EndTime, string State)
        {
            if (!string.IsNullOrEmpty(WorkflowName))
            {
                int inta = WorkflowName.LastIndexOf(',');
                if (inta == WorkflowName.Length - 1)
                    WorkflowName = WorkflowName.Remove(inta, 1);
            }

            if (!string.IsNullOrEmpty(Activities))
            {
                int inta = Activities.LastIndexOf(',');
                if (inta == Activities.Length - 1)
                    Activities = Activities.Remove(inta, 1);
            }

            DateTime startTime = DateTimeUtils.MinValue;
            DateTime endTime = DateTimeUtils.MaxValue;
            if (!string.IsNullOrEmpty(BeginTime))
                startTime = Convert.ToDateTime(BeginTime);
            if (!string.IsNullOrEmpty(EndTime))
                endTime = Convert.ToDateTime(EndTime);

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("WorkflowName", SqlDbType.NVarChar, 200),
                new SqlParameter("Actor", SqlDbType.NVarChar, 50),
                new SqlParameter("Activities", SqlDbType.NVarChar, 200),
                new SqlParameter("StartTime", SqlDbType.VarChar, 50),
                new SqlParameter("EndTime", SqlDbType.VarChar, 50),
                new SqlParameter("State", SqlDbType.Char,2)
            };

            parameters[0].Value = WorkflowName;
            parameters[1].Value = UserName;
            parameters[2].Value = Activities;
            parameters[3].Value = startTime;
            parameters[4].Value = endTime;
            parameters[5].Value = State;

            DataTable results = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "xqp_API_GetMyTask", parameters).Tables[0];

            return results;
        }

        /// <summary>
        ///  获取指定工单标识的需求单明细信息
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns></returns>
        public WorkflowDetail GetWorkflowDetailList(string workflowInstanceId)
        {
            WorkflowDetail DetailResult = null;

            string strSql = string.Format(@"select a.*,b.workflowAlias,us.realname,us.mobile from (
select WorkflowInstanceId,SheetId,State,Title,Secrecy,Urgency,Importance,ExpectFinishedTime,StartedTime,FinishedTime,
dbo.bwwf_Workflows.WorkflowName,dbo.bwwf_Tracking_Workflows.Creator,dbo.bwwf_Workflows.CreatedTime,dbo.bwwf_Workflows.WorkflowId,
dbo.fun_bwwf_getCurrentActivityNames(WorkflowInstanceId) as ActivityName,
dbo.fun_bwwf_getCurrentActors(WorkflowInstanceId) as CurrentActors
from dbo.bwwf_Tracking_Workflows inner join dbo.bwwf_Workflows 
on dbo.bwwf_Tracking_Workflows.WorkflowId=dbo.bwwf_Workflows.WorkflowId
and (dbo.bwwf_Tracking_Workflows.Sheetid='{0}' or 
cast(dbo.bwwf_Tracking_Workflows.WorkflowInstanceId as varchar(50))='{0}')) a
left join (select workflowAlias,workflowName from dbo.xqp_WorkflowMenuGroup,dbo.xqp_WorkflowInMenuGroup
where dbo.xqp_WorkflowInMenuGroup.MenuGroupId =GroupID) b on a.WorkflowName=b.WorkflowName
inner join dbo.bw_Users us on us.username=a.creator
", workflowInstanceId);
            DataTable soure = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            if (soure != null && soure.Rows.Count > 0)
            {
                DetailResult = new WorkflowDetail();

                #region 赋值

                DetailResult.WorkflowInstanceId = soure.Rows[0]["WorkflowInstanceId"].ToString().Trim();
                DetailResult.WorkflowName = soure.Rows[0]["WorkflowName"].ToString().Trim();
                DetailResult.Urgency = int.Parse(soure.Rows[0]["Urgency"].ToString().Trim());
                DetailResult.Title = soure.Rows[0]["Title"].ToString().Trim();
                DetailResult.State = int.Parse(soure.Rows[0]["State"].ToString().Trim());
                DetailResult.Starter = soure.Rows[0]["Creator"].ToString().Trim();
                DetailResult.StartedTime = Util.XmlAnalysisHelp.ToDatetime(soure.Rows[0]["StartedTime"].ToString().Trim());
                DetailResult.SheetId = soure.Rows[0]["SheetId"].ToString().Trim();
                DetailResult.Secrecy = int.Parse(soure.Rows[0]["Secrecy"].ToString().Trim());
                DetailResult.Importance = int.Parse(soure.Rows[0]["Importance"].ToString().Trim());
                DetailResult.FinishedTime = Util.XmlAnalysisHelp.ToDatetime(soure.Rows[0]["FinishedTime"].ToString().Trim());
                DetailResult.ExpectFinishedTime = soure.Rows[0]["ExpectFinishedTime"].ToString().Trim();
                DetailResult.CurrentActvities = soure.Rows[0]["ActivityName"].ToString().Trim();
                DetailResult.CurrentActors = soure.Rows[0]["CurrentActors"].ToString().Trim();
                DetailResult.Category = soure.Rows[0]["workflowAlias"].ToString().Trim();
                DetailResult.StarterName = soure.Rows[0]["realname"].ToString().Trim();
                DetailResult.Mobile = soure.Rows[0]["mobile"].ToString().Trim();

                #endregion

                #region 表单 Field

                List<Field> fields = new List<Field>();
                strSql = string.Format(@" select Fname,Name,value_str,value_decimal,value_text ,
case when value_str is null then
	case when cast(value_decimal as int) = 0 then
		value_text
	else
		cast (value_decimal as varchar(50))
	end
else
	value_str
end as [value]
from dbo.bwdf_FormItemDefinitions a,dbo.bwdf_FormItemInstances b
where a.id=b.FormItemDefinitionId  and b.FormInstanceId='{0}'", DetailResult.WorkflowInstanceId);
                DataTable Field = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
                foreach (DataRow dr in Field.Rows)
                {
                    Field f = new Field();
                    f.Key = dr["Fname"].ToString().Trim();
                    f.Name = dr["Name"].ToString().Trim();
                    f.Value = dr["value"] == null ? "" : dr["value"].ToString().Trim();
                    fields.Add(f);
                }

                DetailResult.Fields = fields.ToArray();

                #endregion

                #region 下一步骤

                List<Activity> Activitys = new List<Activity>();
                strSql = string.Format(@"select UserName,ActivityName from (
select dbo.fun_bwwf_getActivityActors(ActivityInstanceId,0) as UserName,a.NextActivitySetId from 
dbo.bwwf_Tracking_Activities  b,bwwf_Activities a where a.ActivityId=b.ActivityId and WorkflowInstanceId='{0}') f
, (select SetID,ActivityName from bwwf_Activities a,bwwf_ActivitySet b where a.ActivityId=b.ActivityId ) g
where f.NextActivitySetId=g.setid", DetailResult.WorkflowInstanceId);
                DataTable Activityss = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];

                foreach (DataRow dr in Activityss.Rows)
                {
                    Activity a = new Activity();
                    a.Name = dr["ActivityName"].ToString().Trim();
                    a.Actors = dr["UserName"] == null ? null : dr["UserName"].ToString().Split(',');
                    Activitys.Add(a);
                }

                DetailResult.NextActivities = Activitys.ToArray();

                #endregion

                #region 附件

                List<Attachment> Attachments = new List<Attachment>();
                strSql = string.Format(@"SELECT *,us.realname FROM dbo.xqp_Attachment  att inner join dbo.bw_Users us on us.username=att.creator WHERE ID in(
SELECT AttachmentId FROM dbo.xqp_Attachment_Entity WHERE ENTITYID='{0}' and entitytype='W_A')", DetailResult.WorkflowInstanceId);
                DataTable Attachment = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];

                foreach (DataRow dr in Attachment.Rows)
                {
                    Attachment a = new Attachment();
                    a.CreatedTime = Util.XmlAnalysisHelp.ToDatetime(dr["CreatedTime"].ToString().Trim());
                    a.Creator = dr["Creator"].ToString().Trim();
                    a.UserName = dr["realname"].ToString().Trim();
                    a.Name = dr["Title"].ToString().Trim();
                    a.Url = dr["FileName"].ToString().Trim();
                    Attachments.Add(a);
                }

                DetailResult.Attachments = Attachments.ToArray();

                #endregion

            }


            return DetailResult;
        }

        /// <summary>
        /// 获取指定需求单工单的当前步骤以及处理状态（正在处理，已完成，已取消等）
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识流程实例ID或工单号sheetId</param>
        /// <returns></returns>
        public DataTable GetWorkflowStateList(string workflowInstanceId)
        {
            string strSql = string.Format(@"select WorkflowInstanceId,SheetId,State,Title,dbo.bwwf_Workflows.Creator,
dbo.fun_bwwf_getCurrentActivityNames(WorkflowInstanceId) as ActivityName
from dbo.bwwf_Tracking_Workflows inner join dbo.bwwf_Workflows 
on dbo.bwwf_Tracking_Workflows.WorkflowId = dbo.bwwf_Workflows.WorkflowId 
and (cast(dbo.bwwf_Tracking_Workflows.WorkflowInstanceId as varchar(50))='{0}' or dbo.bwwf_Tracking_Workflows.SheetId='{0}')", workflowInstanceId);
            DataTable soure = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            return soure;
        }

        /// <summary>
        /// 获取表单详细信息
        /// </summary>
        /// <param name="WorkflowAlias">；流程类别</param>
        /// <returns></returns>
        public FiledInfo[] GetFieldInfo(string WorkflowAlias)
        {
            List<FiledInfo> li = new List<FiledInfo>();
            string strSql = string.Format(@"select FName, [Name], Comment, ItemDataType, ItemType, DataSource,
DataBinder, DefaultValue, [Left], [Top], Width, Height, RowExclusive, Require,ValidateType, MaxVal, MinVal, Op, OpTarget, ErrorMessage, ShowSet, WriteSet, ReadonlySet, fid.CreatedTime
from dbo.xqp_WorkflowInMenuGroup gr 
inner join dbo.bwwf_Workflows wf on wf.workflowname=gr.workflowname and iscurrent=1 and (gr.WorkflowAlias='{0}' or gr.workflowname='{0}')
inner join dbo.bwdf_FormDefinitionInExternals fdi on fdi.entityid=wf.workflowid
inner join dbo.bwdf_FormItemDefinitions fid on fid.FormDefinitionid=fdi.FormDefinitionid", WorkflowAlias);
            DataTable soure = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];

            if (soure != null && soure.Rows.Count > 0)
            {
                for (int i = 0; i < soure.Rows.Count; i++)
                {
                    FiledInfo fi = new FiledInfo();
                    fi.Comment = soure.Rows[i]["Comment"] == null ? "" : soure.Rows[i]["Comment"].ToString();
                    fi.DataBinder = soure.Rows[i]["DataBinder"] == null ? "" : soure.Rows[i]["DataBinder"].ToString();
                    fi.DataSource = soure.Rows[i]["DataSource"] == null ? "" : soure.Rows[i]["DataSource"].ToString();
                    fi.DefaultValue = soure.Rows[i]["DefaultValue"] == null ? "" : soure.Rows[i]["DefaultValue"].ToString();
                    fi.ErrorMessage = soure.Rows[i]["ErrorMessage"] == null ? "" : soure.Rows[i]["ErrorMessage"].ToString();
                    fi.FName = soure.Rows[i]["FName"] == null ? "" : soure.Rows[i]["FName"].ToString();
                    fi.Height = int.Parse(soure.Rows[i]["Height"].ToString());
                    fi.ItemDataType = int.Parse(soure.Rows[i]["ItemDataType"].ToString());
                    fi.ItemType = int.Parse((soure.Rows[i]["ItemType"].ToString()));
                    fi.Left = int.Parse(soure.Rows[i]["Left"].ToString());
                    fi.MaxVal = soure.Rows[i]["MaxVal"] == null ? "" : soure.Rows[i]["MaxVal"].ToString();
                    fi.MinVal = soure.Rows[i]["MinVal"] == null ? "" : soure.Rows[i]["MinVal"].ToString();
                    fi.Name = soure.Rows[i]["Name"] == null ? "" : soure.Rows[i]["Name"].ToString();
                    fi.Op = soure.Rows[i]["Op"] == null ? "" : soure.Rows[i]["Op"].ToString();
                    fi.OpTarget = soure.Rows[i]["OpTarget"] == null ? "" : soure.Rows[i]["OpTarget"].ToString();
                    fi.ReadonlySet = soure.Rows[i]["ReadonlySet"] == null ? "" : soure.Rows[i]["ReadonlySet"].ToString();
                    fi.Require = bool.Parse(soure.Rows[i]["Require"].ToString());
                    fi.RowExclusive = bool.Parse(soure.Rows[i]["RowExclusive"].ToString());
                    fi.ShowSet = soure.Rows[i]["ShowSet"] == null ? "" : soure.Rows[i]["ShowSet"].ToString();
                    fi.Top = int.Parse(soure.Rows[i]["Top"].ToString());
                    fi.Width = int.Parse(soure.Rows[i]["Width"].ToString());
                    fi.ValidateType = soure.Rows[i]["ValidateType"] == null ? "" : soure.Rows[i]["ValidateType"].ToString();
                    li.Add(fi);
                }
            }
            return li.ToArray();
        }

        /// <summary>
        /// 获取流程提单的处理人
        /// </summary>
        /// <param name="WorkflowAlias">流程类别</param>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public Activity[] GetActivityInfo(string WorkflowAlias, string username)
        {
            string strSql = string.Format(@"select  wc.*
from dbo.xqp_WorkflowInMenuGroup gr
inner join dbo.bwwf_Workflows wf
on gr.workflowname=wf.workflowname and (WorkflowAlias='{0}' or gr.workflowname ='{0}') and iscurrent=1
inner join dbo.bwwf_Activities wc
on wc.workflowid=wf.workflowid
where state=0", WorkflowAlias);
            DataTable soure = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            List<Activity> result = new List<Activity>();
            if (soure != null && soure.Rows.Count > 0)
            {
                Guid workflowid = (Guid)soure.Rows[0]["workflowid"];
                IList<ActivityDefinition> activities = activityDefinitionService.GetStartActivities(workflowid);
                for (int i = 0; i < activities.Count; i++)
                {
                    ActivityDefinition dataItem = activities[i];
                    IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, username, true);

                    Activity item = new Activity();
                    item.Name = dataItem.ActivityName;
                    ArrayList arr = new ArrayList();
                    item.Name = dataItem.ActivityName;
                    foreach (KeyValuePair<string, string> pair in dict)
                    {
                        arr.Add(pair.Key);
                    }
                    item.Actors = (string[])arr.ToArray();
                    result.Add(item);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 获取下一步处理人
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Activity[] GetNextActivityInfo(Guid activityInstanceId, string username)
        {
            List<Activity> result = new List<Activity>();
            IList<ActivityDefinition> activities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
            for (int i = 0; i < activities.Count; i++)
            {
                ActivityDefinition dataItem = activities[i];
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, username, true);

                Activity item = new Activity();
                item.Name = dataItem.ActivityName;
                ArrayList arr = new ArrayList();
                item.Name = dataItem.ActivityName;
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    arr.Add(pair.Key);
                }
                item.Actors = (string[])arr.ToArray();
                result.Add(item);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowId">流程定义ID（标识）</param>
        /// <param name="workflowTitle">需求单工单标题</param>
        /// <param name="startTime">工单发起时间</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <returns></returns>
        public WorkflowExecutionResult StartWorkflow(string username, string workflowId, string workflowTitle, string startTime, string workflowProperties)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();

            string strSql = string.Empty, workflowID = string.Empty, FormDefinitionId = string.Empty;//FormDefinitionId 表单定义ID
            //Guid WorkflowInstanceId = Guid.NewGuid();//bwwf_Tracking_Workflows(流程实例ID)主键值
            WorkflowDetail Detail = null;
            try
            {
                Detail = XmlAnalysisHelp.AnalysisXml(workflowProperties);
            }
            catch
            {
                ExecutionResult.Message = "需求单工单处理属性 数据有误";
                ExecutionResult.Success = "处理失败";
                return ExecutionResult;
            }
            #region workflowId 数据过滤

            strSql = string.Format("select WorkflowId from dbo.bwwf_Workflows where cast(WorkflowId as varchar(50))='{0}' or WorkflowName='{0}' and iscurrent=1", workflowId);
            Object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, strSql);
            if (obj == null)
            {
                ExecutionResult.Message = "流程定义ID（标识） 不存在";
                ExecutionResult.Success = "处理失败";
                return ExecutionResult;
            }
            workflowID = obj.ToString();//流程定义ID

            #endregion

            Botwave.Security.Domain.UserInfo user = null;
            user = userService.GetUserByUserName(username);
            Botwave.Security.LoginUser _user = new Botwave.Security.LoginUser(user);

            Guid _workflowInstanceId = Guid.NewGuid();//(流程实例ID)主键值
            Botwave.Workflow.Domain.WorkflowInstance instance = new Botwave.Workflow.Domain.WorkflowInstance();
            instance.WorkflowId = new Guid(workflowID);
            instance.WorkflowInstanceId = _workflowInstanceId;
            instance.Creator = username;
            instance.Secrecy = Detail.Secrecy;
            instance.Urgency = Detail.Urgency;
            instance.Importance = Detail.Importance;
            instance.ExpectFinishedTime = string.IsNullOrEmpty(Detail.ExpectFinishedTime) ? (DateTime?)null : DateTime.Parse(Detail.ExpectFinishedTime);
            instance.State = Detail.State;
            instance.Title = workflowTitle;
            instance.SheetId = Detail.SheetId;
            instance.StartedTime = DateTime.Parse(startTime);

            Botwave.XQP.Util.FormContext Form = new Botwave.XQP.Util.FormContext();
            IDictionary<string, object> dict = new Dictionary<string, object>();
            if (Detail.Fields != null && Detail.Fields.Length != 0)
            {
                for (int i = 0; i < Detail.Fields.Length; i++)
                {
                    Field f = Detail.Fields[i];
                    dict.Add(f.Key, f.Value);
                }
            }
            Form.Variables = dict;

            Botwave.Workflow.ActivityExecutionContext context = new Botwave.Workflow.ActivityExecutionContext();
            context.Actor = user.UserName;
            context.Command = Botwave.Workflow.ActivityCommands.Approve;
            context.Reason = "同意";

            #region 下一流程

            IDictionary<Guid, IDictionary<string, string>> dicts = new Dictionary<Guid, IDictionary<string, string>>();
            if (Detail.NextActivities != null && Detail.NextActivities.Length != 0)
            {
                for (int i = 0; i < Detail.NextActivities.Length; i++)
                {
                    Activity a = Detail.NextActivities[i];

                    strSql = string.Format(@"select ActivityId from dbo.bwwf_Activities where WorkflowId='{0}' and ActivityName='{1}'", workflowID, a.Name);
                    object objs = IBatisDbHelper.ExecuteScalar(CommandType.Text, strSql);
                    if (objs != null)
                    {
                        IDictionary<string, string> names = new Dictionary<string, string>();
                        for (int s = 0; s < a.Actors.Length; s++)
                        {
                            names.Add(a.Actors[s].ToString(), "");
                        }
                        dicts.Add(new Guid(objs.ToString()), names);
                    }
                }
            }
            #endregion

            context.ActivityAllocatees = dicts;
            if (Form.Variables != null)
                context.Variables = Form.Variables;
            context.Variables["Secrecy"] = instance.Secrecy;
            context.Variables["Urgency"] = instance.Urgency;
            context.Variables["Importance"] = instance.Importance;

            context.Variables["CurrentUser"] = user;//添加当前用户为流程变量
            try
            {
                #region 附件处理
                if (Detail.Attachments != null)
                {
                    for (int i = 0; i < Detail.Attachments.Length; i++)
                    {
                        SaveAttachmentInfo(Detail.Attachments[i], _workflowInstanceId,"W_A");
                    }
                }
                #endregion
                WorkflowTransactionHelper.StartWorkflow(instance, Form, context, _user, false);

                ExecutionResult.Success = "处理成功";
                ExecutionResult.WorkflowInstanceId = _workflowInstanceId.ToString();
            }
            catch (Exception ex)
            {
                ExecutionResult.Message = ex.ToString();
                ExecutionResult.Success = "处理失败";
            }
            
            return ExecutionResult;
        }

        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(流程实例ID)</param>
        /// <param name="command">工单处理命令。approve：通过审核。reject：退回工单。cancel：取消工单。</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        public WorkflowExecutionResult ExecuteWorkflow(string username, Guid activityInstanceId, string command, string workflowProperties, string manageOpinion)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();
            WorkflowDetail Detail = null;
            try
            {
                Detail = XmlAnalysisHelp.AnalysisXml(workflowProperties);
            }
            catch
            {
                ExecutionResult.Message = "需求单工单处理属性 数据有误";
                ExecutionResult.Success = "-1";
                return ExecutionResult;
            }
            Botwave.Security.Domain.UserInfo user = null; string strSql = string.Empty;

            // 用户信息.
            user = userService.GetUserByUserName(username);
            Botwave.Security.LoginUser loginUser = new Botwave.Security.LoginUser(user);

            ActivityInstance activityInstance = activityService.GetActivity(activityInstanceId);
            if (activityInstance == null)
            {
                ExecutionResult.Message = "当前活动实例ID（标识） 不存在";
                ExecutionResult.Success = "-1";
                return ExecutionResult;
            }

            #region form实例

            // 表单实例绑定.
            IDictionary<string, object> formVariables = new Dictionary<string, object>();
            if (Detail.Fields != null && Detail.Fields.Length != 0)
            {
                for (int i = 0; i < Detail.Fields.Length; i++)
                {
                    Field f = Detail.Fields[i];
                    formVariables.Add(f.Key, f.Value);
                }
            }
            #endregion

            // 工单基本信息更新.
            Guid workflowInstanceId = activityInstance.WorkflowInstanceId;
            Botwave.Workflow.Domain.WorkflowInstance instance = workflowService.GetWorkflowInstance(workflowInstanceId);
            instance.Secrecy = Detail.Secrecy;
            instance.Urgency = Detail.Urgency;
            instance.Importance = Detail.Importance;
            instance.ExpectFinishedTime = string.IsNullOrEmpty(Detail.ExpectFinishedTime) ? (DateTime?)null : DateTime.Parse(Detail.ExpectFinishedTime);

            // 工单处理.
            Botwave.Workflow.ActivityExecutionContext context = new Botwave.Workflow.ActivityExecutionContext();
            context.ActivityInstanceId = activityInstanceId;
            context.Actor = username;
            context.Command = command.ToLower();
            context.Variables = formVariables;
            context.Reason = manageOpinion;//备注信息
            context.ExternalEntityType = activityInstance.ExternalEntityType;
            context.ExternalEntityId = activityInstance.ExternalEntityId;
            if (instance != null)
            {
                context.Variables["Secrecy"] = instance.Secrecy;
                context.Variables["Urgency"] = instance.Urgency;
                context.Variables["Importance"] = instance.Importance;
            }
            context.Variables["CurrentUser"] = loginUser;//添加当前用户为流程变量

            // 取消流程.（对取消特别对待）
            if (ActivityCommands.Cancel.Equals(context.Command))
            {
                workflowEngine.CancelWorkflow(context);

                return ExecutionResult;
            }

            #region 获取选中步骤以及处理人.

            Guid workflowId = instance.WorkflowId;
            string selectedActivity = string.Empty;
            IDictionary<Guid, IDictionary<string, string>> dicts = new Dictionary<Guid, IDictionary<string, string>>();
            if (Detail.NextActivities != null && Detail.NextActivities.Length != 0)
            {
                for (int i = 0; i < Detail.NextActivities.Length; i++)
                {
                    Activity a = Detail.NextActivities[i];
                    selectedActivity = a.Name;
                    strSql = string.Format(@"select ActivityId from dbo.bwwf_Activities where WorkflowId='{0}' and ActivityName='{1}'", workflowId, a.Name);
                    object o = IBatisDbHelper.ExecuteScalar(CommandType.Text, strSql);
                    if (o != null)
                    {
                        IDictionary<string, string> names = new Dictionary<string, string>();
                        for (int s = 0; s < a.Actors.Length; s++)
                        {
                            names.Add(a.Actors[s].ToString(), "");
                        }
                        dicts.Add(new Guid(o.ToString()), names);
                    }
                }
            }
            #endregion

            #region 处理

            // "通过"时才检查是否选择分派用户
            bool isApprove = ActivityCommands.Approve.Equals(context.Command);
            if (isApprove)
            {
                context.ActivityAllocatees = dicts;
            }
            // "退还"时设置退还步骤
            else if (ActivityCommands.Reject.Equals(context.Command))
            {
                Hashtable parameters = new Hashtable();
                parameters.Add("ActivityInstanceId", activityInstanceId);
                parameters.Add("RejectActivityName", selectedActivity);
                object obj = IBatisMapper.Mapper.QueryForObject<int>("bwwf_ActivityInstanceReject_Select", activityInstanceId);
                if (null != obj && DbUtils.ToInt32(obj) == 0)
                {
                    IBatisMapper.Insert("bwwf_ActivityInstanceReject_Insert", parameters);
                }
                else
                {
                    IBatisMapper.Update("bwwf_ActivityInstanceReject_Update", parameters);
                }
            }

            string strCompletedActivityNames = String.Empty;
            IList<ActivityInstance> instanceList = activityService.GetActivitiesInSameWorkflow(activityInstanceId);
            foreach (ActivityInstance instances in instanceList)
            {
                if (instances.ActivityInstanceId.Equals(activityInstanceId)) continue;
                ActivityDefinition definition = activityDefinitionService.GetActivityDefinition(instances.ActivityId);
                strCompletedActivityNames += definition.ActivityName;
            }

            context.Variables["CompletedActivities"] = strCompletedActivityNames;   //添加已完成步骤为流程变量
            workflowEngine.ExecuteActivity(context);

            //更新流程实例
            instance.WorkflowInstanceId = workflowInstanceId;
            instance.State = (int)Botwave.Workflow.WorkflowConstants.Executing;
            workflowService.UpdateWorkflowInstance(instance);

            //审批通过进行表单更新
            if (isApprove || Botwave.Workflow.ActivityCommands.Save.Equals(context.Command))
            {
                Botwave.XQP.Util.FormContext formContext = new Botwave.XQP.Util.FormContext();
                formContext.Variables = formVariables;

                formInstanceService.SaveForm(workflowInstanceId, formContext.Variables, username);
            }
            #endregion

            #region 附件处理
            if (Detail.Attachments != null)
            {
                for (int i = 0; i < Detail.Attachments.Length; i++)
                {
                    SaveAttachmentInfo(Detail.Attachments[i], workflowInstanceId,"W_A");
                }
            }
            #endregion

            List<Activity> result = new List<Activity>();
            IList<ActivityDefinition> activities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
            for (int i = 0; i < activities.Count; i++)
            {
                ActivityDefinition dataItem = activities[i];
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, username, true);

                Activity item = new Activity();
                ArrayList arr = new ArrayList();
                item.Name = dataItem.ActivityName;
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    arr.Add(pair.Key);
                }
                item.Actors = (string[])arr.ToArray();
                result.Add(item);
            }
            ExecutionResult.NextActivities = result.ToArray();

            return ExecutionResult;
        }

        /// <summary>
        /// 获取指定需求单处理列表
        /// </summary>
        /// <param name="workflowInstanceId">需求单工单标识(流程实例ID或工单号sheetId)</param>
        /// <returns></returns>
        public IList<WorkflowRecord> GetWorkflowRecord(string workflowInstanceId)
        {
            IList<WorkflowRecord> wrList = new List<WorkflowRecord>();


            #region workflowInstanceId 流程实例ID

            string strSql = string.Format("select WorkflowInstanceId,workflowID,SheetID,Title from dbo.bwwf_Tracking_Workflows where cast(WorkflowInstanceId as varchar(50))='{0}' or SheetID='{0}'", workflowInstanceId);
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            #endregion

            IList<Assignment> workflowAssignments = IBatisMapper.Select<Assignment>("bwwf_Assignment_Select_By_WorkflowInstanceId", workflowInstanceId);//转交信息

            IList<ActivityInstance> Activitys = IBatisMapper.Select<ActivityInstance>("bwwf_ActivityInstance_Select_WorkflowInstanceId", workflowInstanceId);
            foreach (ActivityInstance Activity in Activitys)
            {
                WorkflowRecord wr = new WorkflowRecord();
                wr.ActivityInstanceId = Activity.ActivityInstanceId.ToString();
                wr.ActivityName = Activity.ActivityName == null ? "" : Activity.ActivityName.ToString();
                wr.Actor = Activity.Actor == null ? "" : Activity.Actor.ToString();
                wr.CompletedTime = Activity.FinishedTime == null ? "" : Activity.FinishedTime.ToString();
                wr.CreatedTime = Activity.CreatedTime == null ? "" : Activity.CreatedTime.ToString();
                switch (Activity.Command)
                {
                    case "approve":
                        wr.Command = 1;
                        break;
                    case "reject":
                        wr.Command = 0;
                        break;
                    default:
                        break;
                }

                #region 转交

                if (workflowAssignments.Count > 0)
                {
                    foreach (Assignment ass in workflowAssignments)
                    {
                        if (Activity.ActivityInstanceId == ass.ActivityInstanceId)
                        {
                            WorkflowRecord wr1 = new WorkflowRecord();
                            wr1.ActivityInstanceId = ass.ActivityInstanceId.ToString();
                            wr1.ActivityName = Activity.ActivityName;
                            wr1.Actor = ass.AssignedUser;
                            wr1.Command = 2;
                            wr1.CreatedTime = ass.AssignedTime.ToString();
                            wrList.Add(wr1);
                        }
                    }
                }

                #endregion

                #region 会签

                if (!string.IsNullOrEmpty(Activity.CountersignedCondition))
                {
                    IList<Countersigned> countersigneds = IBatisMapper.Select<Countersigned>("bwwf_Countersigned_Select_By_ActivityInstanceId", Activity.ActivityInstanceId);
                    if (countersigneds != null && countersigneds.Count > 0)
                    {
                        foreach (Countersigned Countersigne in countersigneds)
                        {
                            WorkflowRecord wr2 = new WorkflowRecord();
                            wr2.ActivityInstanceId = Countersigne.ActivityInstanceId.ToString();
                            wr2.ActivityName = Activity.ActivityName;
                            wr2.Actor = Countersigne.UserName;
                            wr2.Command = 1;
                            wr2.CreatedTime = Countersigne.CreatedTime.ToString();
                            wrList.Add(wr2);
                        }
                    }
                }

                #endregion

                wrList.Add(wr);
            }

            return wrList;
        }

        private void SaveAttachmentInfo(Attachment att, Guid workflowInstanceId,string  type)
        {
            Guid newAid = Guid.NewGuid();
            string EntityType = "W_A";
            if (!string.IsNullOrEmpty(type))
                EntityType = type;
            string strSql = string.Format(@"insert into xqp_Attachment(Id,Creator,LastModifier,CreatedTime,LastModTime,Title,FileName,MimeType)
      values ('{0}','{1}','{1}','{2}','{2}','{3}','{4}','{5}')", newAid, att.Creator, att.CreatedTime, att.Name, att.Url, System.IO.Path.GetExtension(att.Url));
            int inta = IBatisDbHelper.ExecuteNonQuery(CommandType.Text, strSql);
            if (inta > 0)
            {
                strSql = string.Format(@"INSERT INTO [dbo].[xqp_Attachment_Entity]
      ([AttachmentId], [EntityId], [EntityType])
      VALUES('{0}','{1}','{2}')", newAid, workflowInstanceId, EntityType);
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, strSql);
            }
        }

        private string[] GetRealName(string[] userName)
        {
            string[] ret_val = new string[userName.Length];
            try
            {
                string strSql = string.Empty;
                for (int i = 0; i < userName.Length; i++)
                {
                    strSql = string.Format(@"select realname+'/'+username Names from dbo.bw_Users where username ='{0}'", userName[i]);
                    ret_val[i] = IBatisDbHelper.ExecuteScalar(CommandType.Text, strSql).ToString();
                }
            }
            catch { }
            return ret_val;
        }

        #region 流程接口 WebService

        /// <summary>
        /// 获取流程分组
        /// </summary>
        /// <returns></returns>
        public IList<MenuGroup> GetMenuGroup()
        {
            IList<MenuGroup> group = new List<MenuGroup>();
            IList<Botwave.XQP.Domain.WorkflowMenuGroup> list = Botwave.XQP.Domain.WorkflowMenuGroup.Select();
            foreach (Botwave.XQP.Domain.WorkflowMenuGroup g in list)
            {
                group.Add(new MenuGroup() { GroupName = g.GroupName });
            }
            return group;
        }

        /// <summary>
        /// 获取流程列表
        /// </summary>
        /// <param name="groupName">流程分组名</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public IList<Botwave.XQP.API.Entity.Workflow> GetWorkflow(string groupName, string userName)
        {
            IList<Botwave.XQP.API.Entity.Workflow> wfList = new List<Botwave.XQP.API.Entity.Workflow>();


            IList<Botwave.XQP.Domain.WorkflowMenuGroup> groups = Botwave.XQP.Domain.WorkflowMenuGroup.Select();

            IList<Botwave.XQP.Domain.WorkflowInMenuGroup> groupWorkflows = Botwave.XQP.Domain.WorkflowInMenuGroup.Select();//所有流程
            IDictionary<string, string> workflowResouces = Botwave.XQP.Util.ResourceHelper.GetResouceByParentId(Botwave.XQP.Util.ResourceHelper.Workflow_ResourceId);//流程权限

            #region 用户权限信息

            IDictionary<string, string> userResources = new Dictionary<string, string>(); bool isTrue = false;
            if (!string.IsNullOrEmpty(userName))
            {
                Botwave.Security.Domain.UserInfo user = userService.GetUserByUserName(userName);
                IList<ResourceInfo> resources = IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByUserId", user.UserId);
                 foreach (ResourceInfo item in resources)
                 {
                     string resourceId = item.ResourceId;
                     if (!userResources.ContainsKey(resourceId))
                         userResources.Add(resourceId, resourceId);
                 }
                //userResources = roleService.GetRolesByUserId(user.UserId);
            }
            else//用户名为空
            {
                isTrue = true;
            }

            #endregion

            Botwave.XQP.API.Entity.Workflow wf = null;
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
                            wf = new Botwave.XQP.API.Entity.Workflow();
                            string requireResouceId = workflowResouces[workflowName] + "0000"; // 取得指定流程公用权限.
                            if (!Botwave.XQP.Util.ResourceHelper.VerifyResource(userResources, requireResouceId) && !isTrue)
                                continue;

                            wf.WorkflowName = item.WorkflowName;
                            wf.WorkflowAlias = item.WorkflowAlias;
                            wf.WorkflowID = item.WorkflowId.ToString();
                            wf.GroupName = menuGroups.GroupName;
                            wfList.Add(wf);
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
                                wf = new Botwave.XQP.API.Entity.Workflow();
                                string requireResouceId = workflowResouces[workflowName] + "0000"; // 取得指定流程公用权限.
                                if (!Botwave.XQP.Util.ResourceHelper.VerifyResource(userResources, requireResouceId) && !isTrue)
                                    continue;

                                wf.WorkflowName = item.WorkflowName;
                                wf.WorkflowAlias = item.WorkflowAlias;
                                wf.WorkflowID = item.WorkflowId.ToString();
                                wf.GroupName = g.GroupName;
                                wfList.Add(wf);
                                // 移除当前流程子目录，减少循环次数.
                                groupWorkflows.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }
            }

            return wfList;
        }

        /// <summary>
        /// 获取流程定义字符串
        /// </summary>
        /// <param name="flowID">唯一标识</param>
        /// <returns></returns>
        public string GetWorkflowDefinitionString(Guid flowID)
        {
            WorkflowDefinition _workflow = new WorkflowDefinition();
            _workflow = workflowDefinitionService.GetWorkflowDefinition(flowID);
            IList<ActivityDefinition> activities = activityDefinitionService.GetSortedActivitiesByWorkflowId(flowID);
            IDictionary<Guid, AllocatorOption> assigmentDict = IBatisMapper.Mapper.QueryForDictionary<Guid, AllocatorOption>("bwwf_AssignmentAllocator_Select_ByWorkflowId", flowID, "ActivityId");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = System.Text.Encoding.UTF8;
            StringBuilder strBuilder = new StringBuilder();
            XmlWriter writer = XmlWriter.Create(strBuilder, settings);
            ExportXML(writer, _workflow, activities, assigmentDict);
            return strBuilder.ToString();
        }

        /// <summary>
        /// 部署流程
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="xmlString">流程定义</param>
        /// <returns></returns>
        public string WorkflowDeploy(string userName, string xmlString)
        {
            string result = string.Empty;
            Botwave.Workflow.ActionResult acitonResult;
            acitonResult = new Botwave.Workflow.ActionResult();
            XmlReader xmlReader = XmlReader.Create(new System.IO.StringReader(xmlString));
            acitonResult = deployService.CheckWorkflow(xmlReader);
            if (acitonResult.Success)
            {
                xmlReader = XmlReader.Create(new System.IO.StringReader(xmlString));
                acitonResult = deployService.DeployWorkflow(xmlReader, userName);
                if (acitonResult.Success)//插入数据成功
                {
                    #region 找到流程名称

                    xmlReader = XmlReader.Create(new System.IO.StringReader(xmlString));
                    string strName = string.Empty; bool isb = false;
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            strName = xmlReader.Name;
                            for (int i = 0; i < xmlReader.AttributeCount; i++)
                            {
                                xmlReader.MoveToAttribute(i);
                                if (strName == "workflow")
                                {
                                    strName = xmlReader.Value.Trim().ToString();
                                    isb = true;
                                    break;
                                }
                            }
                            if (isb)
                                break;
                        }
                    }

                    #endregion

                    if (string.IsNullOrEmpty(strName))//没有找到流程名字
                    {
                        result = "流程部署失败";
                        return result;
                    }
                    string flowid = GetFlowID(strName, "名称");

                    result = flowid;
                }
                else
                {
                    result = acitonResult.Message;
                }
            }
            else
            {
                result = acitonResult.Message;
            }
            return result;
        }

        /// <summary>
        /// 传入表单数据以处理需求单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(流程实例ID)</param>
        /// <param name="workflowProperties">需求单工单处理属性</param>
        /// <param name="manageOpinion">处理意见</param>
        /// <returns></returns>
        public WorkflowExecutionResult SaveWorkflow(string username, Guid activityInstanceId, string workflowProperties, string manageOpinion)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();
            WorkflowDetail Detail = null;
            try
            {
                Detail = XmlAnalysisHelp.AnalysisXml(workflowProperties);
            }
            catch
            {
                ExecutionResult.Message = "需求单工单处理属性 数据有误";
                ExecutionResult.Success = "-1";
                return ExecutionResult;
            }
            Botwave.Security.Domain.UserInfo user = null; string strSql = string.Empty;

            // 用户信息.
            user = userService.GetUserByUserName(username);
            Botwave.Security.LoginUser loginUser = new Botwave.Security.LoginUser(user);

            ActivityInstance activityInstance = activityService.GetActivity(activityInstanceId);
            if (activityInstance == null)
            {
                ExecutionResult.Message = "当前活动实例ID（标识） 不存在";
                ExecutionResult.Success = "-1";
                return ExecutionResult;
            }

            #region form实例

            // 表单实例绑定.
            IDictionary<string, object> formVariables = new Dictionary<string, object>();
            if (Detail.Fields != null && Detail.Fields.Length != 0)
            {
                for (int i = 0; i < Detail.Fields.Length; i++)
                {
                    Field f = Detail.Fields[i];
                    formVariables.Add(f.Key, f.Value);
                }
            }
            #endregion

            // 工单基本信息更新.
            Guid workflowInstanceId = activityInstance.WorkflowInstanceId;
            Botwave.Workflow.Domain.WorkflowInstance instance = workflowService.GetWorkflowInstance(workflowInstanceId);
            instance.Secrecy = Detail.Secrecy;
            instance.Urgency = Detail.Urgency;
            instance.Importance = Detail.Importance;
            instance.ExpectFinishedTime = string.IsNullOrEmpty(Detail.ExpectFinishedTime) ? (DateTime?)null : DateTime.Parse(Detail.ExpectFinishedTime);

            // 工单处理.
            Botwave.Workflow.ActivityExecutionContext context = new Botwave.Workflow.ActivityExecutionContext();
            context.ActivityInstanceId = activityInstanceId;
            context.Actor = username;
            context.Variables = formVariables;
            context.Reason = manageOpinion;//备注信息
            context.Command = ActivityCommands.Save;
            context.ExternalEntityType = activityInstance.ExternalEntityType;
            context.ExternalEntityId = activityInstance.ExternalEntityId;
            if (instance != null)
            {
                context.Variables["Secrecy"] = instance.Secrecy;
                context.Variables["Urgency"] = instance.Urgency;
                context.Variables["Importance"] = instance.Importance;
            }
            context.Variables["CurrentUser"] = loginUser;//添加当前用户为流程变量

            #region 处理

            string strCompletedActivityNames = String.Empty;
            IList<ActivityInstance> instanceList = activityService.GetActivitiesInSameWorkflow(activityInstanceId);
            foreach (ActivityInstance instances in instanceList)
            {
                if (instances.ActivityInstanceId.Equals(activityInstanceId)) continue;
                ActivityDefinition definition = activityDefinitionService.GetActivityDefinition(instances.ActivityId);
                strCompletedActivityNames += definition.ActivityName;
            }

            context.Variables["CompletedActivities"] = strCompletedActivityNames;   //添加已完成步骤为流程变量
            workflowEngine.ExecuteActivity(context);

            //更新流程实例
            instance.WorkflowInstanceId = workflowInstanceId;
            instance.State = (int)Botwave.Workflow.WorkflowConstants.Executing;
            workflowService.UpdateWorkflowInstance(instance);

            //保存表单内容
            Botwave.XQP.Util.FormContext formContext = new Botwave.XQP.Util.FormContext();
            formContext.Variables = formVariables;
            formInstanceService.SaveForm(workflowInstanceId, formContext.Variables, username);

            #endregion

            #region 附件处理
            if (Detail.Attachments != null)
            {
                for (int i = 0; i < Detail.Attachments.Length; i++)
                {
                    SaveAttachmentInfo(Detail.Attachments[i], workflowInstanceId, "W_A");
                }
            }
            #endregion

            List<Activity> result = new List<Activity>();
            IList<ActivityDefinition> activities = activityDefinitionService.GetNextActivityDefinitionsByInstanceId(activityInstanceId);
            for (int i = 0; i < activities.Count; i++)
            {
                ActivityDefinition dataItem = activities[i];
                IDictionary<string, string> dict = activityAllocationService.GetTargetUsers(Guid.Empty, dataItem, username, true);

                Activity item = new Activity();
                item.Name = dataItem.ActivityName;
                ArrayList arr = new ArrayList();
                item.Name = dataItem.ActivityName;
                foreach (KeyValuePair<string, string> pair in dict)
                {
                    arr.Add(pair.Key);
                }
                item.Actors = (string[])arr.ToArray();
                result.Add(item);
            }
            ExecutionResult.NextActivities = result.ToArray();

            return ExecutionResult;
        }

        /// <summary>
        /// 转交工单
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="assignedUser">被指定用户的Portal 登录账号</param>
        /// <param name="activityInstanceId">需求单工单标识(活动实例ID或工单号sheetId)</param>
        /// <param name="Message">转交意见</param>
        /// <returns></returns>
        public WorkflowExecutionResult AssignWorkflow(string username, string assignedUser, string activityInstanceId, string Message)
        {
            WorkflowExecutionResult ExecutionResult = new WorkflowExecutionResult();
            ActivityInstance activityInstance = activityService.GetActivity(new Guid(activityInstanceId));
            if (activityInstance == null)
            {
                ExecutionResult.Message = "当前活动实例ID（标识） 不存在";
                ExecutionResult.Success = "-1";
                return ExecutionResult;
            }
            Assignment assignment = new Assignment();
            assignment.ActivityInstanceId = new Guid(activityInstanceId);
            assignment.AssignedTime = DateTime.Now;
            assignment.AssignedUser = assignedUser;
            assignment.AssigningUser = username;
            assignment.Message = Message;

            taskAssignService.Assign(assignment);  // 转交信息.
            return ExecutionResult;
        }

        /// <summary>
        /// 高级查询条件
        /// </summary>
        /// <param name="username">指定用户的Portal 登录账号</param>
        /// <param name="workflowName">流程名</param>
        /// <returns></returns>
        public SearchQueryResult SearchQuery(string username, string workflowName)
        {
            SearchQueryResult info = new SearchQueryResult();
            IDictionary<string, string> Resources = GetResources(username);
            IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
            workflows = GetAllowedWorkflows(workflows, Resources, "0004");
            List<SearchWorkflowList> WorkflowLists = new List<SearchWorkflowList>();
            foreach (WorkflowDefinition wd in workflows)
            {
                WorkflowLists.Add(new SearchWorkflowList() { WorkflowFullName = wd.WorkflowName, WorkflowName = wd.WorkflowName });
            }
            info.WorkflowList = WorkflowLists.ToArray();

            if (!string.IsNullOrEmpty(workflowName))
            {
                workflows = workflowDefinitionService.GetWorkflowDefinitionListByName(workflowName);
                if (workflows != null && workflows.Count > 0)
                {
                    Guid workflowId = workflows[0].WorkflowId;
                    IList<ActivityDefinition> Activitys = activityDefinitionService.GetActivitiesByWorkflowId(workflowId);
                    List<Activity> ActivityList = new List<Activity>();
                    foreach (ActivityDefinition ad in Activitys)
                    {
                        ActivityList.Add(new Activity() { Name = ad.ActivityName });
                    }
                    info.Activity = ActivityList.ToArray();
                }
            }

            return info;
        }

        /// <summary>
        /// 高级查询
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="BeginTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="workflowName">流程</param>
        /// <param name="activityName">步骤</param>
        /// <param name="creator">发起人</param>
        /// <param name="actor">当前处理人</param>
        /// <param name="titleKeywords">标题关键字</param>
        /// <param name="contentKeywords">内容关键字</param>
        /// <param name="sheetId">受理号</param>
        /// <returns></returns>
        public SearchResult Search(string username, string BeginTime, string EndTime, string workflowName, string activityName, string creator, string actor, string titleKeywords, string contentKeywords, string sheetId)
        {
            SearchResult result = new SearchResult();
            int recordCount = 0;

            //流程若选择全部,则需取出所有流程作为条件查询.
            if (string.IsNullOrEmpty(workflowName))
            {
                activityName = "";

                StringBuilder sbWorkflowNames = new StringBuilder();
                IDictionary<string, string> Resources = GetResources(username);
                IList<WorkflowDefinition> workflows = workflowDefinitionService.GetWorkflowDefinitionList();
                workflows = GetAllowedWorkflows(workflows, Resources, "0004");
                foreach (WorkflowDefinition entity in workflows)
                {
                    sbWorkflowNames.AppendFormat("'{0}',", entity.WorkflowName);
                }
                if (sbWorkflowNames.Length > 0)
                    workflowName = sbWorkflowNames.ToString().Substring(1, sbWorkflowNames.Length - 3);
            }

            AdvancedSearchCondition condition = new AdvancedSearchCondition();
            condition.BeginTime = BeginTime;
            condition.EndTime = EndTime;
            condition.ActivityName = activityName;
            condition.CreatorName = creator;
            condition.Keywords = contentKeywords;
            condition.ProcessorName = actor;
            condition.SheetId = sheetId;
            condition.Title = titleKeywords.Trim();
            condition.WorkflowName = workflowName;

            DataTable dt = advancedSearcher.Search(condition, 0, 100, ref recordCount);
            List<SearchList> list = new List<SearchList>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new SearchList()
                {
                    ActivityName = dr["ActivityName"] == null ? string.Empty : dr["ActivityName"].ToString(),
                    CreatorName = dr["CreatorName"] == null ? string.Empty : dr["CreatorName"].ToString(),
                    CurrentActors = dr["CurrentActors"] == null ? string.Empty : dr["CurrentActors"].ToString(),
                    SheetId = dr["SheetId"] == null ? string.Empty : dr["SheetId"].ToString(),
                    StartedTime = dr["StartedTime"] == null ? string.Empty : dr["StartedTime"].ToString(),
                    Title = dr["Title"] == null ? string.Empty : dr["Title"].ToString(),
                    WorkflowAlias = dr["WorkflowAlias"] == null ? string.Empty : dr["WorkflowAlias"].ToString(),
                    WorkflowInstanceId = dr["WorkflowInstanceId"] == null ? string.Empty : dr["WorkflowInstanceId"].ToString()
                });
            }
            result.Search = list.ToArray();
            return result;
        }

        /// <summary>
        /// 评论工单
        /// </summary>
        /// <param name="workflowInstanceId">工单唯一标识</param>
        /// <param name="username">用户名</param>
        /// <param name="content">评论内容</param>
        /// <param name="CommentProperties">附件信息XML格式</param>
        /// <returns></returns>
        public CommentResult CommentWorkflow(string workflowInstanceId,string activityInstanceId, string username, string content, string CommentProperties)
        {
            CommentResult comResult = new CommentResult();

            IList<Comment> comments = commentService.GetWorkflowComments(new Guid(workflowInstanceId));//评论列表
            DataTable dt = GetAttachemnts(workflowInstanceId);//附件列表
            List<CommentList> list = new List<CommentList>();
            foreach (Comment c in comments)
            {
                list.Add(new CommentList()
                {
                    CreatedTime = c.CreatedTime.ToString(),
                    Message = c.Message,
                    Creator = c.Creator,
                    Attachments = GetAttachemntsHtml(c.Id.Value, dt) == null ? null : GetAttachemntsHtml(c.Id.Value, dt).ToArray()
                });
            }
            comResult.CommentLists = list.ToArray();
            if (!string.IsNullOrEmpty(content))
            {
                #region 附件处理

                if (!string.IsNullOrEmpty(CommentProperties))
                {
                    Attachment[] att = XmlAnalysisHelp.AttachmentXml(CommentProperties);
                    foreach (Attachment a in att)
                    {
                        SaveAttachmentInfo(a, new Guid(workflowInstanceId),"W_C");
                    }
                }

                #endregion

                Comment item = new Comment();
                item.WorkflowInstanceId = new Guid(workflowInstanceId);
                item.ActivityInstanceId = new Guid(activityInstanceId);
                item.Id = Guid.NewGuid();
                item.Creator = username;
                item.Message = content;

                // 添加评论
                commentService.AddComment(item);
            }
            return comResult;
        }

        #region 获取流程定义or部署流程辅助方法

        /// <summary>
        /// 获取流程的唯一标识
        /// </summary>
        /// <param name="str">流程名称以及别名</param>
        /// <param name="str">流程类别</param>
        /// <returns></returns>
        public string GetFlowID(string str, string type)
        {
            string ret_val = string.Empty;
            try
            {
                string strsql = string.Empty;
                if (type == "名称")
                {
                    strsql = string.Format(@"SELECT  WorkflowId FROM bwwf_Workflows a left join xqp_WorkflowInMenuGroup b
on a.WorkflowName=b.WorkflowName WHERE   iscurrent=1 and (a.WorkflowName ='{0}')", str);
                }
                else if (type == "别名")
                {
                    strsql = string.Format(@"SELECT  WorkflowId FROM bwwf_Workflows a left join xqp_WorkflowInMenuGroup b
on a.WorkflowName=b.WorkflowName WHERE  iscurrent=1 and ( WorkflowAlias='{0}')", str);
                }
                ret_val = IBatisDbHelper.ExecuteScalar(CommandType.Text, strsql).ToString();
            }
            catch { }
            return ret_val;
        }

        private readonly string PrefixDisableResource = "#NONE#_";

        private readonly string flowComment = @"
# 流程名称与所有人是必填的，以名称作为标识，如果存在流程名称重复，则新增一个版本并作为当前版本;
# remark：表示流程的备注内容，并且只能有一个 remark 节点;
# 所有流程都必须有且只有一个开始步骤(start-activity)与结束步骤(end-activity)，需要有一个以上(包含一个)的中间步骤(activity);
# 开始步骤(start-activity)的名称(name)可以为空(此时默认为初始化)，结束步骤(end-activity)的名称也可以为空(此时默认为完成);
# prevActivity:表示上一步骤；nextActivity：表示下一步骤。两个属性的值是相应的步骤名称(多个名称之间以','或者'，'隔开);
# 中间步骤(activity)必须有名称(Name)，以及上一步骤(prevActivity)和下一步骤(nextActivity);
# 开始步骤(start-activity)的下一步骤(nextActivity)不能为空，结束步骤(end-activity)的上一步骤(prevActivity)不能为空;
# 在一个流程之中的活动名称必须唯一;
# joinCondition、splitCondition、countersignedCondition分别是合并条件、分支条件、会签条件
# commandRules：表示对应的步骤(activity)的命令规则，主要用于流程自动处理;
# 一个步骤(activity)中只能有一个 commandRules 节点。
# taskAllocator 任务分配配置节点.
# extAllocators 各任务分派实例以分号隔开；冒号后面为其参数，各参数之间以逗号分隔.如, superior:arg1,arg2;processor:1
# decisionType指分支选择类型，分为manual(手动)与auto（自动）两种，默认为手动
# rejectOption指拒绝/退回时的选择,initial退回起始/提单状态,previous退回上一步,none不允许退回,还可以是特定的步骤名称";

        /// <summary>
        /// 导出xml数据
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="_workflow"></param>
        /// <param name="activities"></param>
        /// <param name="assignmentAllocators"></param>
        public void ExportXML(XmlWriter writer, WorkflowDefinition _workflow, IList<ActivityDefinition> activities, IDictionary<Guid, AllocatorOption> assignmentAllocators)
        {
            writer.WriteStartDocument();
            writer.WriteComment(flowComment);

            writer.WriteStartElement("workflow");
            writer.WriteAttributeString("name", _workflow.WorkflowName);
            writer.WriteAttributeString("owner", _workflow.Owner);

            writer.WriteStartElement("remark");
            writer.WriteCData(_workflow.Remark);
            writer.WriteEndElement();

            foreach (ActivityDefinition item in activities)
            {
                Guid activityId = item.ActivityId;
                if (item.State == 0)
                {
                    writer.WriteStartElement("start-activity", "");
                    writer.WriteAttributeString("name", item.ActivityName);
                    writer.WriteAttributeString("nextActivity", GetActivityNames(activities, item.NextActivitySetId));
                    writer.WriteAttributeString("splitCondition", item.SplitCondition);
                    writer.WriteAttributeString("executionHandler", item.ExecutionHandler);
                    writer.WriteAttributeString("postHandler", item.PostHandler);
                    writer.WriteAttributeString("decisionType", item.DecisionType);
                    writer.WriteAttributeString("decisionParser", item.DecisionParser);

                    writer.WriteStartElement("commandRules");
                    writer.WriteCData(item.CommandRules);
                    writer.WriteEndElement();

                    //this.ExportTaskAllocator(writer, item);

                    writer.WriteEndElement();
                }
                else if (item.State == 1)
                {
                    writer.WriteStartElement("activity", "");
                    writer.WriteAttributeString("name", item.ActivityName);
                    writer.WriteAttributeString("prevActivity", GetActivityNames(activities, item.PrevActivitySetId));
                    writer.WriteAttributeString("nextActivity", GetActivityNames(activities, item.NextActivitySetId));
                    writer.WriteAttributeString("joinCondition", item.JoinCondition);
                    writer.WriteAttributeString("splitCondition", item.SplitCondition);
                    writer.WriteAttributeString("countersignedCondition", item.CountersignedCondition);
                    writer.WriteAttributeString("executionHandler", item.ExecutionHandler);
                    writer.WriteAttributeString("postHandler", item.PostHandler);
                    writer.WriteAttributeString("decisionType", item.DecisionType);
                    writer.WriteAttributeString("decisionParser", item.DecisionParser);
                    writer.WriteAttributeString("rejectOption", item.RejectOption);

                    // [2008-5-21]将 CommandRules 属性改为节点表示
                    writer.WriteStartElement("commandRules");
                    writer.WriteCData(item.CommandRules);
                    writer.WriteEndElement();

                    this.ExportTaskAllocator(writer, item);

                    if (assignmentAllocators.ContainsKey(activityId))
                    {
                        this.ExportAssignmentAllocator(writer, assignmentAllocators[activityId]);
                    }
                    else
                    {
                        writer.WriteStartElement("assignmentAllocator");
                        writer.WriteAttributeString("resource", "");
                        writer.WriteAttributeString("users", "");
                        writer.WriteAttributeString("extAllocators", "");
                        writer.WriteAttributeString("extAllocatorArgs", "");
                        writer.WriteAttributeString("default", "");
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }
                else if (item.State == 2)
                {
                    writer.WriteStartElement("end-activity", "");
                    writer.WriteAttributeString("name", item.ActivityName);
                    writer.WriteAttributeString("prevActivity", GetActivityNames(activities, item.PrevActivitySetId));
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        /// <summary>
        /// 导出 AssignmentAllocator 节点.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        protected void ExportAssignmentAllocator(XmlWriter writer, AllocatorOption item)
        {

            writer.WriteStartElement("assignmentAllocator");
            string resource = item.AllocatorResource;
            if (!string.IsNullOrEmpty(resource))
            {
                if (resource.StartsWith(PrefixDisableResource, StringComparison.OrdinalIgnoreCase))
                {
                    // 不启用权限控制
                    resource = "";
                }
                else if (resourceTranslator != null)
                {
                    resource = resourceTranslator.Name2Alias(resource);
                    if (string.IsNullOrEmpty(resource))
                        resource = item.AllocatorResource;
                }
            }
            writer.WriteAttributeString("resource", GetUpperString(resource));
            writer.WriteAttributeString("users", GetLowerString(item.AllocatorUsers));
            writer.WriteAttributeString("extAllocators", GetLowerString(item.ExtendAllocators));
            writer.WriteAttributeString("extAllocatorArgs", GetLowerString(item.ExtendAllocatorArgs));
            writer.WriteAttributeString("default", GetLowerString(item.DefaultAllocator));
            writer.WriteEndElement();
        }

        /// <summary>
        /// 导出 TaskAllocator 节点.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="item"></param>
        protected void ExportTaskAllocator(XmlWriter writer, ActivityDefinition item)
        {
            writer.WriteStartElement("taskAllocator");
            string resource = item.AllocatorResource;
            if (!string.IsNullOrEmpty(resource))
            {
                if (resource.StartsWith(PrefixDisableResource, StringComparison.OrdinalIgnoreCase))
                {
                    // 不启用权限控制
                    resource = "";
                }
                else if (resourceTranslator != null)
                {
                    resource = resourceTranslator.Name2Alias(resource);
                    if (string.IsNullOrEmpty(resource))
                        resource = item.AllocatorResource;
                }
            }
            writer.WriteAttributeString("resource", GetUpperString(resource));
            writer.WriteAttributeString("users", GetLowerString(item.AllocatorUsers));
            writer.WriteAttributeString("extAllocators", GetLowerString(item.ExtendAllocators));
            writer.WriteAttributeString("extAllocatorArgs", GetLowerString(item.ExtendAllocatorArgs));
            writer.WriteAttributeString("default", GetLowerString(item.DefaultAllocator));
            writer.WriteEndElement();
        }

        /// <summary>
        /// 获取活动名称字符串（以","隔开）.
        /// </summary>
        /// <param name="definitions"></param>
        /// <param name="activitySetId"></param>
        /// <returns></returns>
        protected static string GetActivityNames(IList<ActivityDefinition> definitions, Guid activitySetId)
        {
            if (activitySetId == Guid.Empty)
                return string.Empty;

            IActivitySetService activitySetService = new ActivitySetService();
            IList<Guid> idlist = activitySetService.GetActivityIdSets(activitySetId);

            return GetActivityNames(definitions, idlist);
        }

        /// <summary>
        /// 获取活动名称字符串（以","隔开）.
        /// </summary>
        /// <param name="definitions"></param>
        /// <param name="activityIdSet"></param>
        /// <returns></returns>
        protected static string GetActivityNames(IList<ActivityDefinition> definitions, IList<Guid> activityIdSet)
        {
            StringBuilder nameBuilder = new StringBuilder();
            foreach (ActivityDefinition definition in definitions)
            {
                if (activityIdSet.Contains(definition.ActivityId))
                    nameBuilder.AppendFormat(",{0}", definition.ActivityName);
            }
            if (nameBuilder.Length > 1)
                nameBuilder.Remove(0, 1);
            return nameBuilder.ToString();
        }

        /// <summary>
        /// 获取小写字符串.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string GetLowerString(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
                inputString = inputString.ToLower();
            return inputString;
        }

        /// <summary>
        /// 清除字符串首尾的特殊空白字符.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string TrimWhitespace(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;
            return inputString.Trim('\r', '\n', '\t', ' ');  // 去除空白
        }

        /// <summary>
        /// 获取大写字符串.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        private static string GetUpperString(string inputString)
        {
            if (!string.IsNullOrEmpty(inputString))
                inputString = inputString.ToUpper();
            return inputString;
        }

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfix"></param>
        /// <returns></returns>
        private static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> userResources, string resourcePostfix)
        {
            return GetAllowedWorkflows(workflows, Botwave.XQP.Util.ResourceHelper.GetResouceByParentId(Botwave.XQP.Util.ResourceHelper.Workflow_ResourceId), userResources, resourcePostfix);
        }

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="workflowResourceDict"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfix"></param>
        /// <returns></returns>
        private static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> workflowResourceDict, IDictionary<string, string> userResources, string resourcePostfix)
        {
            if (workflows == null)
                return workflows;
            for (int i = 0; i < workflows.Count; i++)
            {
                string workflowName = workflows[i].WorkflowName.ToLower();
                if (workflowResourceDict.ContainsKey(workflowName))
                {
                    string resourceId = workflowResourceDict[workflowName] + resourcePostfix;
                    if (!userResources.ContainsKey(resourceId))
                    {
                        workflows.RemoveAt(i);
                        i--;
                    }
                }
            }
            return workflows;
        }

        private IDictionary<string, string> GetResources(string userName)
        {
            IDictionary<string, string> userResources = null;
            if (!string.IsNullOrEmpty(userName))
            {
                Botwave.Security.Domain.UserInfo user = userService.GetUserByUserName(userName);
                if (!string.IsNullOrEmpty(userName))
                {
                    Botwave.Security.Domain.UserInfo info = userService.GetUserByUserName(userName);
                    IList<ResourceInfo> resources = IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByUserId", info.UserId);
                    foreach (ResourceInfo item in resources)
                    {
                        string resourceId = item.ResourceId;
                        if (!userResources.ContainsKey(resourceId))
                            userResources.Add(resourceId, resourceId);
                    }
                    //userResources = roleService.GetRolesByUserId(user.UserId);
                }
            }
            return userResources;
        }

        private DataTable GetAttachemnts(string workflowInstanceId)
        {
            string strSql=string.Format(@"SELECT atta.*, attaEntity.EntityId FROM (SELECT [Id], Title, [FileName], MimeType, FileSize, Remark ,creator,createdtime
               FROM xqp_Attachment
               WHERE [Id] IN(
                    SELECT AttachmentId FROM xqp_Attachment_Entity WHERE EntityType = '{0}' AND EntityId IN(
                         SELECT [Id] FROM bwwf_Tracking_Comments WHERE WorkflowInstanceId = '{1}'
                    )
               )) atta LEFT JOIN xqp_Attachment_Entity attaEntity ON atta.[Id] = attaEntity.AttachmentId",Comment.EntityType, workflowInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, strSql).Tables[0];
        }

        private List<Attachment> GetAttachemntsHtml(Guid commentId, DataTable attachementTable)
        {
            if (attachementTable == null || attachementTable.Rows.Count == 0)
                return null;
            DataRow[] rows = attachementTable.Select(string.Format("EntityId = '{0}'", commentId));
            if (rows == null || rows.Length == 0)
                return null;
            List<Attachment> list = new List<Attachment>();
            foreach (DataRow row in rows)
            {
                list.Add(new Attachment()
                {
                    Name = row["Title"] == null ? string.Empty : row["Title"].ToString(),
                    Creator = row["Creator"] == null ? string.Empty : row["Creator"].ToString(),
                    CreatedTime = row["CreatedTime"] == null ? string.Empty : row["CreatedTime"].ToString(),
                    Url = row["FileName"] == null ? string.Empty : row["FileName"].ToString(),
                    UserName = row["Title"] == null ? string.Empty : row["Title"].ToString()
                });
            }
            return list;
        }

        #endregion

        #endregion
    }
}
