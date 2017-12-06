using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysRolePrivilege : BaseEntity<int>
    {
        public int MenuId { get; set; }
        public int RoleID { get; set; }
        public virtual SysMenu SysMenu { get; set; }
        public virtual SysRole SysRole { get; set; }
    }
}
