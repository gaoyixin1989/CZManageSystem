using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_Package
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 套餐名称
        /// <summary>
        public string PackageName { get; set; }
        /// <summary>
        /// 套餐价格
        /// <summary>
        public decimal PackagePrice { get; set; }
        /// <summary>
        /// 套餐所属餐时ID
        /// <summary>
        public Nullable<int> MealTimeID { get; set; }
        /// <summary>
        /// 套餐所属餐时
        /// <summary>
        public string MealTimeType { get; set; }
        /// <summary>
        /// 套餐所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }
        /// <summary>
        /// 描述
        /// <summary>
        public string Discription { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }


    }

    public class OrderMeal_PackageQueryBuilder
    {
        public string DinningRoomID { get; set; }
        public string PackageName { get; set; }
        public string MealTimeType { get; set; }

        public string DinningRoomName { get; set; }

    }
}
