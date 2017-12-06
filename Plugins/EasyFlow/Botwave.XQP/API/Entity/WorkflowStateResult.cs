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
    public class WorkflowStateResult : ApiResult
    {

        /// <summary>  
        /// 需求单发起人（Portal 登录账号） 
        /// </summary> 
        public string WorkflowInstanceId { get; set; }

        /// <summary>  
        /// 需求单受理号。 
        /// </summary> 
        public string WorkflowSheetId { get; set; }

        /// <summary>  
        /// 返回需求单标题。 
        /// </summary> 
        public string WorkflowTitle { get; set; }

        /// <summary>  
        /// 需求单发起人（Portal 登录账号） 
        /// </summary> 
        public string Creator { get; set; }

        /// <summary>  
        /// 需求单状态。-1：未知，如果未通过应用系统验证返回-1；0：尚未启动（草稿）；1：工单正在处理中；2：工单已取消；3：工单已完成。 
        /// </summary> 
        public int State { get; set; }

        /// <summary>  
        /// 工单当前正在处理的步骤名称数组。 
        /// </summary> 
        public string[] CurrentActivities { get; set; }
    }
}