using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Botwave.Commons
{
    /// <summary>
    /// OleDb ���ݸ�����.
    /// </summary>
    public static class OleDbHelper
    {
        /// <summary>
        /// Microsoft Access �����ݿ������ַ�����ʽ.
        /// </summary>
        public const string ACCESS_CONNECTIONSTRING_TEMPLATE = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};";

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// ��� System.Data.OleDb.OleDbCommand.Connection ִ�� SQL ��䲢������Ӱ�������.
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.Open);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// ��� System.Data.OleDb.OleDbCommand.Connection ִ�� SQL ��䲢������Ӱ�������.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(OleDbConnection conn, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.AutoDetection);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// ��� System.Data.OleDb.OleDbCommand.Connection ִ�� SQL ��䲢������Ӱ�������.
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(OleDbTransaction trans, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, cmdParms, ConnectionActionType.None);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// �� System.Data.OleDb.OleDbCommand.CommandText ���͵� System.Data.OleDb.OleDbCommand.Connection ������һ�� System.Data.OleDb.OleDbDataReader.
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection conn = new OleDbConnection(connString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.Open);
                OleDbDataReader rdr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// �� System.Data.OleDb.OleDbCommand.CommandText ���͵� System.Data.OleDb.OleDbCommand.Connection ������һ�� System.Data.OleDb.OleDbDataReader.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static OleDbDataReader ExecuteReader(OleDbConnection conn, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.AutoDetection);
                OleDbDataReader rdr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����������л���.
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.Open);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С����������л���.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object ExecuteScalar(OleDbConnection conn, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.AutoDetection);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����ݼ�.
        /// </summary>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string connString, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.Open);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����ݼ�.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(OleDbConnection conn, CommandType cmdType, string cmdText, params OleDbParameter[] cmdParms)
        {
            OleDbCommand cmd = new OleDbCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms, ConnectionActionType.AutoDetection);
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }

        /// <summary>
        /// �����ѯ�� OleDb ��������.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="cmdParms"></param>
        public static void CacheParameters(string cacheKey, params OleDbParameter[] cmdParms)
        {
            parmCache[cacheKey] = cmdParms;
        }

        /// <summary>
        /// �ӻ����ȡָ���Ĳ�����������.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static OleDbParameter[] GetCachedParameters(string cacheKey)
        {
            OleDbParameter[] cachedParms = (OleDbParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            OleDbParameter[] clonedParms = new OleDbParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OleDbParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// ׼���������.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <param name="connActionType"></param>
        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, CommandType cmdType, string cmdText, OleDbParameter[] cmdParms, ConnectionActionType connActionType)
        {
            if (connActionType == ConnectionActionType.Open)
            {
                conn.Open();
            }
            else
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
            }            

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// ���Ӳ�������ö��.
        /// </summary>
        enum ConnectionActionType
        {
            None = 0,
            AutoDetection = 1,
            Open = 2
        }
    }
}
