using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarIport
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid CarIportId { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// �û���
		/// <summary>
		public string UserName { get; set;}
		/// <summary>
		/// �ֻ�
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// �ڣ�
		/// <summary>
		public Nullable<int> Iport { get; set;}
		/// <summary>
		/// ��������ID
		/// <summary>
		public Nullable<int> CarApplyId { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public Nullable<int> Status { get; set;}

	}
}
