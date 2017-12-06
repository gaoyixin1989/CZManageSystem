using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 评论.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// 评论的实体类型.
        /// </summary>
        public static readonly string EntityType = "W_C";

        private Guid? id;
        private Guid? workflowInstanceId;
        private Guid? activityInstanceId;
        private string message;
        private string creator;
        private DateTime? createdTime;
        
        /// <summary>
        /// 评论 Id.
        /// </summary>
        public Guid? Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 流程实例Id.
        /// </summary>
        public Guid? WorkflowInstanceId
        {
            get { return workflowInstanceId; }
            set { workflowInstanceId = value; }
        }

        /// <summary>
        /// 活动实例Id.
        /// </summary>
        public Guid? ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 评论信息.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 创建人.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 转换为字符串形式.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder().AppendFormat("[id:{0}]\r\n", id.HasValue ? id.ToString() : String.Empty)
                .AppendFormat("[workflowInstanceId:{0}]\r\n", workflowInstanceId.HasValue ? workflowInstanceId.ToString() : String.Empty)
                .AppendFormat("[activityInstanceId:{0}]\r\n", activityInstanceId.HasValue ? activityInstanceId.ToString() : String.Empty)
                .AppendFormat("[message:{0}]\r\n", null == message ? string.Empty : message)
                .AppendFormat("[creator:{0}]\r\n", null == creator ? string.Empty : creator)
                .AppendFormat("[createdTime:{0}]\r\n", createdTime.HasValue ? createdTime.ToString() : String.Empty)
                .ToString();
        }
    }
}
