using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.BirthControl
{
    public class BirthControlChildrenInfo
    {
        /// <summary>
		/// 主键
		/// <summary>
        public int id { get; set; }
        /// <summary>
		/// 
		/// <summary>
        public Guid UserId { get; set; }

        /// <summary>
		/// 姓名
		/// <summary>
        public string Name { get; set; }
        /// <summary>
		/// 性别
		/// <summary>
        public string Sex { get; set; }
        /// <summary>
		/// 出生日期
		/// <summary>
        public DateTime Birthday { get; set; }
        /// <summary>
		/// 政策内/外 
		/// <summary>
        public string PolicyPostiton { get; set; }
        /// <summary>
		/// 是否独生子女
		/// <summary>
        public string CISingleChildren { get; set; }
        /// <summary>
		/// 独生证号 
		/// <summary>
        public string CISingleChildNum { get; set; }
        /// <summary>
		/// 处理情况 
		/// <summary>
        public string Treatment { get; set; }
        /// <summary>
		/// 备注 
		/// <summary>
        public string remark { get; set; }

        public string Creator { get; set; }
        public Nullable<DateTime> CreatedTime { get; set; }
        public string LastModifier { get; set; }
        public Nullable<DateTime> LastModTime { get; set; }

    }
}
