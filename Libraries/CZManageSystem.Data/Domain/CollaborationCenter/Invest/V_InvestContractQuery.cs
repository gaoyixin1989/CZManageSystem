using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ��ͬ��ѯ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    public class V_InvestContractQuery
    {
        /// <summary>
        /// 
        /// </summary>
		public Guid ID { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public Nullable<DateTime> ImportTime { get; set; }
        /// <summary>
        /// ��Ŀ���
        /// </summary>
        public string ProjectID { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// ��ͬ���
        /// </summary>
        public string ContractID { get; set; }
        /// <summary>
        /// ��ͬ����
        /// </summary>
        public string ContractName { get; set; }
        /// <summary>
        /// ǩ��ʱ��
        /// </summary>
        public string SignTime { get; set; }
        /// <summary>
        /// ��ͬ���첿��
        /// </summary>
        public string DpCode { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public Nullable<Guid> UserID { get; set; }
        /// <summary>
        /// ��ͬ��Ŀ����ͬ����˰���(Ԫ)
        /// </summary>
        public Nullable<decimal> SignTotal { get; set; }
        /// <summary>
        /// ʵ�ʺ�ͬ���
        /// </summary>
        public Nullable<decimal> PayTotal { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// ��Ŀ����ID
        /// </summary>
        public Nullable<Guid> ManagerID { get; set; }
        /// <summary>
        /// ��Ӧ��
        /// </summary>
        public string Supply { get; set; }
        /// <summary>
        /// ��ͬ�ܽ��
        /// </summary>
        public Nullable<decimal> AllTotal { get; set; }
        /// <summary>
        /// �Ƿ�MIS����
        /// </summary>
        public string IsMIS { get; set; }
        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        public string IsDel { get; set; }
        /// <summary>
        /// ��ͬ״̬
        /// </summary>
        public string ContractState { get; set; }
        /// <summary>
        /// ��ͬ����
        /// </summary>
        public string Attribute { get; set; }
        /// <summary>
        /// ��Ŀ���
        /// </summary>
        public Nullable<decimal> ProjectTotal { get; set; }
        /// <summary>
        /// ������ʼʱ��
        /// </summary>
        public Nullable<DateTime> ApproveStartTime { get; set; }
        /// <summary>
        /// ��������ʱ��
        /// </summary>
        public Nullable<DateTime> ApproveEndTime { get; set; }
        /// <summary>
        /// ��ͬ������
        /// </summary>
        public string ContractFilesNum { get; set; }
        /// <summary>
        /// ӡ��˰��
        /// </summary>
        public string StampTaxrate { get; set; }
        /// <summary>
        /// ӡ��˰��
        /// </summary>
        public string Stamptax { get; set; }
        /// <summary>
        /// ��ͬ�Է�
        /// </summary>
        public string ContractOpposition { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string RequestDp { get; set; }
        /// <summary>
        /// ��ز���
        /// </summary>
        public string RelevantDp { get; set; }
        /// <summary>
        /// ��Ŀ��չԭ��
        /// </summary>
        public string ProjectCause { get; set; }
        /// <summary>
        /// ��ͬ����
        /// </summary>
        public string ContractType { get; set; }
        /// <summary>
        /// ��ͬ����
        /// </summary>
        public string ContractOppositionFrom { get; set; }
        /// <summary>
        /// ��ͬ�Է�ѡ��ʽ
        /// </summary>
        public string ContractOppositionType { get; set; }
        /// <summary>
        /// �ɹ���ʽ
        /// </summary>
        public string Purchase { get; set; }
        /// <summary>
        /// ���ʽ
        /// </summary>
        public string PayType { get; set; }
        /// <summary>
        /// ����˵��
        /// </summary>
        public string PayRemark { get; set; }
        /// <summary>
        /// ��ͬ��Ч������ʼ
        /// </summary>
        public Nullable<DateTime> ContractStartTime { get; set; }
        /// <summary>
        /// ��ͬ��Ч������ֹ
        /// </summary>
        public Nullable<DateTime> ContractEndTime { get; set; }
        /// <summary>
        /// ��ܺ�ͬ
        /// </summary>
        public string IsFrameContract { get; set; }
        /// <summary>
        /// ���ʱ��
        /// </summary>
        public Nullable<DateTime> DraftTime { get; set; }
        /// <summary>
        /// ��ǩ����Ŀ�ܶ�
        /// </summary>
        public Nullable<decimal> ProjectAllTotal { get; set; }
        /// <summary>
        /// MIS���ս��
        /// </summary>
        public Nullable<decimal> MISMoney { get; set; }
        /// <summary>
        /// �Ѹ�����
        /// </summary>
        public Nullable<decimal> Pay { get; set; }
        /// <summary>
        /// �ݹ����
        /// </summary>
        public Nullable<decimal> NotPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Nullable<decimal> MustPay { get; set; }

    }
}
