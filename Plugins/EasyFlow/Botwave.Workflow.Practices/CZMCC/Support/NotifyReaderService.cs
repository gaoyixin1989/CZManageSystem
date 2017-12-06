using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow.Extension.Domain;
using Botwave.Workflow.Extension.Service;
using Botwave.XQP.Domain;

namespace Botwave.Workflow.Practices.CZMCC.Support
{
    public class NotifyReaderService
    {
        #region 字段
        /// <summary>
        /// 邮件信息类型值.
        /// </summary>
        public const int Email = 1;
        /// <summary>
        /// 短信信息类型值.
        /// </summary>
        public const int SMS = 2;
        #endregion

        private IWorkflowUserService workflowUserService;

        public IWorkflowUserService WorkflowUserService
        {
            set { workflowUserService = value; }
        }

        public void SendMessage(int msgType, string title, string content, params string[] receiverNames)
        {
            if (receiverNames == null || receiverNames.Length == 0)
                return;
            foreach (string userName in receiverNames)
            {
                ActorDetail actor = workflowUserService.GetActorDetail(userName);
                string msgTo = (msgType == Email ? actor.Email : actor.Mobile);
                InsertMessage(msgTo, content, msgType);
            }
        }

        /// <summary>
        /// 执行插入电子邮件提醒信息.
        /// </summary>
        /// <param name="messageForm"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void InsertMessage(string msgTo, string msgBody, int msgType)
        {
            string sql = "INSERT INTO Reminders (MsgType, MsgTo, MsgBody, State,CreatedDT,RetriedTimes) VALUES (@MsgType, @MsgTo, @MsgBody,1, getdate(), 0)";

            SqlParameter[] parm = new SqlParameter[]
            {
                new SqlParameter("@MsgType",SqlDbType.Int),
                new SqlParameter("@MsgTo",SqlDbType.NVarChar,100),
                new SqlParameter("@MsgBody",SqlDbType.NVarChar,256)
            };
            parm[0].Value = msgType;
            parm[1].Value = msgTo;
            parm[2].Value = msgBody;

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text, sql, parm);
        }

        
        /// <summary>
        /// 执行插入电子邮件提醒信息.
        /// </summary>
        /// <param name="messageForm"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void CZInsertMessage(string msgTo, string msgBody, int msgType)
        { 
            
        }
    }
}
