using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// ��Ŀ��ѯ
/// </summary>
namespace CZManageSystem.Data.Domain.CollaborationCenter.Invest
{
    //��Ŀ��Ϣ��ѯ����
    public class VInvestProjectQueryQueryBuilder
    {
        public int? Year { get; set; }//�´����
        public string TaskID { get; set; }//�ƻ��������ĺ�
        public string ProjectID { get; set; }//��Ŀ���
        public string ProjectName { get; set; }//��Ŀ����
        public decimal? Total_start { get; set; }//��Ŀ��Ͷ��
        public decimal? Total_end { get; set; }//��Ŀ��Ͷ��
        public decimal? YearTotal_start { get; set; }//�����ĿͶ��
        public decimal? YearTotal_end { get; set; }//�����ĿͶ��
        public string BeginEnd { get; set; }//��ֹ����
        public string Content { get; set; }//��Ƚ�������
        public string FinishDate { get; set; }//Ҫ�����ʱ��
        public string DpCode_Text { get; set; }//����רҵ��
        public string UserID_Text { get; set; }//�Ҹ�����
        public string ManagerID_Text { get; set; }//��Ŀ����

    }

    public class V_InvestProjectQuery
    {
        /// <summary>
        /// �´����,�������
        /// </summary>
		public Nullable<int> Year { get; set; }
        /// <summary>
        /// �ƻ��������ĺ�
        /// </summary>
		public string TaskID { get; set; }
        /// <summary>
        /// ��Ŀ���
        /// </summary>
		public string ProjectID { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
		public string ProjectName { get; set; }
        /// <summary>
        /// ��ֹ����
        /// </summary>
		public string BeginEnd { get; set; }
        /// <summary>
        /// ��Ŀ��Ͷ��
        /// </summary>
		public Nullable<decimal> Total { get; set; }
        /// <summary>
        /// �����ĿͶ��
        /// </summary>
		public Nullable<decimal> YearTotal { get; set; }
        /// <summary>
        /// ��Ƚ�������
        /// </summary>
		public string Content { get; set; }
        /// <summary>
        /// Ҫ�����ʱ��
        /// </summary>
		public string FinishDate { get; set; }
        /// <summary>
        /// ����רҵ��
        /// </summary>
		public string DpCode { get; set; }
        /// <summary>
        /// �Ҹ�����
        /// </summary>
		public Nullable<Guid> UserID { get; set; }
        /// <summary>
        /// ��Ŀ����
        /// </summary>
		public Nullable<Guid> ManagerID { get; set; }
        /// <summary>
        /// ��ͬ��Ŀ����ͬ����˰���(Ԫ)
        /// </summary>
		public Nullable<decimal> SignTotal { get; set; }
        /// <summary>
        /// �ݹ����
        /// </summary>
		public Nullable<decimal> NotPay { get; set; }
        /// <summary>
        /// �Ѹ����
        /// </summary>
		public Nullable<decimal> Pay { get; set; }
        /// <summary>
        /// mis���
        /// </summary>
		public Nullable<decimal> MISMoney { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> MustPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> ProjectRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> TransferRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> BackYearTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> YearMustPay { get; set; }
        /// <summary>
        /// 
        /// </summary>
		public Nullable<decimal> YearRate { get; set; }

    }
}
