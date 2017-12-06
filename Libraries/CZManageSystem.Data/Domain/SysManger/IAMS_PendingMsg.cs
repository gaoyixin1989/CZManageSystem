using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class IAMS_PendingMsg
    {
        public IAMS_PendingMsg()
        {
        }
        public int Id { get; set; }
        public string systemid { get; set; }
        public string sysAccount { get; set; }
        public string sysPassword { get; set; }
        public string MsgID { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public string IssuedTime { get; set; }
        public string MsgType { get; set; }
        public string ActionType { get; set; }
        public Nullable<int> State { get; set; }
        public Nullable<System.DateTime> ProcessedDT { get; set; }
        public Nullable<int> RetriedTimes { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string ParentOwner { get; set; }
    }
}
