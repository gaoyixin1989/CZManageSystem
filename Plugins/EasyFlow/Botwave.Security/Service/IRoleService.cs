using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Security.Domain;

namespace Botwave.Security.Service
{
    /// <summary>
    /// 角色服务接口.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// 新增角色.
        /// </summary>
        /// <param name="item"></param>
        void InsertRole(RoleInfo item);

        /// <summary>
        /// 更新角色.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int UpdateRole(RoleInfo item);

        /// <summary>
        /// 删除指定角色编号的角色.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int DeleteById(Guid roleId);

        /// <summary>
        /// 获取指定父角色编号的子角色列表.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        int GetRoleCountByParentId(Guid parentId);

        /// <summary>
        /// 获取指定角色编号的角色信息.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleInfo GetRoleById(Guid roleId);

        /// <summary>
        /// 获取知道角色名称的角色.
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        RoleInfo GetRoleByName(string roleName);

        /// <summary>
        /// 获取可用的角色列表.
        /// </summary>
        /// <returns></returns>
        IList<RoleInfo> GetRolesByEnabled();

        /// <summary>
        /// 获取可用的角色数据表.
        /// </summary>
        /// <returns></returns>
        DataTable GetRolesTableByEnabeld();

        /// <summary>
        /// 获取指定父角色编号的子角色列表.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IList<RoleInfo> GetRolesByParentId(Guid parentId);

        /// <summary>
        /// 获取指定用户编号的角色列表.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<RoleInfo> GetRolesByUserId(Guid userId);

        /// <summary>
        /// 回收指定用户列表的指定角色.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
        bool RecycleRoles(Guid roleId, IList<Guid > userIds);

        /// <summary>
        /// 为指定用户分配指定角色列表.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        int InsertUserRoles(Guid userId, IList<Guid> roleIds);

        /// <summary>
        /// 为指定用户列表分配指定角色.
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        int InsertUserRoles(IList<Guid> userIds, Guid roleId);

        /// <summary>
        /// 为指定角色分配指定权限资源列表.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="resourceIds"></param>
        /// <returns></returns>
        int InsertRoleResources(Guid roleId, IList<string> resourceIds);
    }
}
