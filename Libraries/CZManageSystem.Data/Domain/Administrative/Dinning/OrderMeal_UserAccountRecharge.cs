using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_UserAccountRecharge
    {
        public long Id { get; set; }
        public string OrderNum { get; set; }
        public string FeedbackID { get; set; }
        public Nullable<DateTime> Time { get; set; }
        public Nullable<Guid> UserBaseinfoID { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string MealCardID { get; set; }
        public Nullable<DateTime> UpDateTime { get; set; }
        public Nullable<int> RechargeType { get; set; }
        public Nullable<int> RechargeState { get; set; }
        public Nullable<decimal> Money { get; set; }
        public Nullable<decimal> BeforeRechargeBalance { get; set; }
        public Nullable<decimal> AfterRechargeBalance { get; set; }
        public string RechargeAdminName { get; set; }
        public string RechargeAdminloginname { get; set; }
        public string Discription { get; set; }


    }

}
