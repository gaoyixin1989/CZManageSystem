using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-���͹��̷���
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderSendCourse
	{
		public string ID { get; set;}
		/// <summary>
		/// ���뵥ID
		/// <summary>
		public string ApplyID { get; set;}
		/// <summary>
		/// ������ID
		/// <summary>
		public string UserID { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> SendTime { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string SendCourse { get; set;}

	}
}
