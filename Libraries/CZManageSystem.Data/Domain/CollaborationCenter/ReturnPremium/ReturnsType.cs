using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.SysManger
{
	public class ReturnsType
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �˷ѷ�ʽ
		/// <summary>
		public string Type { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
