using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-���ս����Ϣ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderReclaim
	{
		/// <summary>
		/// ���
		/// <summary>
		public Guid ID { get; set;}
		/// <summary>
		/// ���뵥ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// �����û�ID
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// ���ս��
		/// <summary>
		public string IsReclaim { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string ReclaimRemark { get; set;}

	}
}
