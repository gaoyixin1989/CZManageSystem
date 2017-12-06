using System;
using System.Collections.Generic;
using System.Text;
using Botwave.Extension.IBatisNet;

namespace Botwave.XQP.Domain
{
    /// <summary>
    /// 消息通知时间块.
    /// </summary>
    public class CZReminderTimeSpan
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(CZReminderTimeSpan));
        #region gets /sets
        private int _timeId;
        private Guid _entityId;
        private int _beginHours;
        private int _beginMinutes;
        private int _endHours;
        private int _endMinutes;
        private int _remindType;

        /// <summary>
        /// 编号.
        /// </summary>
        public int TimeId
        {
            get { return _timeId; }
            set { _timeId = value; }
        }

        /// <summary>
        /// 外联ID.
        /// </summary>
        public Guid EntityId
        {
            get { return _entityId; }
            set { _entityId = value; }
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
        /// 提醒类型（0:待办+待阅;1:待办;2:待阅）
        /// </summary>
        public int RemindType
        {
            get { return _remindType; }
            set { _remindType = value; }
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
        public CZReminderTimeSpan()
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="beginHours"></param>
        /// <param name="beginMinutes"></param>
        /// <param name="endHours"></param>
        /// <param name="endMinutes"></param>
        public CZReminderTimeSpan(int beginHours, int beginMinutes, int endHours, int endMinutes)
            : this(Guid.Empty, beginHours, beginMinutes, endHours, endMinutes)
        { }

        /// <summary>
        /// 构造方法.
        /// </summary>
        /// <param name="workflowName"></param>
        /// <param name="beginHours"></param>
        /// <param name="beginMinutes"></param>
        /// <param name="endHours"></param>
        /// <param name="endMinutes"></param>
        public CZReminderTimeSpan(Guid entityId, int beginHours, int beginMinutes, int endHours, int endMinutes)
        {
            this._entityId = entityId;
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
            IBatisMapper.Insert("cz_ReminderTimespan_Insert", this);
        }

        /// <summary>
        /// 更新时间段.
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
            return IBatisMapper.Update("cz_ReminderTimespan_Update", this);
        }

        /// <summary>
        /// 删除指定编号时间段.
        /// </summary>
        /// <param name="timeId"></param>
        /// <returns></returns>
        public static int Delete(int timeId)
        {
            return IBatisMapper.Delete("cz_ReminderTimespan_Delete", timeId);
        }

        /// <summary>
        /// 选择指定的时间段实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<CZReminderTimeSpan> Select(Guid entityId)
        {
            try
            {
                return IBatisMapper.Select<CZReminderTimeSpan>("cz_ReminderTimespan_Select_By_EntityId", entityId);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
                return new List<CZReminderTimeSpan>();
            }
        }

        /// <summary>
        /// 根据步骤实例ID选择指定的时间段实例.
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public static IList<CZReminderTimeSpan> SelectByActivityInstanceId(Guid entityId)
        {
            try
            {
                return IBatisMapper.Select<CZReminderTimeSpan>("cz_ReminderTimespan_Select_By_Aiid", entityId);
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return new List<CZReminderTimeSpan>();
            }
        }
    }
}
