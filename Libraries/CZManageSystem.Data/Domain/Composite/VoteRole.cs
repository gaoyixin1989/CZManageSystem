using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.Composite
{
    public partial class VoteRole
    {
        public int RoleID { get; set; }
        public Nullable<int> GroupID { get; set; }
        public Nullable<int> Editor { get; set; }
    }
}
