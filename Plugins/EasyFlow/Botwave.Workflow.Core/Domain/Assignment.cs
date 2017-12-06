using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 任务指派.
    /// </summary>
    public class Assignment
    {
        private Guid? activityInstanceId;
        private string assignedUser;
        private string assigningUser;
        private DateTime? assignedTime;
        private string message;

        /// <summary>
        /// 活动实例Id.
        /// </summary>
        public Guid? ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 被指派用户的用户名.
        /// </summary>
        public string AssignedUser
        {
            get { return assignedUser; }
            set { assignedUser = value; }
        }

        /// <summary>
        /// 指派用户的用户名.
        /// </summary>
        public string AssigningUser
        {
            get { return assigningUser; }
            set { assigningUser = value; }
        }

        /// <summary>
        /// 指派时间.
        /// </summary>
        public DateTime? AssignedTime
        {
            get { return assignedTime; }
            set { assignedTime = value; }
        }

        /// <summary>
        /// 说明信息.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 转换为字符串形式.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder().AppendFormat("[activityInstanceId:{0}]\r\n", activityInstanceId.HasValue ? activityInstanceId.ToString() : String.Empty)
                .AppendFormat("[assignedUser:{0}]\r\n", null == assignedUser ? String.Empty : assignedUser)
                .AppendFormat("[assigningUser:{0}]\r\n", null == assigningUser ? String.Empty : assigningUser)
                .AppendFormat("[assignedTime:{0}]\r\n", assignedTime.HasValue ? assignedTime.ToString() : String.Empty)
                .AppendFormat("[message:{0}]\r\n", null == message ? String.Empty : message)
                .ToString();
        }
    }
}
