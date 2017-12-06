using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Security.Web
{
    /// <summary>
    /// 日志记录类.
    /// </summary>
    public interface ILogWriter
    {
        /// <summary>
        /// 记录正常日志.
        /// </summary>
        /// <param name="userName">操作用户.</param>
        /// <param name="operation">操作名称.</param>
        /// <param name="description">描述.</param>
        void WriteNomalLog(string userName, string operation, string description);

        /// <summary>
        /// 记录异常日志.
        /// </summary>
        /// <param name="userName">操作用户.</param>
        /// <param name="operation">操作名称.</param>
        /// <param name="description">描述.</param>
        void WriteExLog(string userName, string operation, string description);

        /// <summary>
        /// 记录异常日志.
        /// </summary>
        /// <param name="userName">操作用户.</param>
        /// <param name="operation">操作名称.</param>
        /// <param name="exception">异常名称.</param>
        /// <param name="description">描述.</param>
        void WriteExLog(string userName, string operation, string exception, string description);
    }
}
