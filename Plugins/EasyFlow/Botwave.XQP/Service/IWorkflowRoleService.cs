using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Security.Domain;

namespace Botwave.XQP.Service
{
    public interface IWorkflowRoleService
    {
        /// <summary>
        /// 根据资源ID获取角色信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        IList<RoleInfo> GetRoleInfoByResourceId(string resourceId);

        /// <summary>
        /// 根据角色ID获取用户信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        IList<UserInfo> GetUsersByRoleId(Guid roleID);

        /// <summary>
        /// 删除角色，包括角色的关联
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        string DeleteRoleByRoleId(Guid roleId);
        /// <summary>
        /// 根据资源名称获取资源信息
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        ResourceInfo GetResourceInfoByName(string resourceName);

        /// <summary>
        /// 根据资源ID获取用户信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        IList<UserInfo> GetUsersByResourceId(string resourceId);

        /// <summary>
        /// 回收指定角色ID的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int RecyleUsersByRoleId(Guid roleId);

        /// <summary>
        /// 分配流程管理员角色
        /// </summary>
        /// <param name="resourceid"></param>
        /// <param name="userIds"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        string InsertWorkflowManager(string resourceid, IList<Guid> userIds, string creator);
    }
}
