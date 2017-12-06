using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;

using Botwave.Commons;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;
using System.Text.RegularExpressions;

namespace Botwave.Workflow.Practices.CZMCC
{
    public class GratuityService : Botwave.Workflow.Practices.CZMCC.IGratuityService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(GratuityService));

        private string workflowName = "销售精英竞赛平台酬金申告流程";
        private string nextActivityName = "营销经理";
        private string employeePrefix = "XSJY_";
        private IUserService userService;
        private IWorkflowDefinitionService workflowDefinitionService;
        private IActivityDefinitionService activityDefinitionService;
        private IPostActivityExecutionHandler postActivityExecutionMessageHandler;

        public string WorkflowName
        {
            set { workflowName = value; }
        }

        public string NextActivityName
        {
            set { nextActivityName = value; }
        }

        /// <summary>
        /// 工号前缀.
        /// </summary>
        public string EmployeePrefix
        {
            set { employeePrefix = value; }
        }

        public IUserService UserService
        {
            set { userService = value; }
        }

        public IWorkflowDefinitionService WorkflowDefinitionService
        {
            set { workflowDefinitionService = value; }
        }

        public IActivityDefinitionService ActivityDefinitionService
        {
            set { activityDefinitionService = value; }
        }

        public IPostActivityExecutionHandler PostActivityExecutionMessageHandler
        {
            set { postActivityExecutionMessageHandler = value; }
        }

        #region IGratuityFlowService Members

        /// <summary>
        /// 提交申告单
        /// </summary>
        /// <param name="title">申请单标题</param>
        /// <param name="fromEmployeeId">发起人工号</param>
        /// <param name="fromEmployeeName">发起人姓名</param>
        /// <param name="toEmployeeId">处理人工号</param>
        /// <param name="fileUrl">附件地址</param>
        /// <param name="detailUrl">详细信息地址</param>
        /// <param name="applyStyle">卡类型</param>
        /// <param name="description">原因描述</param>
        /// <returns></returns>
        public bool SendGratuityFlow(string title, string fromEmployeeId, string fromEmployeeName, string toEmployeeId, string fileUrl, string detailUrl, int applyStyle, string description)
        {
            if (applyStyle < 0 || applyStyle > 5)
            {
                log.WarnFormat("applyStyle {0} 应当在大于等于0并且小于等于5", applyStyle);
                return false;
            }

            if (null == title) title = String.Empty;
            if (null == fromEmployeeId) fromEmployeeId = String.Empty;
            if (null == fromEmployeeName) fromEmployeeName = String.Empty;
            if (null == toEmployeeId) toEmployeeId = String.Empty;
            if (null == fileUrl) fileUrl = String.Empty;
            if (null == detailUrl) detailUrl = String.Empty;
            description = string.IsNullOrEmpty(description) ? string.Empty : FormatDescription(description);

            // 加上前缀.
            fromEmployeeId = employeePrefix + fromEmployeeId;
            //toEmployeeId = employeePrefix + toEmployeeId;

            if (log.IsInfoEnabled)
            {
                HttpRequest req = HttpContext.Current.Request;
                log.InfoFormat("{0} SendGratuityFlow, args [title:{1}]  [fromEmployeeId:{2}]  [fromEmployeeName:{3}]  [toEmployeeId:{4}]  [fileUrl:{5}]  [detailUrl:{6}]  [applyStyle:{7}]  [description:{8}]", req.UserHostAddress, title, fromEmployeeId, fromEmployeeName, toEmployeeId, fileUrl, detailUrl, applyStyle, description);
            }

            try
            {
                UserInfo currentUser = userService.GetUserByUserName(fromEmployeeId);                
                if (null == currentUser)
                {
                    log.WarnFormat("在系统里面找不到发起人 [{0}]/[{1}]", fromEmployeeId, fromEmployeeName);
                    return false;
                }

                UserInfo nextActor = userService.GetUserByUserName(toEmployeeId);
                if (null == nextActor)
                {
                    log.WarnFormat("在系统里面找不到处理人 [{0}]", toEmployeeId);
                    return false;
                }

                //获取当前销售精英竞赛平台流程的流程ID.
                WorkflowDefinition workflowDefinition = workflowDefinitionService.GetCurrentWorkflowDefinition(workflowName);
                if (null == workflowDefinition)
                {
                    log.WarnFormat("在系统里面找不到有效的流程 [{0}]", workflowName);
                    return false;
                }

                string[] activityNames = new string[] { nextActivityName };
                IList<ActivityDefinition> list = activityDefinitionService.GetActivityDefinitionsByActivityNames(workflowDefinition.WorkflowId, activityNames);
                if (null == list || list.Count == 0)
                {
                    log.WarnFormat("在系统里面找不到活动 [{0}]", nextActivityName);
                    return false;
                }

                IDictionary<string, object> formVariables = new Dictionary<string, object>();
                formVariables.Add("F1", fileUrl);
                formVariables.Add("F2", detailUrl);
                formVariables.Add("F3", applyStyle);
                formVariables.Add("F4", description);

                WorkflowInstance workflowInstance = new WorkflowInstance();
                workflowInstance.WorkflowInstanceId = Guid.NewGuid();
                workflowInstance.ExpectFinishedTime = new DateTime(2900, 1, 1);
                workflowInstance.Title = title;
                workflowInstance.Secrecy = 0;
                workflowInstance.Urgency = 0;
                workflowInstance.Importance = 0;
                workflowInstance.WorkflowId = workflowDefinition.WorkflowId;
                workflowInstance.Creator = currentUser.UserName;

                ActivityExecutionContext context = new ActivityExecutionContext();
                context.Actor = currentUser.UserName;
                context.Command = ActivityCommands.Approve;
                context.Reason = "同意";
                context.ActorDescription = currentUser.RealName;

                IDictionary<string, string> nextActors = new Dictionary<string, string>();
                nextActors.Add(nextActor.UserName, null);//

                IDictionary<Guid, IDictionary<string, string>> allocatees = new Dictionary<Guid, IDictionary<string, string>>();
                allocatees.Add(list[0].ActivityId, nextActors);//销售精英竞赛平台流程的营销经理步骤Id

                context.ActivityAllocatees = allocatees;

                context.Variables = formVariables;
                context.Variables["Secrecy"] = workflowInstance.Secrecy;
                context.Variables["Urgency"] = workflowInstance.Urgency;
                context.Variables["Importance"] = workflowInstance.Importance;
                context.Variables["CurrentUser"] = currentUser;

                Guid initActivityInstanceId = WorkflowDataHelper.StartWorkflowByLock(workflowInstance, formVariables, context, currentUser.UserName, false);
                
                // 发送消息通知.
                if (initActivityInstanceId != Guid.Empty)
                {
                    context.ActivityAllocatees.Clear();
                    context.ActivityInstanceId = initActivityInstanceId;

                    this.PostExecuteActivity(context);
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }            
        }

        /// <summary>
        /// 获取申告单列表
        /// </summary>
        /// <param name="employeeId">省工号</param>
        /// <returns></returns>
        /// <summary>        
        public DataSet ApplyListDs(string employeeId)
        {
            if (null == employeeId) 
                employeeId = String.Empty;
            employeeId = employeePrefix + employeeId; // 加上前缀.

            if (log.IsInfoEnabled)
            {
                HttpRequest req = HttpContext.Current.Request;
                log.InfoFormat("{0} ApplyListDs, args [employeeId:{1}]  ", req.UserHostAddress, employeeId);
            }

            try
            {
                //Status 1 处理中  2 已完成
                //AppTitle 申告标题, AppNO 申告单号, AppTime 申告日期, ProcessEndDate 完成日期
                string cmdText = @"select CAST(tw.WorkflowInstanceId AS nvarchar(50)) as AppID, tw.Title as AppTitle, tw.SheetId as AppNO, 
    (CASE WHEN u.[Type]=1 AND ISNULL(u.Ext_Str1,'') <> '' THEN u.Ext_Str1 ELSE tw.Creator END) as AppEmpID, 
    u.RealName as AppEmpName, tw.StartedTime as AppTime, tw.FinishedTime as ProcessEndDate,tw.State as Status 
from dbo.bwwf_Tracking_Workflows as tw 
	LEFT join dbo.bw_Users as u on tw.Creator = u.UserName
	LEFT join dbo.bwwf_Workflows as w on tw.WorkflowId = w.WorkflowId
where tw.Creator = @Creator and w.WorkflowName = @WorkflowName
order by tw.StartedTime DESC";
                IDbDataParameter[] parms = IBatisDbHelper.CreateParameterSet(2);
                parms[0].ParameterName = "@Creator";
                parms[0].Value = employeeId;
                parms[1].ParameterName = "@WorkflowName";
                parms[1].Value = this.workflowName;
                DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.Text, cmdText, parms);
                if (null == ds || ds.Tables.Count == 0)
                {
                    log.Info("the dataset is null");
                    return null;
                }

                LogDataSetInfo(ds);
                return ds;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }            
        }

        /// 获取申告单信息
        /// </summary>
        /// <param name="applyId">单号/受理号</param>
        /// <returns></returns>
        public DataSet ApplyRowDs(string applyId)
        {
            if (null == applyId)
                applyId = String.Empty;

            //string Sql_1 = "exec|insert+|select+|delete|update|count|chr|mid|master+|truncate|char|declare|drop+|drop+table|creat+|creat+table|%20|%2b|%2c|%3a|%3b|%3c|%3e|%5c|%5e|%21|%22|%24|%25|%26|%27|%28|%29|'|(|)|%40|!|+";//<>%"$\@
            //string[] sql_c = Sql_1.Split('|');

            //if (!string.IsNullOrEmpty(applyId))
            //{
            //    foreach (string sl in sql_c)
            //    {
            //        if (applyId.ToLower().IndexOf(sl.Trim()) >= 0)
            //        {
            //            log.Warn("发现SQL注入攻击！警告！" + applyId);
            //            return null;
            //        }
            //    }
            //}

            if (!IsGuidByRegNoComplied(applyId))
            {
                log.Info("the applyId is error：" + applyId);
                return null;
            }

            Guid? workflowInstanceId = DbUtils.ToGuid(applyId);
            if (!workflowInstanceId.HasValue)
            {
                log.Info("the applyId is error：" + applyId);
                return null;
            }

            if (log.IsInfoEnabled)
            {
                HttpRequest req = HttpContext.Current.Request;
                log.InfoFormat("{0} ApplyRowDs, args [applyId:{1}]  ", req.UserHostAddress, applyId);
            }

            try
            {
                //AppTitle,ApplyStyle,AppEmpName,AppNO,MarketingEmpID 营销经理,AppTime,ProcessEndDate,Discript,RevertContent 审批回复,FilePath
                string cmdText = "dbo.cz_Gratuity_GetApplyInfo";
                IDbDataParameter param = IBatisDbHelper.CreateParameter();
                param.ParameterName = "@WorkflowInstanceId";
                //param.Value = applyId;
                param.Value = workflowInstanceId.Value;

                //结果有三个表,第一个表为流程实例信息,第二个为表单信息,第三个为处理信息
                DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, cmdText, param);
                if (null == ds || ds.Tables.Count == 0)
                {
                    log.Info("the dataset is null");
                    return null;
                }

                LogDataSetInfo(ds);

                DataTable dataTable = new DataTable();
                string[] columns = new string[] { "AppID", "AppTitle", "AppEmpID", "AppEmpName", "AppNO", "AppTime", "ProcessEndDate", "ApplyStyle", "Discript", "FilePath", "MarketingEmpID", "RevertContent" };
                foreach (string colName in columns)
                {
                    dataTable.Columns.Add(colName);
                }

                DataRow dataRow = dataTable.NewRow();
                dataRow["AppID"] = DbUtils.ToString(ds.Tables[0].Rows[0]["AppID"]);
                dataRow["AppEmpID"] = ds.Tables[0].Rows[0]["AppEmpID"];
                dataRow["AppTitle"] = ds.Tables[0].Rows[0]["AppTitle"];
                dataRow["AppEmpName"] = ds.Tables[0].Rows[0]["AppEmpName"];
                dataRow["AppNO"] = ds.Tables[0].Rows[0]["AppNO"];
                dataRow["AppTime"] = ds.Tables[0].Rows[0]["AppTime"];
                dataRow["ProcessEndDate"] = ds.Tables[0].Rows[0]["ProcessEndDate"];
                dataRow["MarketingEmpID"] = string.Empty;


                if (ds.Tables.Count >= 2 && ds.Tables[1].Rows.Count >= 1)
                {
                    IDictionary<string, object> dict = new Dictionary<string, object>();
                    DataTable formTable = ds.Tables[1];
                    //Value, FName, Name
                    foreach (DataRow row in formTable.Rows)
                    {
                        string fname = row["FName"].ToString();
                        dict.Add(fname, row["Value"]);
                    }
                    if (dict.ContainsKey("F1"))
                    {
                        dataRow["FilePath"] = dict["F1"];
                    }
                    if (dict.ContainsKey("F3"))
                    {
                        dataRow["ApplyStyle"] = dict["F3"];
                    }
                    if (dict.ContainsKey("F4"))
                    {
                        dataRow["Discript"] = dict["F4"];
                    }
                }

                if (ds.Tables.Count >= 3 && ds.Tables[2].Rows.Count >= 1)
                {
                    //Actor, Reason
                    dataRow["MarketingEmpID"] = ds.Tables[2].Rows[0]["Actor"];
                    dataRow["RevertContent"] = ds.Tables[2].Rows[0]["Reason"];
                }

                dataTable.Rows.Add(dataRow);
                DataSet dataSet = new DataSet();
                dataSet.Tables.Add(dataTable);
                return dataSet;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        #endregion

        static void LogDataSetInfo(DataSet ds)
        {
            int tables = ds.Tables.Count;
            log.InfoFormat("the dataset has {0} tables", tables);
            if (tables > 0)
            {
                log.InfoFormat("the first table has {0} rows", ds.Tables[0].Rows.Count);
            }
        }

        /// <summary>
        /// 格式化描述内容.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        static string FormatDescription(string description)
        {
            description = description.Replace("<br>", "\r\n");
            return description;
        }

        /// <summary>
        /// 执行流程的后续处理.
        /// </summary>
        /// <param name="context"></param>
        private void PostExecuteActivity(ActivityExecutionContext context)
        {
            if (null != postActivityExecutionMessageHandler)
            {
                postActivityExecutionMessageHandler.Execute(context);
                IPostActivityExecutionHandler next = postActivityExecutionMessageHandler.Next;
                while (next != null)
                {
                    next.Execute(context);
                    next = next.Next;
                }
            }
        }

        private bool IsGuidByRegNoComplied(string strSrc)
        {

            Regex reg = new Regex("^[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}$");

            return reg.IsMatch(strSrc.ToUpper());

        }
    }
}
