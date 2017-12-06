using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
    public class PendingData
    {
        /// <summary>
        /// 编号
        /// <summary>
        public int ID { get; set; }
        /// <summary>
        /// 数据来源：待办、待阅、已办、已阅...
        /// <summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 数据标识ID
        /// <summary>
        public string DataID { get; set; }
        /// <summary>
        /// 推送ID
        /// <summary>
        public int SendID { get; set; }
        /// <summary>
        /// 数据所有者
        /// <summary>
        public string Owner { get; set; }
        /// <summary>
        /// 状态：1-未发送，2发送（默认1）
        /// <summary>
        public Nullable<int> State { get; set; }
        /// <summary>
        /// 尝试次数
        /// <summary>
        public int TryTime { get; set; }
        /// <summary>
        /// 处理时间
        /// <summary>
        public Nullable<DateTime> DealTime { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }

    }
}
