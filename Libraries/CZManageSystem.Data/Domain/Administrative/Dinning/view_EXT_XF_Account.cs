using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class view_EXT_XF_Account
    {
        public Nullable<int> AccountID { get; set; }
        public string BankName { get; set; }
        public Nullable<decimal> BelAmount { get; set; }
        public string JobNumber { get; set; }
        public string SystemNumber { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        public Nullable<DateTime> Updatetime { get; set; }

    }
    public class view_EXT_XF_AccountQueryBuilder
    {
        public string Name { get; set; }
    }
}
