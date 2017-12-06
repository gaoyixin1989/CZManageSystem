using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace CZManageSystem.Admin.Base
{
    /// <summary>
    /// 数据库访问对象_短信功能。
    /// </summary>
    [Serializable()]
    public class SmsDAO
    {
        // mysql数据库
        public const int DATABASE_MYSQL = 0;
        // access数据库
        public const int DATABASE_ACCESS = 1;
        // sql server数据库
        public const int DATABASE_SQLSERVER = 2;
        // oracle数据库
        public const int DATABASE_ORACLE = 3;
        // 保存用户设置的数据库类型
        static public int DATABASE = 2;
        // 上次执行SQL语句时是否出错。
        static private int ERRCODE = 0;
        // 数据库连接字符串
        static private string CONNECT = "";
        // 上一条SQL语句返回的记录数或影响行数。
        static private int totalSize = 0;
        //以下是在对象中用到的变量
        // 数据库连接参数
        private string connect;
        // 保存上次建立数据库连接或执行sql语句时的出错代码。
        public int errcode = 0;
        // 保存用户设置的数据库类型
        public int database = 0;
        // 上一条SQL语句返回的记录数或影响行数。
        private int totalsize = 0;
        public SmsDAO()
        {
            connect = CONNECT;    //数据库服务器连接参数
            //设置数据库类型
            database = DATABASE;
        }
        /// <summary>
        /// 初始化数据库连接参数connect和数据库类型database
        /// </summary>
        /// <param name="Connect">数据库服务器连接参数</param>
        /// <param name="Database">设置数据库类型</param>
        public SmsDAO(string Connect, int Database)
        {
            connect = Connect;    //数据库服务器地址
            //设置数据库类型
            database = Database;
        }//end method
        /// <summary>
        /// 判断数据库对象是否初始化过
        /// </summary>
        /// <returns>如果初始化过，返回true，否则返回false</returns>
        static public bool isInited()
        {
            if (CONNECT == null || CONNECT == "") return false;
            return true;
        }//end method

        /// <summary>
        /// 返回建立数据库连接或执行sql语句是否出错。
        /// </summary>
        public bool isError()
        {
            return errcode != 0;
        }//end method

        /// <summary>
        /// 执行PreparedStatement SQL语句,sql影响行数写到totalsize。用conSize()可以读到。
        /// </summary>
        /// <param name="strSql"> SQL语句，其中的参数用?号表示。</param>
        /// <returns>
        /// 返回errcode (0:执行成功，非0值:执行错误)。
        /// </returns>
        public int conExec(string strSql)
        {
            return conExec_SQLSERVER(strSql);
        }

        /// <summary>
        /// SQLSERVER数据库：执行PreparedStatement SQL语句,sql影响行数写到totalsize。用conSize()可以读到。
        /// </summary>
        /// <param name="strSql"> SQL语句，其中的参数用?号表示。</param>
        /// <returns>
        /// 返回errcode (0:执行成功，非0值:执行错误)。
        /// </returns>
        private int conExec_SQLSERVER(string strSql)
        {
            errcode = 0;
            //如果是SQL Server数据库
            SqlConnection conn = null; //临时保存Connection变量和Adapter变量，用于关闭
            try
            {
                conn = new SqlConnection(connect);
                conn.Open();
                SqlCommand command = new SqlCommand(strSql, conn);
                totalsize = command.ExecuteNonQuery();
                if (command != null) { command.Dispose(); }
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
            catch (Exception e)
            {
                //释放Connection和Adapter资源
                if (conn != null) conn.Close();
                //eap.share.Log.save(e, "DAO.conExec方法出现错误(strSql=" + strSql + ")。");
                errcode = 1;
                return errcode;
            }
            return 0;
        }

        /// <summary>
        /// 返回上一条SQL语句返回的记录数或影响行数。
        /// </summary>
        /// <returns>
        /// 返回上一条SQL语句返回的记录数或影响行数。
        /// </returns>
        public int conSize()
        {
            //返回结果
            return totalsize;
        }//end method

        /// <summary>
        /// 执行SQL查询语句，返回(根据ResultSet.getRow()读到)查询到的总记录数。
        /// </summary>
        /// <param name="strSql"> SQL语句</param>
        /// <returns>
        /// 返回(根据ResultSet.getRow()读到)查询到的总记录数。
        /// </returns>
        public int conSize(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return -1;
            //返回结果
            return dataSet.Tables[0].Rows.Count;
        }//end method

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的第一条记录的第一个字段。
        /// </summary>
        /// <param name="strSql"> SQL语句</param>
        /// <returns>
        /// 返回查询结果的第一条记录的第一个字段。
        /// </returns>
        public string conValue(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows[0][0] == null) return null;
            //返回第一行的第一个字段
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </summary>
        /// <param name="strSql"> SQL语句</param>
        /// <returns>
        /// 返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </returns>
        public string conValueList(string strSql)
        {
            return conValueList(strSql, ",");
        }

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的所有记录的第一个字段,用delimiter分隔。
        /// </summary>
        /// <param name="strSql"> SQL语句</param>
        /// <param name="delimiter"> 返回结果的分隔符。</param>
        /// <returns>
        /// 返回查询结果的所有记录的第一个字段,用delimiter分隔。
        /// </returns>
        public string conValueList(string strSql, string delimiter)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows[0][0] == null) return null;
            if (dataSet.Tables[0].Rows.Count > 300)
            {
                StringBuilder myValue = new StringBuilder("");
                if (dataSet.Tables[0].Rows.Count > 0) myValue.Append(dataSet.Tables[0].Rows[0][0].ToString());
                for (int ii = 1; ii < dataSet.Tables[0].Rows.Count; ii++)
                {
                    myValue.Append(delimiter).Append(dataSet.Tables[0].Rows[ii][0].ToString());
                }
                return myValue.ToString();
            }
            else
            {
                string myValue = "";
                if (dataSet.Tables[0].Rows.Count > 0) myValue = dataSet.Tables[0].Rows[0][0].ToString();
                for (int ii = 1; ii < dataSet.Tables[0].Rows.Count; ii++)
                {
                    myValue = myValue + delimiter + dataSet.Tables[0].Rows[ii][0].ToString();
                }
                return myValue;
            }
        }

        /// <summary>
        /// 执行SQL查询语句，返回第一条记录的DataRow结果。
        /// </summary>
        /// <param name="strSql"> SQL语句。</param>
        /// <returns>
        /// 返回第一条记录的DataRow结果。
        /// </returns>
        public DataRow conRow(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null) return null;
            //返回第一行的第一个字段
            return dataSet.Tables[0].Rows[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataRowCollection集合。
        /// </summary>
        /// <param name="strSql"> SQL语句。</param>
        /// <returns>
        /// 返回记录的DataRowCollection集合。
        /// </returns>
        public DataRowCollection conList(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //返回结果
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataRowCollection集合。
        /// </summary>
        /// <param name="strSql"> SQL语句。</param>
        /// <param name="idx"> 起始记录idx(0..),最小值是0而不是1。</param>
        /// <param name="count"> 要读取的记录数，最小值是0表示不限制记录数。</param>
        /// <returns>
        /// 返回记录的DataRowCollection集合。
        /// </returns>
        public DataRowCollection conList(string strSql, int idx, int count)
        {
            DataSet dataSet = conDataSet(strSql, idx, count);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //返回结果
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="strSql"> SQL语句。</param>
        /// <returns>
        /// 返回记录的DataTable集合。
        /// </returns>
        public DataTable conTable(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //返回结果
            return dataSet.Tables[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx(0..),最小值是0而不是1。</param>
        /// <param name="count"> 要读取的记录数，最小值是0表示不限制记录数。</param>
        /// <returns>
        /// 返回记录的DataTable集合。
        /// </returns>
        public DataTable conTable(string strSql, int idx, int count)
        {
            DataSet dataSet = conDataSet(strSql, idx, count);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //返回结果
            return dataSet.Tables[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataSet集合。放在缺省表名中。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <returns>
        /// 返回记录的DataSet集合。
        /// </returns>
        public DataSet conDataSet(string strSql)
        {
            return conDataSet(strSql, 0, 0);
        }

        /// <summary>
        /// 执行SQL查询语句，起始记录idx，以及读取记录数count，返回记录的DataSet集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx(0..),最小值是0而不是1。</param>
        /// <param name="count"> 要读取的记录数，最小值是0表示不限制记录数。</param>
        /// <returns>
        /// 返回记录的DataSet集合。
        /// </returns>
        public DataSet conDataSet(string strSql, int idx, int count) //,out object objError)
        {
            DataSet dataSet = null;
            dataSet = conDataSet_SQLSERVER(strSql, idx, count);
            if (dataSet != null) totalSize = dataSet.Tables[0].Rows.Count;
            return dataSet;
        }

        /// <summary>
        /// SQLSERVER数据库：执行SQL查询语句，起始记录idx，以及读取记录数count，返回记录的DataSet集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx(0..),最小值是0而不是1。</param>
        /// <param name="count"> 要读取的记录数，最小值是0表示不限制记录数。</param>
        /// <returns>
        /// 返回记录的DataSet集合。
        /// </returns>
        private DataSet conDataSet_SQLSERVER(string strSql, int idx, int count) //,out object objError)
        {
            SqlConnection conn = null; SqlDataAdapter adapter = null; //临时保存Connection变量和Adapter变量，用于关闭
            errcode = 0;
            try
            {
                //如果是SQL Server数据库
                conn = new SqlConnection(connect);
                conn.Open();
                adapter = new SqlDataAdapter(strSql, conn);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataSet dataSet = new DataSet();
                if (idx <= 0 && count <= 0) adapter.Fill(dataSet);
                else adapter.Fill(dataSet, idx, count, "main");

                if (conn != null) conn.Close();
                if (adapter != null) adapter.Dispose();

                return dataSet;
            }
            catch (Exception e)
            {
                //释放Connection和Adapter资源
                if (conn != null) conn.Close();
                if (adapter != null) adapter.Dispose();
               // eap.share.Log.save(e, "DAO.conDataSet方法出现错误(strSql=" + strSql + ")。");
                errcode = 1;
            }
            finally { }
            return null;
        }

        ///初始化数据库连接参数connect和数据库类型database
        static public void init(string Connect)
        {
            CONNECT = Connect;
            if (Connect.IndexOf("Jet OLEDB") >= 0)
            {
                DATABASE = 1;
            }
            else if (Connect.ToLower().IndexOf(".mdb") >= 0)
            {
                DATABASE = 1;
            }
            else if (Connect.IndexOf("Initial Catalog") >= 0)
            {
                DATABASE = 2;
            }
            else
            {
                DATABASE = 2;
            }
        }

        //初始化数据库连接参数connect和数据库类型database
        static public void init(string Connect, int Database)
        {
            CONNECT = Connect;
            DATABASE = Database;
        }
        /// <summary>
        /// 执行PreparedStatement SQL语句,sql影响行数写到totalSize。用sqlSize()可以读到。
        /// </summary>
        /// <param name="strSql">SQL语句，其中的参数用?号表示。</param>
        /// <returns>
        /// 返回ERRCODE (0:执行成功，非0值:执行错误)。
        /// </returns>
        static public int sqlExec(string strSql)
        {
            return sqlExec_SQLSERVER(strSql);
        }

        /// <summary>
        /// SQLSERVER数据库：执行PreparedStatement SQL语句,sql影响行数写到totalSize。用sqlSize()可以读到。
        /// </summary>
        /// <param name="strSql">SQL语句，其中的参数用?号表示。</param>
        /// <returns>
        /// 返回ERRCODE (0:执行成功，非0值:执行错误)。
        /// </returns>
        static private int sqlExec_SQLSERVER(string strSql)
        {
            ERRCODE = 0;
            if (strSql != "" && strSql != null)
            {
                SqlConnection conn = null; //临时保存Connection变量和Adapter变量，用于关闭
                try
                {
                    //如果是SQL Server数据库
                    conn = new SqlConnection(CONNECT);
                    conn.Open();
                    SqlCommand command = new SqlCommand(strSql, conn);
                    totalSize = command.ExecuteNonQuery();

                    if (command != null) { command.Dispose(); }
                    if (conn.State == ConnectionState.Open) { conn.Close(); }

                }
                catch (Exception e)
                {
                    //释放Connection和Adapter资源
                    if (conn != null) conn.Close();
                   // eap.share.Log.save(e, "DAO.sqlExec方法出现错误(strSql=" + strSql + ")。");
                    ERRCODE = 1;
                    return ERRCODE;
                }
            }
            return 0;
        }

        /// <summary>
        /// 返回上一条SQL语句返回的记录数或影响行数。
        /// </summary>
        /// <returns>
        /// 返回上一条SQL语句返回的记录数或影响行数。
        /// </returns>
        public int sqlSize()
        {
            //返回结果
            return totalSize;
        }//end method

        /// <summary>
        /// 执行SQL查询语句，返回(根据ResultSet.getRow()读到)查询到的总记录数。
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>
        /// 返回(根据ResultSet.getRow()读到)查询到的总记录数。
        /// </returns>
        static public int sqlSize(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return -1;
            //返回结果
            return dataSet.Tables[0].Rows.Count;
        }//end method

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的第一条记录的第一个字段。如果返回值为null,不一定表示ResultSet为空或出错，有可能字段值为null。
        /// ResultSet为null、为空、出错时返回null(而不是空字符串)。
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>
        /// 返回查询结果的第一条记录的第一个字段。
        /// </returns>
        static public string sqlValue(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows[0][0] == null) return null;
            //返回第一行的第一个字段
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns>
        /// 返回查询结果的所有记录的第一个字段,用逗号分隔。
        /// </returns>
        static public string sqlValueList(string strSql)
        {
            return sqlValueList(strSql, ",");
        }

        /// <summary>
        /// 执行SQL查询语句，返回查询结果的所有记录的第一个字段,用delimiter分隔。
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="delimiter"> 返回结果的分隔符。</param>
        /// <returns>
        /// 返回查询结果的所有记录的第一个字段,用delimiter分隔。
        /// </returns>
        static public string sqlValueList(string strSql, string delimiter)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows[0][0] == null) return null;
            if (dataSet.Tables[0].Rows.Count > 300)
            {
                StringBuilder myValue = new StringBuilder("");
                if (dataSet.Tables[0].Rows.Count > 0) myValue.Append(dataSet.Tables[0].Rows[0][0].ToString());
                for (int ii = 1; ii < dataSet.Tables[0].Rows.Count; ii++)
                {
                    myValue.Append(delimiter).Append(dataSet.Tables[0].Rows[ii][0].ToString());
                }
                return myValue.ToString();
            }
            else
            {
                string myValue = "";
                if (dataSet.Tables[0].Rows.Count > 0) myValue = dataSet.Tables[0].Rows[0][0].ToString();
                for (int ii = 1; ii < dataSet.Tables[0].Rows.Count; ii++)
                {
                    myValue = myValue + delimiter + dataSet.Tables[0].Rows[ii][0].ToString();
                }
                return myValue;
            }
        }

        /// <summary>
        /// 执行SQL查询语句，返回第一条记录的DataRow结果。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <returns>
        /// 返回第一条记录的DataRow结果。
        /// </returns>
        static public DataRow sqlRow(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null) return null;
            return dataSet.Tables[0].Rows[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataRowCollection集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <returns>
        /// 返回记录的DataRowCollection集合。
        /// </returns>
        static public DataRowCollection sqlList(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //返回结果
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataRowCollection集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx</param>
        /// <param name="count"> 要读取的记录数</param>
        /// <returns>
        /// 返回记录的DataRowCollection集合。
        /// </returns>
        static public DataRowCollection sqlList(string strSql, int idx, int count)
        {
            DataSet dataSet = sqlDataSet(strSql, idx, count);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //返回结果
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <returns>
        /// 返回记录的DataTable集合。
        /// </returns>
        static public DataTable sqlTable(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //返回结果
            return dataSet.Tables[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataTable集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx</param>
        /// <param name="count"> 要读取的记录数</param>
        /// <returns>
        /// 返回记录的DataTable集合。
        /// </returns>
        static public DataTable sqlTable(string strSql, int idx, int count)
        {
            DataSet dataSet = sqlDataSet(strSql, idx, count);
            //如果出错，或结果为null，则返回null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //返回结果
            return dataSet.Tables[0];
        }

        /// <summary>
        /// 执行SQL查询语句，返回所有记录的DataSet集合。放在缺省表名中。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <returns>
        /// 返回记录的DataSet集合。
        /// </returns>
        static public DataSet sqlDataSet(string strSql)
        {
            return sqlDataSet(strSql, 0, 0);
        }

        /// <summary>
        /// 执行SQL查询语句，起始记录idx(0..)，以及读取记录数count，返回记录的DataSet集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx</param>
        /// <param name="count"> 要读取的记录数</param>
        /// <returns>
        /// 返回记录的DataSet集合。
        /// </returns>
        static public DataSet sqlDataSet(string strSql, int idx, int count) //,out object objError)
        {
            DataSet dataSet = null;
            dataSet = sqlDataSet_SQLSERVER(strSql, idx, count);
            return dataSet;
        }//end method

        /// <summary>
        /// SQLSERVER数据库：执行SQL查询语句，起始记录idx(0..)，以及读取记录数count，返回记录的DataSet集合。
        /// </summary>
        /// <param name="strSql">SQL语句。</param>
        /// <param name="idx"> 起始记录idx</param>
        /// <param name="count"> 要读取的记录数</param>
        /// <returns>
        /// 返回记录的DataSet集合。
        /// </returns>
        static private DataSet sqlDataSet_SQLSERVER(string strSql, int idx, int count) //,out object objError)
        {
            ERRCODE = 0;
            if (strSql != "" && strSql != null)
            {
                SqlConnection conn = null;
                SqlDataAdapter adapter = null;
                try
                {
                    conn = new SqlConnection(CONNECT);
                    conn.Open();
                    adapter = new SqlDataAdapter(strSql, conn);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    //adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataSet dataSet = new DataSet();
                    if (idx <= 0 && count <= 0) adapter.Fill(dataSet);
                    else adapter.Fill(dataSet, idx, count, "main");

                    if (conn != null) conn.Close();
                    if (adapter != null) adapter.Dispose();

                    return dataSet;
                }
                catch (Exception e)
                {
                    if (conn != null) conn.Close();
                    if (adapter != null) adapter.Dispose();
                    //eap.share.Log.save(e, "DAO.sqlDataSet方法出现错误(strSql=" + strSql + ")。");
                    ERRCODE = 1;
                }
            }
            return null;
        }//end method

    }//end class
}
