using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.AnnualLeave
{
    public class HRAnnualLeaveStatic
    {
        public Guid UserId { get; set; }
        public string RealName { get; set; }
        public string Years { get; set; }
        public Nullable<decimal> Leftdays { get; set; }
        public Nullable<decimal> SpendDays { get; set; }
        public Nullable<decimal> FdYearVDays { get; set; }
        public Nullable<decimal> ThisYearSpendDays { get; set; }
        public Nullable<decimal> ThisYearLeftdays { get; set; }
        public Nullable<decimal> VDays { get; set; }
        public string DpName { get; set; }
        public string UseDate { get; set; }
        public string EmployeeId { get; set; }
    }


    public class HRAnnualLeaveStaticQueryBuilder
    {
        public string DpId { get; set; }
        public string EmployeeId { get; set; }
        public string RealName { get; set; }
        public string Year { get; set; }

    }
}
