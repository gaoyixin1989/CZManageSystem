using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.FileMgr
{
    /// <summary>
    /// 附件数据服务接口.
    /// </summary>
    public interface IAttachmentService
    {
        /// <summary>
        /// 新增附件数据.
        /// </summary>
        /// <param name="item">新增的附件信息对象.</param>
        /// <returns></returns>
        int InsertAttachment(Attachment item);

        /// <summary>
        /// 更新附件数据.
        /// </summary>
        /// <param name="item">要更新附件信息对象.</param>
        /// <returns></returns>
        int UpdateAttachment(Attachment item);

        /// <summary>
        /// 删除指定附件编号的附件信息.
        /// </summary>
        /// <param name="attchmentId">附件编号.</param>
        /// <returns></returns>
        int DeleteAttachment(int attchmentId);

        /// <summary>
        /// 删除指定附件键名的附件信息.
        /// </summary>
        /// <param name="attchmentKey">附件键名.</param>
        /// <returns></returns>
        int DeleteAttachment(Guid attchmentKey);

        /// <summary>
        /// 新增附件实体关联数据.
        /// </summary>
        /// <param name="attchmentKey">附件键名.</param>
        /// <param name="entityType">关联的实体类型.</param>
        /// <param name="entityId">关联的实体编号.</param>
        /// <returns></returns>
        int InsertAttachmentEntity(Guid attchmentKey, string entityType, string entityId);

        /// <summary>
        /// 删除指定关联实体的所有附件实体数据.
        /// </summary>
        /// <param name="entityType">关联的实体类型.</param>
        /// <param name="entityId">关联的实体编号.</param>
        /// <returns></returns>
        int DeleteAttachmentEntity(string entityType, string entityId);

        /// <summary>
        /// 删除指定附件键名与关联实体的附件实体数据.
        /// </summary>
        /// <param name="attchmentKey">附件键名.</param>
        /// <param name="entityType">关联的实体类型.</param>
        /// <param name="entityId">关联的实体编号.</param>
        /// <returns></returns>
        int DeleteAttachmentEntity(Guid attchmentKey, string entityType, string entityId);

        /// <summary>
        /// 获取指定附件编号的附件数据信息.
        /// </summary>
        /// <param name="attchmentId">查询的附件编号.</param>
        /// <returns></returns>
        Attachment GetAttachment(int attchmentId);

        /// <summary>
        /// 获取指定附件键名的附件数据信息.
        /// </summary>
        /// <param name="attachmentKey">查询的附件键名.</param>
        /// <returns></returns>
        Attachment GetAttachment(Guid attachmentKey);

        /// <summary>
        /// 获取指定实体类型与编号的附件数据列表.
        /// </summary>
        /// <param name="entityType">关联的实体类型.</param>
        /// <param name="entityId">关联的实体编号.</param>
        /// <returns></returns>
        IList<Attachment> GetAttachmentsByEntity(string entityType, string entityId);
    }
}
