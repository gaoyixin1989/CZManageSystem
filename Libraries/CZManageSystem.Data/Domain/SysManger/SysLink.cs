using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysLink
    {
        public int LinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public string Remark { get; set; }

        public Nullable<System.DateTime> ValidTime { get; set; }
    }
}
