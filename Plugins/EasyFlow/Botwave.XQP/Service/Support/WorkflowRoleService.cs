using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Botwave.Security.Domain;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Service;

namespace Botwave.XQP.Service.Support
{
    public class WorkflowRoleService : IWorkflowRoleService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowRoleService));

        /// <summary>
        /// 流程运维功能资源集合
        /// </summary>
        public static readonly string[] WORKFLOW_MAINTENANCE_RESOURCES= new string[]{"E001","E004","M001","M002","M004","M005"};

        private IResourceService resourceService;
        private IRoleService roleService;

        public IResourceService ResourceService
        {
            set { resourceService = value; }
        }

        public IRoleService RoleService
        {
            set { roleService = value; }
        }
        /// <summary>
        /// 根据资源ID获取角色信息
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public IList<RoleInfo> GetRoleInfoByResourceId(string resourceId)
        {
            return IBatisMapper.Select<RoleInfo>("cz_RoleInfo_Select_By_ResourceId", resourceId);
        }

        /// <summary>
        /// 根据角色ID获取用户信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IList<UserInfo> GetUsersByRoleId(Guid roleID)
        {
            return IBatisMapper.Select<UserInfo>("cz_UserInfo_Select_By_RoleId",roleID);
        }

        public int RecyleUsersByRoleId(Guid roleId)
        {
            return IBatisMapper.Delete("bw_UsersInRoles_Delete_ByRoleId",roleId);
        }

        public string DeleteRoleByRoleId(Guid roleId)
        {
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                IBatisMapper.Delete("bw_UsersInRoles_Delete_ByRoleId", roleId);//删除用户和角色的关联
                IBatisMapper.Delete("bw_RolesInResources_Delete_ByRoleId", roleId);//删除资源和角色的关联
                IBatisMapper.Delete("bw_Roles_Delete", roleId);//删除角色
                IBatisMapper.Mapper.CommitTransaction();
                return "删除角色成功！";
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                log.Error(ex.ToString());
                return "删除角色操作失败：" + ex.Message;
            }
        }
        /// <summary>
        /// 根据资源名称获取资源信息
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public ResourceInfo GetResourceInfoByName(string resourceName)
        {
            return IBatisMapper.Load<ResourceInfo>("cz_ResourceInfo_Select_ByName", resourceName);
        }

        /// <summary>
        /// 根据资源ID获取用户信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public IList<UserInfo> GetUsersByResourceId(string resourceId)
        {
            return IBatisMapper.Select<UserInfo>("cz_UserInfo_Select_By_ResourceId", resourceId);
        }

        public string InsertWorkflowManager(string resourceid, IList<Guid> userIds,string creator)
        {
            try
            {
                ResourceInfo resource = resourceService.GetResourceById(resourceid);
                //更新流程管理员角色
                IList<RoleInfo> rolelist = GetRoleInfoByResourceId(resourceid);
                bool hasRole = false;
                //IBatisMapper.Mapper.BeginTransaction();
                //roleService.RecycleRoles
                foreach (RoleInfo roleInfo in rolelist)
                {
                    if (roleInfo.RoleName.Equals(resource.Name))
                    {
                        hasRole = true;
                        RecyleUsersByRoleId(roleInfo.RoleId);
                        roleService.InsertUserRoles(userIds, roleInfo.RoleId);
                    }
                }
                if (!hasRole)
                {
                    string roleName = resource.Name;
                    RoleInfo childItem = new RoleInfo();
                    childItem.RoleId = Guid.NewGuid();
                    childItem.ParentId = new Guid("05b2a57d-bd60-4f95-a64e-1f2fde6eb4d2");//默认属于流程角色
                    childItem.RoleName = roleName;
                    childItem.BeginTime = DateTime.Now;
                    childItem.EndTime = DateTime.Now.AddYears(10);
                    childItem.CreatedTime = DateTime.Now;
                    childItem.LastModTime = DateTime.Now;
                    childItem.Creator = creator;
                    roleService.InsertRole(childItem);

                    IList<string> resourceId = new List<string>();
                    resourceId.Add(resourceid);
                    
                    foreach (string maintenance in WORKFLOW_MAINTENANCE_RESOURCES)//添加运维页面权限关联
                    {
                        resourceId.Add(maintenance);
                    }
                    roleService.InsertRoleResources(childItem.RoleId, resourceId);//添加角色关联
                    roleService.InsertUserRoles(userIds, childItem.RoleId);
                }
                //IBatisMapper.Mapper.CommitTransaction();
                return "角色分配成功";
            }
            catch (Exception ex)
            {
                //IBatisMapper.Mapper.RollBackTransaction();
                log.Error(ex.ToString());
                return "分配流程管理员权限操作失败："+ex.Message;
            }
        }
    }
}
