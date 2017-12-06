using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentLoginLog
	{
		/// <summary>
		/// ��־ID
		/// <summary>
		public Guid LogID { get; set;}
		/// <summary>
		/// ��¼ʱ��
		/// <summary>
		public Nullable<DateTime> LoginTime { get; set;}
		/// <summary>
		/// ��¼ID
		/// <summary>
		public string LoginID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string LoginName { get; set;}
		/// <summary>
		/// ��˾����
		/// <summary>
		public string CompanyName { get; set;}
		/// <summary>
		/// ��˾ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}
		/// <summary>
		/// ��¼IP
		/// <summary>
		public string LoginIP { get; set;}
		public string Result { get; set;}

	}
}
