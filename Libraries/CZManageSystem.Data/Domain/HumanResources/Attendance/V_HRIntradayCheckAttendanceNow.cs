using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class V_HRIntradayCheckAttendanceNow
    {
        public Guid AttendanceId { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public Nullable<DateTime> AtDate { get; set; }
        public Nullable<TimeSpan> DoTime { get; set; }
        public Nullable<DateTime> OffDate { get; set; }
        public Nullable<TimeSpan> OffTime { get; set; }
        public Nullable<int> Minute { get; set; }
        public Nullable<TimeSpan> DoReallyTime { get; set; }
        public Nullable<TimeSpan> OffReallyTime { get; set; }
        public Nullable<int> DoFlag { get; set; }
        public Nullable<int> RotateDaysOffFlag { get; set; }

    }
}
