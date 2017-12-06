using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Security.Web
{
    /// <summary>
    /// 日志记录工厂对象类.
    /// </summary>
    public class LogWriterFactory
    {
        private static object syncLocker = new object();
        private static ILogWriter writer;

        /// <summary>
        /// 获取或设置记录对象类.
        /// </summary>
        public static ILogWriter Writer
        {
            get
            {
                if (writer == null)
                {
                    lock (syncLocker)
                    {
                        if (writer == null)
                            writer = new EmptyLogWriter();
                    }
                }
                return writer;
            }
            set
            {
                if (value != null)
                    writer = value;
            }
        }
    }
}
