using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsReason
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �˷�ԭ��
		/// <summary>
		public string Reason { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
