using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysRole : BaseEntity<int>
    {
        public SysRole()
        {
            this.SysRolePrivileges = new List<SysRolePrivilege>();
            this.SysUserRoles = new List<SysUserRole>();
        }

        public string RoleName { get; set; }
        public string Remark { get; set; }
        public virtual ICollection<SysRolePrivilege> SysRolePrivileges { get; set; }
        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }
    }
}
