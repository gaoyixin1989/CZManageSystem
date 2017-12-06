using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class RolesInResources// : BaseEntity<Guid >
    {
        public System.Guid RoleId { get; set; }
        public string ResourceId { get; set; }
        //public virtual Resources Resources { get; set; }
        //public virtual Roles Roles { get; set; } 
    }
}
