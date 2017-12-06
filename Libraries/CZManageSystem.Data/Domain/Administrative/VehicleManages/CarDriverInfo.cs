using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarDriverInfo
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid DriverId { get; set;}
		/// <summary>
		/// �༭��
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// �༭ʱ��
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// ������λ
		/// <summary>
		public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// ˾�����
		/// <summary>
		public string SN { get; set;}
		public string Name { get; set;}
		/// <summary>
		/// �ֻ���
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string DeptName { get; set;}
		/// <summary>
		/// ��ʼ��ʻʱ��
		/// <summary>
		public Nullable<DateTime> CarAge { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public Nullable<DateTime> Birthday { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}

	}
}
