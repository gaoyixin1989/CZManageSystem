using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysServiceStrategy
    {
        public int Id { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<System.DateTime> ValidTime { get; set; }
        public Nullable<System.DateTime> NextRunTime { get; set; }
        public Nullable<int> PeriodNum { get; set; }
        public string PeriodType { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public Nullable<bool> LogFlag { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> Createdtime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }
        public virtual SysServices SysServices { get; set; }
    }
}
