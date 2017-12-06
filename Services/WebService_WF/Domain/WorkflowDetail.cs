using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService_WF.Domain
{
    /// <summary>  
    /// 工单明细信息 
    /// </summary> 
    [Serializable]
    public class WorkflowDetail
    {
        /// <summary>  
        /// 工单标识 
        /// </summary> 
        public string WorkflowInstanceId { get; set; }

        /// <summary>  
        /// 当前流程名称 
        /// </summary> 
        public string WorkflowName { get; set; }
        
        /// <summary>  
        /// 受理号 
        /// </summary> 
        public string SheetId { get; set; }

        /// <summary>  
        /// 需求单状态-1：未知，如果未通过应用系统验证返回-1；0：尚未启动（草稿）1：工单正在处理中；2：工单已取消;3：工单已完成
        /// 该说明是从oa复制过来，我未弄明白这个状态的含义——gyx
        /// </summary> 
        public int State { get; set; }
        
        /// <summary>  
        /// 工单标题 
        /// </summary> 
        public string Title { get; set; }

        /// <summary>  
        /// 保密设置。0：不保密；1：保密 
        /// </summary> 
        public int Secrecy { get; set; }

        /// <summary>  
        /// 紧急程度。0：一般；1：紧急；2：最紧急 
        /// </summary> 
        public int Urgency { get; set; }

        /// <summary>  
        /// 重要级别。0：一般；1：重要；2：最重要 
        /// </summary> 
        public int Importance { get; set; }

        /// <summary>  
        /// 工单期望完成时间 
        /// </summary> 
        public string ExpectFinishedTime { get; set; }

        /// <summary>  
        /// 工单发起时间 
        /// </summary> 
        public string StartedTime { get; set; }

        /// <summary>  
        /// 工单完成时间。若工作没有完成，则值为空 
        /// </summary> 
        public string FinishedTime { get; set; }

        /// <summary>
        /// 工单发起人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 工单发起时间
        /// </summary>
        public string CreatedTime { get; set; }

        /// <summary>
        /// 流程ID
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// 当前步骤节点名称
        /// </summary>
        public string ActivityName { get; set; }

        /// <summary>  
        /// 工单当前处理人，多个处理人之间以逗号隔开 
        /// </summary> 
        public string CurrentActors { get; set; }

        public string WorkflowAlias { get; set; }

        /// <summary>
        /// 工单发起人名字
        /// </summary>
        public string Username { get; set; }

        /// <summary>  
        /// 工单发起人手机号
        /// </summary> 
        public string Mobile { get; set; }
        

        /// <summary>  
        /// 表单字段列表。请见类型Field

        /// </summary> 
        public List<Field> Fields { get; set; }

        /// <summary>  
        /// 当前步骤定义。请见类型Activity。若若工单已完成则无下行步骤 
        /// </summary> 
        public List<Activity> Activities { get; set; }

        /// <summary>  
        /// 上行步骤定义。请见类型Activity。若若工单已完成则无下行步骤 
        /// </summary> 
        public List<Activity> PreActivities { get; set; }
        
        /// <summary>  
        /// 下行步骤定义。请见类型Activity。若若工单已完成则无下行步骤 
        /// </summary> 
        public List<Activity> NextActivities { get; set; }

        /// <summary>  
        /// 附件定义。请见类型Attachment 
        /// </summary> 
        public List<Attachment> Attachments { get; set; }
        
    }
}