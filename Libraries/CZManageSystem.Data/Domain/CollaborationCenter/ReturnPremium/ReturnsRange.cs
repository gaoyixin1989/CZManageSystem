using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsRange
	{
		public Guid ID { get; set;}
		/// <summary>
		/// 区间最低值
		/// <summary>
		public Nullable<double> MiniValue { get; set;}
		/// <summary>
		/// 区间最大值
		/// <summary>
		public Nullable<double> MaxValue { get; set;}
		/// <summary>
		/// 退费区间
		/// <summary>
		public string Range { get; set;}
		/// <summary>
		/// 排序
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
