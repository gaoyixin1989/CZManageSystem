using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsChannel
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �Ǽ�����
		/// <summary>
		public string Channel { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
