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
		/// ����
		/// <summary>
		public Guid FingerprintId { get; set;}
		/// <summary>
		/// �û�ID
		/// <summary>
		public Nullable<Guid> UserId { get; set;}
		/// <summary>
		/// �û���
		/// <summary>
		public string UserName { get; set;}
		/// <summary>
		/// ��ʶ
		/// <summary>
		public Nullable<int> Flag { get; set;}

	}
}
