using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_UserDinningRoom
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }
        /// <summary>
        /// 食堂用户ID (OrderMeal_UserBaseinfo中的ID)
        /// <summary>
        public Guid UserBaseinfoID { get; set; }
        /// <summary>
        /// 是否接受所属食堂的菜谱短信通知
        /// <summary>
        public int GetSms { get; set; }

    }
    public class OrderMeal_UserDinningRoomTmp
    {
        public Guid Id { get; set; }

        public Nullable<int> DinningRoomState { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }

        public string DinningRoomName { get; set; }
        /// <summary>
        /// 食堂用户ID (OrderMeal_UserBaseinfo中的ID)
        /// <summary>
        public Guid UserBaseinfoID { get; set; }
        /// <summary>
        /// 是否接受所属食堂的菜谱短信通知
        /// <summary>
        public Nullable<int> GetSms { get; set; }

    }
    public class OrderMeal_UserDinningRoomQueryBuilder
    {
        public Guid DinningRoomID { get; set; }
    }
}
