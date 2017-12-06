using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Botwave.Commons;

namespace Botwave.XQP.Commons
{
    /// <summary>
    /// Data 辅助类.
    /// </summary>
    public sealed class DataHelper
    {
        /// <summary>
        /// 获取执行数据分页 SQL 的数据集对象.
        /// </summary>
        /// <param name="tableName">查询的表或者视图名称.</param>
        /// <param name="fieldKey">主键.</param>
        /// <param name="fieldShow">显示字段的 SQL 字符串.</param>
        /// <param name="fieldOrder">排序 SQL 字符串.</param>
        /// <param name="where">条件 SQL 字符串.</param>
        /// <param name="pageIndex">页面索引.</param>
        /// <param name="pageSize">每页显示的记录数.</param>
        /// <param name="recordCount">总记录数.</param>
        public static DataSet ExecuteDataPager(string tableName, string fieldKey, string fieldShow, string fieldOrder, 
            string where, int pageIndex, int pageSize, ref int recordCount)
        {
            SqlParameter[] parameters = new SqlParameter[] { 
                new SqlParameter("@TableName", SqlDbType.VarChar, 200),
                new SqlParameter("@FieldKey", SqlDbType.VarChar, 200),
                new SqlParameter("@PageCurrent", SqlDbType.Int),
                new SqlParameter("@PageSize", SqlDbType.Int),
                new SqlParameter("@FieldShow", SqlDbType.VarChar, 1000),
                new SqlParameter("@FieldOrder", SqlDbType.VarChar, 1000),
                new SqlParameter("@Where", SqlDbType.VarChar, 1000),
                new SqlParameter("@RecordCount", SqlDbType.Int)
            };
            parameters[0].Value = tableName;
            parameters[1].Value = fieldKey;
            parameters[2].Value = pageIndex;
            parameters[3].Value = pageSize;
            parameters[4].Value = fieldShow;
            parameters[5].Value = fieldOrder;
            parameters[6].Value = where;
            parameters[7].Direction = ParameterDirection.InputOutput;
            parameters[7].Value = recordCount;

            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "spPageViewByStr", parameters);
            recordCount = Convert.ToInt32(parameters[7].Value);

            return ds;
        }        

        #region 转换

        /// <summary>
        /// 转换为十进制(Decimal)类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? ToDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            decimal output;
            if (decimal.TryParse(value.ToString(), out output))
                return output;
            return null;
        }

        /// <summary>
        /// 转换为时间类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            DateTime output;
            if (DateTime.TryParse(value.ToString(), out output))
                return output;
            return null;
        }

        /// <summary>
        /// 转化为 Guid 类型.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid? ToGuid(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;
            try { return new Guid(value.ToString()); }
            catch { }
            return null;
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string HtmlDecode(string value)
        {
            byte[] space = new byte[] { 0xc2, 0xa0 }; 
            string UTFSpace = Encoding.GetEncoding("UTF-8").GetString(space);
            value = value.Replace(UTFSpace, "&nbsp;");
            return System.Web.HttpUtility.HtmlDecode(value);
        }
        #endregion
    }
}
