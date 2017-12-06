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
    /// ���ݿ���ʶ���_���Ź��ܡ�
    /// </summary>
    [Serializable()]
    public class SmsDAO
    {
        // mysql���ݿ�
        public const int DATABASE_MYSQL = 0;
        // access���ݿ�
        public const int DATABASE_ACCESS = 1;
        // sql server���ݿ�
        public const int DATABASE_SQLSERVER = 2;
        // oracle���ݿ�
        public const int DATABASE_ORACLE = 3;
        // �����û����õ����ݿ�����
        static public int DATABASE = 2;
        // �ϴ�ִ��SQL���ʱ�Ƿ����
        static private int ERRCODE = 0;
        // ���ݿ������ַ���
        static private string CONNECT = "";
        // ��һ��SQL��䷵�صļ�¼����Ӱ��������
        static private int totalSize = 0;
        //�������ڶ������õ��ı���
        // ���ݿ����Ӳ���
        private string connect;
        // �����ϴν������ݿ����ӻ�ִ��sql���ʱ�ĳ�����롣
        public int errcode = 0;
        // �����û����õ����ݿ�����
        public int database = 0;
        // ��һ��SQL��䷵�صļ�¼����Ӱ��������
        private int totalsize = 0;
        public SmsDAO()
        {
            connect = CONNECT;    //���ݿ���������Ӳ���
            //�������ݿ�����
            database = DATABASE;
        }
        /// <summary>
        /// ��ʼ�����ݿ����Ӳ���connect�����ݿ�����database
        /// </summary>
        /// <param name="Connect">���ݿ���������Ӳ���</param>
        /// <param name="Database">�������ݿ�����</param>
        public SmsDAO(string Connect, int Database)
        {
            connect = Connect;    //���ݿ��������ַ
            //�������ݿ�����
            database = Database;
        }//end method
        /// <summary>
        /// �ж����ݿ�����Ƿ��ʼ����
        /// </summary>
        /// <returns>�����ʼ����������true�����򷵻�false</returns>
        static public bool isInited()
        {
            if (CONNECT == null || CONNECT == "") return false;
            return true;
        }//end method

        /// <summary>
        /// ���ؽ������ݿ����ӻ�ִ��sql����Ƿ����
        /// </summary>
        public bool isError()
        {
            return errcode != 0;
        }//end method

        /// <summary>
        /// ִ��PreparedStatement SQL���,sqlӰ������д��totalsize����conSize()���Զ�����
        /// </summary>
        /// <param name="strSql"> SQL��䣬���еĲ�����?�ű�ʾ��</param>
        /// <returns>
        /// ����errcode (0:ִ�гɹ�����0ֵ:ִ�д���)��
        /// </returns>
        public int conExec(string strSql)
        {
            return conExec_SQLSERVER(strSql);
        }

        /// <summary>
        /// SQLSERVER���ݿ⣺ִ��PreparedStatement SQL���,sqlӰ������д��totalsize����conSize()���Զ�����
        /// </summary>
        /// <param name="strSql"> SQL��䣬���еĲ�����?�ű�ʾ��</param>
        /// <returns>
        /// ����errcode (0:ִ�гɹ�����0ֵ:ִ�д���)��
        /// </returns>
        private int conExec_SQLSERVER(string strSql)
        {
            errcode = 0;
            //�����SQL Server���ݿ�
            SqlConnection conn = null; //��ʱ����Connection������Adapter���������ڹر�
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
                //�ͷ�Connection��Adapter��Դ
                if (conn != null) conn.Close();
                //eap.share.Log.save(e, "DAO.conExec�������ִ���(strSql=" + strSql + ")��");
                errcode = 1;
                return errcode;
            }
            return 0;
        }

        /// <summary>
        /// ������һ��SQL��䷵�صļ�¼����Ӱ��������
        /// </summary>
        /// <returns>
        /// ������һ��SQL��䷵�صļ�¼����Ӱ��������
        /// </returns>
        public int conSize()
        {
            //���ؽ��
            return totalsize;
        }//end method

        /// <summary>
        /// ִ��SQL��ѯ��䣬����(����ResultSet.getRow()����)��ѯ�����ܼ�¼����
        /// </summary>
        /// <param name="strSql"> SQL���</param>
        /// <returns>
        /// ����(����ResultSet.getRow()����)��ѯ�����ܼ�¼����
        /// </returns>
        public int conSize(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return -1;
            //���ؽ��
            return dataSet.Tables[0].Rows.Count;
        }//end method

        /// <summary>
        /// ִ��SQL��ѯ��䣬���ز�ѯ����ĵ�һ����¼�ĵ�һ���ֶΡ�
        /// </summary>
        /// <param name="strSql"> SQL���</param>
        /// <returns>
        /// ���ز�ѯ����ĵ�һ����¼�ĵ�һ���ֶΡ�
        /// </returns>
        public string conValue(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows[0][0] == null) return null;
            //���ص�һ�еĵ�һ���ֶ�
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬���ز�ѯ��������м�¼�ĵ�һ���ֶ�,�ö��ŷָ���
        /// </summary>
        /// <param name="strSql"> SQL���</param>
        /// <returns>
        /// ���ز�ѯ��������м�¼�ĵ�һ���ֶ�,�ö��ŷָ���
        /// </returns>
        public string conValueList(string strSql)
        {
            return conValueList(strSql, ",");
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬���ز�ѯ��������м�¼�ĵ�һ���ֶ�,��delimiter�ָ���
        /// </summary>
        /// <param name="strSql"> SQL���</param>
        /// <param name="delimiter"> ���ؽ���ķָ�����</param>
        /// <returns>
        /// ���ز�ѯ��������м�¼�ĵ�һ���ֶ�,��delimiter�ָ���
        /// </returns>
        public string conValueList(string strSql, string delimiter)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
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
        /// ִ��SQL��ѯ��䣬���ص�һ����¼��DataRow�����
        /// </summary>
        /// <param name="strSql"> SQL��䡣</param>
        /// <returns>
        /// ���ص�һ����¼��DataRow�����
        /// </returns>
        public DataRow conRow(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null) return null;
            //���ص�һ�еĵ�һ���ֶ�
            return dataSet.Tables[0].Rows[0];
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataRowCollection���ϡ�
        /// </summary>
        /// <param name="strSql"> SQL��䡣</param>
        /// <returns>
        /// ���ؼ�¼��DataRowCollection���ϡ�
        /// </returns>
        public DataRowCollection conList(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //���ؽ��
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataRowCollection���ϡ�
        /// </summary>
        /// <param name="strSql"> SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx(0..),��Сֵ��0������1��</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼������Сֵ��0��ʾ�����Ƽ�¼����</param>
        /// <returns>
        /// ���ؼ�¼��DataRowCollection���ϡ�
        /// </returns>
        public DataRowCollection conList(string strSql, int idx, int count)
        {
            DataSet dataSet = conDataSet(strSql, idx, count);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //���ؽ��
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataTable���ϡ�
        /// </summary>
        /// <param name="strSql"> SQL��䡣</param>
        /// <returns>
        /// ���ؼ�¼��DataTable���ϡ�
        /// </returns>
        public DataTable conTable(string strSql)
        {
            DataSet dataSet = conDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //���ؽ��
            return dataSet.Tables[0];
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataTable���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx(0..),��Сֵ��0������1��</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼������Сֵ��0��ʾ�����Ƽ�¼����</param>
        /// <returns>
        /// ���ؼ�¼��DataTable���ϡ�
        /// </returns>
        public DataTable conTable(string strSql, int idx, int count)
        {
            DataSet dataSet = conDataSet(strSql, idx, count);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //���ؽ��
            return dataSet.Tables[0];
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataSet���ϡ�����ȱʡ�����С�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <returns>
        /// ���ؼ�¼��DataSet���ϡ�
        /// </returns>
        public DataSet conDataSet(string strSql)
        {
            return conDataSet(strSql, 0, 0);
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬��ʼ��¼idx���Լ���ȡ��¼��count�����ؼ�¼��DataSet���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx(0..),��Сֵ��0������1��</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼������Сֵ��0��ʾ�����Ƽ�¼����</param>
        /// <returns>
        /// ���ؼ�¼��DataSet���ϡ�
        /// </returns>
        public DataSet conDataSet(string strSql, int idx, int count) //,out object objError)
        {
            DataSet dataSet = null;
            dataSet = conDataSet_SQLSERVER(strSql, idx, count);
            if (dataSet != null) totalSize = dataSet.Tables[0].Rows.Count;
            return dataSet;
        }

        /// <summary>
        /// SQLSERVER���ݿ⣺ִ��SQL��ѯ��䣬��ʼ��¼idx���Լ���ȡ��¼��count�����ؼ�¼��DataSet���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx(0..),��Сֵ��0������1��</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼������Сֵ��0��ʾ�����Ƽ�¼����</param>
        /// <returns>
        /// ���ؼ�¼��DataSet���ϡ�
        /// </returns>
        private DataSet conDataSet_SQLSERVER(string strSql, int idx, int count) //,out object objError)
        {
            SqlConnection conn = null; SqlDataAdapter adapter = null; //��ʱ����Connection������Adapter���������ڹر�
            errcode = 0;
            try
            {
                //�����SQL Server���ݿ�
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
                //�ͷ�Connection��Adapter��Դ
                if (conn != null) conn.Close();
                if (adapter != null) adapter.Dispose();
               // eap.share.Log.save(e, "DAO.conDataSet�������ִ���(strSql=" + strSql + ")��");
                errcode = 1;
            }
            finally { }
            return null;
        }

        ///��ʼ�����ݿ����Ӳ���connect�����ݿ�����database
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

        //��ʼ�����ݿ����Ӳ���connect�����ݿ�����database
        static public void init(string Connect, int Database)
        {
            CONNECT = Connect;
            DATABASE = Database;
        }
        /// <summary>
        /// ִ��PreparedStatement SQL���,sqlӰ������д��totalSize����sqlSize()���Զ�����
        /// </summary>
        /// <param name="strSql">SQL��䣬���еĲ�����?�ű�ʾ��</param>
        /// <returns>
        /// ����ERRCODE (0:ִ�гɹ�����0ֵ:ִ�д���)��
        /// </returns>
        static public int sqlExec(string strSql)
        {
            return sqlExec_SQLSERVER(strSql);
        }

        /// <summary>
        /// SQLSERVER���ݿ⣺ִ��PreparedStatement SQL���,sqlӰ������д��totalSize����sqlSize()���Զ�����
        /// </summary>
        /// <param name="strSql">SQL��䣬���еĲ�����?�ű�ʾ��</param>
        /// <returns>
        /// ����ERRCODE (0:ִ�гɹ�����0ֵ:ִ�д���)��
        /// </returns>
        static private int sqlExec_SQLSERVER(string strSql)
        {
            ERRCODE = 0;
            if (strSql != "" && strSql != null)
            {
                SqlConnection conn = null; //��ʱ����Connection������Adapter���������ڹر�
                try
                {
                    //�����SQL Server���ݿ�
                    conn = new SqlConnection(CONNECT);
                    conn.Open();
                    SqlCommand command = new SqlCommand(strSql, conn);
                    totalSize = command.ExecuteNonQuery();

                    if (command != null) { command.Dispose(); }
                    if (conn.State == ConnectionState.Open) { conn.Close(); }

                }
                catch (Exception e)
                {
                    //�ͷ�Connection��Adapter��Դ
                    if (conn != null) conn.Close();
                   // eap.share.Log.save(e, "DAO.sqlExec�������ִ���(strSql=" + strSql + ")��");
                    ERRCODE = 1;
                    return ERRCODE;
                }
            }
            return 0;
        }

        /// <summary>
        /// ������һ��SQL��䷵�صļ�¼����Ӱ��������
        /// </summary>
        /// <returns>
        /// ������һ��SQL��䷵�صļ�¼����Ӱ��������
        /// </returns>
        public int sqlSize()
        {
            //���ؽ��
            return totalSize;
        }//end method

        /// <summary>
        /// ִ��SQL��ѯ��䣬����(����ResultSet.getRow()����)��ѯ�����ܼ�¼����
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <returns>
        /// ����(����ResultSet.getRow()����)��ѯ�����ܼ�¼����
        /// </returns>
        static public int sqlSize(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return -1;
            //���ؽ��
            return dataSet.Tables[0].Rows.Count;
        }//end method

        /// <summary>
        /// ִ��SQL��ѯ��䣬���ز�ѯ����ĵ�һ����¼�ĵ�һ���ֶΡ��������ֵΪnull,��һ����ʾResultSetΪ�ջ�����п����ֶ�ֵΪnull��
        /// ResultSetΪnull��Ϊ�ա�����ʱ����null(�����ǿ��ַ���)��
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <returns>
        /// ���ز�ѯ����ĵ�һ����¼�ĵ�һ���ֶΡ�
        /// </returns>
        static public string sqlValue(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null || dataSet.Tables[0].Rows[0][0] == null) return null;
            //���ص�һ�еĵ�һ���ֶ�
            return dataSet.Tables[0].Rows[0][0].ToString();
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬���ز�ѯ��������м�¼�ĵ�һ���ֶ�,�ö��ŷָ���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <returns>
        /// ���ز�ѯ��������м�¼�ĵ�һ���ֶ�,�ö��ŷָ���
        /// </returns>
        static public string sqlValueList(string strSql)
        {
            return sqlValueList(strSql, ",");
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬���ز�ѯ��������м�¼�ĵ�һ���ֶ�,��delimiter�ָ���
        /// </summary>
        /// <param name="strSql">SQL���</param>
        /// <param name="delimiter"> ���ؽ���ķָ�����</param>
        /// <returns>
        /// ���ز�ѯ��������м�¼�ĵ�һ���ֶ�,��delimiter�ָ���
        /// </returns>
        static public string sqlValueList(string strSql, string delimiter)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
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
        /// ִ��SQL��ѯ��䣬���ص�һ����¼��DataRow�����
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <returns>
        /// ���ص�һ����¼��DataRow�����
        /// </returns>
        static public DataRow sqlRow(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0 || dataSet.Tables[0].Rows[0] == null) return null;
            return dataSet.Tables[0].Rows[0];
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataRowCollection���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <returns>
        /// ���ؼ�¼��DataRowCollection���ϡ�
        /// </returns>
        static public DataRowCollection sqlList(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //���ؽ��
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataRowCollection���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼��</param>
        /// <returns>
        /// ���ؼ�¼��DataRowCollection���ϡ�
        /// </returns>
        static public DataRowCollection sqlList(string strSql, int idx, int count)
        {
            DataSet dataSet = sqlDataSet(strSql, idx, count);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null || dataSet.Tables[0].Rows.Count == 0) return null;
            //���ؽ��
            return dataSet.Tables[0].Rows;
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataTable���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <returns>
        /// ���ؼ�¼��DataTable���ϡ�
        /// </returns>
        static public DataTable sqlTable(string strSql)
        {
            DataSet dataSet = sqlDataSet(strSql, 0, 0);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //���ؽ��
            return dataSet.Tables[0];
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataTable���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼��</param>
        /// <returns>
        /// ���ؼ�¼��DataTable���ϡ�
        /// </returns>
        static public DataTable sqlTable(string strSql, int idx, int count)
        {
            DataSet dataSet = sqlDataSet(strSql, idx, count);
            //�����������Ϊnull���򷵻�null
            if (dataSet == null || dataSet.Tables == null || dataSet.Tables.Count == 0 || dataSet.Tables[0] == null) return null;
            //���ؽ��
            return dataSet.Tables[0];
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬�������м�¼��DataSet���ϡ�����ȱʡ�����С�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <returns>
        /// ���ؼ�¼��DataSet���ϡ�
        /// </returns>
        static public DataSet sqlDataSet(string strSql)
        {
            return sqlDataSet(strSql, 0, 0);
        }

        /// <summary>
        /// ִ��SQL��ѯ��䣬��ʼ��¼idx(0..)���Լ���ȡ��¼��count�����ؼ�¼��DataSet���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼��</param>
        /// <returns>
        /// ���ؼ�¼��DataSet���ϡ�
        /// </returns>
        static public DataSet sqlDataSet(string strSql, int idx, int count) //,out object objError)
        {
            DataSet dataSet = null;
            dataSet = sqlDataSet_SQLSERVER(strSql, idx, count);
            return dataSet;
        }//end method

        /// <summary>
        /// SQLSERVER���ݿ⣺ִ��SQL��ѯ��䣬��ʼ��¼idx(0..)���Լ���ȡ��¼��count�����ؼ�¼��DataSet���ϡ�
        /// </summary>
        /// <param name="strSql">SQL��䡣</param>
        /// <param name="idx"> ��ʼ��¼idx</param>
        /// <param name="count"> Ҫ��ȡ�ļ�¼��</param>
        /// <returns>
        /// ���ؼ�¼��DataSet���ϡ�
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
                    //eap.share.Log.save(e, "DAO.sqlDataSet�������ִ���(strSql=" + strSql + ")��");
                    ERRCODE = 1;
                }
            }
            return null;
        }//end method

    }//end class
}
