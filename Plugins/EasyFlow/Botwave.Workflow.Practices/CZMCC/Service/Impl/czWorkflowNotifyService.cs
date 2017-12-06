using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Botwave.Workflow.Domain;
using Botwave.Workflow.Service;
using Botwave.Extension.IBatisNet;
using Botwave.XQP.Domain;

namespace Botwave.Workflow.Practices.CZMCC.Service.Impl
{
    public class czWorkflowNotifyService
    {
        #region 字段
        /// <summary>
        /// 邮件信息类型值.
        /// </summary>
        private const int EmailMessageType = 1;
        /// <summary>
        /// 短信信息类型值.
        /// </summary>
        private const int SMSMessageType = 2;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowInstance"></param>
        /// <param name="activityInstanceId"></param>
        /// <param name="userName"></param>
        /// <param name="notify"></param>
        public void SendCreatorMessage(WorkflowInstance workflowInstance, ActivityInstance activity, Guid activityInstanceId, string[] userName, int notify)
        {
            WorkflowProfile profile = WorkflowProfile.LoadByWorkflowId(workflowInstance.WorkflowId);

            int operateType = TodoInfo.OpDefault;

            SendMessage(notify, userName, activity, profile, operateType);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notify"></param>
        /// <param name="userName"></param>
        /// <param name="nextActivityIntances"></param>
        /// <param name="_workflowSetting"></param>
        /// <param name="operateType"></param>
        private void SendMessage(int notify, string[] userName, ActivityInstance activity, WorkflowProfile _workflowSetting, int operateType)
        {
            ResourcesExecutionService re = new ResourcesExecutionService();
            
            string fromEmail, fromMobile;

            for (int i = 0; i < userName.Length;i++ )
            {
                DataTable dt = re.GetUserInfo(userName[i].ToString());
                fromEmail = dt.Rows[0]["Email"].ToString();
                fromMobile = dt.Rows[0]["Mobile"].ToString();
                InsertMessage(fromEmail, notify, fromMobile, _workflowSetting.EmailNotifyFormat, _workflowSetting.SmsNotifyFormat, userName[i].ToString(), activity, operateType);
            }
        }

        /// <summary>
        /// 执行插入电子邮件和短信提醒.
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <param name="fromMobile"></param>
        /// <param name="emailNotifyFormat"></param>
        /// <param name="smsNotifyFormat"></param>
        /// <param name="activity"></param>
        /// <param name="operateType"></param>
        private static void InsertMessage(string fromEmail,int i, string fromMobile, string emailNotifyFormat, string smsNotifyFormat, string flowName, ActivityInstance activity, int operateType)
        {
            Guid activityInstanceId = activity.ActivityInstanceId;
            string workflowTitle = activity.WorkItemTitle;
            string activityName = activity.ActivityName;

            string emailContent = FormatNotifyMessage(emailNotifyFormat, flowName, workflowTitle, activityName, operateType);
            string smsContent = FormatNotifyMessage(smsNotifyFormat, flowName, workflowTitle, activityName, operateType);
           
            if (i == 1 || i == 2)
            {
                InsertEmailAndSms(fromEmail, emailContent, EmailMessageType);
            }
            if (i == 1 || i == 3)
            {
                InsertEmailAndSms(fromMobile, smsContent, SMSMessageType);
            }
        }

        /// <summary>
        /// 格式化指定提醒信息内容.
        /// </summary>
        /// <param name="messageFormat"></param>
        /// <param name="workflowTitle"></param>
        /// <param name="activityName"></param>
        /// <param name="operateType"></param>
        /// <returns></returns>
        public static string FormatNotifyMessage(string messageFormat, string creator, string workflowTitle, string activityName, int operateType)
        {
            messageFormat = messageFormat.ToLower();
            messageFormat = messageFormat.Replace("#creator#", creator);
            messageFormat = messageFormat.Replace("#title#", workflowTitle);
            messageFormat = messageFormat.Replace("#activityname#", activityName);
            messageFormat = messageFormat.Replace("#operatetype#", operateType == TodoInfo.OpBack ? "退回" : "进入");
            messageFormat = messageFormat.Replace("处理", "查阅");
            return messageFormat;
        }

        /// <summary>
        /// 执行插入电子邮件提醒信息.
        /// </summary>
        /// <param name="messageForm"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        private static void InsertEmailAndSms(string msgTo, string msgBody,int msgType)
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

            IBatisDbHelper.ExecuteNonQuery(CommandType.Text,sql,parm);
        }
    }
}
