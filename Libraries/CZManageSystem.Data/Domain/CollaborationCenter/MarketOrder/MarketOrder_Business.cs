using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-����ҵ��ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Business
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ����ҵ��
		/// <summary>
		public string Business { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// ˵��
		/// <summary>
		public string Remark { get; set;}

	}
}
