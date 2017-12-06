using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 部署流程 
    /// </summary> 
    [Serializable]
    public class WorkflowDeployResult:ApiResult
    {
        /// <summary>
        /// 流程主键ID
        /// </summary>
        public string WorkflowId { get; set; }
    }
}
