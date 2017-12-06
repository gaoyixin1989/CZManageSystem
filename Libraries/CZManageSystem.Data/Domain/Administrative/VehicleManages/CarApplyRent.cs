using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
	public class CarApplyRent
	{
		public Guid ApplyRentId { get; set;}
		/// <summary>
		/// ����ʵ��Id
		/// <summary>
		public Nullable<Guid> WorkflowInstanceId { get; set;}
        /// <summary>
        /// ���̵���
        /// </summary>
        public string ApplySn { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string ApplyTitle { get; set; }
        /// <summary>
        /// ������λ
        /// <summary>
        public Nullable<int> CorpId { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> CreateTime { get; set;}
		/// <summary>
		/// ���ڲ���
		/// <summary>
		public string DeptName { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string ApplyCant { get; set;}
		/// <summary>
		/// ��ʻ�ˡ�ʹ����
		/// <summary>
		public string Driver { get; set;}
		/// <summary>
		/// ��ϵ�绰
		/// <summary>
		public string Mobile { get; set;}
		/// <summary>
		/// Ԥ�ƽ���ʱ��
		/// <summary>
		public Nullable<DateTime> TimeOut { get; set;}
		/// <summary>
		/// ������ʼʱ��
		/// <summary>
		public Nullable<DateTime> StartTime { get; set;}
		/// <summary>
		/// ����ʱ��
		/// <summary>
		public Nullable<DateTime> EndTime { get; set;}
		/// <summary>
		/// �����ص�
		/// <summary>
		public string Starting { get; set;}
		/// <summary>
		/// Ŀ�ĵ�1
		/// <summary>
		public string Destination1 { get; set;}
		/// <summary>
		/// Ŀ�ĵ�2
		/// <summary>
		public string Destination2 { get; set;}
		/// <summary>
		/// Ŀ�ĵ�3
		/// <summary>
		public string Destination3 { get; set;}
		/// <summary>
		/// Ŀ�ĵ�4
		/// <summary>
		public string Destination4 { get; set;}
		/// <summary>
		/// Ŀ�ĵ�5
		/// <summary>
		public string Destination5 { get; set;}
		/// <summary>
		/// ������
		/// <summary>
		public string PersonCount { get; set;}
		/// <summary>
		/// ·;���
		/// <summary>
		public string Road { get; set;}
		/// <summary>
		/// ������;
		/// <summary>
		public string UseType { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Request { get; set;}
		/// <summary>
		/// ����
		/// <summary>
		public string Attach { get; set;}
		/// <summary>
		/// ����00
		/// <summary>
		public string Field00 { get; set;}
		/// <summary>
		/// ����01
		/// <summary>
		public string Field01 { get; set;}
		/// <summary>
		/// ����02
		/// <summary>
		public string Field02 { get; set;}
		/// <summary>
		/// �����ˡ�������
		/// <summary>
		public string Allocator { get; set;}
		/// <summary>
		/// ������������ʱ��
		/// <summary>
		public Nullable<DateTime> AllotTime { get; set;}
		/// <summary>
		/// ��ע
		/// <summary>
		public string Remark { get; set;}
		/// <summary>
		/// �⳵����
		/// <summary>
		public string Htype { get; set;}
		/// <summary>
		/// ��λ/����
		/// <summary>
		public string CarTonnage { get; set;}
		/// <summary>
		/// �⳵ѯ��
		/// <summary>
		public string Enquiry { get; set;}
		/// <summary>
		/// �Ƿ��ڷ���Ԥ������
		/// <summary>
		public Nullable<bool> AllotRight { get; set;}

        public virtual Tracking_Workflow TrackingWorkflow { get; set; }

    }
}
