using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_DinningRoomMealBookSettings
    {
        public Guid Id { get; set; }
        public Nullable<int> MealTimeID { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }
        /// <summary>
        /// 用餐时段
        /// <summary>
        public string MealTimeType { get; set; }
        /// <summary>
        /// 是否提供预约
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 预约设置 一周内（7天） 
        /// <summary>
        public Nullable<int> Week { get; set; }
        /// <summary>
        /// 预约设置一个月（7天） 
        /// <summary>
        public Nullable<int> Month { get; set; }


    }
}
