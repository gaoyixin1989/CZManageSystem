using System;

namespace Botwave.Workflow
{
    /// <summary>
    /// ���������.
    /// </summary>
    [Serializable]
    public class ActionResult
    {
        private bool success;
        private string message;

        /// <summary>
        /// �����Ƿ�ɹ�.
        /// </summary>
        public bool Success
        {
            get { return success; }
            set { success = value; }
        }

        /// <summary>
        /// ����������Ϣ.
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
