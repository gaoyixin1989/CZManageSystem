using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-�����ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Sat
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �����
		/// <summary>
		public string Sat { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> Order { get; set;}

	}
}
