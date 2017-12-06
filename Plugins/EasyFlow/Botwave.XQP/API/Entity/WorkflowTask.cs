using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    /// <summary>  
    /// 流程工单任务基本信息 
    /// </summary> 
    [Serializable]
    public class WorkflowTask
    {
        /// <summary>  
        /// 工单标识 
        /// </summary> 
        public string WorkflowInstanceId { get; set; }

        /// <summary>  
        /// 流程名称 
        /// </summary> 
        public string WorkflowName { get; set; }

        /// <summary>  
        /// 流程分组 
        /// </summary> 
        public string Category { get; set; }

        /// <summary>  
        /// 受理号 
        /// </summary> 
        public string SheetId { get; set; }

        /// <summary>  
        /// 工单标题 
        /// </summary> 
        public string Title { get; set; }

        /// <summary>  
        /// 工单当前步骤.多个步骤之间以逗号隔开 
        /// </summary> 
        public string CurrentActvities { get; set; }

        /// <summary>  
        /// 工单当前处理人.多个处理人之间以逗号隔开 
        /// </summary> 
        public string CurrentActors { get; set; }

        /// <summary>  
        /// 工单发起人 
        /// </summary> 
        public string Starter { get; set; }

        /// <summary>  
        /// 工单发起时间 
        /// </summary> 
        public string StartedTime { get; set; }

        /// <summary>  
        /// 工单滞留时间 
        /// </summary> 
        public string ResortTime { get; set; }

        // Properties
        public string ActivityActor { get; set; }
        public string ActivityInstanceId { get; set; }
        public string ActivityName { get; set; }
        public string FinishedTime { get; set; }
 

    }
}