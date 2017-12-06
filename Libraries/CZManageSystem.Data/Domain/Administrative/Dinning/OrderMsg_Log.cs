using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMsg_Log
    {
        public int Id { get; set; }
        public Nullable<Guid> UserId { get; set; }
        public Nullable<Guid> RoomId { get; set; }
        /// <summary>
        /// 子端口号,早餐1午餐2晚餐3其他4
        /// <summary>
        public Nullable<int> Num { get; set; }
        /// <summary>
        /// 0已发送,1已回复,2已过期
        /// <summary>
        public Nullable<int> State { get; set; }
        public Nullable<DateTime> WorkingDate { get; set; }
        public Nullable<DateTime> InsertDate { get; set; }
        public string Number { get; set; }
        public string MealTimeType { get; set; }

    }
}
