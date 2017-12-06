using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.Integral
{
    public class HRIntegralStatic
    {
        public Guid UserId { get; set; }
        public string RealName { get; set; }
        public Nullable<decimal> Integral { get; set; }
        public Nullable<decimal> T_Integral { get; set; }
        public Nullable<decimal> C_Integral { get; set; }
        public Nullable<decimal> Alldays { get; set; }
        public Nullable<decimal> FinishPer { get; set; }
        public Nullable<decimal> Gap { get; set; }
        public string DpName { get; set; }
        public string FullDeptName { get; set; }
        public string NeedIntegral { get; set;  }
        public string YearDate { get; set; }
        public string EmployeeId { get; set; }

    }



    public class IntegralStaticQueryBuilder
    {
        public string DpId { get; set; }
        public string EmployeeId { get; set; }
        public string RealName { get; set; }
        public string Year { get; set; }

    }
}
