using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.HumanResources.OverTime
{
    public class OverTimeStatic
    {
        public Guid UserId { get; set; }
        public string RealName { get; set; }
        public Nullable<decimal> GJOTTime { get; set; }
        public Nullable<decimal> FJOTTime { get; set; }
        public Nullable<decimal> AllOTTime { get; set; }
        public string DetailDate { get; set; }
        public string DpName { get; set; }
        public string UserType { get; set; }
        public string EmployeeId { get; set; }
    }

    public class OverTimeStaticQueryBuilder
    {
        public string DpId { get; set; }
        public string EmployeeId { get; set; }
        public string RealName { get; set; }
        public string Year { get; set; }

        public string UserType { get; set; }

    }
}
