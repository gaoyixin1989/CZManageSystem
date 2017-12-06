using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

namespace CZManageSystem.Data.Domain.Administrative.VehicleManages
{
    public class CarApply
    {
        /// <summary>
        /// ����
        /// <summary>
        public Guid ApplyId { get; set; }
        /// <summary>
        /// ����ʵ��Id
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
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
        public Nullable<int> CorpId { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> CreateTime { get; set; }
        /// <summary>
        /// ���ڲ���
        /// <summary>
        public string DeptName { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string ApplyCant { get; set; }
        /// <summary>
        /// ������ID
        /// <summary>
        public Nullable<Guid> ApplyCantId { get; set; }
        /// <summary>
        /// ��ʻ�ˡ�ʹ����
        /// <summary>
        public string Driver { get; set; }
        /// <summary>
		/// ��ʻ�ˡ�ʹ����Ids
		/// <summary>
		public string DriverIds { get; set; }
        /// <summary>
        /// ��ϵ�绰
        /// <summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Ԥ�ƽ���ʱ��
        /// <summary>
        public Nullable<DateTime> TimeOut { get; set; }
        /// <summary>
        /// ������ʼʱ��
        /// <summary>
        public Nullable<DateTime> StartTime { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> EndTime { get; set; }
        /// <summary>
        /// �����ص�
        /// <summary>
        public string Starting { get; set; }
        /// <summary>
        /// Ŀ�ĵ�1
        /// <summary>
        public string Destination1 { get; set; }
        /// <summary>
        /// Ŀ�ĵ�2
        /// <summary>
        public string Destination2 { get; set; }
        /// <summary>
        /// Ŀ�ĵ�3
        /// <summary>
        public string Destination3 { get; set; }
        /// <summary>
        /// Ŀ�ĵ�4
        /// <summary>
        public string Destination4 { get; set; }
        /// <summary>
        /// Ŀ�ĵ�5
        /// <summary>
        public string Destination5 { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string PersonCount { get; set; }
        /// <summary>
        /// ·;���
        /// <summary>
        public string Road { get; set; }
        /// <summary>
        /// ������;
        /// <summary>
        public string UseType { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string Request { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string Attach { get; set; }
        /// <summary>
        /// ����00
        /// <summary>
        public string Field00 { get; set; }
        /// <summary>
        /// ����01
        /// <summary>
        public string Field01 { get; set; }
        /// <summary>
        /// ����02
        /// <summary>
        public string Field02 { get; set; }
        /// <summary>
        /// �����ˡ�������
        /// <summary>
        public string Allocator { get; set; }
        /// <summary>
        /// ������������ʱ��
        /// <summary>
        public Nullable<DateTime> AllotTime { get; set; }
        public string CarIds { get; set; }

        /// <summary>
        /// ����������Ϣ
        /// <summary>
        public string AllotIntro { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// �ó�����ʱ��
        /// <summary>
        public Nullable<DateTime> FinishTime { get; set; }
        public string UptUser { get; set; }
        public Nullable<DateTime> UptTime { get; set; }
        /// <summary>
        /// ������
        /// <summary>
        public string BalUser { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> BalTime { get; set; }
        /// <summary>
        /// ��ʼ������
        /// <summary>
        public Nullable<int> KmNum1 { get; set; }
        /// <summary>
        /// ��ֹ������
        /// <summary>
        public Nullable<int> KmNum2 { get; set; }
        /// <summary>
        /// ����ʹ�����
        /// <summary>
        public Nullable<int> KmCount { get; set; }
        /// <summary>
        /// ·�ŷѹ�����
        /// <summary>
        public Nullable<int> BalCount { get; set; }
        /// <summary>
        /// �ϼƽ��
        /// <summary>
        public Nullable<decimal> BalTotal { get; set; }
        /// <summary>
        /// ��ע��Ϣ
        /// <summary>
        public string BalRemark { get; set; }
        /// <summary>
        /// �����޸���
        /// <summary>
        public string OpinUser { get; set; }
        /// <summary>
        /// �����޸�ʱ��
        /// <summary>
        public Nullable<DateTime> OpinTime { get; set; }
        /// <summary>
        /// �����г���ȫ
        /// <summary>
        public string OpinGrade1 { get; set; }
        /// <summary>
        /// ���۷�������
        /// <summary>
        public string OpinGrade2 { get; set; }
        /// <summary>
        /// ���۳�������
        /// <summary>
        public string OpinGrade3 { get; set; }
        /// <summary>
        /// ���۸����Ǳ�
        /// <summary>
        public string OpinGrade4 { get; set; }
        /// <summary>
        /// ����ʱ�����
        /// <summary>
        public string OpinGrade5 { get; set; }
        /// <summary>
        /// ���۷����
        /// <summary>
        public string OpinGrade6 { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string OpinGrade7 { get; set; }
        /// <summary>
        /// ���۱�ע
        /// <summary>
        public string OpinRemark { get; set; }
        /// <summary>
        /// ����ԭ��˵��
        /// <summary>
        public string SpecialReason { get; set; }
        /// <summary>
        /// �Ƿ��ѿ�ͷ����
        /// <summary>
        public Nullable<bool> Boral { get; set; }
        /// <summary>
        /// ��ͷ�����쵼
        /// <summary>
        public string Leader { get; set; }
        /// <summary>
        /// �ó���������
        /// <summary>
        public Nullable<int> ApplyType { get; set; }
        /// <summary>
		/// ��λ/����
		/// <summary>
		public string CarTonnage { get; set; }
        public virtual Tracking_Workflow TrackingWorkflow { get; set; }
    }
}
