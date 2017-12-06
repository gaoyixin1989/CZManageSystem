using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.MarketPlan
{
	public class Ucs_MarketPlan2
    {
		public Guid Id { get; set;}
		/// <summary>
		/// 营销方案编码
		/// <summary>
		public string Coding { get; set;}
		/// <summary>
		/// 营销方案名称
		/// <summary>
		public string Name { get; set;}
		/// <summary>
		/// 开始时间
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// 结束时间
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// 办理渠道
		/// <summary>
		public string Channel { get; set;}
		/// <summary>
		/// 指令
		/// <summary>
		public string Orders { get; set;}
		/// <summary>
		/// 社会渠道登记端口
		/// <summary>
		public string RegPort { get; set;}
		/// <summary>
		/// 营销活动细则
		/// <summary>
		public string DetialInfo { get; set;}
		/// <summary>
		/// 备注
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// 方案类型
		/// <summary>
		public string PlanType { get; set;}
		/// <summary>
		/// 目标用户群
		/// <summary>
		public string TargetUsers { get; set;}
		/// <summary>
		/// 薪金规则提要
		/// <summary>
		public string PaysRlues { get; set;}
		/// <summary>
		/// 备用模块1
		/// <summary>
		public string Templet1 { get; set;}
		/// <summary>
		/// 备用模块2
		/// <summary>
		public string Templet2 { get; set;}
		/// <summary>
		/// 备用模块3
		/// <summary>
		public string Templet3 { get; set;}
		/// <summary>
		/// 备用模块4
		/// <summary>
		public string Templet4 { get; set;}
		/// <summary>
		/// 号码数量
		/// <summary>
		public Nullable<int> NumCount { get; set;}
		/// <summary>
		/// 是否标准方案
		/// <summary>
		public string IsMarketing { get; set;}

	}
}
