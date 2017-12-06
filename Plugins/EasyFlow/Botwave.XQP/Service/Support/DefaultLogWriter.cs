using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Log.GMCC;

namespace Botwave.XQP.Service.Support
{
    /// <summary>
    /// 日志记录空实现类.
    /// </summary>
    public class DefaultLogWriter : Botwave.Security.Web.ILogWriter
    {
        #region ILogWriter 成员

        /// <summary>
        /// 记录正常日志.
        /// </summary>
        /// <param name="userName">操作用户.</param>
        /// <param name="operation">操作名称.</param>
        /// <param name="description">描述.</param>
        public void WriteNomalLog(string userName, string operation, string description)
        {
            LoggerHelper.WriteNomalLog(userName, operation, description);
        }

        /// <summary>
        /// 记录异常日志.
        /// </summary>
        /// <param name="userName">操作用户.</param>
        /// <param name="operation">操作名称.</param>
        /// <param name="description">描述.</param>
        public void WriteExLog(string userName, string operation, string description)
        {
            LoggerHelper.WriteExLog(userName, operation, "异常操作", description);
        }

        /// <summary>
        /// 记录异常日志.
        /// </summary>
        /// <param name="userName">操作用户.</param>
        /// <param name="operation">操作名称.</param>
        /// <param name="exception">异常名称.</param>
        /// <param name="description">描述.</param>
        public void WriteExLog(string userName, string operation, string exception, string description)
        {
            LoggerHelper.WriteExLog(userName, operation, exception, description);
        }

        #endregion
    }
}
