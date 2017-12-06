using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Commons
{
    /// <summary>
    /// 日期/时间辅助类.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// 最小时间值.
        /// </summary>
        public static readonly DateTime MinValue = new DateTime(1900, 1, 1);

        /// <summary>
        /// 最大时间值.
        /// </summary>
        public static readonly DateTime MaxValue = new DateTime(3000, 1, 1);

        /// <summary>
        /// 将时间解析为易读的方式.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Resolve(DateTime dt, DateTime target)
        {
            if (dt == DateTime.MinValue || target == DateTime.MinValue || dt == target)
                return String.Empty;

            TimeSpan tsOriginal = new TimeSpan(dt.Ticks);
            TimeSpan tsTarget = new TimeSpan(target.Ticks);
            TimeSpan tsResult = TimeSpan.Zero;

            string strResult = String.Empty;
            int i = dt.CompareTo(target);   //比较时间大小
            if (i > 0)
                tsResult = tsOriginal.Subtract(tsTarget);
            else
                tsResult = tsTarget.Subtract(tsOriginal);

            if (TimeSpan.Zero != tsResult)
            {
                if (!tsResult.Days.Equals(0))
                {
                    //间隔时间大于2天时返回 日期
                    if (tsResult.Days >= 2)
                        return target.ToShortDateString();
                    //大于等于1天时返回 "天+小时"
                    else if (tsResult.Days >= 1)
                        strResult = tsResult.Days + "天" + tsResult.Hours + "小时";
                }
                else if (!tsResult.Hours.Equals(0))
                    strResult = tsResult.Hours + "小时";
                else if (!tsResult.Minutes.Equals(0))
                    strResult = tsResult.Minutes + "分钟";
                else if (!tsResult.Seconds.Equals(0))
                    strResult = tsResult.Seconds + "秒";

                //源时间大于目标时间时,返回 "间隔时间+之前"
                if (i > 0)
                    return strResult + "之前";
                //源时间小于目标时间时,返回 "间隔时间+之内"
                else
                    return strResult + "之内";
            }
            return String.Empty;
        }

        /// <summary>
        /// 比较两个时间，并解析为易读的方式.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ResolveInterval(DateTime dt, DateTime target)
        {
            if (dt == DateTime.MinValue || target == DateTime.MinValue || dt == target)
                return String.Empty;

            TimeSpan tsOriginal = new TimeSpan(dt.Ticks);
            TimeSpan tsTarget = new TimeSpan(target.Ticks);
            TimeSpan tsResult = tsResult = tsOriginal.Subtract(tsTarget);

            if (!tsResult.Days.Equals(0))
                return tsResult.Days + "天" + tsResult.Hours + "小时";
            else if (!tsResult.Hours.Equals(0))
                return tsResult.Hours + "小时" + tsResult.Minutes + "分钟";
            else if (!tsResult.Minutes.Equals(0))
                return tsResult.Minutes + "分钟";
            else if (!tsResult.Seconds.Equals(0))
                return tsResult.Seconds + "秒";

            return String.Empty;
        }

        /// <summary>
        /// 比较两个时间，如果是当天则显示时间,否则显示日期.
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <param name="targetTime">需要分析的时间</param>
        /// <returns></returns>
        public static string ParseDateTime(DateTime currentTime, DateTime targetTime)
        {
            string description = String.Empty;
            if (targetTime.Date == currentTime.Date)
            {
                description = targetTime.ToString("HH:mm");
            }
            else
            {
                description = targetTime.ToString("yyyy-MM-dd");
            }
            return description;
        }
    }
}
