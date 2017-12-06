﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using IBatisNet;
using IBatisNet.Common;
using IBatisNet.DataMapper;
using common = IBatisNet.Common;
using dataMapper = IBatisNet.DataMapper;

namespace Botwave.Extension.IBatisNet
{
    /// <summary>
    /// IBatisNet的数据访问帮助类.
    /// 主要用于不需要IBatisNet的映射的情况，如直接执行sql、返回DataSet等.
    /// 仿MS的SqlHelper，主要不同点在于借助IBatisNet的DataSource/DbProvider来创建IDbConnection、IDbCommand、IDbDataAdapter、IDbDataParameter.
    /// 而不是像SqlHelper一样通过SqlConnection、SqlCommand等来访问.
    /// </summary>
    public static class IBatisDbHelper
    {
        #region IBatisNet

        /// <summary>
        /// 数据库连接字符串, 只读.
        /// </summary>
        public static readonly string ConnectionString = GetConnectionString();

        /// <summary>
        /// 数据提供源.
        /// </summary>
        private static readonly common.IDbProvider dbProvider = dataMapper.Mapper.Instance().DataSource.DbProvider;

        /// <summary>
        /// 获取数据库连接.
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            dataMapper.ISqlMapper mapper = dataMapper.Mapper.Instance();
            string key = mapper.DataSource == null ? null : mapper.DataSource.ConnectionString;
            string value = ConfigurationManager.AppSettings[string.IsNullOrEmpty(key) ? "ConnectionString" : key];
            return (string.IsNullOrEmpty(value) ? key : value);
        }

        /// <summary>
        /// 创建数据库连接实例.
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            IDbConnection conn = dbProvider.CreateConnection();
            conn.ConnectionString = ConnectionString;
            return conn;
        }

        /// <summary>
        /// 创建数据操作命令接口.
        /// </summary>
        /// <returns></returns>
        public static IDbCommand CreateCommand()
        {
            return dbProvider.CreateCommand();
        }

        /// <summary>
        /// 创建数据适配器实例.
        /// </summary>
        /// <returns></returns>
        public static IDbDataAdapter CreateDataAdapter()
        {
            return dbProvider.CreateDataAdapter();
        }

        /// <summary>
        /// 创建数据参数实例.
        /// </summary>
        /// <returns></returns>
        public static IDbDataParameter CreateParameter()
        {
            return dbProvider.CreateDataParameter();
        }

        /// <summary>
        /// 创建数据参数集合.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IDbDataParameter[] CreateParameterSet(int size)
        {
            IDbDataParameter[] commandParameters = new IDbDataParameter[size];
            for (int i = 0; i < size; i++)
            {
                commandParameters[i] = CreateParameter();
            }
            return commandParameters;
        }

        #endregion

        #region helper functions

        /// <summary>
        /// 附件参数对象到指定 IDbCommand 对象.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        private static void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (IDbDataParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput
                            || p.Direction == ParameterDirection.Input) && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        //private static void AssignParameterValues(IDbDataParameter[] commandParameters, DataRow dataRow)
        //{
        //    if ((commandParameters == null) || (dataRow == null))
        //    {
        //        return;
        //    }

        //    int i = 0;
        //    foreach (IDbDataParameter commandParameter in commandParameters)
        //    {
        //        if (commandParameter.ParameterName == null
        //            || commandParameter.ParameterName.Length <= 1)
        //            throw new Exception(
        //                string.Format(
        //                "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
        //                i, commandParameter.ParameterName));
        //        if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
        //            commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
        //        i++;
        //    }
        //}

        /// <summary>
        /// 设置参数值.
        /// </summary>
        /// <param name="commandParameters"></param>
        /// <param name="parameterValues"></param>
        private static void AssignParameterValues(IDbDataParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return;
            }

            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        /// 设置命令对象.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="mustCloseConnection"></param>
        private static void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            command.Connection = connection;
            command.CommandType = commandType;
            command.CommandText = commandText;

            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行SQL, 并返回受影响的行数.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// 执行SQL, 并返回受影响的行数.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                IDbDataParameter[] commandParameters = CreateParameterSet(parameterValues.Length);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteNonQuery(CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteNonQuery(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行SQL, 并返回受影响的行数.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection connection = CreateConnection();
            return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行SQL, 并返回受影响的行数.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            IDbCommand cmd = CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }

        /// <summary>
        /// 执行SQL, 并返回受影响的行数.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            IDbCommand cmd = CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            int retval = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();
            return retval;
        }
        #endregion

        #region ExecuteScalar

        /// <summary>
        ///  执行查询，并返回查询所返回的结果集中第一行的第一列. 忽略其他列或行.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(CommandType commandType, string commandText)
        {
            return ExecuteScalar(commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列. 忽略其他列或行.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                IDbDataParameter[] commandParameters = CreateParameterSet(parameterValues.Length);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteScalar(CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteScalar(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列. 忽略其他列或行.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection connection = CreateConnection();
            return ExecuteScalar(connection, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列. 忽略其他列或行.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            IDbCommand cmd = CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        ///  执行查询，并生成一个 System.Data.IDataReader 对象.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return ExecuteReader(commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// 执行查询，并生成一个 System.Data.IDataReader 对象.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                IDbDataParameter[] commandParameters = CreateParameterSet(parameterValues.Length);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteReader(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行查询，并生成一个 System.Data.IDataReader 对象.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection connection = CreateConnection();
            return ExecuteReader(connection, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行查询，并生成一个 System.Data.IDataReader 对象.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;
            IDbCommand cmd = CreateCommand();
            try
            {
                PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

                IDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // Detach the SqlParameters from the command object, so they can be used again.
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader can磘 set its values. 
                // When this happen, the parameters can磘 be used again in other command.
                bool canClear = true;
                foreach (IDbDataParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        #endregion

        #region ExecuteDataset

        /// <summary>
        /// 执行查询，并返回结果数据集.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(CommandType commandType, string commandText)
        {
            return ExecuteDataset(commandType, commandText, (IDbDataParameter[])null);
        }

        /// <summary>
        /// 执行查询，并返回结果数据集.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string spName, params object[] parameterValues)
        {
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                IDbDataParameter[] commandParameters = CreateParameterSet(parameterValues.Length);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteDataset(CommandType.StoredProcedure, spName, commandParameters);
            }
            else
            {
                return ExecuteDataset(CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行查询，并返回结果数据集.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection connection = CreateConnection();
            return ExecuteDataset(connection, commandType, commandText, commandParameters);
        }

        /// <summary>
        /// 执行查询，并返回结果数据集.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            IDbCommand cmd = CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            IDbDataAdapter da = CreateDataAdapter();
            da.SelectCommand = cmd;

            DataSet ds = new DataSet();
            da.Fill(ds);

            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return ds;
        }

        #endregion

        #region FillDataset

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public static void FillDataset(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        public static void FillDataset(IDbConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="commandParameters"></param>
        public static void FillDataset(CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params IDbDataParameter[] commandParameters)
        {
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="parameterValues"></param>
        public static void FillDataset(string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="commandParameters"></param>
        public static void FillDataset(IDbConnection connection, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params IDbDataParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="spName"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="parameterValues"></param>
        public static void FillDataset(IDbConnection connection, string spName,
            DataSet dataSet, string[] tableNames,
            params object[] parameterValues)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (dataSet == null) throw new ArgumentNullException("dataSet");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                IDbDataParameter[] commandParameters = CreateParameterSet(parameterValues.Length);
                AssignParameterValues(commandParameters, parameterValues);
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集填充到指定数据集中.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="dataSet"></param>
        /// <param name="tableNames"></param>
        /// <param name="commandParameters"></param>
        public static void FillDataset(IDbConnection connection, IDbTransaction transaction, CommandType commandType,
            string commandText, DataSet dataSet, string[] tableNames,
            params IDbDataParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            IDbCommand command = CreateCommand();
            bool mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

            IDbDataAdapter dataAdapter = CreateDataAdapter();
            dataAdapter.SelectCommand = command;
            if (tableNames != null && tableNames.Length > 0)
            {
                string tableName = "Table";
                for (int index = 0; index < tableNames.Length; index++)
                {
                    if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");
                    dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                    tableName = "Table" + (index + 1).ToString();
                }
            }

            dataAdapter.Fill(dataSet);

            command.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();
        }

        #endregion

        #region 分页

        /// <summary>
        /// 获取已分页列表.
        /// </summary>
        /// <param name="tableName">要分页显示的表(视图)名.</param>
        /// <param name="fieldKey">于定位记录的主键(惟一键)字段,只能是单个字段.</param>
        /// <param name="pageIndex">要显示的页码.</param>
        /// <param name="pageSize">每页的大小(记录数).</param>
        /// <param name="fieldShow">以逗号分隔的要显示的字段列表,如果不指定,则显示所有字段.</param>
        /// <param name="fieldOrder">以逗号分隔的排序字段列表,可以指定在字段后面指定DESC/ASC 用于指定排序顺序.</param>
        /// <param name="where">查询条件.</param>
        /// <param name="recordCount">总记录数.</param>
        /// <returns></returns>
        public static DataTable GetPagedList(string tableName, string fieldKey, int pageIndex, int pageSize, string fieldShow, string fieldOrder, string where, ref int recordCount)
        {
            //由从0开始的页码改为从1开始

            pageIndex++;

            IDbDataParameter[] paramSet = IBatisDbHelper.CreateParameterSet(8);

            paramSet[0].ParameterName = "@TableName";
            paramSet[0].Value = tableName;
            paramSet[1].ParameterName = "@FieldKey";
            paramSet[1].Value = fieldKey;
            paramSet[2].ParameterName = "@PageCurrent";
            paramSet[2].Value = pageIndex;
            paramSet[3].ParameterName = "@PageSize";
            paramSet[3].Value = pageSize;
            paramSet[4].ParameterName = "@FieldShow";
            paramSet[4].Value = fieldShow;
            paramSet[5].ParameterName = "@FieldOrder";
            paramSet[5].Value = fieldOrder;
            paramSet[6].ParameterName = "@Where";
            paramSet[6].Value = where;
            paramSet[7].ParameterName = "@RecordCount";
            paramSet[7].Direction = ParameterDirection.InputOutput;
            paramSet[7].Value = recordCount;

            DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "spPageViewByStr", paramSet);
            recordCount = Convert.ToInt32(paramSet[7].Value);

            return ds.Tables[0];
        }

        #endregion
    }
}
