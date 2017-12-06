using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_DinningRoomAdmin
    {
        public int Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        /// <summary>
        /// 分食堂管理员登录名
        /// <summary>
        public string Loginname { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Nullable<Guid> DinningRoomID { get; set; }
        /// <summary>
        /// 分食堂管理员
        /// <summary>
        public string RealName { get; set; }
        public string AdminType { get; set; }




    }

}
