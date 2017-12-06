using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace Botwave.Workflow.Extension.Service
{
    /// <summary>
    /// 流程附件服务接口.
    /// </summary>
    public interface IWorkflowAttachmentService
    {
        /// <summary>
        /// 获取指定流程实例的附件数.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        int GetAttachmentCount(Guid workflowInstanceId);

        /// <summary>
        /// 创建附件信息.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="creator"></param>
        /// <param name="fileName"></param>
        /// <returns>返回附件编号.</returns>
        string CreateAttachment(HttpPostedFile fileInfo, string creator, string fileName);

        /// <summary>
        /// 创建附件实体关系.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        void CreateAttachmentEntity(string attachmentId, string entityId, string entityType);

        /// <summary>
        /// 更新指定流程定义编号的附件对应实体对象为流程实例编号.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="workflowInstanceId"></param>
        void UpdateWorkflowAttachmentEntities(Guid workflowId, Guid workflowInstanceId);

        /// <summary>
        /// 删除指定的附件信息.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <returns></returns>
        int DeleteAttachment(string attachmentId);

        /// <summary>
        /// 删除指定的附件实体关系数据.
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        int DeleteAttachmentEntity(string attachmentId, string entityId, string entityType);

        /// <summary>
        /// 根据指定的实体信息获取指定附件信息数据表.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <returns></returns>
        DataTable GetAttachmentsByEntity(string entityId, string entityType);

        /// <summary>
        /// 根据指定的实体信息获取指定附件信息数据表.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        DataTable GetAttachmentsByEntity(string entityId, string entityType, string creator);

        /// <summary>
        /// 获取指定流程步骤实例的评论附件数据表.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        DataTable GetCommentAttaByActivityInstanceId(Guid activityInstanceId);

        /// <summary>
        /// 获取指定流程实例的评论附件数据表.
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        DataTable GetCommentAttaByWorkflowInstanceId(Guid workflowInstanceId);
    }
}
