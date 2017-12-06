using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CZManageSystem.Data.Domain.Administrative.Dinning
{
    public class OrderMeal_DinningRoomMealTimeSettings
    {
        public Guid Id { get; set; }
        public Nullable<int> MealTimeID { get; set; }
        /// <summary>
        /// 所属食堂
        /// <summary>
        public Guid DinningRoomID { get; set; }
        /// <summary>
        /// 订餐时间-开始
        /// <summary>
        public Nullable<TimeSpan> BeginTime { get; set; }
        /// <summary>
        /// 订餐时间-结束
        /// <summary>
        public Nullable<TimeSpan> EndTime { get; set; }
        /// <summary>
        /// 最晚退餐时间
        /// <summary>
        public Nullable<TimeSpan> ClosePayBackTime { get; set; }
        /// <summary>
        /// 短信发送时间-开始
        /// <summary>
        public Nullable<TimeSpan> SmsTime { get; set; }
        /// <summary>
        /// 短信发送时间-结束
        /// <summary>
        public Nullable<TimeSpan> LastSmsTime { get; set; }
        /// <summary>
        /// 用餐时段
        /// <summary>
        public string MealTimeType { get; set; }
        /// <summary>
        /// 是否提供用餐
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 记录推送时间-开始
        /// <summary>
        public Nullable<TimeSpan> OrderMealRecordSendTime { get; set; }
        /// <summary>
        /// 记录推送时间-结束
        /// <summary>
        public Nullable<TimeSpan> LastOrderMealRecordSendTime { get; set; }
        /// <summary>
        /// 创建者
        /// <summary>
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// <summary>
        public Nullable<DateTime> CreatedTime { get; set; }
        /// <summary>
        /// 修改者
        /// <summary>
        public string LastModifier { get; set; }
        /// <summary>
        /// 最近修改时间
        /// <summary>
        public Nullable<DateTime> LastModTime { get; set; }

    }


}
