using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentLoginLogMap : EntityTypeConfiguration<PaymentLoginLog>
	{
		public PaymentLoginLogMap()
		{
			// Primary Key
			 this.HasKey(t => t.LogID);
		/// <summary>
		/// ��¼ID
		/// <summary>
			  this.Property(t => t.LoginID)

			.HasMaxLength(50);
		/// <summary>
		/// ����
		/// <summary>
			  this.Property(t => t.LoginName)

			.HasMaxLength(50);
		/// <summary>
		/// ��˾����
		/// <summary>
			  this.Property(t => t.CompanyName)

			.HasMaxLength(50);
		/// <summary>
		/// ��¼IP
		/// <summary>
			  this.Property(t => t.LoginIP)

			.HasMaxLength(50);
			  this.Property(t => t.Result)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentLoginLog"); 
			// ��־ID
			this.Property(t => t.LogID).HasColumnName("LogID"); 
			// ��¼ʱ��
			this.Property(t => t.LoginTime).HasColumnName("LoginTime"); 
			// ��¼ID
			this.Property(t => t.LoginID).HasColumnName("LoginID"); 
			// ����
			this.Property(t => t.LoginName).HasColumnName("LoginName"); 
			// ��˾����
			this.Property(t => t.CompanyName).HasColumnName("CompanyName"); 
			// ��˾ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
			// ��¼IP
			this.Property(t => t.LoginIP).HasColumnName("LoginIP"); 
			this.Property(t => t.Result).HasColumnName("Result"); 
		 }
	 }
}
