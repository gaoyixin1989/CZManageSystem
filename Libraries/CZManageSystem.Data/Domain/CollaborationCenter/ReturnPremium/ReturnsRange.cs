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
		/// �������ֵ
		/// <summary>
		public Nullable<double> MiniValue { get; set;}
		/// <summary>
		/// �������ֵ
		/// <summary>
		public Nullable<double> MaxValue { get; set;}
		/// <summary>
		/// �˷�����
		/// <summary>
		public string Range { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
