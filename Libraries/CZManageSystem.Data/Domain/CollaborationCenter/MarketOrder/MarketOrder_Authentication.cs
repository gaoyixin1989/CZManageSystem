using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-��Ȩ��ʽά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Authentication
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��Ȩ��ʽ,�����ظ�
		/// <summary>
		public string Authentication { get; set;}
		/// <summary>
		/// ���,�����ظ�
		/// <summary>
		public Nullable<decimal> Order { get; set;}

	}
}
