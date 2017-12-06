using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
    public class HRRfsim
    {
        public Guid RecordId { get; set; }
        public Nullable<int> Tid { get; set; }
        public string SysNo { get; set; }
        public string Serial { get; set; }
        public Nullable<DateTime> CDateTime { get; set; }
        public Nullable<int> DeviceSysId { get; set; }
        public string RecordType { get; set; }
        /// <summary>
        /// ²Ù×÷Ô±ID
        /// <summary>
        public string OperatorId { get; set; }
        public string EmplyName { get; set; }
        public string DptId { get; set; }
        public string EmplyId { get; set; }
        public Nullable<int> ActionStatus { get; set; }

    }
}
