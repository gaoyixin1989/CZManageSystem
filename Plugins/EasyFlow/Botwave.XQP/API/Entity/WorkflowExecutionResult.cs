using System;
using System.Collections.Generic;
//using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 工单执行（发起或处理）结果 
    /// </summary> 
    [Serializable]
    public class WorkflowExecutionResult : ApiResult
    {

        /// <summary>  
        /// 返回是否已执行（发起或处理）成功 
        /// </summary> 
        public string Success { get; set; }

        /// <summary>  
        /// 返回当前发起或处理的工单标识。 
        /// </summary> 
        public string WorkflowInstanceId { get; set; }

        public Activity[] NextActivities { get; set; }

        
    }
}