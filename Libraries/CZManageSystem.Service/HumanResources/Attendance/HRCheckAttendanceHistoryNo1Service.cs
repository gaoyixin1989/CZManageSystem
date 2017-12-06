using CZManageSystem.Core;
using CZManageSystem.Data;
using CZManageSystem.Data.Domain.HumanResources.Attendance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CZManageSystem.Service.HumanResources.Attendance
{
    public class HRCheckAttendanceHistoryNo1Service : BaseService<HRCheckAttendanceHistoryNo1>, IHRCheckAttendanceHistoryNo1Service
    {

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="count">返回记录总数</param>
        /// <param name="objs">条件数组</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public override IEnumerable<dynamic> GetForPaging(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.AtDate) : this._entityStore.Table.OrderByDescending(c => c.AtDate).Where(ExpressionFactory(objs));
                PagedList<HRCheckAttendanceHistoryNo1> pageList = new PagedList<HRCheckAttendanceHistoryNo1>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    AtDate = GetDate(s.AtDate),
                    s.HistoryId ,
                    s.DoFlag,
                    s.DoReallyTime,
                    s.DoTime,
                    s.FlagOff,
                    s.FlagOn,
                    s.IpOff,
                    s.IpOn,
                    s.Minute,
                    s.OffReallyTime,
                    s.OffTime,
                    s.RotateDaysOffFlag,
                    s.UserId,
                    s.Users?.RealName,
                    s.Users?.EmployeeId,
                    s.Users?.Dept?.DpName,
                    
                    State = CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute)
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IEnumerable<dynamic> GetForPaging_(out int count, object objs = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            try
            {
                var source = objs == null ? this._entityStore.Table.OrderByDescending(c => c.AtDate) : this._entityStore.Table.OrderByDescending(c => c.AtDate).Where(ExpressionFactory(objs));
                source.Where(w => w.DoReallyTime != null || w.OffReallyTime != null);
                PagedList<HRCheckAttendanceHistoryNo1> pageList = new PagedList<HRCheckAttendanceHistoryNo1>(source, pageIndex <= 0 ? 0 : (pageIndex - 1), pageSize, out count);
                count = pageList.TotalCount;
                return pageList.Select(s => new
                {
                    AtDate = GetDate(s.AtDate),
                    s.HistoryId,
                    s.DoFlag,
                    s.DoReallyTime,
                    s.DoTime,
                    s.FlagOff,
                    s.FlagOn,
                    s.IpOff,
                    s.IpOn,
                    s.Minute,
                    s.OffReallyTime,
                    s.OffTime,
                    s.RotateDaysOffFlag,
                    s.UserId,
                    s.Users?.RealName,
                    s.Users?.EmployeeId,
                    s.Users?.Dept?.DpName,
                    State = CheckDuty(s.AtDate, s.DoTime, s.OffTime, s.DoReallyTime, s.OffReallyTime, s.Minute)
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #region 方法
        string GetDate(DateTime? date)
        {
            if (date == null)
                return "--";
            DateTime dt = Convert.ToDateTime(date);
            return string.Format("{0}({1})", dt.ToString("yyyy-MM-dd"), System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dt.DayOfWeek));
        }

        /// <summary>
        /// 返回考勤状态
        /// </summary> 
        /// <param name="atDate">日期</param>
        /// <param name="doTime">上班时间</param>
        /// <param name="offTime">下班时间</param>
        /// <param name="doReallyTime">上班签到时间</param>
        /// <param name="offReallyTime">下班签到时间</param>
        /// <param name="_minute">时间间隔</param>
        /// <returns>状态 0 :正常、1:迟到、2:早退、3:旷工、4:其他、5：休假</returns>
        string CheckDuty(DateTime? atDate, TimeSpan? doTime, TimeSpan? offTime, TimeSpan? doReallyTime, TimeSpan? offReallyTime, int? minute)
        {
            //没有上下班时间
            if (!doTime.HasValue && !offTime.HasValue)
                return CheckStateResult.HaveAHoliday;
            int _minute = minute == null ? 0 : Convert.ToInt32(minute);

            TimeSpan _doTime = new TimeSpan(doTime.Value.Hours, doTime.Value.Minutes + _minute, doTime.Value.Seconds);
            TimeSpan _offTime = new TimeSpan(offTime.Value.Hours, offTime.Value.Minutes - _minute, offTime.Value.Seconds);
            //if (offTime != null) _offTime = DateTime.Parse(atDate + " " + offTime);
            //旷工的情况,签到签退都没有记录
            if (doReallyTime == null && offReallyTime == null)
            {
                //如果系统时间小于下班时间算其他，即有可能是当天的记录还未签到（包括未到签到时间或是过了上班时间但未超过下班时间）
                if (IsNowaday(atDate, _offTime, offReallyTime, false))
                    return CheckStateResult.Other;
                return CheckStateResult.Absenteeism;
            }

            //只有签到或签退记录并且过了上班时间,属于其他
            var now = DateTime.Now;
            if ((!doReallyTime.HasValue || !offReallyTime.HasValue) && new TimeSpan(now.Hour, now.Minute, now.Second).CompareTo(_doTime) >= 0)
                return CheckStateResult.Other;

            //迟到又早退的情况，属于其他 
            if (doReallyTime.HasValue && offReallyTime.HasValue && doReallyTime.Value.CompareTo(_doTime) > 0 && offReallyTime.Value.CompareTo(_offTime) < 0)
                return CheckStateResult.Other;

            //迟到
            if ((doReallyTime.Value.CompareTo(_doTime) > 0 && (IsNowaday(atDate, _offTime, offReallyTime) || offReallyTime.Value.CompareTo(_offTime) >= 0)))
                return CheckStateResult.BeLate;


            //早退 
            if ((doReallyTime.Value.CompareTo(_doTime) < 0 && offReallyTime.Value.CompareTo(_offTime) < 0))
                return CheckStateResult.Tardy;


            return CheckStateResult.Normal;
        }
        /// <summary>
        /// 是否为今天的考勤记录
        /// </summary>
        /// <param name="atDate">日期</param>
        /// <returns></returns>
        bool IsNowaday(DateTime? atDate, TimeSpan? offTime, TimeSpan? offReallyTime, bool isTrue = true)
        {
            var now = DateTime.Now;
            //下班时间不为空或者不是当天日期的考勤
            if (offReallyTime.HasValue || now.ToString("yyyy-MM-dd") != DateTime.Parse(atDate?.ToString()).ToString("yyyy-MM-dd"))
                return false;
            //当前时间小于等于下班时间
            if (isTrue)
            {
                if (new TimeSpan(now.Hour, now.Minute, now.Second).CompareTo(offTime) <= 0)
                    return true;
            }
            else
            {
                //当前时间大于下班时间
                if (new TimeSpan(now.Hour, now.Minute, now.Second).CompareTo(offTime) > 0)
                    return true;
            }
            return false;
        }
        #endregion
    }
}
