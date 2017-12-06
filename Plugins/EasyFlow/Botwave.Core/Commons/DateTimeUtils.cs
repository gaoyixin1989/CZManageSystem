using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.Commons
{
    /// <summary>
    /// ����/ʱ�丨����.
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// ��Сʱ��ֵ.
        /// </summary>
        public static readonly DateTime MinValue = new DateTime(1900, 1, 1);

        /// <summary>
        /// ���ʱ��ֵ.
        /// </summary>
        public static readonly DateTime MaxValue = new DateTime(3000, 1, 1);

        /// <summary>
        /// ��ʱ�����Ϊ�׶��ķ�ʽ.
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
            int i = dt.CompareTo(target);   //�Ƚ�ʱ���С
            if (i > 0)
                tsResult = tsOriginal.Subtract(tsTarget);
            else
                tsResult = tsTarget.Subtract(tsOriginal);

            if (TimeSpan.Zero != tsResult)
            {
                if (!tsResult.Days.Equals(0))
                {
                    //���ʱ�����2��ʱ���� ����
                    if (tsResult.Days >= 2)
                        return target.ToShortDateString();
                    //���ڵ���1��ʱ���� "��+Сʱ"
                    else if (tsResult.Days >= 1)
                        strResult = tsResult.Days + "��" + tsResult.Hours + "Сʱ";
                }
                else if (!tsResult.Hours.Equals(0))
                    strResult = tsResult.Hours + "Сʱ";
                else if (!tsResult.Minutes.Equals(0))
                    strResult = tsResult.Minutes + "����";
                else if (!tsResult.Seconds.Equals(0))
                    strResult = tsResult.Seconds + "��";

                //Դʱ�����Ŀ��ʱ��ʱ,���� "���ʱ��+֮ǰ"
                if (i > 0)
                    return strResult + "֮ǰ";
                //Դʱ��С��Ŀ��ʱ��ʱ,���� "���ʱ��+֮��"
                else
                    return strResult + "֮��";
            }
            return String.Empty;
        }

        /// <summary>
        /// �Ƚ�����ʱ�䣬������Ϊ�׶��ķ�ʽ.
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
                return tsResult.Days + "��" + tsResult.Hours + "Сʱ";
            else if (!tsResult.Hours.Equals(0))
                return tsResult.Hours + "Сʱ" + tsResult.Minutes + "����";
            else if (!tsResult.Minutes.Equals(0))
                return tsResult.Minutes + "����";
            else if (!tsResult.Seconds.Equals(0))
                return tsResult.Seconds + "��";

            return String.Empty;
        }

        /// <summary>
        /// �Ƚ�����ʱ�䣬����ǵ�������ʾʱ��,������ʾ����.
        /// </summary>
        /// <param name="currentTime">��ǰʱ��</param>
        /// <param name="targetTime">��Ҫ������ʱ��</param>
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
