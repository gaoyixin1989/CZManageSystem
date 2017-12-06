using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class SummarizingList
    {
        public Guid UserId { get; set; }
        public string RealName { get; set; }
        public string EmployeeId { get; set; }
        public decimal? BeLate { get; set; }//迟到
        public decimal? LeaveEarly { get; set; } //早退
        public decimal? Absenteeism { get; set; }//旷工
        public decimal? Other { get; set; } //其他异常
        public decimal? ProvisionsOfAttendance { get; set; } //规定出勤
        public decimal? ActualAttendance { get; set; } //实际出勤
        public string HolidayType { get; set; }//休假类别
        public decimal? HaveAHolidayCount { get; set; }//休假总数
        public string DateAndReason { get; set; } //日期及原因
    }
}
