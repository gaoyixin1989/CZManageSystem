using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-����״̬ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderStatus
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> Order { get; set;}
		/// <summary>
		/// ����״̬
		/// <summary>
		public string OrderStatus { get; set;}
		/// <summary>
		/// ˵��
		/// <summary>
		public string Remark { get; set;}

	}
}
