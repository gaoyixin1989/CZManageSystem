using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.HumanResources.Employees
{
    public class HRLzUserInfo
    {
        public string EmployeeId { get; set; }
        /// <summary>
        /// 职位职级
        /// <summary>
        public string PositionRank { get; set; }
        /// <summary>
        /// 套入职级
        /// <summary>
        public string SetIntoTheRanks { get; set; }
        /// <summary>
        /// 分位值
        /// <summary>
        public Nullable<int> Tantile { get; set; }
        public Nullable<Guid> UserId { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// 更新时间
        /// <summary>
        public Nullable<DateTime> LastModTime { get; set; }
        /// <summary>
        /// 修改人
        /// <summary>
        public string LastModFier { get; set; }
        /// <summary>
        /// 档位
        /// <summary>
        public string Gears { get; set; }
        public virtual Users  Users { get; set; }
    }
}
