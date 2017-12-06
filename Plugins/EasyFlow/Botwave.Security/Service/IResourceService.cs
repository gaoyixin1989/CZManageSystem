using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Security.Domain;

namespace Botwave.Security.Service
{
    /// <summary>
    /// 权限资源服务接口.
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// 新增指定权限资源.
        /// </summary>
        /// <param name="item"></param>
        void InsertResource(ResourceInfo item);

        /// <summary>
        /// 更新指定权限资源.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int UpdateResource(ResourceInfo item);

        /// <summary>
        /// 更新指定权限资源的可用性.
        /// </summary>
        /// <param name="resourceId">权限资源编号.</param>
        /// <param name="enabled">是否启用.</param>
        /// <returns></returns>
        int UpdateResourceEnabled(string resourceId, bool enabled);

        /// <summary>
        /// 更新指定权限资源的在界面上的可见性.
        /// </summary>
        /// <param name="resourceId">权限资源编号.</param>
        /// <param name="isVisible">是否在界面可见.</param>
        /// <returns></returns>
        int UpdateResourceVisible(string resourceId, bool isVisible);

        /// <summary>
        /// 检查指定资源是否已经存在.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="resourceAlias"></param>
        /// <returns></returns>
        bool ResourceIsExists(string resourceName, string resourceAlias);

        /// <summary>
        /// 删除指定权限资源编号的权限资源信息.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        int DeleteByResourceId(string resourceId);
        
        /// <summary>
        /// 获取指定资源编号的权限资源信息.
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        ResourceInfo GetResourceById(string resourceId);

        /// <summary>
        /// 获取指定父权限资源其子资源的数目.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        int GetResourceCountByParentId(string parentId);

        /// <summary>
        /// 获取指定用户的用户资源字典.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IDictionary<string, string> GetResourcesByUserId(Guid userId);

        /// <summary>
        /// 获取指定用户的用户资源字典.
        /// </summary>
        /// <param name="userName">用户名.</param>
        /// <param name="resourcePrefix">资源编号的前缀.</param>
        /// <returns></returns>
        IDictionary<string, string> GetResourcesByUserName(string userName, string resourcePrefix);

        /// <summary>
        /// 获取指定资源类型的全部权限资源列表.
        /// </summary>
        /// <param name="resourceType"></param>
        /// <returns></returns>
        IList<ResourceInfo> GetResourcesByType(string resourceType);

        /// <summary>
        /// 获取指定父资源编号的子资源列表.
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IList<ResourceInfo> GetResourcesByParentId(string parentId);

        /// <summary>
        /// 获取指定角色的资源列表.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IList<ResourceInfo> GetResourcesByRoleId(Guid roleId);

        /// <summary>
        /// 获取全部可用并可见的权限资源数据表.
        /// </summary>
        /// <returns></returns>
        DataTable GetResourcesByVisible();
    }
}
