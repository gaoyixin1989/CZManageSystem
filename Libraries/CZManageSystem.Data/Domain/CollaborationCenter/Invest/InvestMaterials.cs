using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// ���ʲɹ�
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
	public class InvestMaterials
	{
		/// <summary>
		/// Ψһ��
		/// <summary>
		public Guid ID { get; set;}
        /// <summary>
        /// ��Ŀ���
        /// <summary>
         [Required(ErrorMessage = "��Ŀ��Ų���Ϊ��")]
        public string ProjectID { get; set;}
        /// <summary>
        /// ��Ŀ����
        /// <summary>
        [Required(ErrorMessage = "��Ŀ���Ʋ���Ϊ��")]
        public string ProjectName { get; set;}
        /// <summary>
        /// �������
        /// <summary>
        [Required(ErrorMessage = "������Ų���Ϊ��")]
        public string OrderID { get; set;}
		/// <summary>
		/// ����˵��
		/// <summary>
		public string OrderDesc { get; set;}
		/// <summary>
		/// ����¼�빫˾
		/// <summary>
		public string OrderInCompany { get; set;}
		/// <summary>
		/// ���״̬(��׼)
		/// <summary>
		public string AuditStatus { get; set;}
        /// <summary>
        /// ����¼����
        /// <summary>
        [Required(ErrorMessage = "����¼�����Ϊ��")]
        public Nullable<decimal> OrderInPay { get; set;}
		/// <summary>
		/// �������չ�˾
		/// <summary>
		public string OrderOutCompany { get; set;}
        /// <summary>
        /// �������ս��
        /// <summary>
        [Required(ErrorMessage = "�������ս���Ϊ��")]
        public Nullable<decimal> OrderOutSum { get; set;}
		/// <summary>
		/// ��������ʱ��
		/// <summary>
		public Nullable<DateTime> OrderCreateTime { get; set;}
		/// <summary>
		/// ��ͬ���
		/// <summary>
		public string ContractID { get; set;}
		/// <summary>
		/// ��ͬ����
		/// <summary>
		public string ContractName { get; set;}
		/// <summary>
		/// ��Χϵͳ��ͬ���
		/// <summary>
		public string OutContractID { get; set;}
		/// <summary>
		/// ��������
		/// <summary>
		public string OrderTitle { get; set;}
		/// <summary>
		/// ������ע
		/// <summary>
		public string OrderNote { get; set;}
		/// <summary>
		/// ��Ӧ��
		/// <summary>
		public string Apply { get; set;}
		/// <summary>
		/// �������հٷֱ� SUM
		/// <summary>
		public Nullable<decimal> OrderOutRate { get; set;}
		/// <summary>
		/// δ�����豸��Ԫ��
		/// <summary>
		public Nullable<decimal> OrderUnReceived { get; set;}

	}
}
