using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 工单任务（发起或处理）列表信息结果 
    /// </summary> 
    [Serializable]
    public class WorkflowTaskResult : ApiResult
    {

        /// <summary>  
        /// 返回的用户待办数组.请见类型WorkflowTask

        /// </summary> 
        public WorkflowTask[] Tasks { get; set; }
    }
}