using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-�ն˻���ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_EndType
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string EndType { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<Guid> MarketID { get; set;}
		/// <summary>
		/// ˵��
		/// <summary>
		public string Remark { get; set;}

	}
}
