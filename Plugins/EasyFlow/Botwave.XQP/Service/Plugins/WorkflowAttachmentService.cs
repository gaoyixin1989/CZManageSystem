using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.Workflow.Extension.Util;
using Botwave.XQP.Commons;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    public class WorkflowAttachmentService : IWorkflowAttachmentService
    {
        private const string SqlTemplate_GetEntityAttachments = @"SELECT atta.Id, atta.Title, atta.FileName, atta.MimeType, atta.FileSize, atta.Remark, atta.Creator, atta.CreatedTime, u.RealName
                FROM xqp_Attachment atta LEFT OUTER JOIN
                      bw_Users u ON atta.Creator = u.UserName
                WHERE atta.[Id] IN(
                    SELECT AttachmentId FROM xqp_Attachment_Entity 
                    WHERE EntityId = '{0}' AND EntityType = '{1}')";

        private const string SqlTemplate_GetEntityAttachmentsByCreator = @"SELECT atta.Id, atta.Title, atta.FileName, atta.MimeType, atta.FileSize, atta.Remark, atta.Creator, atta.CreatedTime, u.RealName
                FROM xqp_Attachment atta 
                       LEFT JOIN bw_Users u ON atta.Creator = u.UserName
                WHERE (atta.Creator = '{2}') AND atta.[Id] IN(
                    SELECT AttachmentId FROM xqp_Attachment_Entity 
                    WHERE EntityId = '{0}' AND entityType = '{1}')";

        private const string SqlTemplate_GetActivityAttachments = @"SELECT atta.*, attaEntity.EntityId, u.RealName 
            FROM (SELECT [Id], Title, [FileName], MimeType, FileSize, Remark, Creator, CreatedTime 
               FROM xqp_Attachment
               WHERE [Id] IN(
                    SELECT AttachmentId FROM xqp_Attachment_Entity WHERE (EntityType = '{0}') AND EntityId IN(
                         SELECT [Id] FROM bwwf_Tracking_Comments WHERE (ActivityInstanceId = '{1}')
                    ))
            ) atta LEFT JOIN xqp_Attachment_Entity attaEntity ON (atta.[Id] = attaEntity.AttachmentId)
              LEFT JOIN bw_Users u ON atta.Creator = u.UserName";

        private const string SqlTemplate_GetWorkflowAttachments = @"SELECT atta.*, attaEntity.EntityId, u.RealName
           FROM (SELECT [Id], Title, [FileName], MimeType, FileSize, Remark, Creator, CreatedTime 
                FROM xqp_Attachment
                WHERE [Id] IN(
                    SELECT AttachmentId FROM xqp_Attachment_Entity WHERE (EntityType = '{0}') 
                        AND EntityId IN(
                         SELECT [Id] FROM bwwf_Tracking_Comments WHERE (WorkflowInstanceId = '{1}')
                        ))
            ) atta 
                LEFT JOIN xqp_Attachment_Entity attaEntity ON (atta.[Id] = attaEntity.AttachmentId)
                LEFT JOIN bw_Users u ON atta.Creator = u.UserName";

        #region IWorkflowFileService 成员

        public int GetAttachmentCount(Guid workflowInstanceId)
        {
            return 0;
        }

        public string CreateAttachment(HttpPostedFile fileInfo, string creator, string fileName)
        {
            if (fileInfo == null)
                throw new ArgumentException("HttpPostedFile 对象为 null.");
            Guid attachmentId = Guid.NewGuid();
            Attachment item = new Attachment(fileInfo, creator, fileName);
            item.Id = attachmentId;
            item.Create(attachmentId);
            return attachmentId.ToString();
        }

        public void CreateAttachmentEntity(string attachmentId, string entityId, string entityType)
        {
            AttachmentEntity.CreateAttachmentEntity(new Guid(attachmentId), new Guid(entityId), entityType);
        }

        public void UpdateWorkflowAttachmentEntities(Guid workflowId, Guid workflowInstanceId)
        {
             IList<Attachment> attachments =AttachmentEntity.Select(workflowId);
             foreach (Attachment entity in attachments)
            {
                AttachmentEntity.Update(entity.Id, workflowInstanceId);
            }
        }

        public int DeleteAttachment(string attachmentId)
        {
            if (string.IsNullOrEmpty(attachmentId))
                return 0;
            Attachment.DeleteById(new Guid(attachmentId));
            return 1;
        }

        public int DeleteAttachmentEntity(string attachmentId, string entityId, string entityType)
        {
            if (string.IsNullOrEmpty(attachmentId) || string.IsNullOrEmpty(entityId))
                return 0;
            AttachmentEntity.Delete(new Guid(attachmentId), new Guid(entityId), entityType);
            return 1;
        }

        public DataTable GetAttachmentsByEntity(string entityId, string entityType)
        {
            string sql = string.Format(SqlTemplate_GetEntityAttachments, entityId, entityType);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetAttachmentsByEntity(string entityId, string entityType, string creator)
        {
            string sql = string.Format(SqlTemplate_GetEntityAttachmentsByCreator, entityId, entityType, creator);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetCommentAttaByActivityInstanceId(Guid activityInstanceId)
        {
            string sql = string.Format(SqlTemplate_GetActivityAttachments, Comment.EntityType, activityInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        public DataTable GetCommentAttaByWorkflowInstanceId(Guid workflowInstanceId)
        {
            string sql = string.Format(SqlTemplate_GetWorkflowAttachments, Comment.EntityType, workflowInstanceId);
            return IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
        }

        #endregion
    }
}
