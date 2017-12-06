using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;
using Botwave.Security.Service;

namespace Botwave.Security.IBatisNet
{
    public class RoleService : IRoleService
    {
        #region IRoleService 成员

        public void InsertRole(RoleInfo item)
        {
            IBatisMapper.Insert("bw_Roles_Insert", item);
        }

        public int UpdateRole(RoleInfo item)
        {
            return IBatisMapper.Update("bw_Roles_Update", item);
        }

        public int DeleteById(Guid roleId)
        {
            return IBatisMapper.Delete("bw_Roles_Delete", roleId);
        }

        public int GetRoleCountByParentId(Guid parentId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bw_Roles_Scale_ByParentId", parentId);
        }

        public RoleInfo GetRoleById(Guid roleId)
        {
            return IBatisMapper.Load<RoleInfo>("bw_Roles_Select", roleId);
        }

        public RoleInfo GetRoleByName(string roleName)
        {
            return IBatisMapper.Load<RoleInfo>("bw_Roles_Select_ByRoleName", roleName);
        }

        public bool RecycleRoles(Guid roleId, IList<Guid> userIds)
        {
            if (userIds == null || userIds.Count == 0)
                return false;

            // 删除关系
            string userIdWhere = string.Empty;
            foreach (Guid userId in userIds)
            {
                userIdWhere += string.Format(",'{0}'", userId);
            }
            userIdWhere = userIdWhere.Remove(0, 1);
            string sql = string.Format("DELETE FROM bw_UsersInRoles WHERE (RoleId = '{0}') AND (UserId IN ({1}))", roleId, userIdWhere);
            if (IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql) > 0)
                return true;
            return false;
        }

        public IList<RoleInfo> GetRolesByParentId(Guid parentId)
        {
            return IBatisMapper.Select<RoleInfo>("bw_Roles_Select_ByParentId", parentId);
        }

        public IList<RoleInfo> GetRolesByUserId(Guid userId)
        {
            return IBatisMapper.Select<RoleInfo>("bw_Roles_Select_ByUserId", userId);
        }

        public IList<RoleInfo> GetRolesByEnabled()
        {
            return IBatisMapper.Select<RoleInfo>("bw_Roles_Select_Enabled");
        }

        public DataTable GetRolesTableByEnabeld()
        {
            string sql = @"SELECT RoleId, ParentId, IsInheritable, RoleName, Comment, BeginTime, EndTime, CreatedTime, LastModTime, Creator, LastModifier, SortOrder  
                        FROM bw_Roles WHERE BeginTime <= getdate() AND EndTime >= getdate() ORDER BY RoleName";
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public int InsertUserRoles(Guid userId, IList<Guid> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                // 移除指定用户所分配的全被角色

                IBatisMapper.Delete("bw_UsersInRoles_Delete_ByUserId", userId);
                return 0;
            }

            // 删除不存在于列表中的角色用户关系
            string roleIdWhere = string.Empty;
            foreach (Guid roleId in roleIds)
            {
                roleIdWhere += string.Format(",'{0}'", roleId);
            }
            roleIdWhere = roleIdWhere.Remove(0, 1);
            string deleteSql = string.Format("DELETE FROM bw_UsersInRoles WHERE (UserId = '{0}') AND (RoleId NOT IN({1}))", userId, roleIdWhere);
            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, deleteSql);
           
            // 移除存在的角色用户关系

            string sql = string.Format("SELECT RoleId FROM bw_UsersInRoles WHERE UserId = '{0}'", userId);
            using (DataTable roleTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0])
            {
                for (int i = 0; i < roleIds.Count; i++)
                {
                    if (roleTable.Rows.Count == 0)
                        break;

                    DataRow[] results = roleTable.Select(string.Format("RoleId = '{0}'", roleIds[i]));
                    if (results != null && results.Length > 0)
                    {
                        roleTable.Rows.Remove(results[0]);
                        roleIds.RemoveAt(i);
                        i--;
                    }
                }
            }

            // 插入到数据库中

            foreach (Guid roleId in roleIds)
            {
                InsertUserRole(userId, roleId);
            }

            return roleIds.Count;
        }

        public int InsertUserRoles(IList<Guid> userIds, Guid roleId)
        {
            if (userIds == null || userIds.Count == 0)
            {
                return 0;
            }
            // 获取以及存在的用户

            string userIdWhere = string.Empty;
            foreach (Guid userId in userIds)
            {
                userIdWhere += string.Format(",'{0}'", userId);
            }
            userIdWhere = userIdWhere.Remove(0, 1);
            string sql = string.Format("SELECT [UserId] FROM bw_UsersInRoles WHERE (RoleId = '{0}') AND ([UserId] IN ({1}))", roleId, userIdWhere);
            using (DataTable userTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0])
            {
                if (userTable.Rows.Count > 0)
                {
                    foreach (DataRow row in userTable.Rows)
                    {
                        Guid item = new Guid(row["UserId"].ToString());
                        userIds.Remove(item);
                    }
                }
            }

            foreach (Guid userId in userIds)
            {
                InsertUserRole(userId, roleId);
            }
            
            return userIds.Count;
        }

        public int InsertRoleResources(Guid roleId, IList<string> resourceIds)
        {
            if (resourceIds == null || resourceIds.Count == 0)
            {
                // 移除指定角色所分配的全被权限资源

                IBatisMapper.Delete("bw_RolesInResources_Delete_ByRoleId", roleId);
                return 0;
            }
            // 删除不存在的角色和权限资源关系

            string resourceIdWhere = string.Empty;
            foreach (string resourceId in resourceIds)
            {
                resourceIdWhere += string.Format(",'{0}'", resourceId);
            }
            resourceIdWhere = resourceIdWhere.Remove(0, 1);
            string deleteSql = string.Format("DELETE FROM bw_RolesInResources WHERE (RoleId = '{0}') AND (ResourceId NOT IN({1}))", roleId, resourceIdWhere);
            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, deleteSql);

            // 移除存在的权限资源关系

            string sql = string.Format("SELECT ResourceId FROM bw_RolesInResources WHERE RoleId = '{0}' AND (ResourceId IN ({1}))", roleId, resourceIdWhere);
            using (DataTable resourceTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0])
            {
                for (int i = 0; i < resourceIds.Count; i++)
                {
                    if (resourceTable.Rows.Count == 0)
                        break;

                    DataRow[] results = resourceTable.Select(string.Format("ResourceId = '{0}'", resourceIds[i]));
                    if (results != null && results.Length > 0)
                    {
                        resourceTable.Rows.Remove(results[0]);
                        resourceIds.RemoveAt(i);
                        i--;
                    }
                }
            }

            // 插入到数据库
            foreach (string resourceId in resourceIds)
            {
                InsertRoleResource(roleId, resourceId);
            }

            return resourceIds.Count;
        }

        #endregion

        #region private methods

        /// <summary>
        /// 插入用户与角色关系.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        private static void InsertUserRole(Guid userId, Guid roleId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("UserId", userId);
            parameters.Add("RoleId", roleId);
            IBatisMapper.Insert("bw_UsersInRoles_Insert", parameters);
        }

        /// <summary>
        /// 插入角色与权限资源关系.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="resourceId"></param>
        private static void InsertRoleResource(Guid roleId, string resourceId)
        {
            Hashtable parameters = new Hashtable(2);
            parameters.Add("RoleId", roleId);
            parameters.Add("ResourceId", resourceId);
            IBatisMapper.Insert("bw_RolesInResources_Insert", parameters);
        }

        #endregion
    }
}
