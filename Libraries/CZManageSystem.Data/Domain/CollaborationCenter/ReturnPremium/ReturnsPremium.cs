using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsPremium
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 手机号码
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 退费金额
		/// <summary>
		public Nullable<decimal> Money { get; set;}
		/// <summary>
		/// 退费区间
		/// <summary>
		public string Range { get; set;}
		/// <summary>
		/// 退费方式
		/// <summary>
		public string Type { get; set;}
		/// <summary>
		/// 退费原因
		/// <summary>
		public string Causation { get; set;}
		/// <summary>
		/// 情况说明
		/// <summary>
		public string Explain { get; set;}
		/// <summary>
		/// 月份
		/// <summary>
		public string Month { get; set;}
		/// <summary>
		/// 日期
		/// <summary>
		public Nullable<DateTime> Date { get; set;}
		/// <summary>
		/// 登记渠道
		/// <summary>
		public string Channel { get; set;}
		/// <summary>
		/// SP端口号
		/// <summary>
		public string SpPort { get; set;}
		/// <summary>
		/// 退费详细原因
		/// <summary>
		public string Remark { get; set;}
		public string Series { get; set;}

	}
}
