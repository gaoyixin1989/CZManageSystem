using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ��ʷ��Ŀ�ݹ��޸���־
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestAgoEstimateLogMap : EntityTypeConfiguration<InvestAgoEstimateLog>
	{
		public InvestAgoEstimateLogMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��Ŀid
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͬid
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// �����������޸ġ������޸ġ������޸ġ�������롢ɾ��
		/// <summary>
			  this.Property(t => t.OpType)

			.HasMaxLength(50);
		/// <summary>
		/// ������
		/// <summary>
			  this.Property(t => t.OpName)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("InvestAgoEstimateLog"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��Ŀid
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ͬid
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// �����������޸ġ������޸ġ������޸ġ�������롢ɾ��
			this.Property(t => t.OpType).HasColumnName("OpType"); 
			// ������
			this.Property(t => t.OpName).HasColumnName("OpName"); 
			// �޸�ʱ��
			this.Property(t => t.OpTime).HasColumnName("OpTime"); 
			// �޸�ǰ��ͬʵ�ʽ��
			this.Property(t => t.BfPayTotal).HasColumnName("BfPayTotal"); 
			// �޸ĺ��ͬʵ�ʽ��
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// �޸�ǰ��Ŀ�������
			this.Property(t => t.BfRate).HasColumnName("BfRate"); 
			// �޸ĺ���Ŀ�������
			this.Property(t => t.Rate).HasColumnName("Rate"); 
			// �޸�ǰ�Ѹ����
			this.Property(t => t.BfPay).HasColumnName("BfPay"); 
			// �޸ĺ��Ѹ����
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// �޸�ǰ�ݹ����
			this.Property(t => t.BfNotPay).HasColumnName("BfNotPay"); 
			// �޸ĺ��ݹ����
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
		 }
	 }
}
