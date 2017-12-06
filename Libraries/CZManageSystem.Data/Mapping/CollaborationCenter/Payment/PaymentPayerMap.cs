using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentPayerMap : EntityTypeConfiguration<PaymentPayer>
	{
		public PaymentPayerMap()
		{
			// Primary Key
			 this.HasKey(t => t.PayerID);
		/// <summary>
		/// �������˻�
		/// <summary>
			  this.Property(t => t.Account)

			.HasMaxLength(50);
		/// <summary>
		/// �������˻�����
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(50);
		/// <summary>
		/// �����˷��д���
		/// <summary>
			  this.Property(t => t.BranchCode)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPayer"); 
			this.Property(t => t.PayerID).HasColumnName("PayerID"); 
			// �������˻�
			this.Property(t => t.Account).HasColumnName("Account"); 
			// �������˻�����
			this.Property(t => t.Name).HasColumnName("Name"); 
			// �����˷��д���
			this.Property(t => t.BranchCode).HasColumnName("BranchCode"); 
			// ���˾ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
		 }
	 }
}
