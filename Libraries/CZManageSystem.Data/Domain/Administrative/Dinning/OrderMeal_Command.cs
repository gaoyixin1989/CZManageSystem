using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_Command
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 用餐地点ID
        /// <summary>
        public Guid PlaceId { get; set; }
        /// <summary>
        /// 套餐ID
        /// <summary>
        public Guid PackageId { get; set; }
        /// <summary>
        /// 订餐命令
        /// <summary>
        public string Command { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }


    }

    public class OrderMeal_CommandQueryBuilder
    {
        public string PackageName { get; set; }
        public string MealTimeType { get; set; }

        public Guid DinningRoomId { get; set; }

    }

}
