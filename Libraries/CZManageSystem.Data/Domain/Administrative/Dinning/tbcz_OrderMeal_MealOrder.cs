using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class tbcz_OrderMeal_MealOrder
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public string MealCardID { get; set; }
        public Nullable<DateTime> OrderTime { get; set; }
        public string MealTimeType { get; set; }
        public string DinningRoomName { get; set; }
        public string MealPlaceName { get; set; }
        public string PackageName { get; set; }
        public Nullable<decimal> PackagePrice { get; set; }
        public Nullable<int> Status { get; set; }
        public string MealTime { get; set; }

    }

}
