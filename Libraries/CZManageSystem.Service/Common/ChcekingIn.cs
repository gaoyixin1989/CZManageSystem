using CZManageSystem.Data;
using CZManageSystem.Service.HumanResources.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Service.Common
{
    public class ChcekingIn
    {
        IHRCheckAttendanceService hrCheckAttendanceService = new HRCheckAttendanceService();
        #region 方法
        public static string GetDate(DateTime? date)
        {
            if (date == null)
                return "--";
            DateTime dt = Convert.ToDateTime(date);
            return string.Format("{0}({1})", dt.ToString("yyyy-MM-dd"), System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dt.DayOfWeek));
        }
        public static string GetDateTime(DateTime? date)
        {
            if (date == null)
                return "--";
            DateTime dt = Convert.ToDateTime(date);
            return string.Format("{0}({1})", dt.ToString("yyyy-MM-dd HH:mm"), System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(dt.DayOfWeek));
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
        public static string CheckDuty(DateTime? atDate, TimeSpan? doTime, TimeSpan? offTime, TimeSpan? doReallyTime, TimeSpan? offReallyTime, int? minute, int? doFlag, int? RotateDaysOffFlag)
        {
            try
            {
                if (doFlag != null)
                {
                    //0、申报中 1、已申报；2、休假；3、外出
                    switch (doFlag)
                    {
                        case 0:
                            return CheckStateResult.InTheDeclaration;
                        case 1:
                            return CheckStateResult.HaveBeenDeclared;
                        case 2:
                            return CheckStateResult.HaveAHoliday;
                        case 3:
                            return CheckStateResult.GoOut;
                        default:
                            return CheckStateResult.Other; ;
                    }
                }
                //没有上下班时间
                if (!doTime.HasValue && !offTime.HasValue)
                {
                    if (RotateDaysOffFlag != null && RotateDaysOffFlag == 1)
                        return CheckStateResult.HaveAHolidaysByTurns;
                    return CheckStateResult.NormalTest;

                }
                int _minute = minute == null ? 0 : Convert.ToInt32(minute);
                var now = DateTime.Now;
                TimeSpan _doTime = new TimeSpan(doTime.Value.Hours, doTime.Value.Minutes + _minute, doTime.Value.Seconds);
                TimeSpan _offTime = new TimeSpan(offTime.Value.Hours, offTime.Value.Minutes - _minute, offTime.Value.Seconds);

                //if (offTime != null) _offTime = DateTime.Parse(atDate + " " + offTime);
                //旷工的情况,签到签退都没有记录
                if (!doReallyTime.HasValue && !offReallyTime.HasValue)
                {
                    //如果系统时间小于下班时间算其他，即有可能是当天的记录还未签到（包括未到签到时间或是过了上班时间但未超过下班时间）
                    if (IsNowaday(atDate, _offTime, doReallyTime))//是否在上班前或者下班的时间。是的话就是还没开始上班或者还没下班
                        return CheckStateResult.Other;
                    // if (IsNowaday(atDate, _offTime, null,false ))//过了下班时间没有上班签到记录就意味着是矿工 
                    return CheckStateResult.Absenteeism;
                }
                #region !doReallyTime.HasValue && offReallyTime.HasValue


                //有上班签到没有下班签到
                if (doReallyTime.HasValue && !offReallyTime.HasValue)
                {
                    //没有迟到 
                    if (doReallyTime.Value.CompareTo(_doTime) <= 0)
                    {
                        if (IsNowaday(atDate, _offTime, null) || IsNowaday(atDate, _offTime, null, false))//是当天还未下班或者下班了但还没来得急签到
                            return CheckStateResult.Normal;
                        //不是当天的，那就是没有下班签到。算其他？
                        return CheckStateResult.Other;
                    }
                    //下面是在迟到的情况下没有下班签到的情况：
                    //1.是当天，还未到下班时间。  
                    if (IsNowaday(atDate, _offTime, null))
                        //这暂时属于迟到
                        return CheckStateResult.BeLate;
                    //2.到了下班时间却没有签到 。算其他
                    return CheckStateResult.Other;

                }


                //没有上班签到有下班签到。
                if (!doReallyTime.HasValue && offReallyTime.HasValue)//没有上班签到有下班签到。有下班签到说明上班签到时间已经过了，但却没有上班签到 1.忘记上班签到
                {
                    if (IsNowaday(atDate, _doTime, null))//当天早上的话就是还没到考勤时间
                        return CheckStateResult.Other;
                    if (IsNowaday(atDate, _doTime, null, false))//当天早上的话并且已经过了上班签到时间
                    {
                        if (IsNowaday(atDate, _offTime, null))//还没到下班时间
                            return CheckStateResult.Other;
                        if (IsNowaday(atDate, _offTime, null, false))//过了下班时间还未签到
                            return CheckStateResult.Other;
                    }
                    if (offReallyTime.Value.CompareTo(_offTime) < 0)
                        return CheckStateResult.Other;//早退 
                    return CheckStateResult.Other;//下班正常
                }

                #endregion

                //上下班都有签到
                if (doReallyTime.HasValue && offReallyTime.HasValue)
                {
                    //上班时间正常
                    if (doReallyTime.Value.CompareTo(_doTime) <= 0)
                    {
                        if (offReallyTime.Value.CompareTo(_offTime) >= 0)//下班签到正常
                            return CheckStateResult.Normal;
                        if (offReallyTime.Value.CompareTo(_offTime) < 0)//下班签到过早
                            return CheckStateResult.Tardy;
                    }
                    //迟到
                    if (offReallyTime.Value.CompareTo(_offTime) >= 0)//下班签到正常
                        return CheckStateResult.BeLate;//迟到
                    if (offReallyTime.Value.CompareTo(_offTime) < 0)//下班签到过早
                        return CheckStateResult.Other;//迟到又早退
                }



                //只有签到或签退记录并且过了上班时间,属于其他
                if ((!doReallyTime.HasValue || !offReallyTime.HasValue) && new TimeSpan(now.Hour, now.Minute, now.Second).CompareTo(_doTime) >= 0)
                    return CheckStateResult.Other;

                #region MyRegion
                ////迟到又早退的情况，属于其他 
                //if (doReallyTime.HasValue && offReallyTime.HasValue && doReallyTime.Value.CompareTo(_doTime) > 0 && offReallyTime.Value.CompareTo(_offTime) < 0)
                //    return CheckStateResult.Other;

                ////迟到： 
                //if (
                //    //1.有签到但时间已经过了上班时间，下班时间是正常的(包过当天的还没到下班时间却已经迟到的)。
                //    ((doReallyTime.HasValue && doReallyTime.Value.CompareTo(_doTime) > 0 && ((offReallyTime.HasValue && offReallyTime.Value.CompareTo(_offTime) >= 0) || (!offReallyTime.HasValue && IsNowaday(atDate, _offTime, doReallyTime, false))))//有签到但时间已经过了上班时间
                //      ||//2.没签到上班，下班时间是正常的(包过当天的还没到下班时间却已经迟到的)。
                //     (!doReallyTime.HasValue //没有签到
                //     && IsNowaday(atDate, _doTime, doReallyTime, false)//看是否为当天的，如果是，而且时间已经超过上班时间那就是迟到
                //     && ((offReallyTime.HasValue && offReallyTime.Value.CompareTo(_offTime) >= 0) || (!offReallyTime.HasValue && IsNowaday(atDate, _offTime, doReallyTime, false)))))
                //    )
                //    return CheckStateResult.BeLate;


                ////早退 
                //if (doReallyTime.HasValue && doReallyTime.Value.CompareTo(_doTime) < 0 && offReallyTime.HasValue && offReallyTime.Value.CompareTo(_offTime) < 0)
                //    return CheckStateResult.Tardy; 
                #endregion


                return CheckStateResult.Normal;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// 是否为今天的考勤记录
        /// </summary>
        /// <param name="atDate">日期</param>
        /// <returns></returns>
        static bool IsNowaday(DateTime? atDate, TimeSpan? compareTime, TimeSpan? reallyTime, bool isTrue = true)
        {
            var now = DateTime.Now;
            //下班时间不为空或者不是当天日期的考勤
            if (reallyTime.HasValue || now.ToString("yyyy-MM-dd") != DateTime.Parse(atDate?.ToString()).ToString("yyyy-MM-dd"))
                return false;
            var timeSpanNow = new TimeSpan(now.Hour, now.Minute, now.Second);
            //当前时间小于等于要比较的时间
            if (isTrue)
            {
                if (timeSpanNow.CompareTo(compareTime) <= 0)
                    return true;
            }
            else
            {
                //当前时间大于比较的时间
                if (timeSpanNow.CompareTo(compareTime) > 0)
                    return true;
            }
            return false;
        }
        #endregion
        public IEnumerable<string> GetDpIdList(string parameters)
        {
            try
            {
                string sql = "SELECT DpId  FROM getDeptByPid('" + parameters + "')";
                return hrCheckAttendanceService.Execute<string>(sql).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
