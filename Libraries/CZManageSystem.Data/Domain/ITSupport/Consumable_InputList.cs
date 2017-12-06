using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.ITSupport
{
	public class Consumable_InputList
	{
		public int ID { get; set;}
		/// <summary>
		/// 标题
		/// <summary>
		public string Title { get; set;}
		/// <summary>
		/// 入库单号
		/// <summary>
		public string Code { get; set;}
		/// <summary>
		/// 创建时间
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// 入库时间
		/// <summary>
		public Nullable<DateTime> InputTime { get; set;}
		/// <summary>
		/// 操作人
		/// <summary>
		public Nullable<Guid> Operator { get; set; }
        /// <summary>
        /// 提交人
        /// <summary>
        public Nullable<Guid> SumbitUser { get; set; }
        /// <summary>
        /// 备注
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// 入库单状态：0-保存，1-提交入库
        /// <summary>
        public Nullable<int> State { get; set; }

    }
}
