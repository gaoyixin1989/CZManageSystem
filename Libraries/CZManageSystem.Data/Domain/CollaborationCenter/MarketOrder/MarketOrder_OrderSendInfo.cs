using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-���ͽ����Ϣ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderSendInfo
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ���뵥ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// ������ID
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// ���ͽ��
		/// <summary>
		public string IsSuccess { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string SendInfo { get; set;}

	}
}
