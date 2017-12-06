using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 工单待办列表信息结果 
    /// </summary> 
    [Serializable]
    public class WorkflowTodoResult : ApiResult
    {
     
        /// <summary>  
        /// 返回的用户待办数组. 请见类型WorkflowTodo
        /// </summary> 
        public WorkflowTodo[] Todos { get; set; }
    }
}