using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Ӫ������-�ط����
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
	public class MarketOrder_OrderVisit
	{
		public Guid ID { get; set;}
		/// <summary>
		/// ���뵥ID
		/// <summary>
		public Nullable<Guid> ApplyID { get; set;}
		/// <summary>
		/// �û�ID
		/// <summary>
		public Nullable<Guid> UserID { get; set;}
		public Nullable<DateTime> Time { get; set;}
		/// <summary>
		/// �����ID
		/// <summary>
		public Nullable<Guid> SatID { get; set;}
		/// <summary>
		/// �ɹ���ע
		/// <summary>
		public string SuccessRemark { get; set;}
		/// <summary>
		/// ʧ�ܻط�ID
		/// <summary>
		public Nullable<Guid> VisitID { get; set;}
		/// <summary>
		/// ʧ�ܱ�ע
		/// <summary>
		public string FailRemark { get; set;}
		public string IsAgain { get; set;}

	}
}
