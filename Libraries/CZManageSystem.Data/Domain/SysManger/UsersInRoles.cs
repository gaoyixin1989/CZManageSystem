using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class UsersInRoles
    {
        public System.Guid UserId { get; set; }
        public System.Guid RoleId { get; set; }
        public virtual Users Users { get; set; }
        public virtual Roles Roles { get; set; } 
    }
}
