using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Service.WS
{
    /// <summary>
    /// 流程协作接口
    /// </summary>
    public interface IWorkflowCooperation
    {
        /// <summary>
        /// 执行流程活动.
        /// </summary>
        /// <param name="sysAccount">系统接入帐号</param>
        /// <param name="sysPassword">系统接入密码</param>
        /// <param name="activityInstanceId">待执行活动实例Id</param>
        /// <param name="actor">处理人用户名.</param>
        /// <param name="content">短信内容.</param>
        /// <returns></returns>
        ActionResult ExecuteActivity(string sysAccount, string sysPassword, Guid activityInstanceId, string actor, string content);

        /// <summary>
        /// 拷贝流程实例
        ///     应用于人力流程每月自动创建工单.
        /// </summary>
        /// <param name="sysAccount">系统接入帐号</param>
        /// <param name="sysPassword">系统接入密码</param>
        /// <param name="workflowInstanceId">流程实例Id</param>
        /// <returns></returns>
        ActionResult CopyWorkflowInstance(string sysAccount, string sysPassword, Guid workflowInstanceId);

     }
}
