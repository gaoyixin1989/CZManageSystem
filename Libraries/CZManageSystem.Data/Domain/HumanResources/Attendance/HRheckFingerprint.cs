using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.HumanResources.Attendance
{
	public class HRheckFingerprint
	{
		/// <summary>
		/// 主键
		/// <summary>
		public Guid FingerprintId { get; set;}
		/// <summary>
		/// 用户ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// 用户名
		/// <summary>
		public string UserName { get; set;}
		/// <summary>
		/// 标识
		/// <summary>
		public Nullable<int> Flag { get; set;}

	}
}
