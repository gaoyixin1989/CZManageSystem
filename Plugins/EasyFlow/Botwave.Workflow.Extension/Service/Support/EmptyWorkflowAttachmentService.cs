using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Botwave.Workflow.Extension.Service.Support
{
    /// <summary>
    /// 流程附件服务的空实现类.
    /// </summary>
    public class EmptyWorkflowAttachmentService : IWorkflowAttachmentService
    {
        #region IWorkflowFileService 成员

        /// <summary>
        /// 获取指定流程实例的附件数.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public int GetAttachmentCount(Guid workflowInstanceId)
        {
            return 0;
        }

        /// <summary>
        /// 新增附件.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="creator"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateAttachment(HttpPostedFile fileInfo, string creator, string fileName)
        {
            return string.Empty;
        }

        /// <summary>
        ///  创建附件实体关系对象.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        public void CreateAttachmentEntity(string attachmentId, string entityId, string entityType)
        {

        }

        /// <summary>
        /// 更新流程附件实体关系.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        public void UpdateWorkflowAttachmentEntities(Guid workflowId, Guid workflowInstanceId)
        {

        }

        /// <summary>
        /// 删除指定附件.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        public int DeleteAttachment(string attachmentId)
        {
            //string attachmentId = row["AttachmentId"];

            return 0;
        }

        /// <summary>
        /// 删除附件实体关系.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="EntityId"></param>
        /// <param name="EntityType"></param>
        /// <returns></returns>
        public int DeleteAttachmentEntity(string attachmentId, string EntityId, string EntityType)
        {
            //string attachmentId = row["AttachmentId"];

            return 0;
        }

        /// <summary>
        /// 获取指定实体的附件数据表.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public DataTable GetAttachmentsByEntity(string entityId, string entityType)
        {
            return null;
        }

        /// <summary>
        /// 获取指定实体以及用户的附件数据表.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public DataTable GetAttachmentsByEntity(string entityId, string entityType, string creator)
        {
            return null;
        }

        /// <summary>
        /// 获取指定流程活动实例的评论附件数据表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public DataTable GetCommentAttaByActivityInstanceId(Guid activityInstanceId)
        {
            return null;
        }

        /// <summary>
        /// 获取指定流程实例的评论附件数据表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public DataTable GetCommentAttaByWorkflowInstanceId(Guid workflowInstanceId)
        {
            return null;
        }

        #endregion
    }
}
