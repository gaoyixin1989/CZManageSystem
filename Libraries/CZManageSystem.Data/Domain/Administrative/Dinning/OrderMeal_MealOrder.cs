using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_MealOrder
    {
        public long Id { get; set; }
        /// <summary>
        /// 订单号
        /// <summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 订餐系统用户ID
        /// <summary>
        public Guid UserBaseinfoID { get; set; }
        /// <summary>
        /// 用户名
        /// <summary>
        public string UserName { get; set; }
        /// <summary>
        /// 登录名
        /// <summary>
        public string LoginName { get; set; }
        public string MealCardID { get; set; }
        /// <summary>
        /// 订餐食堂ID
        /// <summary>
        public Guid DinningRoomID { get; set; }
        /// <summary>
        /// 订餐食堂
        /// <summary>
        public string DinningRoomName { get; set; }
        /// <summary>
        /// 套餐ID
        /// <summary>
        public Guid PackageID { get; set; }
        /// <summary>
        /// 套餐
        /// <summary>
        public string PackageName { get; set; }
        /// <summary>
        /// 套餐价格
        /// <summary>
        public decimal PackagePrice { get; set; }
        public Nullable<int> MealTimeID { get; set; }
        /// <summary>
        /// 餐时
        /// <summary>
        public string MealTimeType { get; set; }
        /// <summary>
        /// 用餐地点ID
        /// <summary>
        public Guid MealPlaceID { get; set; }
        /// <summary>
        /// 用餐地点
        /// <summary>
        public string MealPlaceName { get; set; }
        /// <summary>
        /// 预定时间
        /// <summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 预定状态
        /// <summary>
        public int OrderState { get; set; }
        /// <summary>
        /// 订单状态
        /// <summary>
        public string OrderStateName { get; set; }
        /// <summary>
        /// 描述
        /// <summary>
        public string Discription { get; set; }
        /// <summary>
        /// 预定后的余额
        /// <summary>
        public decimal AfterOrderBalance { get; set; }
        /// <summary>
        /// 用餐时间
        /// <summary>
        public DateTime DinningDate { get; set; }
        public Nullable<int> Flag { get; set; }
        /// <summary>
        /// 退餐时间
        /// <summary>
        public Nullable<DateTime> BackOrderTime { get; set; }


    }


    public class OrderMeal_MealOrderTmp
    {
        /// <summary>
        /// 订餐食堂ID
        /// <summary>
        public Guid DinningRoomID { get; set; }
        /// <summary>
        /// 套餐ID
        /// <summary>
        public Guid MealPackage { get; set; }
        /// <summary>
        /// 餐时
        /// <summary>
        public string MealTime { get; set; }
        /// <summary>
        /// 用餐地点ID
        /// <summary>
        public Guid MealPlace { get; set; }
        /// <summary>
        /// 预约订餐类型
        /// <summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 预约设置
        /// <summary>
        public string OrderBook { get; set; }
        /// <summary>
        /// 用餐开始时间
        /// <summary>
        public Nullable<DateTime> StartDate { get; set; }
    }
}
