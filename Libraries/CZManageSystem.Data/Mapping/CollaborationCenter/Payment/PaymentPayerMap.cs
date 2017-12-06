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
		/// 付款人账户
		/// <summary>
			  this.Property(t => t.Account)

			.HasMaxLength(50);
		/// <summary>
		/// 付款人账户名称
		/// <summary>
			  this.Property(t => t.Name)

			.HasMaxLength(50);
		/// <summary>
		/// 付款人分行代码
		/// <summary>
			  this.Property(t => t.BranchCode)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentPayer"); 
			this.Property(t => t.PayerID).HasColumnName("PayerID"); 
			// 付款人账户
			this.Property(t => t.Account).HasColumnName("Account"); 
			// 付款人账户名称
			this.Property(t => t.Name).HasColumnName("Name"); 
			// 付款人分行代码
			this.Property(t => t.BranchCode).HasColumnName("BranchCode"); 
			// 付款公司ID
			this.Property(t => t.CompanyID).HasColumnName("CompanyID"); 
		 }
	 }
}
