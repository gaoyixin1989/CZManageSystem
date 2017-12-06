using CZManageSystem.Core;
using System;
using System.Collections.Generic;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class UsersGrounp_Member
    {
        public UsersGrounp_Member()
        {
        }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string MemberType { get; set; }
        public string MemberId { get; set; }
    }
}
