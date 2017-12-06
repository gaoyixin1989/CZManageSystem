using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.Commons;
using Botwave.XQP.Commons;
using System.Data.SqlClient;

namespace Botwave.XQP.Service.Plugins
{
    public class PostDeployHandler : IPostDeployHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostDeployHandler));
        private IRoleService roleService;
        private IWorkflowAttachmentService workflowAttachmentService;
        

        public IRoleService RoleService
        {
            set { roleService = value; }
        }

        public IWorkflowAttachmentService WorkflowAttachmentService
        {
            set { workflowAttachmentService = value; }
        }

        #region IPostDeployHandler Members

        private IPostDeployHandler next;

        public IPostDeployHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        public void Execute(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow, ICollection<DeployActivityDefinition> newActivities)
        {
            log.Debug("xqp2 deploy handler..");

            this.UpdateWorkflowProfile(newWorkflow);
            this.UpdateWorkflowTemplate(oldWorkflow, newWorkflow);
            this.SetWorkflowGroup(newWorkflow);
            
            // this.SetRoles(newWorkflow, newActivities);  // 自动创建/更新角色.
            this.OnUpdateSystemAccess(oldWorkflow,newWorkflow);
            this.UpdateWorkflowResourceVisible(newWorkflow.WorkflowId, true);
            this.UpdateNotifyTable(newWorkflow.WorkflowName);
            this.OnUpdateReminderTimeSpans(oldWorkflow, newWorkflow);
            this.OnImportRules(oldWorkflow, newWorkflow);
            XQPHelper.PushWorkflowList();
        }

        #endregion
        
        /// <summary>
        /// 新增流程设置.
        /// </summary>
        /// <param name="workflow"></param>
        private void UpdateWorkflowProfile(WorkflowDefinition workflow)
        {
            string workflowName = workflow.WorkflowName;
            WorkflowProfile item = WorkflowProfile.LoadByWorkflowName(workflowName);
            if (item == null)
            {
                item = WorkflowProfile.Default;
                item.WorkflowName = workflowName;
                item.Insert();
            }
        }

        /// <summary>
        /// 更新流程模板
        /// </summary>
        /// <param name="oldWorkflow"></param>
        /// <param name="newWorkflow"></param>
        private void UpdateWorkflowTemplate(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {
            if (oldWorkflow == null || newWorkflow == null)
                return;

            Guid oldId = oldWorkflow.WorkflowId;
            Guid newId = newWorkflow.WorkflowId;
            workflowAttachmentService.UpdateWorkflowAttachmentEntities(oldId, newId);
        }

        /// <summary>
        /// 设置流程默认分组
        /// </summary>
        /// <param name="workflow"></param>
        private void SetWorkflowGroup(WorkflowDefinition workflow)
        {
            string workflowName = workflow.WorkflowName;
            if (!WorkflowInMenuGroup.IsExists(workflowName))
            {
                WorkflowInMenuGroup item = new WorkflowInMenuGroup();
                item.WorkflowName = workflowName;
                item.MenuGroupId = 11;
                item.Create();
            }
        }

        /// <summary>
        /// 设置流程默认角色
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="activities"></param>
        private void SetRoles(WorkflowDefinition workflow, ICollection<DeployActivityDefinition> activities)
        {
            if (workflow.Version != 1)
                return;

            Guid newRoleGroupId = Guid.NewGuid();
            RoleInfo item = new RoleInfo();
            item.RoleId = newRoleGroupId;
            item.ParentId = Guid.Empty;
            item.RoleName = workflow.WorkflowName;
            item.BeginTime = DateTime.Now;
            item.EndTime = DateTime.Now.AddYears(10);
            item.CreatedTime = DateTime.Now;
            item.LastModTime = DateTime.Now;
            item.Creator = workflow.Creator;
            roleService.InsertRole(item);

            foreach (DeployActivityDefinition activityDefinition in activities)
            {
                string roleName = workflow.WorkflowName + "-" + activityDefinition.ActivityName;
                RoleInfo childItem = new RoleInfo();
                childItem.RoleId = Guid.NewGuid();
                childItem.ParentId = newRoleGroupId;
                childItem.RoleName = roleName;
                childItem.BeginTime = DateTime.Now;
                childItem.EndTime = DateTime.Now.AddYears(10);
                childItem.CreatedTime = DateTime.Now;
                childItem.LastModTime = DateTime.Now;
                childItem.Creator = workflow.Creator;
                roleService.InsertRole(childItem);

                string sql = String.Format("SELECT ResourceId FROM bw_Resources WHERE Name = '{0}'", roleName);
                object obj = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
                if (null != obj)
                {
                    IList<string> resourceId = new List<string>();
                    resourceId.Add(obj.ToString());
                    roleService.InsertRoleResources(childItem.RoleId, resourceId);
                }
            }
        }

        private string connectionString = IBatisDbHelper.ConnectionString;
        private string notifyTableName = "xqp_WorkflowNotify";
        private DataTable notifyTable = null;

        /// <summary>
        /// 更新用户提醒类型(默认都设置为只发送短信通知提醒) [广电移动需求].
        /// </summary>
        /// <param name="workflowName"></param>
        private void UpdateNotifyTable(string workflowName)
        {
            if (string.IsNullOrEmpty(workflowName))
                return;

            string sql = @"SELECT DISTINCT UserID, '{0}' AS WorkflowName, 3 AS NotifyType FROM bw_Users u
	WHERE UserID NOT IN(SELECT UserID FROM xqp_WorkflowNotify WHERE WorkflowName = '{0}') ";

            notifyTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(sql, workflowName.Trim())).Tables[0];

            if (notifyTable == null || notifyTable.Rows.Count == 0)
                return;

            try
            {
                // 使用另一个线程启动更新数据库.
                Thread thread = new Thread(new ThreadStart(OnUpdateNotifyTable));
                thread.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private void OnUpdateNotifyTable()
        {
            if (notifyTable == null || notifyTable.Rows.Count == 0)
                return;

            NameValueCollection mappings = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            mappings.Add("UserId", "UserId");
            mappings.Add("WorkflowName", "WorkflowName");
            mappings.Add("NotifyType", "NotifyType");

            Botwave.Workflow.Extension.Util.SqlBulkCopyHelper.ExecuteBulkCopy(connectionString, notifyTableName, mappings, notifyTable);
        }

        /// <summary>
        /// 更新指定流程的权限资源可见性.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public int UpdateWorkflowResourceVisible(Guid workflowId, bool isVisible)
        {
            Hashtable paramters = new Hashtable();
            paramters.Add("WorkflowId", workflowId);
            paramters.Add("Visible", isVisible);
            return IBatisMapper.Update("bwwf_Resources_Update_Visible_ByWorkflowId", paramters);

        }

        /// <summary>
        /// 自动同步数据推送接口设置
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="newId"></param>
        private void OnUpdateSystemAccess(WorkflowDefinition oldWorkflow,WorkflowDefinition newWorkflow)
        {
            try
            {
                if (oldWorkflow == null || newWorkflow == null)
                    return;

                Guid oldId = oldWorkflow.WorkflowId;
                Guid newId = newWorkflow.WorkflowId;
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text,string.Format(@"insert into xqp_SystemAccess (workflowid,systemid,url,createdtime,[Type])
select '{0}',systemid,url,getdate(),[Type] from xqp_SystemAccess where workflowid='{1}'", newId,oldId));
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 自动同步消息推送时间段设置
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="newId"></param>
        private void OnUpdateReminderTimeSpans(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {
            try
            {
                if (oldWorkflow == null || newWorkflow == null)
                    return;

                Guid oldId = oldWorkflow.WorkflowId;
                Guid newId = newWorkflow.WorkflowId;
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"insert into cz_Reminder_TimeSpans (EntityId,RemindType,BeginHours,BeginMinutes,[EndHours],EndMinutes)
select '{0}',RemindType,BeginHours,BeginMinutes,[EndHours],EndMinutes from cz_Reminder_TimeSpans where EntityId='{1}'", newId, oldId));
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, string.Format(@"insert into cz_Reminder_TimeSpans (EntityId,RemindType,BeginHours,BeginMinutes,[EndHours],EndMinutes)
select newba.ActivityId,RemindType,BeginHours,BeginMinutes,[EndHours],EndMinutes from cz_Reminder_TimeSpans rt 
inner join bwwf_activities ba on rt.EntityId=ba.activityid
inner join bwwf_activities newba on ba.activityname=newba.activityname
 where ba.WorkflowId='{1}' and newba.WorkflowId='{0}'", newId, oldId));
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 自动同步规则库设置
        /// </summary>
        /// <param name="oldId"></param>
        /// <param name="newId"></param>
        private void OnImportRules(WorkflowDefinition oldWorkflow, WorkflowDefinition newWorkflow)
        {
            try
            {
                if (oldWorkflow == null || newWorkflow == null)
                    return;

                Guid oldId = oldWorkflow.WorkflowId;
                Guid newId = newWorkflow.WorkflowId;
                SqlParameter[] param = { new SqlParameter("@oldWorkflowId", oldId), 
                                         new SqlParameter("@newWorkflowId", newId) };
                IBatisDbHelper.ExecuteNonQuery(CommandType.StoredProcedure, "[dbo].[bwwf_sz_importrules]",param);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
    }
}
