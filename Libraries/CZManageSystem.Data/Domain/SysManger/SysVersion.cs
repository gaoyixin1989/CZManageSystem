using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Models
{
    public partial class SysVersion
    {
        public int VerId { get; set; }
        public string Version { get; set; }
        public string VerDsc { get; set; }
        public string UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
