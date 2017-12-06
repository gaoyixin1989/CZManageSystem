using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 工单明细结果 
    /// </summary> 
    [Serializable]
    public class WorkflowDetailResult : ApiResult
    {

        /// <summary>  
        /// 返回的工单明细，请见类型WorkflowDetail

        /// </summary> 
        public WorkflowDetail Detail { get; set; }
    }
}