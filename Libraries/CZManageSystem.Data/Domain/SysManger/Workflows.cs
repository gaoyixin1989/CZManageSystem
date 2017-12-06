using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Workflows
    {
        public System.Guid WorkflowId { get; set; }
        public string WorkflowName { get; set; }
        public string Owner { get; set; }
        public bool Enabled { get; set; }
        public bool IsCurrent { get; set; }
        public int Version { get; set; }
        public string Creator { get; set; }
        public string Remark { get; set; }
        public string LastModifier { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public System.DateTime LastModTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
