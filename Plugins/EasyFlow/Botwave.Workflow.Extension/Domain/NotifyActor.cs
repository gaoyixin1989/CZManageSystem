using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 流程处理提醒人.
    /// </summary>
    [Serializable]
    public class NotifyActor
    {
        #region properties

        private Guid _activityInstanceId;
        private string _activityName;
        private string _userName;
        private string _realName;
        private string _email;
        private string _mobile;
        private string _employeeId;

        /// <summary>
        /// 流程步骤实例编号.
        /// </summary>
        public Guid ActivityInstanceId
        {
            get { return _activityInstanceId; }
            set { _activityInstanceId = value; }
        }

        /// <summary>
        /// 流程步骤名称.
        /// </summary>
        public string ActivityName
        {
            get { return _activityName; }
            set { _activityName = value; }
        }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        /// <summary>
        /// 用户真实姓名.
        /// </summary>
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }

        /// <summary>
        /// 用户电子邮箱.
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        /// <summary>
        /// 用户手机号码.
        /// </summary>
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        /// <summary>
        /// 用户工号.
        /// </summary>
        public string EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }
        #endregion

    }
}
