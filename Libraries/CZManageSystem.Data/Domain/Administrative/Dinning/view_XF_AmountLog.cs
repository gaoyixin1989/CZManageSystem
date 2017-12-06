using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class view_XF_AmountLog
    {
        public string DepartmentName { get; set; }
        public string Name { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string JobNumber { get; set; }
        public Nullable<int> LogID { get; set; }
        public Nullable<int> TypeID { get; set; }
        public Nullable<int> AccountID { get; set; }
        public Nullable<decimal> AddAmount { get; set; }
        public Nullable<decimal> BelAmount { get; set; }
        public Nullable<int> Status { get; set; }
        public string TypeContent { get; set; }
        public string Memo { get; set; }
        public string Operator { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public string BankName { get; set; }
        public Nullable<int> Expr1 { get; set; }

    }

    public class view_XF_AmountLogQueryBuilder
    {
        public string Name { get; set; }
        public string TypeContent { get; set; }
        public DateTime? CreateTime_Start { get; set; }
        public DateTime? CreateTime_End { get; set; }

    }

}
