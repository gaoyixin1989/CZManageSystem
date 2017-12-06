using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysServices
    {
        public SysServices()
        {
            this.SysServiceStrategy = new List<SysServiceStrategy>();
        }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string ServiceDesc { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> Createdtime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }
        public virtual ICollection<SysServiceStrategy> SysServiceStrategy { get; set; }
    }
}
