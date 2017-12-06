using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysNotice
    {
        public SysNotice()
        {
        }
        public int NoticeId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> ValidTime { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public Nullable<System.DateTime> Createdtime { get; set; }
        public string Creator { get; set; }
    }
}
