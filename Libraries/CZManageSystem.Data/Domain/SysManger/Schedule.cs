using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Schedule
    {
        public Schedule()
        {
        }
        public int ScheduleId { get; set; }
        public Guid UserId { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<System.DateTime> Createdtime { get; set; }
        public string Content { get; set; }
        public Nullable<Boolean> Sms { get;set; }//是否已经发送通知
    }
}
