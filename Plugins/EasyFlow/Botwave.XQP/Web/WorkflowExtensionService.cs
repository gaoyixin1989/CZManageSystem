using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;
using System.Data.SqlClient;

namespace Botwave.XQP.Web
{
    /// <summary>
    /// 流程 AJAX Web Service 类.
    /// </summary>
    [WebService(Namespace = "Botwave.Workflow.Extension.WebServices.WorkflowAjaxService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class WorkflowExtensionService : System.Web.Services.WebService
    {
        #region properties

        private IWorkflowService workflowService;
        private IWorkflowUserService workflowUserService;

        /// <summary>
        /// 流程服务.
        /// </summary>
        public IWorkflowService WorkflowService
        {
            get { return workflowService; }
            set { workflowService = value; }
        }

        /// <summary>
        /// 流程用户服务.
        /// </summary>
        public IWorkflowUserService WorkflowUserService
        {
            get { return workflowUserService; }
            set { workflowUserService = value; }
        }

        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowExtensionService()
        {
            this.workflowService = Spring.Context.Support.WebApplicationContext.Current[WorkflowUtility.Object_WorkflowService] as IWorkflowService;
            this.workflowUserService = Spring.Context.Support.WebApplicationContext.Current[WorkflowUtility.Object_WorkflowUserService] as IWorkflowUserService;
        }

        /// <summary>
        /// 获取指定工单的步骤集合
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetCurrentActNames(string workflowInstanceId,int state)
        {
            if(state == 2)
                return "完成";
            if(state == 99)
                return "取消";
            else{
                SqlParameter[] pa = new SqlParameter[1];
                pa[0] = new SqlParameter("@WorkflowInstanceId", SqlDbType.UniqueIdentifier);
            pa[0].Value = new Guid(workflowInstanceId);
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select dbo.fn_bwwf_GetCurrentActivityNames(@WorkflowInstanceId)", pa);
            return Botwave.Commons.DbUtils.ToString(result);
            }
        }

        /// <summary>
        /// 获取指定工单的处理人集合
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetCurrentActors(string workflowInstanceId, int state)
        {
            if (state == 2)
                return string.Empty;
            if (state == 99)
                return string.Empty;
            else
            {
                SqlParameter[] pa = new SqlParameter[1];
                pa[0] = new SqlParameter("@WorkflowInstanceId", SqlDbType.UniqueIdentifier);
                pa[0].Value = new Guid(workflowInstanceId);
                object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select dbo.fn_bwwf_GetCurrentActors(@WorkflowInstanceId)", pa);
                if (result != null)
                {
                    string str = Botwave.Commons.DbUtils.ToString(result);
                    str = WorkflowUtility.FormatWorkflowActor(str);
                    return str;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取发送人
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetPreActors(string prevSetId, string actor)
        {
            SqlParameter[] pa = new SqlParameter[2];
            pa[0] = new SqlParameter("@PrevActivityInstanceSetId", SqlDbType.UniqueIdentifier);
            pa[0].Value = new Guid(prevSetId);
            pa[1] = new SqlParameter("@Creator", SqlDbType.NVarChar, 50);
            pa[1].Value = actor;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select dbo.fn_bwwf_GetPreviousActors(@PrevId,@Actor)", pa);
            return Botwave.Commons.DbUtils.ToString(result);
        }

        /// <summary>
        /// 获取手机待办数
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetTodoCount(string actor)
        {
            SqlParameter[] pa = new SqlParameter[2];
            pa[0] = new SqlParameter("@actor", SqlDbType.NVarChar,50);
            pa[0].Value = actor;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select count(0) from vw_mp_bwwf_tracking_todo where (IsCompleted = 0) AND (UserName = @actor) ", pa);
            return Botwave.Commons.DbUtils.ToString(result);
        }

        /// <summary>
        /// 获取手机待阅数
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetToReviewCount(string actor)
        {
            SqlParameter[] pa = new SqlParameter[2];
            pa[0] = new SqlParameter("@actor", SqlDbType.NVarChar, 50);
            pa[0].Value = actor;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, "select count(0) from vw_mp_bwwf_Tracking_ToReview_Detail where (State = 0) AND (UserName = @actor) ", pa);
            return Botwave.Commons.DbUtils.ToString(result);
        }

        /// <summary>
        /// 获取手机草稿箱数
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetDraftCount(string actor)
        {
            SqlParameter[] pa = new SqlParameter[2];
            pa[0] = new SqlParameter("@actor", SqlDbType.NVarChar, 50);
            pa[0].Value = actor;
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, @"select count(0) from bwwf_tracking_workflows tw 
inner join bwwf_workflows bw on tw.workflowid=bw.workflowid 
inner join xqp_WorkflowSettings ws on bw.WorkflowName=ws.WorkflowName
 where (tw.State = 0) AND (tw.Creator = @actor) and isnull(IsMobile,1) = 1 ", pa);
            return Botwave.Commons.DbUtils.ToString(result);
        }
    }
}
