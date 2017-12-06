using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.DynamicForm;
using Botwave.DynamicForm.Plugin;
using Botwave.DynamicForm.Services;

namespace Botwave.DynamicForm.IBatisNet
{
    /// <summary>
    /// 流程表单项实例数据分表服务实现类.
    /// </summary>
    public class WorkflowPartTableService : IPartTableService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(WorkflowPartTableService));

        #region IPartTableService 成员

        public bool CreateTable(int tableIndex)
        {
            #region SQL 创建数据表
            
            string sql = @"
                    if NOT exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[bwdf_FormItemInstances_{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
                    BEGIN
                        CREATE TABLE [dbo].[bwdf_FormItemInstances_{0}] (
	                        [Id] [uniqueidentifier] NOT NULL ,
	                        [FormItemDefinitionId] [uniqueidentifier] NOT NULL ,
	                        [FormInstanceId] [uniqueidentifier] NOT NULL ,
	                        [Value_Str] [nvarchar] (3000) COLLATE Chinese_PRC_CI_AS NULL ,
	                        [Value_Decimal] [decimal](18, 2) NULL ,
	                        [Value_Text] [ntext] COLLATE Chinese_PRC_CI_AS NULL 
                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

                        ALTER TABLE [dbo].[bwdf_FormItemInstances_{0}] WITH NOCHECK ADD 
	                        CONSTRAINT [PK_bwdf_FormItemInstances_{0}] PRIMARY KEY  CLUSTERED 
	                        (
		                        [Id]
	                        ) WITH  FILLFACTOR = 90  ON [PRIMARY];

                         CREATE  INDEX [ix_bwdf_FormItemInstances_{0}_FormItemDefinitionId_FormInstanceId] ON [dbo].[bwdf_FormItemInstances_{0}]([FormItemDefinitionId], [FormInstanceId]) ON [PRIMARY]
                    ;

                        if (select DATABASEPROPERTY(DB_NAME(), N'IsFullTextEnabled')) <> 1 
                        exec sp_fulltext_database N'enable' 

                        ;

                            if not exists (select * from dbo.sysfulltextcatalogs where name = N'XQPCatalog')
                            exec sp_fulltext_catalog N'XQPCatalog', N'create' 

                        ;

                            exec sp_fulltext_table N'[dbo].[bwdf_FormItemInstances_{0}]', N'create', N'XQPCatalog', N'PK_bwdf_FormItemInstances_{0}'
                       ;

                        exec sp_fulltext_column N'[dbo].[bwdf_FormItemInstances_{0}]', N'Value_Str', N'add', 2052  
                        ;

                        exec sp_fulltext_column N'[dbo].[bwdf_FormItemInstances_{0}]', N'Value_Text', N'add', 2052  
                       ;

                        exec sp_fulltext_table N'[dbo].[bwdf_FormItemInstances_{0}]', N'activate'  
                        ;

                        ALTER TABLE [dbo].[bwdf_FormItemInstances_{0}] ADD 
	                        CONSTRAINT [FK_bwdf_FormItemInstances_{0}_bwdf_FormInstances] FOREIGN KEY 
	                        (
		                        [FormInstanceId]
	                        ) REFERENCES [dbo].[bwdf_FormInstances] (
		                        [Id]
	                        ),
	                        CONSTRAINT [FK_bwdf_FormItemInstances_{0}_bwdf_FormItemDefinitions] FOREIGN KEY 
	                        (
		                        [FormItemDefinitionId]
	                        ) REFERENCES [dbo].[bwdf_FormItemDefinitions] (
		                        [Id]
	                        ) ON DELETE CASCADE 
                    END";
            #endregion

            try
            {
                sql = string.Format(sql, tableIndex);
                IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        public IList<Guid> GetCompleteInstanceList(int topCount)
        {IList<Guid> results = new List<Guid>();
            if (topCount <= 0)
                return results;

            string sql = @"SELECT TOP {0} fii.Id
                        FROM bwdf_FormInstances fii 
                            LEFT JOIN  bwwf_Tracking_Workflows tw ON fii.Id = tw.WorkflowInstanceId
                        WHERE (tw.State = '99' OR tw.State = '2') AND (fii.TableIndex = '0')
                        ORDER BY fii.CreatedTime";
            sql = string.Format(sql, topCount);
            DataSet ds = new DataSet();
            DataTable table = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            if(table!= null && table.Rows.Count>0)
            {
                foreach (DataRow row in table.Rows)
                {
                    results.Add(new Guid(DbUtils.ToString(row[0])));
                }   
            }
            return results;
        }

        public bool MigrateData(int topCount, int tableIndex)
        {
            if (this.CreateTable(tableIndex))
            {
                IList<Guid> list = this.GetCompleteInstanceList(topCount);

                log.Info("instances:" + (list == null ? "null" : list.Count.ToString()));

                if (this.MigrateData(list, tableIndex))
                {
                    return true;
                }
            }
            return false;
        }

        public bool MigrateData(IList<Guid> formInstanceIdList, int tableIndex)
        {
            if (formInstanceIdList == null || formInstanceIdList.Count == 0 || tableIndex <= 0)
                return false;

            using (IDbConnection connection = IBatisDbHelper.CreateConnection())
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    foreach (Guid formInstanceId in formInstanceIdList)
                    {
                        if (MigrateData(transaction, formInstanceId, tableIndex))
                        {
                            DeleteData(transaction, formInstanceId);
                            UpdateTableIndex(transaction, formInstanceId, tableIndex);
                        }
                    }
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    log.Error(ex);
                    return false;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }

        #endregion

        #region 迁移数据到分表

        /// <summary>
        /// 更新表单实例的分表索引.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="formInstanceId"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        protected static bool UpdateTableIndex(IDbTransaction transaction, Guid formInstanceId, int tableIndex)
        {
            string sql = string.Format("UPDATE bwdf_FormInstances SET TableIndex = '{0}'  WHERE [Id] = '{1}'", tableIndex, formInstanceId);
            int rowAffected = IBatisDbHelper.ExecuteNonQuery(transaction, CommandType.Text, sql);
            return (rowAffected == 1);
        }

        /// <summary>
        /// 删除默认表中的指定表单项实例数据.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="formInstanceId"></param>
        protected static void DeleteData(IDbTransaction transaction, Guid formInstanceId)
        {
            string sql = string.Format("DELETE FROM bwdf_FormItemInstances WHERE (FormInstanceId = '{0}')", formInstanceId);
            IBatisDbHelper.ExecuteNonQuery(transaction, CommandType.Text, sql);
        }

        /// <summary>
        /// 转移指定表单项实例数据到指定分表中.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="formInstanceId"></param>
        /// <param name="tableIndex"></param>
        /// <returns></returns>
        protected static bool MigrateData(IDbTransaction transaction, Guid formInstanceId, int tableIndex)
        {
            if (tableIndex <= 0)
                return false;
            string sql = @"INSERT INTO bwdf_FormItemInstances_{0} ([Id], FormItemDefinitionId, FormInstanceId, Value_Str, Value_Decimal, Value_Text)
                SELECT [Id], FormItemDefinitionId, FormInstanceId, Value_Str, Value_Decimal, Value_Text
                FROM bwdf_FormItemInstances
                WHERE (FormInstanceId = '{1}')";
            sql = string.Format(sql, tableIndex, formInstanceId);
            int rowAffected = IBatisDbHelper.ExecuteNonQuery(transaction, CommandType.Text, sql);
            return (rowAffected > 0);
        }
        #endregion
    }
}
