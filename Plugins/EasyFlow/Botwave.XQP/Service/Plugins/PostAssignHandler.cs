using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Botwave.Commons;
using Botwave.Extension.IBatisNet;
using Botwave.Workflow;
using Botwave.Workflow.Domain;
using Botwave.Workflow.Plugin;
using Botwave.Workflow.Extension.Util;
using Botwave.Security.Domain;
using Botwave.Security.Service;
using Botwave.GMCCServiceHelpers;
using Botwave.XQP.Domain;

namespace Botwave.XQP.Service.Plugins
{
    public class PostAssignHandler : IPostAssignHandler
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(typeof(PostAssignHandler));
        private SMSProfile smsProfile = SMSProfile.GetProfile();

        private IUserService userService;

        public IUserService UserService
        {
            set { userService = value; }
        }

        #region IPostAssignHandler 成员

        private IPostAssignHandler next = null;

        public IPostAssignHandler Next
        {
            get { return next; }
            set { next = value; }
        }

        public void Execute(Assignment assignment)
        {
            if (null != assignment && assignment.ActivityInstanceId.HasValue)
            {
                log.Info(assignment);

                // 接收人（被授权人）
                Guid activityInstanceId = assignment.ActivityInstanceId.Value;
                string receiverName = assignment.AssignedUser;
                UserInfo receiver = userService.GetUserByUserName(receiverName);

                string aiid = activityInstanceId.ToString();
                //删除指派人相关的待办工作
                UserInfo sender = userService.GetUserByUserName(assignment.AssigningUser);
                AsynExtendedPendingJobHelper.DeletePendingJobByEntity(ActivityInstance.EntityType, aiid, sender.EmployeeId);
                string senderRealName = sender.RealName;
                if (string.IsNullOrEmpty(senderRealName))
                    senderRealName = sender.UserName;

                //发送待办信息给被指派人
                string jobName = String.Format("您有新的转交任务，转交人 {0}", senderRealName);
                string url = WorkflowPostHelper.TransformUrlByActivityInstanceId(aiid);
                AsynExtendedPendingJobHelper.AddPendingJob(receiverName,
                    jobName,
                    url,
                    ActivityInstance.EntityType,
                    aiid);

                if (!string.IsNullOrEmpty(receiverName))
                {
                    string title = GetWorkflowTitle(activityInstanceId);
                    string msgContent = smsProfile.AssignmentMessage.ToLower().Replace("#from#", senderRealName).Replace("#title#", title);
                    //AsynNotifyHelper.SendSMS(mobile, msgContent);
                    Botwave.GMCCServiceHelpers.CZ.AsynNotifyHelper.SendSMS(receiverName, sender.UserName, msgContent, ActivityInstance.EntityType, activityInstanceId.ToString());
                }
            }
        }

        #endregion

        /// <summary>
        /// 获取工单标题.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        private static string GetWorkflowTitle(Guid activityInstanceId)
        {
            string sql = string.Format("SELECT WorkItemTitle FROM vw_bwwf_Tracking_Activities_All_Ext WHERE ActivityInstanceId = '{0}'", activityInstanceId);
            object result = IBatisDbHelper.ExecuteScalar(CommandType.Text, sql);
            if (result == null)
                return string.Empty;
            return Botwave.Commons.DbUtils.ToString(result, string.Empty);
        }
    }
}
