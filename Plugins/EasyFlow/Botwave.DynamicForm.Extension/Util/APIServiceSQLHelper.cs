using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IBatis = IBatisNet.DataMapper;
using IBatisNet.Common;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using System.Data.SqlClient;
using Botwave.Extension.IBatisNet;
using System.Data.SqlClient;

namespace Botwave.DynamicForm.Extension.Util
{
    public class APIServiceSQLHelper
    {
        public static DataTable QueryForDataSet(string statementName, object paramObject)
        {
            DataSet ds = new DataSet();
            IBatis.ISqlMapper mapper = IBatisMapper.Mapper;
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
            IBatis.ISqlMapper sqlMap = IBatis.Mapper.Instance();
            IBatis.ISqlMapper mapper = sqlMap;
            IMappedStatement statement = mapper.GetMappedStatement(statementName);
            if (!mapper.IsSessionStarted)
            {
                mapper.OpenConnection();
            }
            RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
            return scope.PreparedStatement.PreparedSql;
        }
    }
}
