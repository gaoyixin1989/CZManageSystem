using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Botwave.XQP.Service
{
    /// <summary>
    /// 流程通用服务接口.
    /// </summary>
    public interface IWorkflowGenericService
    {
        /// <summary>
        /// 授权指定委托用户(proxyName)的全部代办任务给指定被授权用户(userName)来处理.
        /// </summary>
        /// <param name="userName">被授权处理的用户.</param>
        /// <param name="proxyName">指定委托用户.</param>
        bool AuzhorizeTodo(string userName, string proxyName);
    }
}
