using System;

namespace Botwave.Workflow
{
    /// <summary>
    /// 动作结果类.
    /// </summary>
    [Serializable]
    public class ActionResult
    {
        private bool success;
        private string message;

        /// <summary>
        /// 动作是否成功.
        /// </summary>
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        /// <summary>
        /// 动作返回信息.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
