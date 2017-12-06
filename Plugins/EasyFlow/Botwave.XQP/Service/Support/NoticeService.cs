using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;
using Botwave.XQP.Commons;
using Botwave.XQP.Domain;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 公告通知数据服务的默认实现类.
    /// </summary>
    public class NoticeService : INoticeService
    {
        #region INoticeService 成员

        /// <summary>
        /// 新增公告
        /// </summary>
        public int InsertNotice(Notice item)
        {
            string sql = @"INSERT INTO bwcms_Notices(Title, Content, Enabled, StartTime, EndTime, Creator, CreatedTime, LastModifier, LastModTime, EntityType, EntityId)
                    VALUES(@Title, @Content, @Enabled, @StartTime, @EndTime, @Creator, getdate(), @LastModifier, getdate(), @EntityType, @EntityId)
                    ;SELECT @@IDENTITY";
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Title", SqlDbType.NVarChar, 255),
                new SqlParameter("@Content", SqlDbType.NText),
                new SqlParameter("@Enabled", SqlDbType.Bit),
                new SqlParameter("@StartTime", SqlDbType.DateTime),
                new SqlParameter("@EndTime", SqlDbType.DateTime),
                new SqlParameter("@Creator", SqlDbType.NVarChar, 50),
                new SqlParameter("@LastModifier", SqlDbType.NVarChar, 50),
                new SqlParameter("@EntityType", SqlDbType.NVarChar, 32),
                new SqlParameter("@EntityId", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = item.Title;
            parameters[1].Value = item.Content;
            parameters[2].Value = item.Enabled;
            parameters[3].Value = item.StartTime;
            parameters[4].Value = item.EndTime;
            parameters[5].Value = item.Creator;
            parameters[6].Value = item.LastModifier;
            parameters[7].Value = item.EntityType;
            parameters[8].Value = item.EntityId;

            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql, parameters);
            return ((result == null) ? 0 : Convert.ToInt32(result));
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        public int UpdateNotice(Notice item)
        {
            string sql = @"UPDATE bwcms_Notices SET
                        Title=@Title, Content=@Content, Enabled=@Enabled, StartTime=@StartTime, 
                        EndTime=@EndTime,  LastModifier=@LastModifier, LastModTime=getdate()
                    WHERE NoticeId=@NoticeId";

            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@Title", SqlDbType.NVarChar, 255),
                new SqlParameter("@Content", SqlDbType.NText),
                new SqlParameter("@Enabled", SqlDbType.Bit),
                new SqlParameter("@StartTime", SqlDbType.DateTime),
                new SqlParameter("@EndTime", SqlDbType.DateTime),
                new SqlParameter("@LastModifier", SqlDbType.NVarChar, 50),
                new SqlParameter("@NoticeId", SqlDbType.Int)
            };
            parameters[0].Value = item.Title;
            parameters[1].Value = item.Content;
            parameters[2].Value = item.Enabled;
            parameters[3].Value = item.StartTime;
            parameters[4].Value = item.EndTime;
            parameters[5].Value = item.LastModifier;
            parameters[6].Value = item.NoticeId;

            return IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 启用/停止公告
        /// </summary>
        public int UpdateNoticeEnabled(int noticeId, bool enabled)
        {
            string sql = @"UPDATE bwcms_Notices SET  Enabled = @Enabled WHERE NoticeId=@NoticeId";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@Enabled", SqlDbType.Bit),
                new SqlParameter("@NoticeId", SqlDbType.Int)
            };
            parameters[0].Value = enabled;
            parameters[1].Value = noticeId;

            return IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql, parameters);
        }

        /// <summary>
        /// 删除公告
        /// </summary>
        public int DeleteNotice(int noticeId)
        {
            string sql = string.Format("DELETE FROM bwcms_Notices WHERE (NoticeId = '{0}')", noticeId);

            return IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        public virtual Notice GetNotice(int noticeId)
        {
            string sql = @"SELECT NoticeId, Title, Content, Enabled, StartTime, EndTime, Creator, CreatedTime, 
                          LastModifier, LastModTime, EntityType, EntityId
                    FROM bwcms_Notices WHERE NoticeId = '{0}'";

            sql = string.Format(sql, noticeId);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (resultTable == null || resultTable.Rows.Count == 0)
                return null;
            return ToNotice(resultTable.Rows[0]);
        }

        /// <summary>
        /// 获取关联实体的所有公告.
        /// </summary>
        public virtual IList<Notice> GetNotices(string entityType, string entityId)
        {
            string sql = @"SELECT NoticeId, Title, Content, Enabled, StartTime, EndTime, Creator, CreatedTime, 
                          LastModifier, LastModTime, EntityType, EntityId
                    FROM bwcms_Notices 
                    WHERE (EntityType = '{0}' AND EntityId='{1}') 
                          AND (Enabled = 1) AND (getdate() >= StartTime AND getdate() <= EndTime)";

            sql = string.Format(sql, entityType, entityId);
            DataTable resultTable = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if (resultTable == null || resultTable.Rows.Count == 0)
                return new List<Notice>();
            IList<Notice> results = new List<Notice>();
            foreach (DataRow row in resultTable.Rows)
            {
                results.Add(ToNotice(row));
            }
            return results;
        }

        /// <summary>
        /// 获取公告列表
        /// </summary>
        public virtual List<Notice> GetNoticeList(string creator, bool? enabled, int noticeNum)
        {
            string sql = @"SELECT NoticeId, Title, Content, Enabled, StartTime, EndTime, Creator, CreatedTime, 
                          LastModifier, LastModTime, EntityType, EntityId
                    FROM bwcms_Notices WHERE 1=1";

            if (!string.IsNullOrEmpty(creator))
                sql += string.Format(" AND (Creator = '{0}')", creator);
            if (enabled.HasValue)
                sql += string.Format(" AND (Enabled = '{0}')", (enabled.Value ? 1 : 0));

            sql += " ORDER BY LastModTime desc";

            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            noticeNum = dt.Rows.Count > noticeNum ? noticeNum : dt.Rows.Count;
            List<Notice> lst = new List<Notice>();
            for (int i = 0; i < noticeNum; i++)
            {
                lst.Add(ToNotice(dt.Rows[i]));
            }

            return lst;
        }

        /// <summary>
        /// 获取分页公告列表
        /// </summary>
        public virtual List<Notice> GetNoticeList(string creator, bool? enabled, int pageIndex, int pageSize, ref int recordCount)
        {
            DataTable dt = this.GetNotices(creator, enabled, pageIndex, pageSize, ref recordCount);
            List<Notice> lst = new List<Notice>();
            foreach (DataRow row in dt.Rows)
            {
                lst.Add(ToNotice(row));
            }

            return lst;
        }

        /// <summary>
        /// 获取分页公告列表
        /// </summary>
        public virtual DataTable GetNotices(string creator, bool? enabled, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwcms_Notices";
            string fieldKey = "NoticeId";
            string fieldShow = "NoticeId, Title, Enabled, StartTime, EndTime, Creator, CreatedTime, LastModifier, LastModTime, EntityType, EntityId, CreatorName";
            string fieldOrder = "LastModTime desc";
            string where = " (1=1) ";
            if (!string.IsNullOrEmpty(creator))
                where += string.Format(" AND (Creator = '{0}')", creator);
            if (enabled.HasValue)
                where += string.Format(" AND (Enabled = '{0}')", (enabled.Value ? 1 : 0));

            DataSet resultSet = DataHelper.ExecuteDataPager(tableName, fieldKey, fieldShow, fieldOrder, where, pageIndex, pageSize, ref recordCount);
            return resultSet.Tables[0];
        }

        /// <summary>
        /// 分页获取指定条件的公告信息数据表.
        /// </summary>
        /// <param name="creator">公告创建人.为空时表示不限制公告创建人.</param>
        /// <param name="entityType">公告类型.为空时表示不限制公告类型.</param>
        /// <param name="enabled">是否可用. null 时表示包括可用与不可用公告.</param>
        /// <param name="pageIndex">页面索引.</param>
        /// <param name="pageSize">每页显示的数据记录数.</param>
        /// <param name="recordCount">数据总记录数.</param>
        /// <returns></returns>
        public virtual DataTable GetNotices(string creator, string entityType, bool? enabled, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwcms_Notices";
            string fieldKey = "NoticeId";
            string fieldShow = "NoticeId, Title, Enabled, StartTime, EndTime, Creator, CreatedTime, LastModifier, LastModTime, EntityType, EntityId, CreatorName";
            string fieldOrder = "LastModTime desc";
            string where = " (1=1) ";
            if (!string.IsNullOrEmpty(creator))
                where += string.Format(" AND (Creator = '{0}')", creator);
            if (!string.IsNullOrEmpty(entityType))
                where += string.Format(" AND (EntityType = '{0}')", entityType);
            if (enabled.HasValue)
                where += string.Format(" AND (Enabled = '{0}')", (enabled.Value ? 1 : 0));

            DataSet resultSet = DataHelper.ExecuteDataPager(tableName, fieldKey, fieldShow, fieldOrder, where, pageIndex, pageSize, ref recordCount);
            return resultSet.Tables[0];
        }


        /// <summary>
        /// 获取分页公告列表
        /// </summary>
        public virtual DataTable GetNotices(string creator, string entityType, string entityId, bool? enabled, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwcms_Notices";
            string fieldKey = "NoticeId";
            string fieldShow = "NoticeId, Title, Enabled, StartTime, EndTime, Creator, CreatedTime, LastModifier, LastModTime, EntityType, EntityId, CreatorName";
            string fieldOrder = "LastModTime desc";
            string where = " (1=1) ";
            if (!string.IsNullOrEmpty(creator))
                where += string.Format(" AND (Creator = '{0}')", creator);
            if (!string.IsNullOrEmpty(entityType))
                where += string.Format(" AND (EntityType = '{0}')", entityType);
            if (!string.IsNullOrEmpty(entityId))
                where += string.Format(" AND (EntityId = '{0}' OR EntityId='0')", entityId);
            if (enabled.HasValue)
                where += string.Format(" AND (Enabled = '{0}')", (enabled.Value ? 1 : 0));

            DataSet resultSet = DataHelper.ExecuteDataPager(tableName, fieldKey, fieldShow, fieldOrder, where, pageIndex, pageSize, ref recordCount);
            return resultSet.Tables[0];
        }

        #endregion

        /// <summary>
        /// 转化为 Notice 对象.
        /// </summary>
        public static Notice ToNotice(DataRow row)
        {
            Notice item = new Notice();
            item.NoticeId = DbUtils.ToInt32(row["NoticeId"]);
            item.Title = DbUtils.ToString(row["Title"]);
            item.Content = DbUtils.ToString(row["Content"]);
            item.Enabled = DbUtils.ToBoolean(row["Enabled"]);
            item.StartTime = ToDateTime(row["StartTime"]);
            item.EndTime = ToDateTime(row["EndTime"]);
            item.Creator = DbUtils.ToString(row["Creator"]);
            item.LastModifier = DbUtils.ToString(row["LastModifier"]);
            item.CreatedTime = ToDateTime(row["CreatedTime"]).Value;
            item.LastModTime = ToDateTime(row["LastModTime"]).Value;
            item.EntityType = DbUtils.ToString(row["EntityType"]);
            item.EntityId = DbUtils.ToString(row["EntityId"]);

            return item;
        }

        /// <summary>
        /// 转换指定对象为时间类型.
        /// </summary>
        public static DateTime? ToDateTime(object input)
        {
            if (input == null || input == DBNull.Value)
                return null;
            DateTime outputTime;
            if (DateTime.TryParse(input.ToString(), out outputTime))
                return outputTime;
            return null;
        }
    }
}
