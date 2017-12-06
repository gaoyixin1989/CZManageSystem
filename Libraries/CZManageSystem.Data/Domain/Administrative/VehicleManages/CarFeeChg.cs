using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarFeeChg
	{
		public Guid CarFeeChgId { get; set;}
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
		public Nullable<DateTime> PayTime { get; set;}
		/// <summary>
		/// ���¹�����
		/// <summary>
		public Nullable<decimal> RoadLast { get; set;}
		/// <summary>
		/// ���¹�����
		/// <summary>
		public Nullable<decimal> RoadThis { get; set;}
		/// <summary>
		/// ������ʻ������
		/// <summary>
		public Nullable<decimal> RoadCount { get; set;}
		/// <summary>
		/// ʵ������
		/// <summary>
		public Nullable<decimal> OilCount { get; set;}
		/// <summary>
		/// �ͼ�
		/// <summary>
		public Nullable<decimal> OilPrice { get; set;}
		/// <summary>
		/// ���ͷ�
		/// <summary>
		public Nullable<decimal> OilFee { get; set;}
		/// <summary>
		/// ά�޷�
		/// <summary>
		public Nullable<decimal> FixFee { get; set;}
		/// <summary>
		/// ·��/ͣ����
		/// <summary>
		public Nullable<decimal> RoadFee { get; set;}
		/// <summary>
		/// ס�޷�
		/// <summary>
		public Nullable<decimal> LiveFee { get; set;}
		/// <summary>
		/// �ͷ�
		/// <summary>
		public Nullable<decimal> EatFee { get; set;}
		/// <summary>
		/// �����ӷ�
		/// <summary>
		public Nullable<decimal> OtherFee { get; set;}
		/// <summary>
		/// ����С��
		/// <summary>
		public Nullable<decimal> TotalFee { get; set;}
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
