using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.AnnualLeave
{
    public class HRAnnualLeave
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        /// <summary>
        /// 姓名
        /// <summary>
        public string UserName { get; set; }
        /// <summary>
        /// 年度
        /// <summary>
        public string VYears { get; set; }
        /// <summary>
        /// 年休假天数
        /// <summary>
        public string VDays { get; set; }
        /// <summary>
        /// 上年度法定年休假剩余天数
        /// <summary>
        public Nullable<decimal> FdLastYearVDays { get; set; }
        /// <summary>
        /// 本年度法定年休假天数
        /// <summary>
        public Nullable<decimal> FdYearVDays { get; set; }
        /// <summary>
        /// 本年度补充年休假天数
        /// <summary>
        public Nullable<decimal> BcYearVDays { get; set; }



    }

    public class HRAnnualLeaveQueryBuilder
    {
        public string EmployeeID { get; set; }//地点
        public List<string> DpId { get; set; }//部门ID
        public string RealName { get; set; }//名称
        public string VYears { get; set; }
    }

}
