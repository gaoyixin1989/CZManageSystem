using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarStatus
	{
		public Guid Id { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// ��������ID
		/// <summary>
		public Nullable<Guid> CarApplyId { get; set;}
		/// <summary>
		/// Ԥ�ƽ���ʱ��
		/// <summary>
		public Nullable<DateTime> TimeOut { get; set;}
		/// <summary>
		/// �ó�����ʱ��
		/// <summary>
		public Nullable<DateTime> FinishTime { get; set;}

	}
}
