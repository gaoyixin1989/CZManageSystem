using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-�����ײ�ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Setmeal
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �ײ�����
		/// <summary>
		public string Setmeal { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<decimal> Order { get; set;}
		/// <summary>
		/// ˵����ע
		/// <summary>
		public string Remark { get; set;}

	}
}
