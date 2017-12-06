using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Roles  
    {
        public Roles()
        {
            this.UsersInRoles = new List<UsersInRoles>();
            this.RolesInResources = new List<RolesInResources>();
        }
        public System.Guid RoleId { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public Nullable<bool> IsInheritable { get; set; }
        public string RoleName { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> BeginTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> LastModTime { get; set; }
        public string Creator { get; set; }
        public string LastModifier { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public virtual ICollection<UsersInRoles> UsersInRoles { get; set; }
        public virtual ICollection<RolesInResources > RolesInResources { get; set; }
    }
}
