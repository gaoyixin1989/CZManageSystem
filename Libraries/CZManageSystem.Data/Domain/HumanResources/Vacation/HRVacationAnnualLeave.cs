using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Vacation
{
    public class HRVacationAnnualLeave
    {
        /// <summary>
        /// 主键ID
        /// <summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 用户ID
        /// <summary>
        public Nullable<Guid> UserID { get; set; }
        public Nullable<Guid> ComID { get; set; }
        public string YearDate { get; set; }
        public Nullable<decimal> AnnualLeave { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        /// <summary>
        /// 创建者ID
        /// <summary>
        public Nullable<Guid> CreateID { get; set; }
        /// <summary>
        /// 使用日期
        /// <summary>
        public Nullable<DateTime> UseDate { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }

        /// <summary>
        /// 申请单ID
        /// </summary>
        public Nullable<Guid> AppID { get; set; }
        /// <summary>
        /// 年休假天数
        /// <summary>
        public Nullable<decimal> SpendDays { get; set; }
        /// <summary>
        /// 开始时间
        /// <summary>
        public Nullable<DateTime> StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// <summary>
        public Nullable<DateTime> EndTime { get; set; }
        public string Toflag { get; set; }

    }

}
