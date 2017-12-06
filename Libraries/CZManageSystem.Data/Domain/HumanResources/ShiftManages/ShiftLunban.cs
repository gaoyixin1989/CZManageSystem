using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.ShiftManages
{
    /// <summary>
    /// 轮班信息
    /// </summary>
	public class ShiftLunban
	{
        public ShiftLunban() {
            this.ShiftLbusers = new List<ShiftLbuser>();
        }
		/// <summary>
		/// 轮班ID
		/// <summary>
		public Guid Id { get; set;}
		/// <summary>
		/// 排班信息表ID
		/// <summary>
		public Guid ZhibanId { get; set;}
		/// <summary>
		/// 开始日
		/// <summary>
		public string StartDay { get; set;}
		/// <summary>
		/// 结束日
		/// <summary>
		public string EndDay { get; set;}


        public virtual ICollection<ShiftLbuser> ShiftLbusers { get; set; }

    }
}
