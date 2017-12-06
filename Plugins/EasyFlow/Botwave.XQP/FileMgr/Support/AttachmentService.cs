using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;
using Botwave.XQP.Commons;

namespace Botwave.XQP.FileMgr.Support
{
    /// <summary>
    /// 附件数据服务的默认实现类.
    /// </summary>
    public class AttachmentService : IAttachmentService
    {
        #region IAttachmentService 成员

        public int InsertAttachment(Attachment item)
        {
            string sql = @"INSERT INTO bwiap_Attachments(AttachmentKey, SysId, State, FileName, FilePath, FileLength, FileType, Remark, Creator, CreatedTime, LastModifier, LastModTime)
                                   VALUES(@AttachmentKey, @SysId, @State, @FileName, @FilePath, @FileLength, @FileType, @Remark, @Creator, getdate(), @LastModifier, getdate())";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@AttachmentKey", SqlDbType.UniqueIdentifier),
                    new SqlParameter("@SysId", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@FileName", SqlDbType.NVarChar, 255),
                    new SqlParameter("@FilePath", SqlDbType.NVarChar, 255),
                    new SqlParameter("@FileLength", SqlDbType.Decimal),
                    new SqlParameter("@FileType", SqlDbType.NVarChar, 128),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 255),
                    new SqlParameter("@Creator", SqlDbType.NVarChar, 64),
                    new SqlParameter("@LastModifier", SqlDbType.NVarChar, 64)
            };
            parameters[0].Value = item.AttachmentKey;
            parameters[1].Value = item.SysId;
            parameters[2].Value = item.State;
            parameters[3].Value = item.FileName;
            parameters[4].Value = item.FilePath;
            parameters[5].Value = item.FileLength;
            parameters[6].Value = item.FileType;
            parameters[7].Value = item.Remark;
            parameters[8].Value = item.Creator;
            parameters[9].Value = item.LastModifier;

            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int UpdateAttachment(Attachment item)
        {
            string sql = @"UPDATE bwiap_Attachments SET 
                                       SysId = @SysId, State = @State, FileName = @FileName, FilePath = @FilePath, FileLength = @FileLength, 
                                       FileType = @FileType, Remark = @Remark, LastModifier = @LastModifier, LastModTime = getdate()
                                   WHERE AttachmentId=@AttachmentId";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@SysId", SqlDbType.Int),
                    new SqlParameter("@State", SqlDbType.Int),
                    new SqlParameter("@FileName", SqlDbType.NVarChar, 255),
                    new SqlParameter("@FilePath", SqlDbType.NVarChar, 255),
                    new SqlParameter("@FileLength", SqlDbType.Decimal),
                    new SqlParameter("@FileType", SqlDbType.NVarChar, 128),
                    new SqlParameter("@Remark", SqlDbType.NVarChar, 255),
                    new SqlParameter("@LastModifier", SqlDbType.NVarChar, 64),
                    new SqlParameter("@AttachmentId", SqlDbType.UniqueIdentifier)
            };
            parameters[0].Value = item.SysId;
            parameters[1].Value = item.State;
            parameters[2].Value = item.FileName;
            parameters[3].Value = item.FilePath;
            parameters[4].Value = item.FileLength;
            parameters[5].Value = item.FileType;
            parameters[6].Value = item.Remark;
            parameters[7].Value = item.LastModifier;
            parameters[8].Value = item.AttachmentId;

            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int DeleteAttachment(int attchmentId)
        {
            string sql = "UPDATE SET State = 1 WHERE AttachmentId=@AttachmentId";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@AttachmentId", SqlDbType.Int) };
            parameters[0].Value = attchmentId;
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int DeleteAttachment(Guid attchmentKey)
        {
            string sql = "UPDATE SET State = 1 WHERE AttachmentKey=@AttachmentKey";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@AttachmentKey", SqlDbType.UniqueIdentifier) };
            parameters[0].Value = attchmentKey;
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int InsertAttachmentEntity(Guid attchmentKey, string entityType, string entityId)
        {
            string sql = @"INSERT INTO bwiap_Attachments_Entity(AttachmentKey, EntityType, EntityId)
                    VALUES (@AttachmentKey, @EntityType, @EntityId)";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@AttachmentKey", SqlDbType.UniqueIdentifier),
                    new SqlParameter("EntityType", SqlDbType.NVarChar, 50),
                    new SqlParameter("EntityId", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = attchmentKey;
            parameters[1].Value = entityType;
            parameters[2].Value = entityId;
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int DeleteAttachmentEntity(string entityType, string entityId)
        {
            string sql = @"DELETE FROM bwiap_Attachments_Entity WHERE EntityType=@EntityType AND EntityId=@EntityId";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@EntityType", SqlDbType.NVarChar, 50),
                    new SqlParameter("@EntityId", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = entityType;
            parameters[1].Value = entityId;
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public int DeleteAttachmentEntity(Guid attchmentKey, string entityType, string entityId)
        {
            string sql = @"DELETE FROM bwiap_Attachments_Entity WHERE (
                    AttachmentKey =@AttachmentKey AND EntityType=@EntityType AND EntityId=@EntityId)";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@AttachmentKey", SqlDbType.UniqueIdentifier),
                    new SqlParameter("@EntityType", SqlDbType.NVarChar, 50),
                    new SqlParameter("@EntityId", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = attchmentKey;
            parameters[1].Value = entityType;
            parameters[2].Value = entityId;

            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        public Attachment GetAttachment(int attchmentId)
        {
            string sql = @"SELECT AttachmentId, AttachmentKey, SysId, State, FileName, FilePath, FileLength, FileType, Remark, Creator, CreatedTime, LastModifier, LastModTime
                                   FROM bwiap_Attachments WHERE AttachmentId = @AttachmentId";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@AttachmentId", SqlDbType.Int)
            };
            parameters[0].Value = attchmentId;

            DataSet ds = SqlHelper.ExecuteDataset(CommandType.Text, sql, parameters);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            return ToAttachment(ds.Tables[0].Rows[0]);
        }

        public Attachment GetAttachment(Guid attachmentKey)
        {
            string sql = @"SELECT AttachmentId, AttachmentKey, SysId, State, FileName, FilePath, FileLength, FileType, Remark, Creator, CreatedTime, LastModifier, LastModTime
                                   FROM bwiap_Attachments WHERE AttachmentKey = @AttachmentKey";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@AttachmentKey", SqlDbType.UniqueIdentifier)
            };
            parameters[0].Value = attachmentKey;

            DataSet ds = SqlHelper.ExecuteDataset(CommandType.Text, sql, parameters);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            return ToAttachment(ds.Tables[0].Rows[0]);
        }

        public IList<Attachment> GetAttachmentsByEntity(string entityType, string entityId)
        {
            string sql = @"SELECT AttachmentId, AttachmentKey, SysId, State, FileName, FilePath, FileLength, FileType, Remark, Creator, CreatedTime, LastModifier, LastModTime
                                   FROM bwiap_Attachments WHERE (AttachmentKey IN (
                                        SELECT AttachmentKey FROM bwiap_Attachments_Entity WHERE EntityType = @EntityType AND EntityID = @EntityID))";
            SqlParameter[] parameters = new SqlParameter[] { 
                    new SqlParameter("@EntityType", SqlDbType.NVarChar, 50),
                    new SqlParameter("@EntityID", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = entityType;
            parameters[1].Value = entityId;

            DataSet ds = SqlHelper.ExecuteDataset(CommandType.Text, sql, parameters);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return new List<Attachment>();
            DataRowCollection rows = ds.Tables[0].Rows;
            IList<Attachment> results = new List<Attachment>();
            foreach (DataRow row in rows)
            {
                results.Add(ToAttachment(row));
            }
            return results;
        }

        #endregion

        #region methods

        /// <summary>
        /// 将附件数据行转换为 Attachment 对象实例.
        /// </summary>
        /// <param name="attachmentRow"></param>
        /// <returns></returns>
        public static Attachment ToAttachment(DataRow attachmentRow)
        {
            if (attachmentRow == null)
                return null;
            Attachment item = new Attachment();
            item.AttachmentId = DbUtils.ToInt32(attachmentRow["AttachmentId"]);
            item.AttachmentKey = DataHelper.ToGuid(attachmentRow["AttachmentKey"]).Value;
            item.FileName = DbUtils.ToString(attachmentRow["FileName"]);
            item.FilePath = DbUtils.ToString(attachmentRow["FilePath"]);
            item.FileLength = DataHelper.ToDecimal(attachmentRow["FileLength"]).Value;
            item.FileType = DbUtils.ToString(attachmentRow["FileType"]);
            item.Remark = DbUtils.ToString(attachmentRow["Remark"]);
            item.Creator = DbUtils.ToString(attachmentRow["Creator"]);
            item.LastModifier = DbUtils.ToString(attachmentRow["LastModifier"]);
            item.CreatedTime = DataHelper.ToDateTime(attachmentRow["LastModifier"]).Value;
            item.LastModTime = DataHelper.ToDateTime(attachmentRow["LastModifier"]).Value;
            return item;
        }

        #endregion
    }
}
