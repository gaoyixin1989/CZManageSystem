using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Workflow.Extension.Domain
{
    /// <summary>
    /// 操作人详细信息.
    /// </summary>
    [Serializable]
    public class ActorDetail
    {
        #region gets / sets

        private string userName;
        private string realName;
        private string dpFullName;
        private string email;
        private string tel;
        private string mobile;
        private string employeeId;

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 用户真实姓名.
        /// </summary>
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }

        /// <summary>
        /// 用户所在部门全名.
        /// </summary>
        public string DpFullName
        {
            get { return dpFullName; }
            set { dpFullName = value; }
        }

        /// <summary>
        /// 用户电子邮箱.
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// 用户固定电话.
        /// </summary>
        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }

        /// <summary>
        /// 用户手机号码.
        /// </summary>
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        /// <summary>
        /// 用户工号.
        /// </summary>
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }
        #endregion
    }
}
