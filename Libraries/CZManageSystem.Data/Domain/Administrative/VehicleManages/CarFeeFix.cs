using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarFeeFix
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid CarFeeFixId { get; set;}
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
		/// ����ID
		/// <summary>
		public Nullable<Guid> CarId { get; set;}
		/// <summary>
		/// �ɷ�����
		/// <summary>
		public Nullable<DateTime> PayTime { get; set;}
		/// <summary>
		/// ���շ�
		/// <summary>
		public Nullable<decimal> FolicyFee { get; set;}
		/// <summary>
		/// ����˰
		/// <summary>
		public Nullable<decimal> TaxFee { get; set;}
		/// <summary>
		/// ��·����
		/// <summary>
		public Nullable<decimal> RoadFee { get; set;}
		/// <summary>
		/// �����ӷ�
		/// <summary>
		public Nullable<decimal> OtherFee { get; set;}
		/// <summary>
		/// ����С��
		/// <summary>
		public Nullable<decimal> TotalFee { get; set;}
		/// <summary>
		/// �Ʒѿ�ʼ����
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
