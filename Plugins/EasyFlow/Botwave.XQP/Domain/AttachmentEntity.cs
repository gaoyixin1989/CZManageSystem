using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 附件实体关系类.
    /// </summary>
    public class AttachmentEntity 
    {    
        /// <summary>
        /// 依据实例和类型查询相关附件.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static IList<Attachment> Select(string entityType, Guid entityId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("EntityId", entityId);
            ht.Add("EntityType", entityType);
            return IBatisMapper.Select<Attachment>("Attachment_Select_By_EntityIdAndEntityType", ht);
        }

        /// <summary>
        /// 依据实例查询相关附件
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static IList<Attachment> Select(Guid entityId)
        {
            return IBatisMapper.Select<Attachment>("Attachment_Select_By_EntityId", entityId);
        }

        /// <summary>
        /// 创建附件关系
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        public static void CreateAttachmentEntity(Guid attachmentId, Guid entityId, string entityType)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AttachmentId", attachmentId);
            ht.Add("EntityId", entityId);
            ht.Add("EntityType", entityType);
            IBatisMapper.Insert("Attachment_Entity_Insert", ht);
        }

        /// <summary>
        /// 删除附件关系
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        public static void Delete(Guid attachmentId, Guid entityId, string entityType)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AttachmentId", attachmentId);
            ht.Add("EntityId", entityId);
            ht.Add("EntityType", entityType);
            IBatisMapper.Delete("AttachmentEntity_Delete", ht);
        }

        /// <summary>
        /// 更新附件关系
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        /// <param name="entityType"></param>
        public static void Update(Guid attachmentId, Guid entityId, string entityType)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AttachmentId", attachmentId);
            ht.Add("EntityId", entityId);
            ht.Add("EntityType", entityType);
            IBatisMapper.Update("AttachmentEntity_Update", ht);
        }

        /// <summary>
        /// 更新附件关系
        /// </summary>
        /// <param name="attachmentId"></param>
        /// <param name="entityId"></param>
        public static void Update(Guid attachmentId, Guid entityId)
        {
            Hashtable ht = new Hashtable();
            ht.Add("AttachmentId", attachmentId);
            ht.Add("EntityId", entityId);
            IBatisMapper.Update("AttachmentEntity_UpdateEnitityId", ht);
        }
    }
}
