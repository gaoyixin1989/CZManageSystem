using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentLoginLog
	{
		/// <summary>
		/// 日志ID
		/// <summary>
		public Guid LogID { get; set;}
		/// <summary>
		/// 登录时间
		/// <summary>
		public Nullable<DateTime> LoginTime { get; set;}
		/// <summary>
		/// 登录ID
		/// <summary>
		public string LoginID { get; set;}
		/// <summary>
		/// 姓名
		/// <summary>
		public string LoginName { get; set;}
		/// <summary>
		/// 公司名称
		/// <summary>
		public string CompanyName { get; set;}
		/// <summary>
		/// 公司ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}
		/// <summary>
		/// 登录IP
		/// <summary>
		public string LoginIP { get; set;}
		public string Result { get; set;}

	}
}
