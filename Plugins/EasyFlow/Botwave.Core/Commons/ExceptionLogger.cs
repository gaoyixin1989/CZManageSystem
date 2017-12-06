using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using log4net;
using Botwave.Commons;
using System.Data.SqlClient;
using System.Data;
using System.Web.Caching;
using System.Collections;
using System.Configuration;
using Botwave.Configuration;

namespace Botwave.Commons
{
    /// <summary>
    /// �쳣��־����.
    /// </summary>
    public class ExceptionLogger
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ExceptionLogger));
        private const string m_CmdText = "INSERT INTO bw_ExceptionLog (Message,[Describe],ClientIP,ServerIP,PageURL,ExceptionTime,ExceptionContent,StackTrace) VALUES (@Message,@Describe,@ClientIP,@ServerIP,@PageURL,@ExceptionTime,@ExceptionContent,@StackTrace) if @@Error<>0 SELECT 0 else SELECT @@IDENTITY";

        /// <summary>
        /// �쳣����ϸ����.
        /// </summary>
        public static string ExceptionLogBody
        {
            get
            {
                if (HttpContext.Current.Session["BOTWAVE_EXCEPTIONBODY"] == null)
                    return "";
                else
                    return HttpContext.Current.Session["BOTWAVE_EXCEPTIONBODY"].ToString();
            }
            set
            {
                HttpContext.Current.Session["BOTWAVE_EXCEPTIONBODY"] = value.Replace("\r\n", "");
            }
        }

        /// <summary>
        /// ִ���쳣�����߼�.
        /// </summary>
        /// <param name="describe">�Ե�ǰ�쳣������</param>
        /// <returns>������ϸ�ĳ�����ʾ����</returns>
        public static string Log(string describe)
        {
            Exception ex = HttpContext.Current.Server.GetLastError();
            HttpContext.Current.Server.ClearError();
            if (ex == null)
                return "ϵͳ��æ�������ԡ�";

            string msg = "";
            try
            {
                if (ex.InnerException != null && ex.InnerException is ApplicationException)
                {
                    msg = ex.InnerException.Message;
                }
                else
                {
                    string s = GetExceptionType(ex);
                    int exID = SaveException2DB(describe, s, ex);
                    msg = string.Format("{0}����������ֹ��������{1}�������ԣ�������������ϵϵͳ����Ա��", s, exID);
                }
                string exMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                string exStack = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;
                ExceptionLogBody = string.Format("{0} Time��{1}��PageURL��{2}��ExceptionMessage��{3}��ExceptionStackTrace��{4}��", describe, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), HttpContext.Current.Request.Url, exMsg, exStack);

            }
            catch
            {
                msg = "���ڷ������ݿ�ʧ�ܣ���������ֹ�������ԣ�������������ϵϵͳ����Ա��";
                log.Error(msg, ex);
            }
            return msg;
        }

        /// <summary>
        /// ���浽���ݿ�.
        /// </summary>
        private static int SaveException2DB(string describe, string msg, Exception ex)
        {
            SqlParameter[] pa = new SqlParameter[8];
            pa[0] = new SqlParameter("@Message", SqlDbType.VarChar, 500);
            pa[1] = new SqlParameter("@Describe", SqlDbType.VarChar, 500);
            pa[2] = new SqlParameter("@ClientIP", SqlDbType.VarChar, 32);
            pa[3] = new SqlParameter("@ServerIP", SqlDbType.VarChar, 32);
            pa[4] = new SqlParameter("@PageURL", SqlDbType.VarChar, 200);
            pa[5] = new SqlParameter("@ExceptionTime", SqlDbType.DateTime, 8);
            pa[6] = new SqlParameter("@ExceptionContent", SqlDbType.NVarChar, 1000);
            pa[7] = new SqlParameter("@StackTrace", SqlDbType.NText);

            pa[0].Value = msg;
            pa[1].Value = describe;
            pa[2].Value = HttpContext.Current.Request.UserHostAddress;
            pa[3].Value = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            pa[4].Value = HttpContext.Current.Request.Url.ToString();
            pa[5].Value = DateTime.Now;

            string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            if (null != message && message.Length >= 990)
            {
                message = message.Substring(0, 990) + "������Ϣ�ѱ��ض�";
            }
            pa[6].Value = message;

            pa[7].Value = ex.InnerException != null ? ex.InnerException.StackTrace : ex.StackTrace;

            object obj = SqlHelper.ExecuteScalar(CommandType.Text, m_CmdText, pa);
            return DbUtils.ToInt32(obj, 0);
        }

        /// <summary>
        /// �õ���ʾ���û��Ĵ���˵��.
        /// </summary>
        /// <param name="ex">Դ�쳣</param>
        /// <returns>��������Ĵ���˵��</returns>
        private static string GetExceptionType(Exception ex)
        {
            string ext = "";
            if (ex is System.Data.DataException)
            {
                ext = ExceptionConfig.Default.DataException;
            }
            else if (ex is System.Web.HttpException)
            {
                ext = ExceptionConfig.Default.HttpException;
            }
            else if (ex is System.Web.HttpUnhandledException)
            {
                ext = ExceptionConfig.Default.HttpUnhandledException;
            }
            else if (ex is System.Data.SqlClient.SqlException)
            {
                ext = ExceptionConfig.Default.SqlException;
            }
            else
            {
                ext = ExceptionConfig.Default.OtherException;
            }
            return ext;
        }

        /// <summary>
        /// �õ�������־.
        /// </summary>
        /// <param name="pageCurrent">��ǰҳ��</param>
        /// <param name="pageSize">ÿҳ��ʾ����</param>
        /// <param name="recordCount">����</param>
        /// <param name="where">��ѯ����</param>
        /// <returns></returns>
        public static DataSet GetExceptionDataByPage(int pageCurrent, int pageSize, out int recordCount, string where)
        {
            SqlParameter[] pa = new SqlParameter[8];
            pa[0] = new SqlParameter("@TableName", SqlDbType.NVarChar, 255);
            pa[1] = new SqlParameter("@FieldKey", SqlDbType.VarChar, 255);
            pa[2] = new SqlParameter("@PageCurrent", SqlDbType.Int, 4);
            pa[3] = new SqlParameter("@PageSize", SqlDbType.Int, 4);
            pa[4] = new SqlParameter("@FieldShow", SqlDbType.NVarChar, 1000);
            pa[5] = new SqlParameter("@FieldOrder", SqlDbType.NVarChar, 1000);
            pa[6] = new SqlParameter("@Where", SqlDbType.NVarChar, 1000);
            pa[7] = new SqlParameter("@RecordCount", SqlDbType.Int, 4);

            pa[0].Value = "bw_ExceptionLog";
            pa[1].Value = "ID";
            pa[2].Value = pageCurrent;
            pa[3].Value = pageSize;
            pa[4].Value = "";
            pa[5].Value = "ID desc";
            pa[6].Value = where;
            pa[7].Direction = ParameterDirection.Output;

            DataSet ds = SqlHelper.ExecuteDataset(CommandType.StoredProcedure, "spPageViewByStr", pa);
            recordCount = DbUtils.ToInt32(pa[7].Value);
            return ds;

        }
        /// <summary>
        /// �����Զ����������ȡ�쳣��־����.
        /// </summary>
        /// <param name="where">�Զ���Ĳ�ѯ������where����ӵ����ݣ�</param>
        /// <returns></returns>
        public static DataSet ExecSqlScriptForDataSet(string where)
        {
            string sqlScript = string.Format("select [ID],[Describe],ClientIP,ServerIP,PageURL,ExceptionTime,ExceptionContent,StackTrace from bw_ExceptionLog {0} order by id desc", where);
            return SqlHelper.ExecuteDataset(CommandType.Text, sqlScript);
        }
    }
}
