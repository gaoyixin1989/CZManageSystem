using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data
{
    #region CheckStateResult
    /// <summary>
    /// 返回考勤状态 0 :正常、1:迟到、2:早退、3:旷工、4:其他、5：休假
    /// </summary>
    public class CheckStateResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        public static readonly string Normal = "正常";
        /// <summary>
        /// 迟到
        /// </summary>
        public static readonly string BeLate = "迟到";
        /// <summary>
        /// 早退
        /// </summary>
        public static readonly string Tardy = "早退";
        /// <summary>
        /// 旷工
        /// </summary>
        public static readonly string Absenteeism = "旷工";
        /// <summary>
        /// 其他
        /// </summary>
        public static readonly string Other = "其他";
        /// <summary>
        /// 休假
        /// </summary>
        public static readonly string HaveAHoliday = "休假";
        /// <summary>
        /// 正常休息
        /// </summary>
        public static readonly string NormalTest = "正常休息";
        /// <summary>
        /// 轮班休息
        /// </summary>
        public static readonly string HaveAHolidaysByTurns = "轮班休息";
        /// <summary>
        /// 外出
        /// </summary>
        public static readonly string GoOut = "外出";
        /// <summary>
        /// 已申报
        /// </summary>
        public static readonly string HaveBeenDeclared = "已申报";
        /// <summary>
        /// 已申报
        /// </summary>
        public static readonly string InTheDeclaration = "申报中";
    }
    #endregion

}
