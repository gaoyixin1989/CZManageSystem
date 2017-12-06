using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPaymentApply
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid ApplyID { get; set;}
		/// <summary>
		/// 标题
		/// <summary>
		public string ApplyTitle { get; set;}
		/// <summary>
		/// 流程实例Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		public string ApplySn { get; set;}
		/// <summary>
		/// 联系号码
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// 创建时间、申请时间
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// 代垫日期
		/// <summary>
		public Nullable<DateTime> PayDay { get; set;}
		/// <summary>
		/// 代垫公司ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}
		/// <summary>
		/// 工单状态
		/// <summary>
		public string Status { get; set;}
		/// <summary>
		/// 序列号
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// 提交时间
		/// <summary>
		public Nullable<DateTime> SubmitTime { get; set;}

	}
}
