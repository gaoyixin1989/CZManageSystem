using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.DynamicForm.Domain
{
    public class FormResult
    {
        private int appAuth;
        private int success;
        private string message;
        private string data;

        /// <summary>
        /// 接口验证结果。0：验证失败；1：验证成功。
        /// </summary>
        public int AppAuth
        {
            set { appAuth = value; }
            get { return appAuth; }
        }

        /// <summary>
        /// 接口调用结果。0：调用失败；1：调用成功。
        /// </summary>
        public int Success
        {
            set { success = value; }
            get { return success; }
        }

        /// <summary>
        /// 接口调用返回的消息内容。
        /// </summary>
        public string Message
        {
            set { message = value; }
            get { return message; }
        }

        /// <summary>
        /// 接口调用返回的数据内容。如：表单XML。
        /// </summary>
        public string Data
        {
            set { data = value; }
            get { return data; }
        }
    }
}
