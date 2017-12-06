using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Botwave.XQP.Service.WS;
using Botwave.XQP.Service.Support;

namespace Botwave.XQP.Web
{
    /// <summary>
    /// Summary description for WorkflowCooperation
    /// </summary>
    [WebService(Namespace = "http://ef.cz.gmcc.net/")]
    //[WebService(Namespace = "http://tempuri.org")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WorkflowCooperationService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowCooperationService));
        private static IWorkflowCooperation workflowCooperation;

        static WorkflowCooperationService()
        {
            workflowCooperation = Spring.Context.Support.WebApplicationContext.Current["workflowCooperation"] as IWorkflowCooperation;
        }

        public WorkflowCooperationService()
        {  }

        /// <summary>
        /// 执行流程活动.
        /// </summary>
        /// <param name="sysAccount">系统接入帐号</param>
        /// <param name="sysPassword">系统接入密码</param>
        /// <param name="activityInstanceId">待执行活动实例Id</param>
        /// <param name="actor">操作人.</param>
        /// <param name="content">短信内容.</param>
        /// <returns></returns>
        [WebMethod]
        public Botwave.XQP.Service.WS.ActionResult ExecuteActivity(string sysAccount, string sysPassword, string activityInstanceId, string actor, string content)
        {
            Botwave.XQP.Service.WS.ActionResult result = new Botwave.XQP.Service.WS.ActionResult();
            result.ReturnValue = true;
            Guid? activityInstanceIdValue = null;
            if (!string.IsNullOrEmpty(activityInstanceId))
            {
                try
                {
                    activityInstanceIdValue = new Guid(activityInstanceId.Trim());
                }
                catch
                { }
            }
            if (!activityInstanceIdValue.HasValue)
            {
                result.ReturnValue = false;
                result.ReturnMessage = "activityInstanceId 参数错误.";
                return result;
            }
            return workflowCooperation.ExecuteActivity(sysAccount, sysPassword, activityInstanceIdValue.Value, actor, content);
        }

        /// <summary>
        /// 拷贝流程实例.
        ///     应用于人力流程每月自动创建工单.
        /// </summary>
        /// <param name="sysAccount">系统接入帐号</param>
        /// <param name="sysPassword">系统接入密码</param>
        /// <param name="workflowInstanceId">流程实例Id</param>
        /// <returns></returns>
        [WebMethod]
        public Botwave.XQP.Service.WS.ActionResult CopyWorkflowInstance(string sysAccount, string sysPassword, string workflowInstanceId)
        {
            Botwave.XQP.Service.WS.ActionResult result = new Botwave.XQP.Service.WS.ActionResult();
            result.ReturnValue = true;
            Guid? workflowInstanceIdValue = null;
            if (!string.IsNullOrEmpty(workflowInstanceId))
            {
                try
                {
                    workflowInstanceIdValue = new Guid(workflowInstanceId.Trim());
                }
                catch
                { }
            }
            if (!workflowInstanceIdValue.HasValue)
            {
                result.ReturnValue = false;
                result.ReturnMessage = "workflowInstanceId 参数错误.";
                return result;
            }
            return workflowCooperation.CopyWorkflowInstance(sysAccount, sysPassword, workflowInstanceIdValue.Value);
        }   
    }

}
