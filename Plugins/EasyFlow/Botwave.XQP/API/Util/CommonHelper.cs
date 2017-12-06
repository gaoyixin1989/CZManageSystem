using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using Botwave.Workflow.Domain;
using Botwave.XQP.Util;
using System.Collections;
using Botwave.Extension.IBatisNet;
using Botwave.Security.Domain;

namespace Botwave.XQP.API.Util
{
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public class CommonHelper
    {
        public static DataTable GetUserInfo(string userName)
        {
            return APIServiceSQLHelper.QueryForDataSet("API_Select_User", userName);
        }

        /// <summary>
        /// 获取处理用户
        /// </summary>
        /// <param name="userName">用户数组</param>
        /// <returns></returns>
        public static string GetRealNames(string[] userName)
        {
            string ret_val = string.Empty;

            for (int i = 0; i < userName.Length; i++)
            {
                DataTable dt = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_UserName", userName[i]);
                ret_val += "," + dt.Rows[0]["Names"].ToString();
            }

            if (ret_val.Length > 0)
            {
                ret_val = ret_val.Remove(0, 1);
            }
            return ret_val;
        }
        /// <summary>
        /// 获取处理用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public static string GetRealName(string userName)
        {
            string ret_val = string.Empty;

            DataTable dt = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_RealName", userName);
            if (dt != null && dt.Rows.Count > 0)
                ret_val = dt.Rows[0]["Names"].ToString();

            return ret_val;
        }

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfix"></param>
        /// <returns></returns>
        public static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> userResources, string resourcePostfix)
        {
            return GetAllowedWorkflows(workflows, ResourceHelper.GetResouceByParentId(ResourceHelper.Workflow_ResourceId), userResources, resourcePostfix);
        }

        /// <summary>
        /// 获取允许的流程列表.
        /// </summary>
        /// <param name="workflows"></param>
        /// <param name="workflowResourceDict"></param>
        /// <param name="userResources"></param>
        /// <param name="resourcePostfix"></param>
        /// <returns></returns>
        public static IList<WorkflowDefinition> GetAllowedWorkflows(IList<WorkflowDefinition> workflows, IDictionary<string, string> workflowResourceDict, IDictionary<string, string> userResources, string resourcePostfix)
        {
            if (workflows == null)
                return workflows;
            for (int i = 0; i < workflows.Count; i++)
            {
                string workflowName = workflows[i].WorkflowName.ToLower();
                if (workflowResourceDict.ContainsKey(workflowName))
                {
                    string resourceId = workflowResourceDict[workflowName] + resourcePostfix;
                    if (!userResources.ContainsKey(resourceId))
                    {
                        workflows.RemoveAt(i);
                        i--;
                    }
                }
            }
            return workflows;
        }

        /// <summary>
        /// 获取该用户的权限
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetResources(string userName)
        {
            IDictionary<string, string> userResources = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(userName))
            {
                IList<ResourceInfo> resources = IBatisMapper.Select<ResourceInfo>("API_Resources_Select_ByUserName", userName);
                foreach (ResourceInfo item in resources)
                {
                    string resourceId = item.ResourceId;
                    if (!userResources.ContainsKey(resourceId))
                        userResources.Add(resourceId, resourceId);
                }
                //userResources = roleService.GetRolesByUserId(user.UserId);
            }
            return userResources;
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public static DataTable GetAttachemnts(string workflowInstanceId)
        {
            Hashtable ha = new Hashtable();
            ha.Add("WorkflowInstanceId", workflowInstanceId);
            ha.Add("EntityType", Comment.EntityType);
            DataTable dt = APIServiceSQLHelper.QueryForDataSet("API_Select_Workflow_Comment_Attachment", ha);
            return dt;
        }

        #region 数据操作

        /// <summary>
        /// 插入附件信息
        /// </summary>
        /// <param name="att"></param>
        /// <param name="workflowInstanceId"></param>
        /// <param name="type"></param>
        public static void SaveAttachmentInfo(Botwave.XQP.API.Entity.Attachment att, Guid workflowInstanceId, string type)
        {
            Guid newAid = Guid.NewGuid();
            string EntityType = "W_A";
            if (!string.IsNullOrEmpty(type))
                EntityType = type;
            Hashtable ha = new Hashtable();
            ha.Add("Id", newAid);
            ha.Add("Creator", att.Creator);
            ha.Add("CreatedTime", att.CreatedTime);
            ha.Add("Title", att.Name);
            ha.Add("FileName", att.Url);
            ha.Add("MimeType", System.IO.Path.GetExtension(att.Url));
            IBatisMapper.Insert("API_Insert_Workflow_Comment_Attachment", ha);

            Hashtable ha1 = new Hashtable();
            ha1.Add("AttachmentId", newAid);
            ha1.Add("EntityId", workflowInstanceId);
            ha1.Add("EntityType", EntityType);
            IBatisMapper.Insert("API_Insert_Workflow_Comment_AttachmentEntityType", ha1);

        }

        #endregion
    }
}
