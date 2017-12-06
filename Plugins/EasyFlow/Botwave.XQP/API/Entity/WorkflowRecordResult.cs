using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 工单处理（已办）记录结果 
    /// </summary> 
    [Serializable]
    public class WorkflowRecordResult : ApiResult
    {

        /// <summary>  
        /// 返回的处理记录列表.请见类型WorkflowRecord

        /// </summary> 
        public WorkflowRecord[] Records { get; set; }
    }
}