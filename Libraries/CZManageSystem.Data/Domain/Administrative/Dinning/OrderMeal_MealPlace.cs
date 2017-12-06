using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_MealPlace
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 用餐地点
        /// <summary>
        public string MealPlaceName { get; set; }
        /// <summary>
        /// 用餐地点简称
        /// <summary>
        public string MealPlaceShortName { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Nullable<Guid> DinningRoomID { get; set; }
        /// <summary>
        /// 用餐地点描述
        /// <summary>
        public string Discription { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }


    public class OrderMeal_MealPlaceQueryBuilder
    {
        public string MealPlaceName { get; set; }
        public string DinningRoomID { get; set; }
        public string DinningRoomName { get; set; }

    }
}
