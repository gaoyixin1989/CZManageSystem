using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Botwave.Extension.IBatisNet;
using System.Data;
using Botwave.Workflow.Extension.Domain;
using System.Collections;
using Botwave.Workflow.Domain;
using System.Text;

namespace Botwave.XQP.Domain
{
	/// <summary>
    /// [sz_SZIntelligentRemind] 的实体类.
	/// 创建日期: 2012-12-3
	/// </summary>
    public class SZIntelligentRemind
	{
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(SZIntelligentRemind));
		#region Getter / Setter
		
		private int id;
        private string workflowName = String.Empty;
        private string activityName = String.Empty;
        private string extArgs = String.Empty;
        private decimal stayHours = -1;
        private decimal toHours = -1;
        private decimal toInterval = -1;
        private int toTimes = -1;
        private decimal timeoutInterval = -1;
        private int timeoutTimes = -1;
        private int remindType;
        private int statisticsType;
        private int settingType = 1;
		private string creator = String.Empty;
		private DateTime createdTime;
        private string expectFinishTime;
        private string allocatorUsers;
        private string extendAllocators;
        private string extendAllocatorArgs;
        private string excludedSteps;
        private string startActivityName;
        private string endActivityName;
        private string warnningAllocatorUsers;
        private string warnningExtendAllocators;
        private string warnningExtendAllocatorArgs;

		/// <summary>
        /// ID.
        /// </summary>
		public int Id
		{
			get{ return id; }
			set{ id = value; }
		}

		/// <summary>
        /// 流程Name.
        /// </summary>
        public string WorkflowName
		{
            get { return workflowName; }
            set { workflowName = value; }
		}

		/// <summary>
        /// 活动Name
        /// </summary>
        public string ActivityName
		{
            get { return activityName; }
            set { activityName = value; }
		}

        /// <summary>
        /// 紧急、重要单的控制参数.(00:一般 01:紧急 02:最紧急)
        /// </summary>
        public string ExtArgs
        {
            get { return extArgs; }
            set { extArgs = value; }
        }

		/// <summary>
        /// 允许滞留时间.
        /// </summary>
        public decimal StayHours
		{
            get { return stayHours; }
            set { stayHours = value; }
		}

        /// <summary>
        /// 预警提醒时间
        /// </summary>
        public decimal ToHours
        {
            get { return toHours; }
            set { toHours = value; }
        }

        /// <summary>
        /// 预警提醒时间间隔
        /// </summary>
        public decimal ToInterval
        {
            get { return toInterval; }
            set { toInterval = value; }
        }

        /// <summary>
        /// 预警提醒次数
        /// </summary>
        public int ToTimes
        {
            get { return toTimes; }
            set { toTimes = value; }
        }

        /// <summary>
        /// 超时提醒时间间隔
        /// </summary>
        public decimal TimeoutInterval
        {
            get { return timeoutInterval; }
            set { timeoutInterval = value; }
        }

        /// <summary>
        /// 超时提醒次数
        /// </summary>
        public int TimeoutTimes
        {
            get { return timeoutTimes; }
            set { timeoutTimes = value; }
        }

		/// <summary>
        /// 提醒方式(1电子邮件，2短信，3电子邮件+短信).
        /// </summary>
		public int RemindType
		{
			get{ return remindType; }
			set{ remindType = value; }
		}

		/// <summary>
        /// 计算类型（0：自然日 1：工作日(排除节假日) 2：客服工作日）.
        /// </summary>
        public int StatisticsType
		{
            get { return statisticsType; }
            set { statisticsType = value; }
		}

        /// <summary>
        /// 时效设置类型(0:工单时效 1:步骤时效)
        /// </summary>
        public int SettingType
        {
            get { return settingType; }
            set { settingType = value; }
        }

		/// <summary>
        /// 创建者用户名.
        /// </summary>
		public string Creator
		{
			get{ return creator; }
			set{ creator = value; }
		}

		/// <summary>
        /// 创建日期.
        /// </summary>
		public DateTime CreatedTime
		{
			get{ return createdTime; }
			set{ createdTime = value; }
		}

        /// <summary>
        /// 分配用户字符串（多个用户之间以逗号，即","或者"，"隔开）.
        /// </summary>
        public string AllocatorUsers
        {
            get { return allocatorUsers; }
            set { allocatorUsers = value; }
        }

        /// <summary>
        /// 扩展分配器名称字符串（多个分配器以逗号，即","或者"，"隔开,即"分配器名称1,分配器名称2"）.
        /// </summary>
        public string ExtendAllocators
        {
            get { return extendAllocators; }
            set { extendAllocators = value; }
        }

        /// <summary>
        /// 扩展分配器的输入参数字符串，分配器名称以逗号隔开（分配器参数表示为"分配器名称1:参数1,分配器名称2:参数2"）.
        /// </summary>
        public string ExtendAllocatorArgs
        {
            get { return extendAllocatorArgs; }
            set { extendAllocatorArgs = value; }
        }

        /// <summary>
        /// 排除步骤(工单时效考核类型)
        /// </summary>
        public string ExcludedSteps
        {
            get { return excludedSteps; }
            set { excludedSteps = value; }
        }

        /// <summary>
        /// 工单时效统计开始步骤
        /// </summary>
        public string StartActivityName
        {
            get { return startActivityName; }
            set { startActivityName = value; }
        }

        /// <summary>
        /// 工单时效统计结束步骤
        /// </summary>
        public string EndActivityName
        {
            get { return endActivityName; }
            set { endActivityName = value; }
        }

        /// <summary>
        /// 分配用户字符串(预警)（多个用户之间以逗号，即","或者"，"隔开）.
        /// </summary>
        public string WarnningAllocatorUsers
        {
            get { return warnningAllocatorUsers; }
            set { warnningAllocatorUsers = value; }
        }

        /// <summary>
        ///  扩展分配器的输入参数字符串，分配器名称以逗号隔开(预警)（分配器参数表示为"分配器名称1:参数1,分配器名称2:参数2"）.
        /// </summary>
        public string WarnningExtendAllocators
        {
            get { return warnningExtendAllocators; }
            set { warnningExtendAllocators = value; }
        }

        /// <summary>
        /// 扩展分配器的输入参数字符串，分配器名称以逗号隔开(预警)（分配器参数表示为"分配器名称1:参数1,分配器名称2:参数2"）.
        /// </summary>
        public string WarnningExtendAllocatorArgs
        {
            get { return warnningExtendAllocatorArgs; }
            set { warnningExtendAllocatorArgs = value; }
        }

        /// <summary>
        /// 期望完成时间
        /// </summary>
        public string ExpectFinishTime
        {
            get { return expectFinishTime; }
            set { expectFinishTime = value; }
        }
		#endregion		
		
        #region 数据操作
		
        /// <summary>
        /// 创建智能提醒设置.
        /// </summary>
        /// <returns></returns>
        public int Create()
        {
            if (!IsExists())
                IBatisMapper.Insert("sz_IntelligentRemind_Insert", this);
            else 
                Update();
            return this.Id;
        }

        /// <summary>
        /// 更新智能提醒设置.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("sz_IntelligentRemind_Update", this);
        }

        /// <summary>
        /// 删除智能提醒设置.
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return IBatisMapper.Delete("sz_IntelligentRemind_Delete", this.Id);
        }

        /// <summary>
        /// 获取指定流程步骤实例的智能提醒编号.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public static int GetIntelligentRemindId(Guid activityInstanceId)
        {
            object result = IBatisMapper.Mapper.QueryForObject("sz_IntelligentRemind_Select_Id_ByActivityInstanceId", activityInstanceId);
            if (result == null) // 不存在智能提醒设置时.
                return 0;
            return (int)result;
        }

        /// <summary>
        /// 获取指定流程, 紧急、重要程度 的智能提醒设置(步骤).
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static IList<SZIntelligentRemind> SelectByWorkflowId(string workflowName, string extArgs)
        {
            System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            htParams.Add("WorkflowName", workflowName);
            htParams.Add("ExtArgs", extArgs);
            return IBatisMapper.Select<SZIntelligentRemind>("sz_IntelligentRemind_Select", htParams);
        }

        /// <summary>
        /// 获取全部的智能提醒设置列表.
        /// </summary>
        /// <returns></returns>
        public static IList<SZIntelligentRemind> Select()
        {
            return IBatisMapper.Select<SZIntelligentRemind>("sz_IntelligentRemind_Select");
        }

        /// <summary>
        /// 获取指定智能提醒设置
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static SZIntelligentRemind SelectById(int id)
        {
            return IBatisMapper.Load<SZIntelligentRemind>("sz_IntelligentRemind_Select_By_Id", id);
        }

        /// <summary>
        /// 检查指定智能提醒设置是否存在.
        /// </summary>
        /// <returns></returns>
        public bool IsExists()
        {
            //System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            //htParams.Add("WorkflowName", this.WorkflowName);
            //htParams.Add("ActivityName", this.ActivityName);
            //htParams.Add("SettingType", this.SettingType);
            //htParams.Add("ExtArgs", this.ExtArgs);
            object result = null;
            //if(!string.IsNullOrEmpty(this.ActivityName))
            result = IBatisMapper.Mapper.QueryForObject("sz_IntelligentRemind_Select_IsExists", this.Id);
            //else
            //result = IBatisMapper.Mapper.QueryForObject("sz_IntelligentRemind_Select_Instance_IsExists", htParams);
            return (Botwave.Commons.DbUtils.ToInt32(result) > 0);
        }

        /// <summary>
        /// 获取指定流程,紧急重要程度 的智能提醒设置( 工单).
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static SZIntelligentRemind SelectInstanceByWorkflowId(string workflowName, string extArgs)
        {
            System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            htParams.Add("WorkflowName", workflowName);
            htParams.Add("ExtArgs", extArgs);
            return IBatisMapper.Load<SZIntelligentRemind>("sz_IntelligentRemind_Instance_Select", htParams);
        }

        /// <summary>
        /// 根据表单ID，时效设置类型获取指定流程的智能提醒设置.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static IList<SZIntelligentRemind> SelectByInstanceIdAndType(string workflowInstanceId, int settingType)
        {
            System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            htParams.Add("WorkflowInstanceId", workflowInstanceId);
            htParams.Add("SettingType", settingType);
            return IBatisMapper.Select<SZIntelligentRemind>("sz_IntelligentRemind_Select_By_WorkflowInstanceId_And_Type", htParams);
        }

        /// <summary>
        /// 更新指定流程,紧急重要程度 的智能提醒设置.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static int StatisticsTypeUpdateByWorkflowName(string workflowName, int statisticsType)
        {
            System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            htParams.Add("WorkflowName", workflowName);
            htParams.Add("StatisticsType", statisticsType);
            return IBatisMapper.Update("sz_IntelligentRemind_StatisticsType_Update", htParams);
        }

        public static DataRow GetNotifyFormat(string workflowname)
        {
            string sql = string.Format("SELECT STEPWARNINGNOTIFYFORMAT,STEPTIMEOUTNOTIFYFORMAT,WORKORDERWARNINGNOTIFYFORMAT,WORKORDERTIMEOUTNOTIFYFORMAT FROM xqp_workflowSettings WHERE WorkflowName = '{0}'", workflowname);
            DataTable dt = IBatisDbHelper.ExecuteDataset(CommandType.Text, sql).Tables[0];
            return dt.Rows.Count != 0 ? dt.Rows[0] : null;
        }

        public static int NotifyFormatUpdate(string[] arr)
        {
            System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            htParams.Add("StepWarningNotifyformat", arr[0]);
            htParams.Add("StepTimeoutNotifyformat", arr[1]);
            htParams.Add("WorkOrderWarningNotifyformat", arr[2]);
            htParams.Add("WorkOrderTimeoutNotifyformat", arr[3]);
            htParams.Add("WorkflowName", arr[4]);
            return IBatisMapper.Update("sz_IntelligentRemind_Notify_Update", htParams);
        }

        public static void SendTimerInfo(ActorDetail sender, WorkflowNotifyActor receiver, IDictionary<int, string> notifies, ActivityInstance activityInstance)
        {
            try
            {
                IBatisMapper.Mapper.BeginTransaction();
                //工单时效考核
                if (Botwave.Commons.DbUtils.ToDouble(IBatisMapper.Load<int>("sz_IntelligentRemind_Is_Instance", activityInstance.WorkflowInstanceId), -1) > 0)
                {
                    IDictionary<int, string> arr = new Dictionary<int, string>();
                    DataTable result = IBatisDbHelper.ExecuteDataset(CommandType.Text, string.Format(@"select messageid,NotifyType from sz_Reminders where workflowinstanceid = '{0}' and settingType = 0", activityInstance.WorkflowInstanceId)).Tables[0];
                    if (result.Rows.Count != 0)
                    {
                        foreach (DataRow dw in result.Rows)
                        {
                            arr.Add(int.Parse(dw["NotifyType"].ToString()),(string)dw["messageid"]);
                        }
                    }
                    foreach (KeyValuePair<int, string> messageBody in notifies)
                    {
                        if (messageBody.Key == 0 || messageBody.Key == 1)
                        {
                            if (!arr.ContainsKey(messageBody.Key))
                                IBatisMapper.Insert("sz_WorkflowReminders_Insert", GetMessageParameters(Guid.NewGuid().ToString(), sender.UserName, receiver.UserName, messageBody.Value, activityInstance.ActivityInstanceId.ToString(), activityInstance.WorkflowInstanceId.ToString(), messageBody.Key, 0));
                            else
                                IBatisMapper.Update("sz_WorkflowReminders_Update", GetMessageParameters(arr[messageBody.Key].ToString(), sender.UserName, receiver.UserName, messageBody.Value, activityInstance.ActivityInstanceId.ToString(), activityInstance.WorkflowInstanceId.ToString(), messageBody.Key, 0));
                        }
                    }
                }
                //步骤时效考核
                System.Collections.Hashtable htParams = new System.Collections.Hashtable();
                htParams.Add("WorkflowInstanceId", activityInstance.WorkflowInstanceId);
                htParams.Add("ActivityName", activityInstance.ActivityName);
                if (Botwave.Commons.DbUtils.ToDouble(IBatisMapper.Load<int>("sz_IntelligentRemind_Is_Activity", htParams), -1) > 0)
                {
                    foreach (KeyValuePair<int, string> messageBody in notifies)
                    {
                        if (messageBody.Key != 0 && messageBody.Key != 1)
                            IBatisMapper.Insert("sz_WorkflowReminders_Insert", GetMessageParameters(Guid.NewGuid().ToString(), sender.UserName, receiver.UserName, messageBody.Value, activityInstance.ActivityInstanceId.ToString(), activityInstance.WorkflowInstanceId.ToString(), messageBody.Key, 1));
                    }
                }
                IBatisMapper.Mapper.CommitTransaction();
            }
            catch (Exception ex)
            {
                IBatisMapper.Mapper.RollBackTransaction();
                log.Error("[Error in SZIntelligentRemind.SendTimerInfo]" + ex.ToString());
            }
        }

        /// <summary>
        /// 删除sz_reminders 的已完成实例 
        /// </summary>
        /// <param name="workflowinstanceid"></param>
        /// <returns></returns>
        public static int TimerInfoDelete(string workflowinstanceid)
        {
            return IBatisMapper.Delete("sz_WorkflowReminders_Delete", workflowinstanceid);
        }

        /// <summary>
        /// 统计步骤的处理时间
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public static DataTable GetTimeRateReprotAll(string where, int pageIndex, int pageSize, ref int recordCount)
        {
            string tableName = "vw_bwwf_cz_report_timerate_all";
            string fieldKey = "ActivityInstanceId";
            string fieldShow = @"workflowinstanceid,activityinstanceid,workflowname,sheetid,title,activityname, realname,createdtime,finishedtime,hours,stayhours,istimeout";
            string fieldOrder = "sheetid ASC, CreatedTime DESC";

            return IBatisDbHelper.GetPagedList(tableName, fieldKey, pageIndex, pageSize, fieldShow, fieldOrder, where, ref recordCount);
        }
        /// <summary>
        /// 获取提醒信息插入数据参数集合.
        /// </summary>
        /// <param name="messageFrom"></param>
        /// <param name="messageTo"></param>
        /// <param name="MessageType"></param>
        /// <param name="messageBody"></param>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        private static System.Collections.Hashtable GetMessageParameters(string guid, string messageFrom, string messageTo, string messageBody, string activityInstanceId, string workflowinstanceid, int notifyType, int type)
        {
            System.Collections.Hashtable parameters = new System.Collections.Hashtable();
            parameters.Add("MessageId", guid);
            parameters.Add("MessageFrom", messageFrom);
            parameters.Add("MessageTo", messageTo);
            parameters.Add("MessageBody", messageBody);
            parameters.Add("ActivityInstanceId", activityInstanceId);
            parameters.Add("WorkflowInstanceId", workflowinstanceid);
            parameters.Add("NotifyType", notifyType);
            parameters.Add("SettingType", type);
            return parameters;
        }
        #endregion
    }
}
	
