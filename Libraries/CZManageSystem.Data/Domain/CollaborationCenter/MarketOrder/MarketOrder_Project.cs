using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-��Ŀ���ά��
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_Project
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ��Ŀ��ţ������ظ�
		/// <summary>
		public string ProjectID { get; set;}
		/// <summary>
		/// ��ţ����ظ�
		/// <summary>
		public Nullable<decimal> Order { get; set;}

	}
}
