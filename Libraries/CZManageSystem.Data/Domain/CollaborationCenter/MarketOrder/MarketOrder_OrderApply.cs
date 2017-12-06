using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CZManageSystem.Data.Domain.SysManger;

/// <summary>
/// Ӫ������-Ӫ����������
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.MarketOrder
{
    /// <summary>
    /// ��ʷ��Ŀ�ݹ���ѯ
    /// </summary>
    public class OrderApplyQueryBuilder
    {
        public string SerialNo { get; set; }//�������
        public List<string> ListOrderStatus { get; set; }//����״̬
        public DateTime? ApplyTime_start { get; set; }//����ʱ�䡢����ʱ��
        public DateTime? ApplyTime_end { get; set; }//����ʱ�䡢����ʱ��
        public List<string> ListStatus { get; set; }//����״̬
        public string CustomPhone { get; set; }//�ͻ�����
        public string CustomName { get; set; }//�ͻ�����
        public List<Guid?> ListEndTypeID { get; set; }//�ն˻���
        public List<Guid?> ListAreaID { get; set; }//��������
        public DateTime? DealTime_start { get; set; }//����ʱ��
        public DateTime? DealTime_end { get; set; }//����ʱ�� 
        public string ProjectID { get; set; }//��Ŀ���

        public string Title { get; set; }//����
        public bool? isJK { get; set; }//�Ƿ�ҿ�ҵ��
        public Guid? Applicant { get; set; }//������
    }


    public class MarketOrder_OrderApply
    {
        /// <summary>
        /// ����ID
        /// <summary>
        public Guid ApplyID { get; set; }
        /// <summary>
        /// ����ʵ��ID
        /// <summary>
        public Nullable<Guid> WorkflowInstanceId { get; set; }
        /// <summary>
        /// �������
        /// <summary>
        public string SerialNo { get; set; }
        /// <summary>
        /// ����ʱ��
        /// <summary>
        public Nullable<DateTime> ApplyTime { get; set; }
        /// <summary>
        /// ������ID
        /// <summary>
        public Nullable<Guid> Applicant { get; set; }
        /// <summary>
        /// �������ֻ�����
        /// <summary>
        public string MobilePh { get; set; }
        /// <summary>
        /// ����״̬
        /// <summary>
        public string Status { get; set; }
        /// <summary>
        /// ����״̬
        /// <summary>
        public string OrderStatus { get; set; }
        /// <summary>
        /// ����
        /// <summary>
        public string Title { get; set; }
        /// <summary>
        /// Ӫ������
        /// <summary>
        public Nullable<Guid> MarketID { get; set; }
        /// <summary>
        /// Ŀ��ͻ�����
        /// <summary>
        public string CustomPhone { get; set; }
        /// <summary>
        /// �ͻ�����
        /// <summary>
        public string CustomName { get; set; }
        /// <summary>
        /// �ͻ����֤��
        /// <summary>
        public string CustomPersonID { get; set; }
        /// <summary>
        /// �ͻ���ϵ��ַ
        /// <summary>
        public string CustomAddr { get; set; }
        /// <summary>
        /// �ͻ���ϵ�绰
        /// <summary>
        public string CustomOther { get; set; }
        /// <summary>
        /// �ն˻���ID
        /// <summary>
        public Nullable<Guid> EndTypeID { get; set; }
        /// <summary>
        /// ���ú���
        /// <summary>
        public string UseNumber { get; set; }
        /// <summary>
        /// SIM����
        /// <summary>
        public string SIMNumber { get; set; }
        /// <summary>
        /// ����-IMEI��
        /// <summary>
        public string IMEI { get; set; }
        /// <summary>
        /// �����ײ�ID
        /// <summary>
        public Nullable<Guid> SetmealID { get; set; }
        /// <summary>
        /// ����ҵ��
        /// <summary>
        public Nullable<Guid> BusinessID { get; set; }
        /// <summary>
        /// ��ע
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// ��������ID
        /// <summary>
        public Nullable<Guid> AreaID { get; set; }
        /// <summary>
        /// ��Ŀ���
        /// <summary>
        public string ProjectID { get; set; }
        public Nullable<DateTime> YZSubmitTime { get; set; }
        /// <summary>
        /// ����״̬�����ͳ�ʱ��
        /// </summary>
		public string SendStatus { get; set; }
        public string GDOrderID { get; set; }
        /// <summary>
        /// BOSS��Ʒ��ʶ
        /// </summary>
        public string BossOfferID { get; set; }
        public string MainOrder { get; set; }
        public string SubOrder { get; set; }
        /// <summary>
        /// �ʼ�����
        /// </summary>
        public string MailNo { get; set; }
        /// <summary>
        /// ���Ͷ����˺�
        /// </summary>
		public string SendTo { get; set; }

        /// <summary>
        /// ��Ȩ��ʽ
        /// </summary>
        public Nullable<Guid> AuthenticationID { get; set; }


        public virtual Tracking_Workflow Tracking_Workflow { get; set; }//����ʵ��
        public virtual Users ApplicantObj { get; set; }//������
        public virtual MarketOrder_Market MarketObj { get; set; }//Ӫ������
        public virtual MarketOrder_EndType EndTypeObj { get; set; }//�ն˻���
        public virtual MarketOrder_Setmeal SetmealObj { get; set; }//�����ײ�
        public virtual MarketOrder_Business BusinessObj { get; set; }//����ҵ��
        public virtual MarketOrder_Area AreaObj { get; set; }//��������
        public virtual MarketOrder_Authentication AuthenticationObj { get; set; }//��Ȩ��ʽ

    }
}
