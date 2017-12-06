using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Domain
{
    /// <summary>
    /// 会签.
    /// </summary>
    public class Countersigned
    {
        /// <summary>
        /// 构造方法.
        /// </summary>
        public Countersigned() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <param name="message"></param>
        public Countersigned(Guid activityInstanceId, string userName, string message) 
        {
            this.activityInstanceId = activityInstanceId;
            this.userName = userName;
            this.message = message;
            this.createdTime = DateTime.Now;
        }

        private Guid activityInstanceId;
        private string userName;
        private string message;
        private DateTime createdTime;

        /// <summary>
        /// 流程活动（步骤）实例编号.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return activityInstanceId; }
            set { activityInstanceId = value; }
        }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 消息内容.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 创建时间.
        /// </summary>
        public DateTime CreatedTime
        {
            get { return createdTime; }
            set { createdTime = value; }
        }

        /// <summary>
        /// 转化为字符串形式.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder()
                .AppendFormat("activityInstanceId:{0}", activityInstanceId)
                .AppendFormat("userName:{0}", userName)
                .AppendFormat("message:{0}", message).ToString();
        }
    }
}
