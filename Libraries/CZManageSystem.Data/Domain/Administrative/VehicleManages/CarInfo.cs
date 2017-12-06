using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarInfo
	{
		/// <summary>
		/// ����
		/// <summary>
		public Guid CarId { get; set;}
		/// <summary>
		/// �༭��
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
		/// ���
		/// <summary>
		public string SN { get; set;}
		/// <summary>
		/// ���ƺ���
		/// <summary>
		public string LicensePlateNum { get; set;}
		/// <summary>
		/// ����Ʒ��
		/// <summary>
		public string CarBrand { get; set;}
		/// <summary>
		/// �ͺ�
		/// <summary>
		public string CarModel { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string CarEngine { get; set;}
		/// <summary>
		/// ���ܺ�
		/// <summary>
		public string CarNum { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string CarType { get; set;}
		/// <summary>
		/// ��λ/����
		/// <summary>
		public string CarTonnage { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string DeptName { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public Nullable<DateTime> BuyDate { get; set;}
		/// <summary>
		/// �����
		/// <summary>
		public string CarPrice { get; set;}
		/// <summary>
		/// �۾�����
		/// <summary>
		public string CarLimit { get; set;}
		/// <summary>
		/// ÿ���۾�
		/// <summary>
		public string Depre { get; set;}
		/// <summary>
		/// ���޿�ʼʱ��
		/// <summary>
		public Nullable<DateTime> RentTime1 { get; set;}
		/// <summary>
		/// ���޽���ʱ��
		/// <summary>
		public Nullable<DateTime> RentTime2 { get; set;}
		/// <summary>
		/// ���տ�ʼʱ��
		/// <summary>
		public Nullable<DateTime> PolicyTime1 { get; set;}
		/// <summary>
		/// ���ս���ʱ��
		/// <summary>
		public Nullable<DateTime> PolicyTime2 { get; set;}
		/// <summary>
		/// ����ʼʱ��
		/// <summary>
		public Nullable<DateTime> AnnualTime1 { get; set;}
		/// <summary>
		/// �������ʱ��
		/// <summary>
		public Nullable<DateTime> AnnualTime2 { get; set;}
		/// <summary>
		/// ��ʻԱ
		/// <summary>
		public Nullable<Guid> DriverId { get; set;}
		/// <summary>
		/// ״̬
		/// <summary>
		public Nullable<int> Status { get; set;}
		public string Field00 { get; set;}
		public string Field01 { get; set;}
		public string Field02 { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}

        public virtual CarDriverInfo CarDriverInfo { get; set; }

    }


}
