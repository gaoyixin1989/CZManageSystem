using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_Cuisine
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 菜式名称
        /// <summary>
        public string CuisineName { get; set; }
        /// <summary>
        /// 菜式类型
        /// <summary>
        public string CuisineType { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }
        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }



    public class OrderMeal_CuisineQueryBuilder
    {
        public string DinningRoomID { get; set; }
        public string CuisineName { get; set; }
        public string CuisineType { get; set; }

        public string DinningRoomName { get; set; }

    }
    public class OrderMeal_CuisineTreeData

    {

        public string CuisineName { get; set; }

        public string CuisineType { get; set; }
        public string ParentId { get; set; }
        public Guid Id { get; set; }




    }
}
