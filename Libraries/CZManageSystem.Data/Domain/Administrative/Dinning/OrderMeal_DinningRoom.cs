using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_DinningRoom
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 食堂名称
        /// <summary>
        public string DinningRoomName { get; set; }
        /// <summary>
        /// 食堂简介
        /// <summary>
        public string Discription { get; set; }

    }


    public class OrderMeal_DinningRoomQueryBuilder
    {
        public string DinningRoomName { get; set; }
    }
}
