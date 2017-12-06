using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
	/// <summary>
	/// [xqp_IntelligentRemind] 的实体类.
	/// 创建日期: 2008-5-31
	/// </summary>
	public class IntelligentRemind
	{
		#region Getter / Setter
		
		private int id;
        private string workflowName = String.Empty;
        private string activityName = String.Empty;
        private string extArgs = String.Empty;
		private int stayHours;
		private string remindType = String.Empty;
		private int remindTimes;
		private string creator = String.Empty;
		private DateTime createdTime;
		
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
        /// 紧急、重要单的控制参数.
        /// </summary>
        public string ExtArgs
        {
            get { return extArgs; }
            set { extArgs = value; }
        }

		/// <summary>
        /// 允许滞留时间.
        /// </summary>
		public int StayHours
		{
            get { return stayHours; }
            set { stayHours = value; }
		}

		/// <summary>
        /// 提醒方式(1电子邮件，2短信，3电子邮件+短信).
        /// </summary>
		public string RemindType
		{
			get{ return remindType; }
			set{ remindType = value; }
		}

		/// <summary>
        /// 提醒次数（-1表无限制）.
        /// </summary>
		public int RemindTimes
		{
			get{ return remindTimes; }
			set{ remindTimes = value; }
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
		
		#endregion		
		
        #region 数据操作
		
        /// <summary>
        /// 创建智能提醒设置.
        /// </summary>
        /// <returns></returns>
        public int Create()
        {
            if (!IsExists())
                IBatisMapper.Insert("xqp_IntelligentRemind_Insert", this);
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
            return IBatisMapper.Update("xqp_IntelligentRemind_Update", this);
        }

        /// <summary>
        /// 删除智能提醒设置.
        /// </summary>
        /// <returns></returns>
        public int Delete()
        {
            return IBatisMapper.Delete("xqp_IntelligentRemind_Delete", this.Id);
        }

        /// <summary>
        /// 获取指定流程步骤实例的智能提醒编号.
        /// </summary>
        /// <param name="activityInstanceId"></param>
        /// <returns></returns>
        public static int GetIntelligentRemindId(Guid activityInstanceId)
        {
            object result = IBatisMapper.Mapper.QueryForObject("xqp_IntelligentRemind_Select_Id_ByActivityInstanceId", activityInstanceId);
            if (result == null) // 不存在智能提醒设置时.
                return 0;
            return (int)result;
        }

        /// <summary>
        /// 获取指定流程的智能提醒设置.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <returns></returns>
        public static IList<IntelligentRemind> SelectByWorkflowId(string workflowName)
        {
            return IBatisMapper.Select<IntelligentRemind>("xqp_IntelligentRemind_Select", workflowName);
        }

        /// <summary>
        /// 获取全部的智能提醒设置列表.
        /// </summary>
        /// <returns></returns>
        public static IList<IntelligentRemind> Select()
        {
            return IBatisMapper.Select<IntelligentRemind>("xqp_IntelligentRemind_Select");
        }

        /// <summary>
        /// 检查指定智能提醒设置是否存在.
        /// </summary>
        /// <returns></returns>
        public bool IsExists()
        {
            System.Collections.Hashtable htParams = new System.Collections.Hashtable();
            htParams.Add("WorkflowName", this.WorkflowName);
            htParams.Add("ActivityName", this.ActivityName);
            object result = IBatisMapper.Mapper.QueryForObject("xqp_IntelligentRemind_Select_IsExists", htParams);
            return (result != null);
        }
        #endregion
    }
}
	
