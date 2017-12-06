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
    public class ResourceService : IResourceService
    {
        #region IResourceService 成员

        public void InsertResource(ResourceInfo item)
        {
            IBatisMapper.Insert("bw_Resources_Insert", item);
        }

        public int UpdateResource(ResourceInfo item)
        {
            return IBatisMapper.Update("bw_Resources_Update", item);
        }

        public int UpdateResourceEnabled(string resourceId, bool enabled)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("ResourceId", resourceId);
            parameters.Add("Enabled", enabled);

            return IBatisMapper.Update("bw_Resources_Update_Enabled", parameters);
        }

        public int UpdateResourceVisible(string resourceId, bool isVisible)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("ResourceId", resourceId);
            parameters.Add("Visible", isVisible);

            return IBatisMapper.Update("bw_Resources_Update_Visible", parameters);
        }

        public bool ResourceIsExists(string resourceName, string resourceAlias)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("Name", resourceName);
            parameters.Add("Alias", resourceAlias);

            int count = IBatisMapper.Mapper.QueryForObject<int>("bw_Resources_Select_IsExists", parameters);
            return (count >= 1);
        }

        public int DeleteByResourceId(string resourceId)
        {
            return IBatisMapper.Mapper.Delete("bw_Resources_Delete", resourceId);
        }

        public ResourceInfo GetResourceById(string resourceId)
        {
            return IBatisMapper.Load<ResourceInfo>("bw_Resources_Select", resourceId);
        }

        public int GetResourceCountByParentId(string parentId)
        {
            return IBatisMapper.Mapper.QueryForObject<int>("bw_Resources_Select_Count_ByParentId", parentId);
        }

        public IDictionary<string, string> GetResourcesByUserId(Guid userId)
        {
            IList<ResourceInfo> resources = IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByUserId", userId);
            return ToDictionary(resources);
        }

        public IDictionary<string, string> GetResourcesByUserName(string userName, string resourcePrefix)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("UserName", userName);
            parameters.Add("ResourcePrefix", resourcePrefix);
            IList<ResourceInfo> resources = IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByUserName", parameters);
            return ToDictionary(resources);
        }

        public IList<ResourceInfo> GetResourcesByType(string resourceType)
        {
            return IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByType", resourceType);
        }

        public IList<ResourceInfo> GetResourcesByParentId(string parentId)
        {
            return IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByParentId", parentId);
        }

        public IList<ResourceInfo> GetResourcesByRoleId(Guid roleId)
        {
            return IBatisMapper.Select<ResourceInfo>("bw_Resources_Select_ByRoleId", roleId);
        }

        public DataTable GetResourcesByVisible()
        {
            string sql = @"SELECT ResourceId, ParentId, [Type], [Name], Alias, Enabled, Visible 
            FROM bw_Resources WHERE ([Enabled] = 1) AND (Visible = 1) 
            ORDER BY ResourceId ASC";
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        #endregion

        #region methods

        /// <summary>
        /// 将资源列表转换为字典类型.
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        protected static IDictionary<string, string> ToDictionary(IList<ResourceInfo> resources)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (resources == null || resources.Count == 0)
                return dict;

            foreach (ResourceInfo item in resources)
            {
                string resourceId = item.ResourceId.Trim();
                if (!dict.ContainsKey(resourceId))
                {
                    dict.Add(resourceId, resourceId);
                }
            }
            return dict;
        }

        #endregion
    }
}
