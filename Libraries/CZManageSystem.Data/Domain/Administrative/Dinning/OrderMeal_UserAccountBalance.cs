using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_UserAccountBalance
    {
        public long Id { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<DateTime> RecordTime { get; set; }
        public Nullable<Guid> UserBaseinfoID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string MealCardID { get; set; }
        public string RecordType { get; set; }
        public Nullable<decimal> Payments { get; set; }
        public string Reason { get; set; }


    }

}
