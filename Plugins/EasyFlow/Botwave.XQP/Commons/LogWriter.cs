using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Botwave.Commons;

namespace Botwave.XQP.Commons
{
    public class LogWriter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(LogWriter));
        private const string m_CmdText = "INSERT INTO bw_ExceptionLog (Message,[Describe],ClientIP,ServerIP,PageURL,ExceptionTime,ExceptionContent,StackTrace) VALUES (@Message,@Describe,@ClientIP,@ServerIP,@PageURL,@ExceptionTime,@ExceptionContent,@StackTrace) if @@Error<>0 SELECT 0 else SELECT @@IDENTITY";

        public static void Write(string message, string detail)
        {
            Write(Botwave.Security.LoginHelper.UserName, message, detail);
        }

        public static void Write(string actor, string message, string detail)
        {
            Write(actor, message, detail, new object[0]);
        }

        public static void Write(string actor, string message, string detail, params object[] args)
        {
            if (args != null && args.Length > 0)
                detail = string.Format(detail, args);
            OnWrite(actor, message, detail, string.Empty);
        }

        public static void Write(Exception exception)
        {
            Write(Botwave.Security.LoginHelper.UserName, exception);
        }

        public static void Write(Exception exception, string message)
        {
            Write(Botwave.Security.LoginHelper.UserName, exception, message);
        }

        public static void Write(string actor, Exception exception)
        {
            string message = ((exception.InnerException != null && exception.InnerException is ApplicationException) ? exception.InnerException.Message : exception.Message);
            Write(actor, exception, message);
        }

        public static void Write(string actor, Exception exception, string message)
        {
            string detail = exception.ToString();
            string stackTrace = (exception.InnerException != null ? exception.InnerException.StackTrace : exception.StackTrace);
            OnWrite(actor, message, detail, stackTrace);
        }

        private static void OnWrite(string actor, string message, string detail, string stackTrace)
        {
            try
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

                pa[0].Value = message;
                pa[1].Value = actor;
                pa[2].Value = HttpContext.Current.Request.UserHostAddress;
                pa[3].Value = HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
                pa[4].Value = HttpContext.Current.Request.Url.ToString();
                pa[5].Value = DateTime.Now;

                if (null != detail && detail.Length >= 990)
                {
                    detail = detail.Substring(0, 990) + "以下信息已被截断";
                }
                pa[6].Value = detail;
                pa[7].Value = stackTrace;

                object obj = SqlHelper.ExecuteScalar(CommandType.Text, m_CmdText, pa);
                //return DbUtils.ToInt32(obj, 0);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
