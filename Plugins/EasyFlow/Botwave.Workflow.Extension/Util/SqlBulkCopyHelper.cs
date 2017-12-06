using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Botwave.Workflow.Extension.Util
{
    /// <summary>
    /// SqlBulkCopy 辅助类.
    /// </summary>
    public static class SqlBulkCopyHelper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SqlBulkCopyHelper));

        #region 批量执行

        /// <summary>
        /// 批量复制.
        /// </summary>
        /// <param name="connectionString">数据库连接字符串.</param>
        /// <param name="destinationTableName">数据库目标表名称.</param>
        /// <param name="columnMappings">数据列名映射集合(/key/sourceColumn, /value/destinationColumn)</param>
        /// <param name="sourceTable">要复制到数据库中源数据表对象.</param>
        /// <returns>True 表示插入成功. False 失败.</returns>
        public static bool ExecuteBulkCopy(string connectionString, string destinationTableName, NameValueCollection columnMappings, DataTable sourceTable)
        {
            if (sourceTable == null || sourceTable.Rows.Count == 0)
                return false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    if (!SqlBulkCopyHelper.ExecuteBulkCopy(connection, transaction, destinationTableName, columnMappings, sourceTable))
                    {
                        transaction.Rollback();
                        return false;
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // 回滚
                    log.Info(ex);
                    return false;
                }
                finally
                {
                    transaction.Dispose();
                    connection.Close();
                }
            }
            return true;
        }

        /// <summary>
        /// 执行批量复制.
        /// </summary>
        /// <param name="connection">数据库连接对象.</param>
        /// <param name="destinationTableName">数据库目标表名称.</param>
        /// <param name="columnMappings">数据列名映射集合(/key/sourceColumn, /value/destinationColumn).</param>
        /// <param name="sourceTable">要复制到数据库中源数据表对象.</param>
        /// <returns>True 表示插入成功. False 失败.</returns>
        public static bool ExecuteBulkCopy(SqlConnection connection, string destinationTableName, NameValueCollection columnMappings, DataTable sourceTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
            {
                return SqlBulkCopyHelper.ExecuteBulkCopy(bulkCopy, destinationTableName, columnMappings, sourceTable);
            }
        }

        /// <summary>
        /// 执行批量复制.
        /// </summary>
        /// <param name="connection">数据库连接对象.</param>
        /// <param name="transaction">数据库事务对象.</param>
        /// <param name="destinationTableName">数据库目标表名称.</param>
        /// <param name="columnMappings">数据列名映射集合(/key/sourceColumn, /value/destinationColumn).</param>
        /// <param name="sourceTable">要复制到数据库中源数据表对象.</param>
        /// <returns>True 表示插入成功. False 失败.</returns>
        public static bool ExecuteBulkCopy(SqlConnection connection, SqlTransaction transaction, string destinationTableName, NameValueCollection columnMappings, DataTable sourceTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
            {
                return SqlBulkCopyHelper.ExecuteBulkCopy(bulkCopy, destinationTableName, columnMappings, sourceTable);
            }
        }

        /// <summary>
        /// 执行批量复制.
        /// </summary>
        /// <param name="bulkCopy">批量执行对象.</param>
        /// <param name="destinationTableName">数据库目标表名称.</param>
        /// <param name="columnMappings">数据列名映射集合(/key/sourceColumn, /value/destinationColumn).</param>
        /// <param name="sourceTable">要复制到数据库中源数据表对象.</param>
        /// <returns>True 表示插入成功. False 失败.</returns>
        private static bool ExecuteBulkCopy(SqlBulkCopy bulkCopy, string destinationTableName, NameValueCollection columnMappings, DataTable sourceTable)
        {
            try
            {
                bulkCopy.DestinationTableName = destinationTableName;
                if (columnMappings != null && columnMappings.Count > 0)
                {
                    foreach (string key in columnMappings.Keys)
                    {
                        bulkCopy.ColumnMappings.Add(key, columnMappings[key]);
                    }
                }
                bulkCopy.WriteToServer(sourceTable);
                return true;
            }
            catch (Exception ex)
            {
                log.Info(ex);
                return false;
            }
        }

        #endregion
    }
}
