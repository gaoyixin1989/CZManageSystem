using CZManageSystem.Data.Domain.CollaborationCenter.Invest;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

/// <summary>
/// ��ͬ�Ѹ�����
/// </summary>
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Invest
{
	public class InvestContractPayMap : EntityTypeConfiguration<InvestContractPay>
	{
		public InvestContractPayMap()
		{
			// Primary Key
			 this.HasKey(t => t.ID);
		/// <summary>
		/// ��
		/// <summary>
			  this.Property(t => t.Batch)

			.HasMaxLength(500);
		/// <summary>
		/// �ռ��ʷ�¼
		/// <summary>
			  this.Property(t => t.DateAccount)

			.HasMaxLength(500);
		/// <summary>
		/// ��˵��(Ψһ��
		/// <summary>
			  this.Property(t => t.RowNote)

			.HasMaxLength(500);
		/// <summary>
		/// ��Ŀ���
		/// <summary>
			  this.Property(t => t.ProjectID)

			.HasMaxLength(50);
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
			// Table & Column Mappings
 			 this.ToTable("InvestContractPay"); 
			this.Property(t => t.ID).HasColumnName("ID"); 
			// ��
			this.Property(t => t.Batch).HasColumnName("Batch"); 
			// �ռ��ʷ�¼
			this.Property(t => t.DateAccount).HasColumnName("DateAccount"); 
			// ��˵��(Ψһ��
			this.Property(t => t.RowNote).HasColumnName("RowNote"); 
			// ��Ŀ���
			this.Property(t => t.ProjectID).HasColumnName("ProjectID"); 
			// ��ͬ���
			this.Property(t => t.ContractID).HasColumnName("ContractID"); 
			// ��Ӧ��
			this.Property(t => t.Supply).HasColumnName("Supply"); 
			// �Ѹ�����
			this.Property(t => t.Pay).HasColumnName("Pay"); 
			// ¼��ʱ��
			this.Property(t => t.Time).HasColumnName("Time"); 
			// ¼����
			this.Property(t => t.UserID).HasColumnName("UserID"); 
		 }
	 }
}
