using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class DataDictionary
    {
        public System.Guid DDId { get; set; }
        public string DDName { get; set; }
        public string DDValue { get; set; }
        public string DDText { get; set; }
        public string ValueType { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public Nullable<bool> DefaultFlag { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> Createdtime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }
    }
}
