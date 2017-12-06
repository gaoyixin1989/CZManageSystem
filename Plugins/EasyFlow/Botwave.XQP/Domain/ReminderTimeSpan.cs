using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 消息通知时间块.
    /// </summary>
    public class ReminderTimeSpan
    {
        #region gets /sets
        private int _timeId;
        private string _workflowName;
        private int _beginHours;
        private int _beginMinutes;
        private int _endHours;
        private int _endMinutes;

        /// <summary>
        /// 编号.
        /// </summary>
        public int TimeId
        {
            get { return _timeId; }
            set { _timeId = value; }
        }

        /// <summary>
        /// 流程名称.
        /// </summary>
        public string WorkflowName
        {
            get { return _workflowName; }
            set { _workflowName = value; }
        }

        /// <summary>
        /// 起始时间的小时数.
        /// </summary>
        public int BeginHours
        {
            get { return _beginHours; }
            set { _beginHours = value; }
        }

        /// <summary>
        /// 起始时间的分钟数.
        /// </summary>
        public int BeginMinutes
        {
            get { return _beginMinutes; }
            set { _beginMinutes = value; }
        }

        /// <summary>
        /// 截止时间的小时数.
        /// </summary>
        public int EndHours
        {
            get { return _endHours; }
            set { _endHours = value; }
        }

        /// <summary>
        /// 截止时间的分钟数.
        /// </summary>
        public int EndMinutes
        {
            get { return _endMinutes; }
            set { _endMinutes = value; }
        }

        /// <summary>
        /// 获取时间段起始时间.只读.
        /// </summary>
        public TimeSpan BeginTimeSpan
        {
            get { return new TimeSpan(this.BeginHours, BeginMinutes, 0); }
        }

        /// <summary>
        /// 获取时间段截止时间.只读.
        /// </summary>
        public TimeSpan EndTimeSpan
        {
            get { return new TimeSpan(this.EndHours, this.EndMinutes, 0); }
        }

        #endregion

        #region 构造方法
        /// <summary>
        /// 构造方法.
        /// </summary>
        public ReminderTimeSpan()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="beginHours"></param>
        /// <param name="beginMinutes"></param>
        /// <param name="endHours"></param>
        /// <param name="endMinutes"></param>
        public ReminderTimeSpan(int beginHours, int beginMinutes, int endHours, int endMinutes)
            : this(null, beginHours, beginMinutes, endHours, endMinutes)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="beginHours"></param>
        /// <param name="beginMinutes"></param>
        /// <param name="endHours"></param>
        /// <param name="endMinutes"></param>
        public ReminderTimeSpan(string workflowName, int beginHours, int beginMinutes, int endHours, int endMinutes)
        {
            this._workflowName = workflowName;
            this._beginHours = beginHours;
            this._beginMinutes = beginMinutes;
            this._endHours = endHours;
            this._endMinutes = endMinutes;
        }

        #endregion

        /// <summary>
        /// 指定时间是否正确属于当前时间段.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public bool IsRightTime(DateTime current)
        {
            TimeSpan currentTime = current.TimeOfDay;
            if (currentTime >= this.BeginTimeSpan && currentTime < this.EndTimeSpan)
                return true;
            return false;
        }

        /// <summary>
        /// 创建时间段.
        /// </summary>
        public void Insert()
        {
            IBatisMapper.Insert("xqp_ReminderTimespan_Insert", this);
        }

        /// <summary>
        /// 更新时间段.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("xqp_ReminderTimespan_Update", this);
        }

        /// <summary>
        /// 删除指定编号时间段.
        /// </summary>
        /// <param name="timeId"></param>
        /// <returns></returns>
        public static int Delete(int timeId)
        {
            return IBatisMapper.Delete("xqp_ReminderTimespan_Delete", timeId);
        }

        /// <summary>
        /// 选择指定流程的时间段实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<ReminderTimeSpan> Select(Guid workflowId)
        {
            return IBatisMapper.Select<ReminderTimeSpan>("xqp_ReminderTimespan_Select_By_WorkflowId", workflowId);
        }
    }
}
