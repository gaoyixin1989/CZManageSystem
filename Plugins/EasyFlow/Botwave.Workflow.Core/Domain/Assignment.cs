using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ����ָ��.
    /// </summary>
    public class Assignment
    {
        private Guid? activityInstanceId;
        private string assignedUser;
        private string assigningUser;
        private DateTime? assignedTime;
        private string message;

        /// <summary>
        /// �ʵ��Id.
        /// </summary>
        public Guid? ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// ��ָ���û����û���.
        /// </summary>
        public string AssignedUser
        {
            get { return assignedUser; }
            set { assignedUser = value; }
        }

        /// <summary>
        /// ָ���û����û���.
        /// </summary>
        public string AssigningUser
        {
            get { return assigningUser; }
            set { assigningUser = value; }
        }

        /// <summary>
        /// ָ��ʱ��.
        /// </summary>
        public DateTime? AssignedTime
        {
            get { return assignedTime; }
            set { assignedTime = value; }
        }

        /// <summary>
        /// ˵����Ϣ.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// ת��Ϊ�ַ�����ʽ.
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
