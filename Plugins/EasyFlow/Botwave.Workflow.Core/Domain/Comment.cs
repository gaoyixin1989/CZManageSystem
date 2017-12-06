using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// ����.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// ���۵�ʵ������.
        /// </summary>
        public static readonly string EntityType = "W_C";

        private Guid? id;
        private Guid? workflowInstanceId;
        private Guid? activityInstanceId;
        private string message;
        private string creator;
        private DateTime? createdTime;
        
        /// <summary>
        /// ���� Id.
        /// </summary>
        public Guid? Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// ����ʵ��Id.
        /// </summary>
        public Guid? WorkflowInstanceId
        {
            get { return workflowInstanceId; }
            set { workflowInstanceId = value; }
        }

        /// <summary>
        /// �ʵ��Id.
        /// </summary>
        public Guid? ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// ������Ϣ.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// ������.
        /// </summary>
        public string Creator
        {
            get { return creator; }
            set { creator = value; }
        }

        /// <summary>
        /// ����ʱ��.
        /// </summary>
        public DateTime? CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// ת��Ϊ�ַ�����ʽ.
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
