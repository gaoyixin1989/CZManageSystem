using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-����ʱ��ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Area
	{
		public Guid ID { get; set;}
		/// <summary>
		/// �������
		/// <summary>
		public string DpCode { get; set;}
		/// <summary>
		/// ������������
		/// <summary>
		public string DpName { get; set;}
		/// <summary>
		/// ����ʱ�ޣ�Сʱ��
		/// <summary>
		public Nullable<int> LimitTime { get; set;}
		/// <summary>
		/// ���
		/// <summary>
		public Nullable<int> Order { get; set;}

	}
}
