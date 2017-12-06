using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IBatisNet.DataMapper;
using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using System.Data.SqlClient;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.API.Util
{
    public class APIServiceSQLHelper
    {
        public static DataTable QueryForDataSet(string statementName, object paramObject)
        {
            DataSet ds = new DataSet();
            ISqlMapper mapper = IBatisMapper.Mapper;
            IMappedStatement statement = mapper.GetMappedStatement(statementName);
            if (!mapper.IsSessionStarted)
            {
                mapper.OpenConnection();
            }
            RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
            statement.PreparedCommand.Create(scope, mapper.LocalSession, statement.Statement, paramObject);
            //mapper.LocalSession.CreateDataAdapter(scope.IDbCommand).Fill(ds);//1.5版本

            IDbCommand command = mapper.LocalSession.CreateCommand(CommandType.Text);
            command.CommandText = scope.IDbCommand.CommandText;

            foreach (IDataParameter pa in scope.IDbCommand.Parameters)
            {
                command.Parameters.Add(new SqlParameter(pa.ParameterName, pa.Value));
            }

            mapper.LocalSession.CreateDataAdapter(command).Fill(ds);
            mapper.CloseConnection();
            return ds.Tables[0];
        }

       public static string GetSql(string statementName, object paramObject)
       {
           IBatisNet.DataMapper.ISqlMapper sqlMap = Mapper.Instance();
           ISqlMapper mapper = sqlMap;
           IMappedStatement statement = mapper.GetMappedStatement(statementName);
           if (!mapper.IsSessionStarted)
           {
               mapper.OpenConnection();
           }
           RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
           return scope.PreparedStatement.PreparedSql;
       }

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
       public static DataTable GetPagedList(string tableName, string fieldKey, int pageIndex, int pageSize, string fieldShow, string fieldOrder, string fieldGroup, string where, ref int recordCount)
       {
           //由从0开始的页码改为从1开始

           pageIndex++;

           IDbDataParameter[] paramSet = IBatisDbHelper.CreateParameterSet(8);

           paramSet[0].ParameterName = "@TableName";
           paramSet[0].Value = tableName;

           paramSet[1].ParameterName = "@PageIndex";
           paramSet[1].Value = pageIndex;
           paramSet[2].ParameterName = "@PageSize";
           paramSet[2].Value = pageSize;
           paramSet[3].ParameterName = "@GetFields";
           paramSet[3].Value = fieldShow;
           paramSet[5].ParameterName = "@OrderField";
           paramSet[5].Value = fieldOrder;
           paramSet[6].ParameterName = "@WhereCondition";
           paramSet[6].Value = where;
           paramSet[7].ParameterName = "@RecordCount";
           paramSet[7].Direction = ParameterDirection.InputOutput;
           paramSet[7].Value = recordCount;
           paramSet[4].ParameterName = "@GroupBy";
           paramSet[4].Value = fieldGroup;

           DataSet ds = IBatisDbHelper.ExecuteDataset(CommandType.StoredProcedure, "sp_PageList2005", paramSet);
           recordCount = Convert.ToInt32(paramSet[7].Value);

           return ds.Tables[0];
       }
    }
}
