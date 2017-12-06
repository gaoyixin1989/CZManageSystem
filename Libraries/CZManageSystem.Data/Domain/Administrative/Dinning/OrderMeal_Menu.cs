using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_Menu
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 菜谱名称
        /// <summary>
        public string MenuName { get; set; }
        public Nullable<DateTime> CreateTime { get; set; }
        /// <summary>
        /// 菜谱可用日期
        /// <summary>
        public Nullable<DateTime> WorkingDate { get; set; }
        public int MealTimeID { get; set; }
        /// <summary>
        /// 用餐时段
        /// <summary>
        public string MealTimeType { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }
        public int Flag { get; set; }
        public Nullable<int> SendTimes { get; set; }
        /// <summary>
        /// 是否发送信息
        /// <summary>
        public Nullable<int> CanSendSms { get; set; }
        public Nullable<int> Bookflag { get; set; }
        public Nullable<int> IsCompleted { get; set; }
        public Nullable<int> IsPreOrder { get; set; }


    }

    public class OrderMeal_MenuQueryBuilder
    {
        public string DinningRoomID { get; set; }
        public string MealTimeType { get; set; }
        public string MenuName { get; set; }
        public DateTime CreateTime_Start { get; set; }
        public DateTime CreateTime_End { get; set; }

        public string DinningRoomName { get; set; }
    }

}
