using CZManageSystem.Data.Domain.HumanResources.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CZManageSystem.Data.Domain.SysManger
{
    public partial class Users
    {
        public Users()
        {
            this.UsersInRoles = new List<UsersInRoles>();
        }
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Tel { get; set; }
        public string EmployeeId { get; set; }
        public string RealName { get; set; }
        public Nullable<byte> Type { get; set; }
        public Nullable<int> Status { get; set; }
        public string DpId { get; set; }
        public Nullable<int> Ext_Int { get; set; }
        public Nullable<decimal> Ext_Decimal { get; set; }
        public string Ext_Str1 { get; set; }
        public string Ext_Str2 { get; set; }
        public string Ext_Str3 { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> LastModTime { get; set; }
        public string Creator { get; set; }
        public string LastModifier { get; set; }
        public Nullable<int> SortOrder { get; set; }

        public Nullable<DateTime> JoinTime { get; set; }
        public Nullable<int> UserType { get; set; }

        public virtual ICollection<UsersInRoles> UsersInRoles { get; set; }
        //[NotMapped]
        public virtual Depts Dept { get; set; }

        /// <summary>
        /// ¿¼ÇÚ´ò¿¨ip
        /// </summary>
        public string CheckIP { get; set; }
    }
}
