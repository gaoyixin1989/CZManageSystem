using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Depts 
    {
        public string DpId { get; set; }
        public string DpName { get; set; }
        public string ParentDpId { get; set; }
        public string DpFullName { get; set; }
        public Nullable<int> DpLevel { get; set; }
        public Nullable<int> DeptOrderNo { get; set; }
        public Nullable<bool> IsTmpDp { get; set; }
        public Nullable<byte> Type { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> LastModTime { get; set; }
        public string Creator { get; set; }
        public string LastModifier { get; set; }
    }
}
