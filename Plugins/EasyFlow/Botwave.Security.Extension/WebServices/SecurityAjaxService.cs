using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Botwave.Security;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.Extension.WebServices
{
    /// <summary>
    /// 用户安全 AJAX Web 服务.
    /// </summary>
    [WebService(Namespace = "Botwave.Security.Extension.WebServices.SecurityAjaxService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    public class SecurityAjaxService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SecurityAjaxService));

        private IUserService userService;
        private IDepartmentService departmentService;

        /// <summary>
        /// 用户服务.
        /// </summary>
        public IUserService UserService
        {
            get { return userService; }
            set { userService = value; }
        }

        /// <summary>
        /// 部门服务.
        /// </summary>
        public IDepartmentService DepartmentService
        {
            get { return departmentService; }
            set { departmentService = value; }
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        public SecurityAjaxService()
        {
            Spring.Context.IApplicationContext applicationContext = Spring.Context.Support.ContextRegistry.GetContext();
            this.userService = applicationContext["userService"] as IUserService;
            this.departmentService = applicationContext["departmentService"] as IDepartmentService;
        }

        #region 自动完成列表

        /// <summary>
        /// 获取指定匹配用户名或者真实姓名的用户自动完成列表.
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] GetUserCompletionList(string prefixText, int count)
        {
            IList<UserInfo> users = userService.GetUsersLikeName(prefixText);
            int usersCount = users.Count;
            string[] names = new string[usersCount];
            for (int i = 0; i < usersCount; i++)
            {
                UserInfo item = users[i];
                if (string.IsNullOrEmpty(item.DpFullName))
                    names[i] = string.Format("{0}<\"{1}\">", item.RealName, item.UserName);
                else
                    names[i] = string.Format("{0}<\"{1}\">\t{2}", item.RealName, item.UserName, item.DpFullName);
            }
            return names;
        }

        /// <summary>
        /// 获取指定匹配部门名称的部门完成列表.
        /// </summary>
        /// <param name="prefixText"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public string[] GetCompletionDepartments(string prefixText, int count)
        {
            IList<Department> departments = departmentService.GetDepartmentsLikeName(prefixText);
            int resultCount = departments.Count;
            string[] fullNames = new string[resultCount];
            for (int i = 0; i < resultCount; i++)
            {
                fullNames[i] = departments[i].DpFullName;
            }
            return fullNames;
        }

        #endregion

        /// <summary>
        /// 获取用户信息.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [WebMethod]
        public UserInfo GetUserInfo(string userName)
        {
            return userService.GetUserByUserName(userName);
        }
    }
}
