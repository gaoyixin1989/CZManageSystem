using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-��Ʒά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Product
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public string ProductID { get; set;}
		/// <summary>
		/// ��Ʒ����
		/// <summary>
		public string ProductName { get; set;}
		/// <summary>
		/// ��Ʒ����
		/// <summary>
		public Nullable<Guid> ProductTypeID { get; set;}
		/// <summary>
		/// ˵��
		/// <summary>
		public string Remark { get; set;}

	}
}
