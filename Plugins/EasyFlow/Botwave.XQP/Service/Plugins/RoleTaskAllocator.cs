using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Allocator;
using Botwave.Security.Domain;

namespace Botwave.XQP.Service.Plugins
{
    /// <summary>
    /// 按角色分派任务.
    /// </summary>
    public class RoleTaskAllocator : ITaskAllocator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(RoleTaskAllocator));

        private IWorkflowRoleService workflowRoleService;

        public IWorkflowRoleService WorkflowRoleService
        {
            set { workflowRoleService = value; }
        }

        private const string SQL_SELECT_ACTORS = @"";

        private string spName = "bwwf_ext_GetAuditUsersByOrg";

        public string SpName
        {
            set { spName = value; }
        }

        #region ITaskAllocator Members
        public IList<string> GetTargetUsers(TaskVariable variable)
        {
            try
            {
                if (null == variable || null == variable.Expression)
                {
                    return null;
                }

                IList<string> list = new List<string>();
                foreach (string roleId in variable.Expression.ToString().Split(',', '，'))
                {
                    IList<UserInfo> userList = workflowRoleService.GetUsersByRoleId(new Guid(roleId));
                    foreach (UserInfo user in userList)
                    {
                        if (!list.Contains(user.UserName))
                            list.Add(user.UserName);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return null;
            }
        }
        
        #endregion
    }
}
