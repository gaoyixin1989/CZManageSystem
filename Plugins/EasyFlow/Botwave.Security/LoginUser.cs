using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Security.Domain;

namespace Botwave.Security
{
    /// <summary>
    /// 登录用户.
    /// </summary>
    [Serializable]
    public class LoginUser
    {
        #region Getter / Setter

        private Guid userId = Guid.Empty;
        private string userName = string.Empty;
        private string realName = string.Empty;
        private string dpId = string.Empty;
        private string tel = string.Empty;
        private string mobile = string.Empty;
        private string email = string.Empty;
        private IDictionary<string, string> resources;
        private IDictionary<string, object> properties;

        /// <summary>
        /// 用户 ID.
        /// </summary>
        public Guid UserId
        {
            get { return userId; }
            set { userId = value; }
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
        /// 用户真实姓名.
        /// </summary>
        public string RealName
        {
            get { return realName; }
            set { realName = value; }
        }

        /// <summary>
        /// 部门Id.
        /// </summary>
        public string DpId
        {
            get { return dpId; }
            set { dpId = value; }
        }

        /// <summary>
        /// 固定电话.
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
        /// 用户电子邮箱.
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// 允许访问资源键名字典(用户拥有的访问资源字典).
        /// </summary>
        public IDictionary<string, string> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        /// <summary>
        /// 附加信息.
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get { return properties; }
        }
        #endregion

        #region 构造方法

        /// <summary>
        /// 默认构造方法.
        /// </summary>
        public LoginUser()
        {
            this.resources = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            this.properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="user">用户信息对象.</param>
        public LoginUser(UserInfo user)
            : this(user, new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase))
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="user">用户信息对象.</param>
        /// <param name="resources">允许访问资源数组.</param>
        public LoginUser(UserInfo user, IDictionary<string, string> resources)
            : this(user.UserId, user.UserName, user.RealName, resources)
        {
            if (user == null)
                return;
            this.dpId = (user.DpId == null ? string.Empty : user.DpId.Trim());
            this.tel = (user.Tel == null ? string.Empty : user.Tel.Trim());
            this.mobile = (user.Mobile == null ? string.Empty : user.Mobile.Trim());
            this.email = (user.Email == null ? string.Empty : user.Email.Trim());
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="realName"></param>
        /// <param name="resources">允许访问资源数组.</param>
        public LoginUser(Guid userId, string userName, string realName, IDictionary<string, string> resources)
        {
            this.userId = userId;
            this.UserName = userName;
            this.RealName = realName;
            this.resources = resources;
            this.properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion

        /// <summary>
        /// 检查用户对指定资源数组参数中的全部资源是否具有访问权限.
        /// </summary>
        /// <param name="requiredResources"></param>
        /// <returns></returns>
        public virtual bool HasAllResources(params string[] requiredResources)
        {
            if (requiredResources == null || requiredResources.Length == 0)
                return true;
            if (this.Resources.Count == 0)
                return false;

            int count = requiredResources.Length;
            for (int i = 0; i < count; i++)
            {
                if (!this.Resources.ContainsKey(requiredResources[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 检查用户对指定资源数组参数中的任一资源是否具有访问权限.
        /// </summary>
        /// <param name="requiredResources"></param>
        /// <returns></returns>
        public virtual bool HasAnyResources(params string[] requiredResources)
        {
            if (requiredResources == null || requiredResources.Length == 0)
                return true;
            if (this.Resources.Count == 0)
                return false;

            int count = requiredResources.Length;
            for (int i = 0; i < count; i++)
            {
                if (this.Resources.ContainsKey(requiredResources[i]))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取指定属性值.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public virtual object GetProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !Properties.ContainsKey(propertyName))
                return string.Empty;
            return this.Properties[propertyName];
        }
    }
}
