using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-ʧ�ܻط�ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Visit
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ʧ�ܻط����
		/// <summary>
		public string Visit { get; set;}
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
