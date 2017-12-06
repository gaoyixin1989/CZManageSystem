using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Events
{
    /// <summary>
    /// 应用程序的事件类.
    /// </summary>
    [Serializable]
    public class AppEvent
    {
        #region constructors

        /// <summary>
        /// 构造方法.
        /// </summary>
        public AppEvent() { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="appName">应用程序名称.</param>
        public AppEvent(string appName)
        {
            this.appName = appName;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="appName">应用程序名称.</param>
        /// <param name="category">事件类别.</param>
        public AppEvent(string appName, string category)
            : this(appName)
        {
            this.category = category;
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="appName">应用程序名称.</param>
        /// <param name="category">事件类别.</param>
        /// <param name="message">事件消息.</param>
        public AppEvent(string appName, string category, string message)
            : this(appName, category)
        {
            this.message = message;
        }
        #endregion

        #region properties
        private string appName;
        private string category;
        private string actor;
        private DateTime actionTime = DateTime.Now;
        private string message;
        private object data;

        /// <summary>
        /// 应用名称.
        /// </summary>
        public string AppName
        {
            get { return appName; }
            set { appName = value; }
        }

        /// <summary>
        /// 事件类别.
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// 执行者.
        /// </summary>
        public string Actor
        {
            get { return actor; }
            set { actor = value; }
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ActionTime
        {
            get { return actionTime; }
            set { actionTime = value; }
        }

        /// <summary>
        /// 事件消息.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// 附加参数/数据.
        /// </summary>
        public object Data
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        /// <summary>
        /// 重写转换为字符串的方法.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("[appName:{0}\r\n", null == this.appName ? String.Empty : this.appName);
            sb.AppendFormat("category:{0}\r\n", null == this.category ? String.Empty : this.category);
            sb.AppendFormat("actor:{0}\r\n", null == this.actor ? String.Empty : this.actor);
            sb.AppendFormat("actionTime:{0}\r\n", this.ActionTime.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendFormat("message:{0}\r\n", null == this.message ? String.Empty : this.message);
            sb.AppendFormat("data:{0}]", null == this.data ? String.Empty : this.data.ToString());
            return sb.ToString();
        }
    }
}
