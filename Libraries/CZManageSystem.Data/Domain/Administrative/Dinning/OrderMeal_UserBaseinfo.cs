using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_UserBaseinfo
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 用户名
        /// <summary>
        public string RealName { get; set; }
        /// <summary>
        /// 用户登录名
        /// <summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 员工编号
        /// <summary>
        public string EmployId { get; set; }
        public string MealCardID { get; set; }
        /// <summary>
        /// 手机号码
        /// <summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 部门
        /// <summary>
        public string DeptId { get; set; }
        /// <summary>
        /// 状态
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 余额
        /// <summary>
        public Nullable<decimal> Balance { get; set; }


    }
    public class OrderMeal_UserBaseinfoTmp
    {
        /// <summary>
        /// 用户名
        /// <summary>
        public string RealName { get; set; }
        /// <summary>
        /// 用户登录名
        /// <summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 手机号码
        /// <summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 部门
        /// <summary>
        public string DeptId { get; set; }

        public string EmployId { get; set; }

        public string DeptName { get; set; }
        /// <summary>
        /// 状态
        /// <summary>
        public Nullable<int> State { get; set; }


    }
    

    public class OrderMeal_UserBaseinfoQueryBuilder
    {
        public string Tel { get; set; }
        public string RealName { get; set; }
        public List<string> DpId { get; set; }//部门ID
    }
    
}
