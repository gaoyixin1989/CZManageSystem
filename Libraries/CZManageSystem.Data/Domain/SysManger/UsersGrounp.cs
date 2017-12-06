using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class UsersGrounp
    {
        public UsersGrounp()
        {
        }
        public int Id { get; set; }
        public string GroupName { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public string Remark { get; set; }
    }
}
