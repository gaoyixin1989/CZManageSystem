using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPaymentApplySub
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid ApplyID { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string ApplyTitle { get; set;}
		/// <summary>
		/// ����ʵ��Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
		public string ApplySn { get; set;}
		/// <summary>
		/// ��ϵ����
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// ������ID
		/// <summary>
		public Nullable<Guid> MainApplyID { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<DateTime> PayDay { get; set;}
		/// <summary>
		/// ���к�
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// ������ID
		/// <summary>
		public string  AppliCant { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> ApplyTime { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public string Status { get; set;}
		/// <summary>
		/// ��Ӫ��ID
		/// <summary>
		public string HallID { get; set;}
		/// <summary>
		/// ���湫˾
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}

	}
}
