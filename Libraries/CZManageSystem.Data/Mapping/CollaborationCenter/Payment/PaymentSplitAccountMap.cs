using CZManageSystem.Data.Domain.CollaborationCenter.Payment;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
namespace CZManageSystem.Data.Mapping.CollaborationCenter.Payment
{
	public class PaymentSplitAccountMap : EntityTypeConfiguration<PaymentSplitAccount>
	{
		public PaymentSplitAccountMap()
		{
			// Primary Key
			 this.HasKey(t => t.AccountID);
		/// <summary>
		/// 拆帐收款人帐户
		/// <summary>
			  this.Property(t => t.Account)

			.HasMaxLength(50);
			// Table & Column Mappings
 			 this.ToTable("PaymentSplitAccount"); 
			// 帐户ID
			this.Property(t => t.AccountID).HasColumnName("AccountID"); 
			// 拆帐收款人帐户
			this.Property(t => t.Account).HasColumnName("Account"); 
		 }
	 }
}
