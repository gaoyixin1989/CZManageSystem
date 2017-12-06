using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 需求单处理记录信息 
    /// </summary> 
    [Serializable]
    public class WorkflowRecord
    {
        /// <summary>  
        /// 工单步骤标识 
        /// </summary> 
        public string ActivityInstanceId { get; set; }

        /// <summary>  
        /// 工单步骤名称 
        /// </summary> 
        public string ActivityName { get; set; }

        /// <summary>  
        /// 工单步骤处理人 
        /// </summary> 
        public string Actor { get; set; }

        /// <summary>  
        /// 处理命令。0，表示退还处理；1，表示通过处理；2，转交处理 
        /// </summary> 
        public int Command { get; set; }

        /// <summary>  
        /// 工单步骤创建时间 
        /// </summary> 
        public string CreatedTime { get; set; }

        /// <summary>  
        /// 工单步骤完成（处理）时间 
        /// </summary> 
        public string CompletedTime { get; set; }
    }
}