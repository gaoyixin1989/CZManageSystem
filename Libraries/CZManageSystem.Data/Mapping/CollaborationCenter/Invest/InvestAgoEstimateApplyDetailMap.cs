using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using CZManageSystem.Data.Domain.SysManger;
using System.Data.Entity.ModelConfiguration;


/// <summary>
/// ��ʷ��Ŀ�ݹ�������ϸ��
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestAgoEstimateApplyDetailMap : EntityTypeConfiguration<InvestAgoEstimateApplyDetail>
	{
		public InvestAgoEstimateApplyDetailMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��Ŀ����
		/// <summary>
			  this.Property(t => t.ProjectName)

			.HasMaxLength(500);
		/// <summary>
		/// ��Ŀ���
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(500);
		/// <summary>
		/// ��ͬ����
		/// <summary>
			  this.Property(t => t.ContractName)

			.HasMaxLength(1000);
		/// <summary>
		/// ��ͬ���
		/// <summary>
			  this.Property(t => t.ContractID)

			.HasMaxLength(100);
		/// <summary>
		/// ��Ӧ��
		/// <summary>
			  this.Property(t => t.Supply)

			.HasMaxLength(500);
		/// <summary>
		/// ����רҵ
		/// <summary>
			  this.Property(t => t.Study)

			.HasMaxLength(500);
		/// <summary>
		/// ��Ŀ
		/// <summary>
			  this.Property(t => t.Course)

			.HasMaxLength(500);
			// Table & Column Mappings
 			 this.ToTable("InvestAgoEstimateApplyDetail"); 
			// ���
			this.Property(t => t.ID).HasColumnName("ID"); 
			// �������뵥id
			this.Property(t => t.ApplyID).HasColumnName("ApplyID"); 
			// ���
			this.Property(t => t.Year).HasColumnName("Year"); 
			// �·�
			this.Property(t => t.Month).HasColumnName("Month"); 
			// ��Ŀ����
			this.Property(t => t.ProjectName).HasColumnName("ProjectName"); 
			// ��Ŀ���
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ͬ����
			this.Property(t => t.ContractName).HasColumnName("ContractName"); 
			// ��ͬ���
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// ��Ӧ��
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// ��ͬ�ܽ��
			this.Property(t => t.SignTotal).HasColumnName("SignTotal"); 
			// ��ͬʵ�ʽ��
			this.Property(t => t.PayTotal).HasColumnName("PayTotal"); 
			// ����רҵ
			this.Property(t => t.Study).HasColumnName("Study"); 
			// ������ID
			this.Property(t => t.ManagerID).HasColumnName("ManagerID"); 
			// ��Ŀ
			this.Property(t => t.Course).HasColumnName("Course"); 
			// �ϸ��½���
			this.Property(t => t.BackRate).HasColumnName("BackRate"); 
			// ��Ŀ�������
			this.Property(t => t.Rate).HasColumnName("Rate"); 
			// �Ѹ����
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// �ݹ����
			this.Property(t => t.NotPay).HasColumnName("NotPay"); 
			// �ݹ���ԱID
			this.Property(t => t.EstimateUserID).HasColumnName("EstimateUserID"); 
		 }
	 }
}
