using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// 班次信息
    /// </summary>
	public class ShiftBanci
    {
        public ShiftBanci()
        {
            this.ShiftRichs = new List<ShiftRich>();
        }

        /// <summary>
        /// 班次ID
        /// <summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 编辑人
        /// <summary>
        public Nullable<Guid> Editor { get; set; }
        /// <summary>
        /// 编辑时间
        /// <summary>
        public Nullable<DateTime> EditTime { get; set; }
        /// <summary>
        /// 排班信息表Id
        /// <summary>
        public Guid ZhibanId { get; set; }
        /// <summary>
        /// 班次名称
        /// <summary>
        public string BcName { get; set; }
        /// <summary>
        /// 开始小时值
        /// <summary>
        public string StartHour { get; set; }
        /// <summary>
        /// 开始分钟值
        /// <summary>
        public string StartMinute { get; set; }
        /// <summary>
        /// 结束小时值
        /// <summary>
        public string EndHour { get; set; }
        /// <summary>
        /// 结束分钟值
        /// <summary>
        public string EndMinute { get; set; }
        /// <summary>
        /// 值班人数
        /// <summary>
        public Nullable<int> StaffNum { get; set; }
        /// <summary>
        /// 班次排序
        /// <summary>
        public Nullable<int> OrderNo { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }


        public virtual Users EditorObj { get; set; }
        public virtual ICollection<ShiftRich> ShiftRichs { get; set; }

    }
}
