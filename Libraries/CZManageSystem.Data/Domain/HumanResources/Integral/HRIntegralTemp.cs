using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Integral
{
	public class HRIntegralTemp
	{
		public Guid IntegralId { get; set;}
		/// <summary>
		/// 用词ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// 积分
		/// <summary>
		public Nullable<decimal> Integral { get; set;}
		/// <summary>
		/// 年份
		/// <summary>
		public Nullable<int> YearDate { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 积分来源
		/// <summary>
		public Nullable<decimal> Source { get; set;}
		/// <summary>
		/// 删除
		/// <summary>
		public Nullable<bool> Del { get; set;}
		/// <summary>
		/// 类型
		/// <summary>
		public string IntegralType { get; set;}
		/// <summary>
		/// 培训积分
		/// <summary>
		public Nullable<decimal> CIntegral { get; set;}
		/// <summary>
		/// 授课积分
		/// <summary>
		public Nullable<int> TIntegral { get; set;}
		/// <summary>
		/// 申请单ID
		/// <summary>
		public Nullable<Guid> ApplyId { get; set;}
		public Nullable<decimal> TPeriodTime { get; set;}
        public Nullable<Guid> Daoid { get; set; }
        public string FinishTime { get; set;}

	}
}
