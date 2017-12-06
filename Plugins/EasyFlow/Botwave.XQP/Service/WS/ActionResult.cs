using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.Service.WS
{
    /// <summary>
    /// 操作结果类.
    /// </summary>
    public class ActionResult
    {
        private int appAuth;
        private bool returnValue;
        private string returnMessage;

        /// <summary>
        /// 应用系统接入认证结果.
        /// </summary>
        public int AppAuth
        {
            get { return appAuth; }
            set { appAuth = value; }
        }

        /// <summary>
        /// 方法调用结果(成功/失败).
        /// </summary>
        public bool ReturnValue
        {
            get { return returnValue; }
            set { returnValue = value; }
        }

        /// <summary>
        /// 返回信息/异常信息.
        /// </summary>
        public string ReturnMessage
        {
            get { return returnMessage; }
            set { returnMessage = value; }
        }
    }
}
