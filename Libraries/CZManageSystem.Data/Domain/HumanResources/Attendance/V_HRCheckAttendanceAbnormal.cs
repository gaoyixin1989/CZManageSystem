using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class AttendanceListQueryBuilder
    {
        public string EmployeeId { get; set; }
        public string IpOn { get; set; }
        public string IpOff { get; set; }
        public DateTime? AtDate_Start { get; set; }
        public DateTime? AtDate_End { get; set; }
        public List<Guid?> UserId { get; set; }
        public List<string> DpId { get; set; }
        //public List<string> RealName { get; set; }
        //public List<string> DpName { get; set; }

    }

    public class V_HRCheckAttendanceAbnormal
    {
        public Nullable<Guid> AttendanceId { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public Nullable<DateTime> AtDate { get; set; }
        public Nullable<DateTime> OffDate { get; set; }
        public Nullable<TimeSpan> DoTime { get; set; }
        public Nullable<TimeSpan> OffTime { get; set; }
        public Nullable<TimeSpan> DoReallyTime { get; set; }
        public Nullable<TimeSpan> OffReallyTime { get; set; }
        public string IpOn { get; set; }
        public string IpOff { get; set; }
        public Nullable<int> Minute { get; set; }
        public Nullable<int> DoFlag { get; set; }
        public Nullable<int> RotateDaysOffFlag { get; set; }
        public Nullable<int> FlagOn { get; set; }
        public Nullable<int> FlagOff { get; set; }
        public string RealName { get; set; }
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string EmployeeId { get; set; }

    }
}
