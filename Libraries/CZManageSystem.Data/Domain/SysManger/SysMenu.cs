using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class SysMenu : BaseEntity<int>
    {
        public SysMenu()
        {
            this.SysRolePrivileges = new List<SysRolePrivilege>();
        }
         
        public string MenuName { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string MenuFullName { get; set; }
        public Nullable<int> MenuLevel { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string PageUrl { get; set; }
        public string MenuType { get; set; }
        public Nullable<bool> EnableFlag { get; set; }
        public string Remark { get; set; }
        public string ResourceId { get; set; }
        public virtual ICollection<SysRolePrivilege> SysRolePrivileges { get; set; }
    }
}
