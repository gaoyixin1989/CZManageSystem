using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 短信设置类.
    /// </summary>
    public class SMSProfile
    {
        /// <summary>
        /// 系统名称.
        /// </summary>
        public const string SystemName = "潮州综合管理平台";
        /// <summary>
        /// 销售精英平台流程名称.
        /// </summary>
        public const string DefaultGratuityWorkflowName = "销售精英竞赛平台酬金申告流程";
        /// <summary>
        /// 销售精英流程自定义短信内容步骤名称.
        /// </summary>
        public const string DefaultGratuityNotifyActivity = "结束答复会员";

        #region proprties

        private int _id = -1;
        private string _activityRejectMessage;
        private string _assignmentMessage;
        private string _feedbackSuccessMessage;
        private string _feedbackErrorMessage;
        private string _receiveInvalidMessage;
        private string _lastReceiveInvalidMessage;
        private string _gratuityReplyMessage;
        private string _gratuityWorkflowName;
        private string _gratuityNotifyActivity;
        private int _maxInvalidSMS = 5;

        /// <summary>
        /// 配置编号.
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 工单退回的消息通知内容(参数：#title#, #ActivityName#).
        /// </summary>
        public string ActivityRejectMessage
        {
            get { return _activityRejectMessage; }
            set { _activityRejectMessage = value; }
        }

        /// <summary>
        /// 转交工单的消息通知内容(#from#, #title#).
        /// </summary>
        public string AssignmentMessage
        {
            get { return _assignmentMessage; }
            set { _assignmentMessage = value; }
        }

        /// <summary>
        /// 短信审批成功的反馈消息通知内容.
        /// </summary>
        public string FeedbackSuccessMessage
        {
            get { return _feedbackSuccessMessage; }
            set { _feedbackSuccessMessage = value; }
        }

        /// <summary>
        /// 短信审批失败的反馈消息通知内容.
        /// </summary>
        public string FeedbackErrorMessage
        {
            get { return _feedbackErrorMessage; }
            set { _feedbackErrorMessage = value; }
        }

        /// <summary>
        /// 接收短信内容格式有误的消息通知内容.
        /// </summary>
        public string ReceiveInvalidMessage
        {
            get { return _receiveInvalidMessage; }
            set { _receiveInvalidMessage = value; }
        }

        /// <summary>
        /// 接收短信内容格式有误的最后一次消息通知内容.
        /// </summary>
        public string LastReceiveInvalidMessage
        {
            get { return _lastReceiveInvalidMessage; }
            set { _lastReceiveInvalidMessage = value; }
        }

        /// <summary>
        /// 销售精英竞赛平台酬金申告流程的结束答复会员步骤的短信通知内容.
        /// </summary>
        public string GratuityReplyMessage
        {
            get { return _gratuityReplyMessage; }
            set { _gratuityReplyMessage = value; }
        }

        /// <summary>
        /// / 销售精英竞赛平台酬金申告流程的流程名称.
        /// </summary>
        public string GratuityWorkflowName
        {
            get
            {
                if (string.IsNullOrEmpty(_gratuityWorkflowName))
                    _gratuityWorkflowName = DefaultGratuityWorkflowName;
                return _gratuityWorkflowName;
            }
            set { _gratuityWorkflowName = value; }
        }

        /// <summary>
        /// / 销售精英竞赛平台酬金申告流程的结束答复会员步骤的步骤名称.
        /// </summary>
        public string GratuityNotifyActivity
        {
            get
            {
                if (string.IsNullOrEmpty(_gratuityNotifyActivity))
                    _gratuityNotifyActivity = DefaultGratuityNotifyActivity;
                return _gratuityNotifyActivity;
            }
            set { _gratuityNotifyActivity = value; }
        }

        /// <summary>
        /// 接收的短信内容格式有误的最大次数.
        /// </summary>
        public int MaxInvalidSMS
        {
            get { return _maxInvalidSMS; }
            set { _maxInvalidSMS = value; }
        }
        #endregion

        public SMSProfile()
        {

        }

        /// <summary>
        /// 更新.
        /// </summary>
        /// <returns></returns>
        public void Update()
        {
            if (this.ID > -1)
            {
                // 更新.
                IBatisMapper.Update("xqp_SMSProfile_Update_ByID", this);
            }
            else
            { 
                // 新增.
                IBatisMapper.Insert("xqp_SMSProfile_Insert", this);
            }
        }

        /// <summary>
        /// 获取默认对象.
        /// </summary>
        /// <returns></returns>
        internal static SMSProfile DefaultProfile()
        {
            SMSProfile current = new SMSProfile();
            current._activityRejectMessage = string.Format("{0}：您发起的工单 #title# 已被退回，可以在待办事宜或草稿箱中修改后再提单。", SystemName);
            current._assignmentMessage = string.Format("{0}：#from# 将工单 #title# 转交给您，请您处理。", SystemName);
            current._feedbackSuccessMessage = string.Format("{0}：您的短信审批已经成功执行。", SystemName);
            current._feedbackErrorMessage = string.Format("{0}：您的短信审批执行失败，工单步骤已关闭或已完成。", SystemName);
            current._receiveInvalidMessage = string.Format("{0}：您的短信审批执行失败。回复内容格式不正确，请重新回复。回复1同意，回复0不同意。", SystemName);
            current._lastReceiveInvalidMessage = string.Format("{0}：您的短信审批失败，由于审批次数已达最大，系统将不再受理您的审批请求。", SystemName);
            current._gratuityReplyMessage = string.Format("{0}：尊敬的会员，您提交的酬金申告单：#title#，已经处理完成，请确认。", SystemName);
            current._gratuityWorkflowName = DefaultGratuityWorkflowName;
            current._gratuityNotifyActivity = DefaultGratuityNotifyActivity;
            return current;
        }

        /// <summary>
        /// 获取短信通知消息配置对象.
        /// </summary>
        /// <returns></returns>
        public static SMSProfile GetProfile()
        {
            SMSProfile profile = IBatisMapper.Load<SMSProfile>("xqp_SMSProfile_Select_Profile", null);
            if (profile == null)
                return DefaultProfile();
            return profile;
        }
    }
}
 