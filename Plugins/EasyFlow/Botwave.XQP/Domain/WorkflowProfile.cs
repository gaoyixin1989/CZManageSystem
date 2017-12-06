using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Botwave.Workflow.Domain;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 流程配置信息类(xqp_workflowSettings).
    /// </summary>
    public class WorkflowProfile
    {
        /// <summary>
        /// 默认提醒内容格式.
        /// </summary>
        public static string DefaultNotifyFormat = "[潮州综合管理平台]您有来自#PrevActors#的待办工单：#Title#，请及时处理！";
        //public static string DefaultNotifyFormat = Botwave.GlobalSettings.Instance.Signature + " 现有工单：#Title# #OperateType# #ActivityName#步骤,请您处理！";
        /// <summary>
        /// 短信审批内容格式.
        /// </summary>
        public static string DefaultSMSAudit = @"您有来自#PrevActors#的待办工单：#Title#，请及时处理！
流程名称：#Title#     
发起人：#Creator#
回复1同意，回复0不同意。";
        //public static string DefaultSMSAudit = Botwave.GlobalSettings.Instance.Signature + " 现有 #Creator# 发起的工单:#Title# #OperateType# #ActivityName# 步骤 请您处理!如需短信审批,同意请回复1;退回则回复0.";
        /// <summary>
        /// 默认待阅(抄送)的消息提醒格式.
        /// </summary>
        public static string DefaultReviewMessage = "潮州综合管理平台：您有来自 #from# 的待阅工单：#title#。";
        /// <summary>
        /// WorkflowProfile 的默认实例对象.
        /// </summary>
        public static WorkflowProfile Default = new WorkflowProfile(null, 0, DefaultNotifyFormat, DefaultNotifyFormat, DefaultNotifyFormat, DefaultNotifyFormat, true); 

        #region gets / sets
        private bool _isDefault;
        private string _workflowName;
        private string basicFields;
        private int _minNotifyTaskCount;
        private string _smsNotifyFormat;
        private string _emailNotifyFormat;
        private string _statSmsNodifyFormat;
        private string _statEmailNodifyFormat;
        private string _creationControlType;
        private int _maxCreationInMonth;
        private int _maxCreationInWeek;
        private int _maxCreationUndone;
        private string _smsAuditNotifyFormat = DefaultSMSAudit;
        private bool _isSMSAudit;
        private string _smsAuditActivities;
        private bool _isReview = false;
        private bool _isClassicReviewType = false;
        private string _reviewNotifyMessage;
        private int _reviewActorCount = -1;
        private string _workflowInstanceTitle;
        private bool _isAutoContinue = false;
        private string _autoContinueActivities = string.Empty;
        private int _printAndExp;
        private int _printAmount;
        private string _workOrderTimeoutNotifyformat;
        private string _workOrderWarningNotifyformat;
        private string _stepTimeoutNotifyformat;
        private string _stepWarningNotifyformat;
        private string _depts;
        private string _manager;
        private bool _isMobile;
        private bool _isTimeOutContinue = false;
        private bool _isToShortDateString;

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }
        
        /// <summary>
        /// 属性显示类型.
        /// 用四位按顺序标识: 期望完成时间、保密设置、紧急程度、重要级别.
        /// 如：
        ///     1111 代表四个属性都需要;
        ///     1100 代表只需要期望完成时间、保密设置.
        /// </summary>
        public string BasicFields
        {
            get { return basicFields; }
            set { basicFields = value; }
        }

        /// <summary>
        /// 最小需要消息通知的汇总任务数量，默认为0(不需要汇总).
        /// </summary>
        public int MinNotifyTaskCount
        {
            get { return _minNotifyTaskCount; }
            set { _minNotifyTaskCount = value; }
        }

        /// <summary>
        /// 短信通知内容格式.
        /// </summary>
        public string SmsNotifyFormat
        {
            get { return _smsNotifyFormat; }
            set { _smsNotifyFormat = value; }
        }

        /// <summary>
        /// 电子邮件通知内容格式.
        /// </summary>
        public string EmailNotifyFormat
        {
            get { return _emailNotifyFormat; }
            set { _emailNotifyFormat = value; }
        }

        /// <summary>
        /// 统计短信通知内容格式.
        /// </summary>
        public string StatSmsNodifyFormat
        {
            get { return _statSmsNodifyFormat; }
            set { _statSmsNodifyFormat = value; }
        }

        /// <summary>
        /// 统计电子邮件通知内容格式.
        /// </summary>
        public string StatEmailNodifyFormat
        {
            get { return _statEmailNodifyFormat; }
            set { _statEmailNodifyFormat = value; }
        }

        #region 普通单发起控制

        /// <summary>
        /// 发单数量控制类型.
        /// 默认(NULL 或空值)为总体控件.
        /// dept:部门控制.
        /// room:科室控制.
        /// </summary>
        public string CreationControlType
        {
            get { return _creationControlType; }
            set { _creationControlType = value; }
        }
        
        /// <summary>
        /// 每月的最大发单数量.
        /// </summary>
        public int MaxCreationInMonth
        {
            get { return _maxCreationInMonth; }
            set { _maxCreationInMonth = value; }
        }

        /// <summary>
        /// 每周的最大发单数量.
        /// </summary>
        public int MaxCreationInWeek
        {
            get { return _maxCreationInWeek; }
            set { _maxCreationInWeek = value; }
        }

        /// <summary>
        /// 未完成工单的最大发单数.
        /// </summary>
        public int MaxCreationUndone
        {
            get { return _maxCreationUndone; }
            set { _maxCreationUndone = value; }
        }
        #endregion

        /// <summary>
        /// 短信审批发送的短信内容格式.
        /// </summary>
        public string SMSAuditNotifyFormat
        {
            get { return _smsAuditNotifyFormat; }
            set { _smsAuditNotifyFormat = value; }
        }

        /// <summary>
        /// 是否允许工单短信审批.
        /// </summary>
        public bool IsSMSAudit
        {
            get { return _isSMSAudit; }
            set { _isSMSAudit = value; }
        }

        /// <summary>
        /// 允许短信审批的流程步骤(活动)列表.
        /// </summary>
        public string SMSAuditActivities
        {
            get { return _smsAuditActivities; }
            set { _smsAuditActivities = value; }
        }

        /// <summary>
        /// 是否启用抄送.
        /// </summary>
        public bool IsReview
        {
            get { return _isReview; }
            set { _isReview = value; }
        }

        /// <summary>
        /// 是否采用原有(经典)方式显示，即提供选择框，供用户自行选择.
        /// </summary>
        public bool IsClassicReviewType
        {
            get { return _isClassicReviewType; }
            set { _isClassicReviewType = value; }
        }

        /// <summary>
        /// 抄送的消息提醒内容.
        /// </summary>
        public string ReviewNotifyMessage
        {
            get
            {
                if (string.IsNullOrEmpty(_reviewNotifyMessage))
                    _reviewNotifyMessage = DefaultReviewMessage;
                return _reviewNotifyMessage;
            }
            set { _reviewNotifyMessage = value; }
        }

        /// <summary>
        /// 抄送人员数限制(即最多抄送人数)，-1 表示不限制人数.
        /// </summary>
        public int ReviewActorCount
        {
            get { return _reviewActorCount; }
            set { _reviewActorCount = value; }
        }

        /// <summary>
        /// 获取流程实例标题格式(支持参数：$datetime，$date，$user，$dept，$workflow).
        /// </summary>
        public string WorkflowInstanceTitle
        {
            get { return _workflowInstanceTitle; }
            set { _workflowInstanceTitle = value; }
        }

        /// <summary>
        /// 自动连续处理.
        /// </summary>
        public bool IsAutoContinue
        {
            get { return _isAutoContinue; }
            set { _isAutoContinue = value; }
        }

        /// <summary>
        /// 自动连续处理的步骤名称（多个名称之间以逗号隔开","）.
        /// </summary>
        public string AutoContinueActivities
        {
            get { return _autoContinueActivities; }
            set { _autoContinueActivities = value; }
        }

        /// <summary>
        /// 是否为默认流程设置.
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
        }

        /// <summary>
        /// 导出打印标识（0：默认可导出和打印；1：只能导出；2：只能打印；3：不能导出不能打印）
        /// </summary>
        public int PrintAndExp
        {
            get { return _printAndExp; }
            set { _printAndExp = value; }
        }

        /// <summary>
        /// 工单可打印次数
        /// </summary>
        public int PrintAmount
        {
            get { return _printAmount; }
            set { _printAmount = value; }
        }

        /// <summary>
        /// 环节超时预警提醒格式
        /// </summary>
        public string StepWarningNotifyformat
        {
            get { return _stepWarningNotifyformat; }
            set { _stepWarningNotifyformat = value; }
        }

        /// <summary>
        /// 环节超时提醒格式
        /// </summary>
        public string StepTimeoutNotifyformat
        {
            get { return _stepTimeoutNotifyformat; }
            set { _stepTimeoutNotifyformat = value; }
        }

        /// <summary>
        /// 工单预警提醒格式
        /// </summary>
        public string WorkOrderWarningNotifyformat
        {
            get { return _workOrderWarningNotifyformat; }
            set { _workOrderWarningNotifyformat = value; }
        }

        /// <summary>
        /// 工单超时提醒格式
        /// </summary>
        public string WorkOrderTimeoutNotifyformat
        {
            get { return _workOrderTimeoutNotifyformat; }
            set { _workOrderTimeoutNotifyformat = value; }
        }

        /// <summary>
        /// 流程所属部门
        /// </summary>
        public string Depts
        {
            get { return _depts; }
            set { _depts = value; }
        }

        /// <summary>
        /// 流程经理人
        /// </summary>
        public string Manager
        {
            get { return _manager; }
            set { _manager = value; }
        }

        /// <summary>
        /// 是否允许手机审批
        /// </summary>
        public bool IsMobile
        {
            get { return _isMobile; }
            set { _isMobile = value; }
        }

        /// <summary>
        /// 是否允许超时时自动处理该步骤
        /// </summary>
        public bool IsTimeOutContinue
        {
            get { return _isTimeOutContinue; }
            set { _isTimeOutContinue = value; }
        }

        /// <summary>
        /// 处理记录时间是否只显示短时间格式
        /// </summary>
        public bool IsToShortDateString
        {
            get { return _isToShortDateString; }
            set { _isToShortDateString = value; }
        }
        #endregion

        /// <summary>
        /// 构造方法.
        /// </summary>
        public WorkflowProfile()
        {
            this.Init();
        }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="minNotifyTaskCount"></param>
        /// <param name="smsNotifyFormat"></param>
        /// <param name="emailNotifyFormat"></param>
        /// <param name="statSmsNodifyFormat"></param>
        /// <param name="statEmailNodifyFormat"></param>
        public WorkflowProfile(string workflowName, int minNotifyTaskCount, string smsNotifyFormat, string emailNotifyFormat, string statSmsNodifyFormat, string statEmailNodifyFormat)
            : this(workflowName, minNotifyTaskCount, smsNotifyFormat, emailNotifyFormat, statSmsNodifyFormat, statEmailNodifyFormat, false)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="minNotifyTaskCount"></param>
        /// <param name="smsNotifyFormat"></param>
        /// <param name="emailNotifyFormat"></param>
        /// <param name="statSmsNodifyFormat"></param>
        /// <param name="statEmailNodifyFormat"></param>
        /// <param name="isEmpty"></param>
        protected WorkflowProfile(string workflowName, int minNotifyTaskCount, string smsNotifyFormat, string emailNotifyFormat, string statSmsNodifyFormat, string statEmailNodifyFormat, bool isEmpty)
        {
            this._workflowName = workflowName;
            this._minNotifyTaskCount = minNotifyTaskCount;
            this._smsNotifyFormat = smsNotifyFormat;
            this._emailNotifyFormat = emailNotifyFormat;
            this._statSmsNodifyFormat = statSmsNodifyFormat;
            this._statEmailNodifyFormat = statEmailNodifyFormat;
            this._isDefault = isEmpty;

            this.Init();
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        protected void Init()
        {
            this._creationControlType = null;
            this.basicFields = "0000";
            this._maxCreationInMonth = -1;
            this._maxCreationInWeek = -1;
            this._maxCreationUndone = -1;
            this._workflowInstanceTitle = "$dept$workflow";
        }

        #region 方法


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
            return FormatNotifyMessage(messageFormat, Guid.Empty, creator, workflowTitle, activityName, operateType);
        }

        public static string FormatNotifyMessage(string messageFormat, Guid activityInstanceId, string creator, string workflowTitle, string activityName, int operateType)
        {
            if (string.IsNullOrEmpty(messageFormat))
                return string.Empty;
            messageFormat = messageFormat.ToLower();
            messageFormat = messageFormat.Replace("#creator#", creator);
            messageFormat = messageFormat.Replace("#title#", workflowTitle);
            messageFormat = messageFormat.Replace("#activityname#", activityName);
            messageFormat = messageFormat.Replace("#operatetype#", operateType == TodoInfo.OpBack ? "退回" : "进入");
            if (activityInstanceId != Guid.Empty && messageFormat.Contains("#prevactors#"))
            {
                IDictionary<string, string> prevActors = GetPreviousActors(activityInstanceId);
                string actors = string.Empty;
                if (prevActors != null && prevActors.Count > 0)
                {
                    foreach(string value in prevActors.Values)
                        actors += string.Format(",{0}", value);
                }
                actors = string.IsNullOrEmpty(actors) ? "无" : actors.Remove(0, 1);
                messageFormat = messageFormat.Replace("#prevactors#", actors);
            }
            return messageFormat;
        }

        /// <summary>
        /// 新增流程设置.
        /// </summary>
        public void Insert()
        {
            if (ExistsProfile(this.WorkflowName))
                Update();
            else
                IBatisMapper.Insert("xqp_WorkflowSetting_Insert", this);
        }

        /// <summary>
        /// 更新流程设置.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_WorkflowSetting_Update", this);
        }

        /// <summary>
        /// 更新流程的短信审批设置.
        /// </summary>
        /// <returns></returns>
        public int UpdateSMSAudit()
        {
            return IBatisMapper.Update("xqp_WorkflowSetting_Update_SMSAudit", this);
        }

        /// <summary>
        /// 更新流程的抄送设置.
        /// </summary>
        /// <returns></returns>
        public int UpdateReview()
        {
            return IBatisMapper.Update("xqp_WorkflowSetting_Update_Review", this);
        }

        /// <summary>
        /// 更正自动处理的设置.
        /// </summary>
        /// <returns></returns>
        public int UpdateAutoContinue()
        {
            return IBatisMapper.Update("xqp_WorkflowSetting_Update_AutoContinue", this);
        }

        /// <summary>
        /// 获取指定流程定义的流程设置对象.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static WorkflowProfile LoadByWorkflowId(Guid workflowId)
        {
            WorkflowProfile result = IBatisMapper.Load<WorkflowProfile>("xqp_WorkflowSetting_Select_ByWorkflowId", workflowId);
            return result;
        }

        /// <summary>
        /// 获取指定流程名称的流程设置对象.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static WorkflowProfile LoadByWorkflowName(string workflowName)
        {
            WorkflowProfile result = IBatisMapper.Load<WorkflowProfile>("xqp_WorkflowSetting_Select_ByWorkflowName", workflowName);
            return result;
        }

        /// <summary>
        /// 获取指定流程名称是否存在配置.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static bool ExistsProfile(string workflowName)
        {
            int count = IBatisMapper.Mapper.QueryForObject<int>("xqp_WorkflowSetting_Select_Exists", workflowName);
            return (count >= 1);
        }

        /// <summary>
        /// 获取指定流程霍都实例的前续活动处理人.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetPreviousActors(Guid activityInstanceId)
        {
            if (activityInstanceId == Guid.Empty)
                return new Dictionary<string, string>();
            IDictionary<string, string> results = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            IList<Botwave.Entities.BasicUser> actors = IBatisMapper.Select<Botwave.Entities.BasicUser>("bwdf_MessageTemplate_Select_PrevActors_By_ActivitynstanceId", activityInstanceId);
            if (actors != null && actors.Count > 0)
            {
                foreach (Botwave.Entities.BasicUser item in actors)
                {
                    if (!results.ContainsKey(item.UserName))
                        results.Add(item.UserName, item.RealName);
                }
            }
            return results;
        }
        #endregion

        #region 基本字段 方法

        public int GetBasicFieldsCount()
        {
            //如果没有设置，则认为全有
            const int ALL = 4;
            if (null == basicFields || basicFields.Length != ALL)
            {
                return ALL;
            }

            int count = 0;
            for (int i = 0; i < ALL; i++)
            {
                if (basicFields[i] == '1')
                {
                    count++;
                }
            }
            return count;
        }

        public bool HasBasicField(BasicFieldType fieldType)
        {
            //如果没有设置，则认为有.
            //如果格式错误（不是四位），也认为有.
            if (null == basicFields || basicFields.Length != 4)
            {
                return true;
            }

            bool hasField = false;
            switch (fieldType)
            {
                case BasicFieldType.ExpectFinishedTime:
                    hasField = (basicFields[0] == '1');
                    break;
                case BasicFieldType.Secrecy:
                    hasField = (basicFields[1] == '1');
                    break;
                case BasicFieldType.Urgency:
                    hasField = (basicFields[2] == '1');
                    break;
                case BasicFieldType.Importance:
                    hasField = (basicFields[3] == '1');
                    break;
            }
            return hasField;
        }

        /// <summary>
        /// 基本字段类型.
        /// </summary>
        public enum BasicFieldType
        {
            /// <summary>
            /// 期望完成时间.
            /// </summary>
            ExpectFinishedTime = 0,
            /// <summary>
            /// 保密级别.
            /// </summary>
            Secrecy = 1,
            /// <summary>
            /// 紧急程度.
            /// </summary>
            Urgency = 2,
            /// <summary>
            /// 重要级别.
            /// </summary>
            Importance = 3
        }

        public static string ConvertSecrecyToDesc(int status)
        {
            return (status == 1) ? "保密" : "不保密";
        }

        public static string ConvertUrgencyToDesc(int status)
        {
            string desc = "一般";
            switch (status)
            {
                case 1:
                    desc = "紧急";
                    break;
                case 2:
                    desc = "特别紧急";
                    break;
                case 3:
                    desc = "最紧急";
                    break;
            }
            return desc;
        }

        public static string ConvertImportanceToDesc(int status)
        {
            string desc = "一般";
            switch (status)
            {
                case 1:
                    desc = "重要";
                    break;
                case 2:
                    desc = "特别重要";
                    break;
            }
            return desc;
        }

        /// <summary>
        /// 更新导出打印标识
        /// </summary>
        /// <returns></returns>
        public int UpdatePrintAndExp()
        {
            return IBatisMapper.Update("xqp_WorkflowSetting_Update_PrintAndExp", this);
        }

        /// <summary>
        /// 获取短信审批流程步骤名称字段.
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> GetSMSAuditActivities()
        {
            if (string.IsNullOrEmpty(this.SMSAuditActivities))
                return new Dictionary<string, string>();
            string[] activityArray = this.SMSAuditActivities.Split(',', '，');
            IDictionary<string, string> results = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string item in activityArray)
            {
                if (!results.ContainsKey(item))
                    results.Add(item, item);
            }
            return results;
        }

        #endregion
    }
}
