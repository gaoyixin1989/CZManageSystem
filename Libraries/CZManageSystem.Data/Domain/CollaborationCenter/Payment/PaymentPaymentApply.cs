using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.CollaborationCenter.Payment
{
	public class PaymentPaymentApply
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
		/// ����ʱ�䡢����ʱ��
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<DateTime> PayDay { get; set;}
		/// <summary>
		/// ���湫˾ID
		/// <summary>
		public Nullable<Guid> CompanyID { get; set;}
		/// <summary>
		/// ����״̬
		/// <summary>
		public string Status { get; set;}
		/// <summary>
		/// ���к�
		/// <summary>
		public string Series { get; set;}
		/// <summary>
		/// �ύʱ��
		/// <summary>
		public Nullable<DateTime> SubmitTime { get; set;}

	}
}
