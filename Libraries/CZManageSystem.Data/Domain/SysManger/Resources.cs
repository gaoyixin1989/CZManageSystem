using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Resources  
    {
        public Resources()
        {
            this.RolesInResources = new List<RolesInResources>();
        }
        public string ResourceId { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public Nullable<bool> Enabled { get; set; }
        public System.DateTime CreatedTime { get; set; }
        public Nullable<bool> Visible { get; set; }
        public Nullable<int> SortIndex { get; set; }
        public virtual ICollection<RolesInResources> RolesInResources { get; set; }
    }
}
