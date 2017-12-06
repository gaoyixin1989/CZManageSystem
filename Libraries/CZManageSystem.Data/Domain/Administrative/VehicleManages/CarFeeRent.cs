using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarFeeRent
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid CarFeeRentId { get; set;}
		/// <summary>
		/// �༭��ID
		/// <summary>
		public Nullable<Guid> EditorId { get; set;}
		/// <summary>
		/// �༭����
		/// <summary>
		public Nullable<DateTime> EditTime { get; set;}
		/// <summary>
		/// ������λ
		/// <summary>
		public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// ʹ�õ�λ
		/// <summary>
		public string CorpName { get; set;}
		/// <summary>
		/// ����ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// �ɷ�����
		/// <summary>
		public Nullable<int> SortId { get; set;}
		/// <summary>
		/// ���޷���
		/// <summary>
		public Nullable<decimal> RentFee { get; set;}
		/// <summary>
		/// ���޹���
		/// <summary>
		public Nullable<decimal> RentCount { get; set;}
		/// <summary>
		/// ʵ����ʻ����
		/// <summary>
		public Nullable<decimal> RoadCount { get; set;}
		/// <summary>
		/// ���������
		/// <summary>
		public Nullable<decimal> MoreRoad { get; set;}
		/// <summary>
		/// ��������̷���
		/// <summary>
		public Nullable<decimal> MoreFee { get; set;}
		/// <summary>
		/// ���ͷ�
		/// <summary>
		public Nullable<decimal> GasFee { get; set;}
		/// <summary>
		/// ·��/ͣ����
		/// <summary>
		public Nullable<decimal> RoadFee { get; set;}
		/// <summary>
		/// ��ʻԱ����
		/// <summary>
		public Nullable<decimal> DriverFee { get; set;}
		/// <summary>
		/// ����С��
		/// <summary>
		public Nullable<decimal> TotalFee { get; set;}
		/// <summary>
		/// ��ʼ����
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string Person { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}

        public virtual CarInfo CarInfo { get; set; }

    }
}
