using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 流程通用服务的默认实现类.
    /// </summary>
    public class WorkflowGenericService : IWorkflowGenericService
    {

        #region IWorkflowGenericService 成员

        public bool AuzhorizeTodo(string userName, string proxyName)
        {
            return true;
        }

        #endregion
    }
}
